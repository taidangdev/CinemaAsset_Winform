use CinemaAssetDB
go

-- view hiển thị đối tác còn hợp tác
CREATE OR ALTER VIEW dbo.vw_VendorActiveWithCatalog AS
SELECT
  v.vendor_id,
  v.name,
  v.phone,
  v.email,
  v.address,
  STRING_AGG(ui.[display], N', ') WITHIN GROUP (ORDER BY ui.[display]) AS asset_types -- đã Việt hoá
FROM dbo.Vendor v
LEFT JOIN dbo.VendorCatalog vc
  ON vc.vendor_id = v.vendor_id AND vc.is_active = 1
LEFT JOIN dbo.vw_AssetTypes_UI ui
  ON ui.[key] = vc.asset_type_id
WHERE v.is_active = 1
GROUP BY v.vendor_id, v.name, v.phone, v.email, v.address;
GO


--View (bảng hàng) để bind combobox/chi tiết: Vendor ↔ AssetType (dạng dòng)
CREATE OR ALTER VIEW dbo.vw_VendorCatalogActive AS
SELECT
  v.vendor_id, v.name AS vendor_name,
  at.asset_type_id, at.name AS asset_type_name
FROM Vendor v
JOIN VendorCatalog vc
  ON vc.vendor_id = v.vendor_id AND vc.is_active = 1
JOIN AssetType at
  ON at.asset_type_id = vc.asset_type_id
WHERE v.is_active = 1;
GO

--TVF: lấy danh sách loại hàng 1 vendor (dễ dùng trong SELECT có tham số)
CREATE OR ALTER FUNCTION dbo.fn_VendorAssetTypes(@vendor_id INT)
RETURNS TABLE
AS
RETURN (
  SELECT at.asset_type_id, at.name AS asset_type_name
  FROM VendorCatalog vc
  JOIN AssetType at ON at.asset_type_id = vc.asset_type_id
  WHERE vc.vendor_id = @vendor_id AND vc.is_active = 1
);
GO


--Thêm đối tác mới kèm danh mục cung cấp (JSON)
CREATE OR ALTER PROCEDURE dbo.sp_Vendor_CreateWithCatalog
  @name NVARCHAR(200),
  @phone NVARCHAR(30) = NULL,
  @email NVARCHAR(120) = NULL,
  @address NVARCHAR(300) = NULL,
  @AssetTypesJson NVARCHAR(MAX) = NULL,   -- optional
  @vendor_id INT OUTPUT
AS
BEGIN
  SET NOCOUNT ON;

  INSERT INTO Vendor(name, phone, email, address)
  VALUES(@name, @phone, @email, @address);

  SET @vendor_id = SCOPE_IDENTITY();

  -- không có JSON thì thôi (tạo vendor trước)
  IF @AssetTypesJson IS NULL OR LEN(@AssetTypesJson)=0 RETURN;

  DECLARE @T TABLE(asset_type_id INT PRIMARY KEY);

  INSERT INTO @T(asset_type_id)
  SELECT COALESCE(
           TRY_CONVERT(INT, JSON_VALUE(j.value,'$.asset_type_id')),
           (SELECT asset_type_id FROM AssetType WHERE name = JSON_VALUE(j.value,'$.asset_type_name'))
         )
  FROM OPENJSON(@AssetTypesJson) j;

  DELETE FROM @T WHERE asset_type_id IS NULL;

  -- map VendorCatalog (bật is_active)
  MERGE VendorCatalog AS tgt
  USING (SELECT @vendor_id AS vendor_id, t.asset_type_id FROM @T t) src
  ON (tgt.vendor_id = src.vendor_id AND tgt.asset_type_id = src.asset_type_id)
  WHEN MATCHED THEN UPDATE SET is_active = 1
  WHEN NOT MATCHED THEN INSERT(vendor_id, asset_type_id, is_active) VALUES(src.vendor_id, src.asset_type_id, 1);

  -- không động chạm các map khác (nếu có)
END
GO

--Cập nhật danh mục cung cấp của 1 vendor (set theo JSON)
CREATE OR ALTER PROCEDURE dbo.sp_VendorCatalog_Set
  @vendor_id INT,
  @AssetTypesJson NVARCHAR(MAX)
AS
BEGIN
  SET NOCOUNT ON;

  IF NOT EXISTS (SELECT 1 FROM Vendor WHERE vendor_id=@vendor_id AND is_active=1)
  BEGIN
    RAISERROR(N'Vendor không tồn tại hoặc đã ngưng hợp tác.',16,1); RETURN;
  END

  DECLARE @T TABLE(asset_type_id INT PRIMARY KEY);

  INSERT INTO @T(asset_type_id)
  SELECT COALESCE(
           TRY_CONVERT(INT, JSON_VALUE(j.value,'$.asset_type_id')),
           (SELECT asset_type_id FROM AssetType WHERE name = JSON_VALUE(j.value,'$.asset_type_name'))
         )
  FROM OPENJSON(@AssetTypesJson) j;

  DELETE FROM @T WHERE asset_type_id IS NULL;

  -- bật những loại trong JSON
  MERGE VendorCatalog AS tgt
  USING (SELECT @vendor_id AS vendor_id, t.asset_type_id FROM @T t) src
  ON (tgt.vendor_id = src.vendor_id AND tgt.asset_type_id = src.asset_type_id)
  WHEN MATCHED THEN UPDATE SET is_active = 1
  WHEN NOT MATCHED THEN INSERT(vendor_id, asset_type_id, is_active) VALUES(src.vendor_id, src.asset_type_id, 1);

  -- tắt những loại không còn trong JSON
  UPDATE VendorCatalog
  SET is_active = 0
  WHERE vendor_id=@vendor_id
    AND asset_type_id NOT IN (SELECT asset_type_id FROM @T);
END
GO


--Dừng hợp tác / Xóa mềm đối tác .Tắt is_active của Vendor và toàn bộ VendorCatalog.
CREATE OR ALTER PROCEDURE dbo.sp_Vendor_StopCooperation
  @vendor_id INT
AS
BEGIN
  SET NOCOUNT ON;

  UPDATE Vendor SET is_active = 0 WHERE vendor_id=@vendor_id;
  UPDATE VendorCatalog SET is_active = 0 WHERE vendor_id=@vendor_id;

  IF @@ROWCOUNT = 0
    RAISERROR(N'Không tìm thấy vendor.',16,1);
END
GO


--Nhập hàng từ đối tác (JSON) → tạo Bill + BillItem + Cập nhật Kho
CREATE OR ALTER PROCEDURE dbo.usp_ReceiveBill
  @bill_no NVARCHAR(50),
  @bill_date DATE,
  @vendor_id INT,
  @ItemsJson NVARCHAR(MAX),   -- JSON: [{"asset_type_id":1,"qty":10,"unit_cost":200000}, ...]
  @BillId INT OUTPUT
AS
BEGIN
  SET NOCOUNT ON;
  SET XACT_ABORT ON;

  -- Chống race condition khi cập nhật Warehouse
  EXEC sp_getapplock 
       @Resource = N'WAREHOUSE_LOCK',
       @LockMode = 'Exclusive',
       @LockOwner = 'Session',
       @LockTimeout = 10000;

  BEGIN TRY
    BEGIN TRAN;

      -- 1. Kiểm tra vendor
      IF NOT EXISTS (SELECT 1 FROM Vendor WHERE vendor_id=@vendor_id AND is_active=1)
      BEGIN
        RAISERROR(N'Vendor không tồn tại hoặc đã ngừng hợp tác.',16,1);
        ROLLBACK TRAN; RETURN;
      END

      -- 2. Parse JSON vào bảng tạm
      DECLARE @Items TABLE(
        asset_type_id INT NOT NULL,
        qty INT NOT NULL CHECK(qty>0),
        unit_cost DECIMAL(18,2) NOT NULL CHECK(unit_cost>=0)
      );

      INSERT INTO @Items(asset_type_id, qty, unit_cost)
      SELECT 
        TRY_CONVERT(INT, JSON_VALUE(j.value,'$.asset_type_id')),
        TRY_CONVERT(INT, JSON_VALUE(j.value,'$.qty')),
        TRY_CONVERT(DECIMAL(18,2), JSON_VALUE(j.value,'$.unit_cost'))
      FROM OPENJSON(@ItemsJson) j;

      -- validate parse
      IF EXISTS (SELECT 1 FROM @Items WHERE asset_type_id IS NULL OR qty<=0 OR unit_cost<0)
      BEGIN
        RAISERROR(N'Dữ liệu JSON không hợp lệ.',16,1);
        ROLLBACK TRAN; RETURN;
      END

      -- 3. Kiểm tra vendor có cung cấp các mặt hàng này không
      IF EXISTS (
        SELECT 1
        FROM @Items i
        LEFT JOIN VendorCatalog vc
          ON vc.vendor_id=@vendor_id AND vc.asset_type_id=i.asset_type_id AND vc.is_active=1
        WHERE vc.vendor_id IS NULL
      )
      BEGIN
        RAISERROR(N'Có mặt hàng không thuộc danh mục của Vendor.',16,1);
        ROLLBACK TRAN; RETURN;
      END

      -- 4. Thêm Bill
      INSERT INTO Bill(bill_no, bill_date, vendor_id, total_amount)
      VALUES(@bill_no, @bill_date, @vendor_id, 0);

      SET @BillId = SCOPE_IDENTITY();

      -- 5. Thêm BillItem
      INSERT INTO BillItem(bill_id, asset_type_id, qty, unit_cost)
      SELECT @BillId, asset_type_id, qty, unit_cost
      FROM @Items;

      -- 6. Cập nhật Warehouse bằng MERGE
      MERGE Warehouse AS w
      USING @Items i ON w.asset_type_id=i.asset_type_id
      WHEN MATCHED THEN
        UPDATE SET w.stock_qty = w.stock_qty + i.qty
      WHEN NOT MATCHED THEN
        INSERT(asset_type_id, stock_qty, min_stock)
        VALUES(i.asset_type_id, i.qty, 0);

      -- 7. Cập nhật tổng tiền (trigger trg_BillItem_RecalcTotal cũng đã cover)
      UPDATE b
      SET b.total_amount = (
        SELECT SUM(qty*unit_cost) FROM BillItem WHERE bill_id=@BillId
      )
      FROM Bill b WHERE b.bill_id=@BillId;

    COMMIT;

    -- 8. Trả ra kết quả cho UI: Bill, Items, Warehouse snapshot
    SELECT * FROM Bill WHERE bill_id=@BillId;
    SELECT * FROM BillItem WHERE bill_id=@BillId;
    SELECT * FROM Warehouse;

  END TRY
  BEGIN CATCH
    IF @@TRANCOUNT>0 ROLLBACK TRAN;
    DECLARE @m NVARCHAR(4000)=ERROR_MESSAGE();
    RAISERROR(@m,16,1);
  END CATCH
END
GO
-- đặt số hóa đơn auto
CREATE OR ALTER PROCEDURE dbo.sp_Vendor_ReceivePurchase
  @vendor_id INT,
  @ItemsJson NVARCHAR(MAX),   -- [{"asset_type_id":1,"qty":10,"unit_cost":200000}, ...]
  @bill_no NVARCHAR(50) = NULL,
  @bill_date DATE = NULL,
  @BillId INT OUTPUT
AS
BEGIN
  SET NOCOUNT ON;

  IF @bill_date IS NULL SET @bill_date = CAST(GETDATE() AS DATE);
  IF @bill_no IS NULL
  BEGIN
    -- auto tạo bill_no đơn giản: HDN-YYYYMMDD-<ticks mod 10000>
    SET @bill_no = N'HDN-' + CONVERT(NVARCHAR(8), @bill_date, 112) + N'-' + RIGHT(CAST(ABS(CHECKSUM(NEWID())) AS NVARCHAR(10)), 4);
  END

  EXEC dbo.usp_ReceiveBill
       @bill_no    = @bill_no,
       @bill_date  = @bill_date,
       @vendor_id  = @vendor_id,
       @ItemsJson  = @ItemsJson,
       @BillId     = @BillId OUTPUT;
END
GO



