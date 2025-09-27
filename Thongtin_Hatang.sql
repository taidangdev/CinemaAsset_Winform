USE CinemaAssetDB
GO

CREATE OR ALTER VIEW dbo.vw_Auditoriums_Active AS
SELECT auditorium_id, name
FROM dbo.Auditorium
WHERE is_active = 1;
GO

select * from dbo.vw_Auditoriums_Active

-- view hiển thị thiết bị trong phòng chiếu phim 
CREATE OR ALTER VIEW dbo.vw_AuditoriumAssets AS
SELECT
  au.auditorium_id,
  au.name              AS auditorium_name,
  a.asset_id,
  at.asset_type_id,
  at.name              AS asset_type_name,
  a.unit_no,
  a.status,
  a.installed_at
FROM Asset a
JOIN Auditorium au ON au.auditorium_id = a.auditorium_id
JOIN AssetType  at ON at.asset_type_id  = a.asset_type_id;
GO

-- view hiển thị chi tiết ghế trong từng phòng chiếu phim
CREATE OR ALTER VIEW dbo.vw_AuditoriumSeats AS
SELECT
  au.auditorium_id,
  au.name          AS auditorium_name,
  s.seat_id,
  s.asset_type_id,                 -- chính là loại SEAT
  at.name          AS asset_type_name,
  s.seat_row,
  s.seat_pos,
  s.status
FROM Seat s
JOIN Auditorium au ON au.auditorium_id = s.auditorium_id
JOIN AssetType  at ON at.asset_type_id  = s.asset_type_id;
GO

-- Procedure cập nhật trạng thái (Asset)
-- Nhân viên báo hỏng: OK -> BROKEN
CREATE OR ALTER PROCEDURE dbo.sp_Asset_MarkBroken
  @asset_id INT
AS
BEGIN
  SET NOCOUNT ON;

  UPDATE Asset
  SET status = 'BROKEN'
  WHERE asset_id = @asset_id AND status = 'OK';

  IF @@ROWCOUNT = 0
    RAISERROR (N'Không thể đánh dấu hỏng (không tồn tại hoặc không ở trạng thái OK).', 16, 1);

  SELECT asset_id, status FROM Asset WHERE asset_id=@asset_id;
END
GO

-- Admin thay thế: BROKEN -> OK ( TRỪ KHO 1 ĐÚNG LOẠI )
CREATE OR ALTER PROCEDURE dbo.sp_Asset_ReplaceFromWarehouse
  @asset_id INT
AS
BEGIN
  SET NOCOUNT ON;
  SET XACT_ABORT ON;

  -- khóa logic kho để an toàn khi trừ tồn
  EXEC sp_getapplock @Resource=N'WAREHOUSE_LOCK', @LockMode='Exclusive', @LockOwner='Session', @LockTimeout=10000;

  DECLARE @atype INT;

  BEGIN TRY
    BEGIN TRAN;

      SELECT @atype = asset_type_id
      FROM Asset
      WHERE asset_id=@asset_id AND status='BROKEN';

      IF @atype IS NULL
      BEGIN
        RAISERROR(N'Asset không ở trạng thái BROKEN hoặc không tồn tại.',16,1);
        ROLLBACK TRAN; RETURN;
      END

      IF NOT EXISTS (SELECT 1 FROM Warehouse WHERE asset_type_id=@atype AND stock_qty>=1)
      BEGIN
        RAISERROR(N'Kho không đủ để thay thế.',16,1);
        ROLLBACK TRAN; RETURN;
      END

      UPDATE Warehouse SET stock_qty = stock_qty - 1 WHERE asset_type_id=@atype;

      UPDATE Asset
      SET status='OK', installed_at=SYSUTCDATETIME()
      WHERE asset_id=@asset_id;

    COMMIT;

    SELECT asset_id=@asset_id, new_status='OK';
    SELECT * FROM Warehouse WHERE asset_type_id=@atype;
  END TRY
  BEGIN CATCH
    IF @@TRANCOUNT>0 ROLLBACK TRAN;
    DECLARE @m NVARCHAR(4000)=ERROR_MESSAGE(); RAISERROR(@m,16,1);
  END CATCH
END
GO

-- Procedure cập nhật trạng thái (Seat)

-- Báo hỏng ghế: OK → BROKEN
CREATE OR ALTER PROCEDURE dbo.sp_Seat_MarkBroken
  @seat_id INT
AS
BEGIN
  SET NOCOUNT ON;

  UPDATE Seat
  SET status = 'BROKEN'
  WHERE seat_id = @seat_id AND status = 'OK';

  IF @@ROWCOUNT = 0
    RAISERROR (N'Không thể đánh dấu hỏng (ghế không tồn tại hoặc không ở trạng thái OK).', 16, 1);

  SELECT seat_id, status FROM Seat WHERE seat_id=@seat_id;
END
GO

--Thay thế ghế từ kho: BROKEN → OK (trừ kho 1 chiếc SEAT)
CREATE OR ALTER PROCEDURE dbo.sp_Seat_ReplaceFromWarehouse
  @seat_id INT
AS
BEGIN
  SET NOCOUNT ON;
  SET XACT_ABORT ON;

  EXEC sp_getapplock @Resource=N'WAREHOUSE_LOCK',
                     @LockMode='Exclusive', @LockOwner='Session', @LockTimeout=10000;

  DECLARE @atype INT;

  BEGIN TRY
    BEGIN TRAN;

      -- chỉ cho phép khi đang BROKEN
      SELECT @atype = asset_type_id
      FROM Seat
      WHERE seat_id = @seat_id AND status='BROKEN';

      IF @atype IS NULL
      BEGIN
        RAISERROR(N'Ghế không ở trạng thái BROKEN hoặc không tồn tại.',16,1);
        ROLLBACK TRAN; RETURN;
      END

      -- kiểm tra kho (loại SEAT)
      IF NOT EXISTS (SELECT 1 FROM Warehouse WHERE asset_type_id=@atype AND stock_qty>=1)
      BEGIN
        RAISERROR(N'Kho không đủ ghế để thay thế.',16,1);
        ROLLBACK TRAN; RETURN;
      END

      -- trừ kho 1
      UPDATE Warehouse SET stock_qty = stock_qty - 1 WHERE asset_type_id=@atype;

      -- thay thế thành OK
      UPDATE Seat SET status='OK' WHERE seat_id=@seat_id;

    COMMIT;

    SELECT seat_id=@seat_id, new_status='OK';
    SELECT * FROM Warehouse WHERE asset_type_id=@atype;
  END TRY
  BEGIN CATCH
    IF @@TRANCOUNT>0 ROLLBACK;
    DECLARE @m NVARCHAR(4000)=ERROR_MESSAGE(); RAISERROR(@m,16,1);
  END CATCH
END
GO


-- lắp thêm thiết bị ( trừ kho theo số lượng, thêm các dòng vào asset với unit no nối tiếp trong phòng)
CREATE OR ALTER PROCEDURE dbo.sp_Auditorium_AddAssets
  @auditorium_id INT,
  @ItemsJson NVARCHAR(MAX)   -- [{"asset_type_id":2,"qty":2}, ...]
AS
BEGIN
  SET NOCOUNT ON;
  SET XACT_ABORT ON;

  EXEC sp_getapplock @Resource=N'WAREHOUSE_LOCK', @LockMode='Exclusive', @LockOwner='Session', @LockTimeout=10000;

  DECLARE @Items TABLE(asset_type_id INT PRIMARY KEY, qty INT CHECK(qty>0));

  INSERT INTO @Items(asset_type_id, qty)
  SELECT TRY_CONVERT(INT, JSON_VALUE(j.value,'$.asset_type_id')),
         TRY_CONVERT(INT, JSON_VALUE(j.value,'$.qty'))
  FROM OPENJSON(@ItemsJson) j;

  DELETE FROM @Items WHERE asset_type_id IS NULL OR qty IS NULL OR qty<=0;

  IF NOT EXISTS (SELECT 1 FROM @Items)
  BEGIN
    RAISERROR(N'Danh sách thiết bị không hợp lệ.',16,1); RETURN;
  END

  -- đủ kho?
  IF EXISTS (
    SELECT 1 FROM @Items i
    LEFT JOIN Warehouse w ON w.asset_type_id=i.asset_type_id
    WHERE ISNULL(w.stock_qty,0) < i.qty
  )
  BEGIN
    RAISERROR(N'Kho không đủ để lắp thêm thiết bị.',16,1); RETURN;
  END

  BEGIN TRY
    BEGIN TRAN;

      -- trừ kho
      UPDATE w SET w.stock_qty = w.stock_qty - i.qty
      FROM Warehouse w JOIN @Items i ON i.asset_type_id=w.asset_type_id;

      -- chèn Asset (unit_no nối tiếp từng loại)
      ;WITH bases AS (
        SELECT i.asset_type_id,
               ISNULL(MAX(a.unit_no),0) AS base_no,
               i.qty
        FROM @Items i
        LEFT JOIN Asset a
          ON a.auditorium_id=@auditorium_id AND a.asset_type_id=i.asset_type_id
        GROUP BY i.asset_type_id, i.qty
      ),
      exp AS (
        SELECT b.asset_type_id,
               ROW_NUMBER() OVER (PARTITION BY b.asset_type_id ORDER BY (SELECT 1)) + b.base_no AS unit_no
        FROM bases b
        CROSS APPLY (SELECT TOP (b.qty) 1 z FROM sys.all_objects) t
      )
      INSERT INTO Asset(asset_type_id, auditorium_id, unit_no, status, installed_at)
      SELECT e.asset_type_id, @auditorium_id, e.unit_no, 'OK', SYSUTCDATETIME()
      FROM exp e;

    COMMIT;

    SELECT * FROM dbo.vw_AuditoriumAssets WHERE auditorium_id=@auditorium_id ORDER BY asset_type_name, unit_no;  -- tiện để reload lưới
  END TRY
  BEGIN CATCH
    IF @@TRANCOUNT>0 ROLLBACK;
    DECLARE @m NVARCHAR(4000)=ERROR_MESSAGE(); RAISERROR(@m,16,1);
  END CATCH
END
GO


--(Tuỳ chọn) Xóa thiết bị (trả kho nếu đang OK; nếu BROKEN thì không trả)

CREATE OR ALTER PROCEDURE dbo.sp_Auditorium_RemoveAsset
  @asset_id INT
AS
BEGIN
  SET NOCOUNT ON;
  SET XACT_ABORT ON;

  DECLARE @atype INT, @st VARCHAR(20);

  SELECT @atype=asset_type_id, @st=status FROM Asset WHERE asset_id=@asset_id;
  IF @atype IS NULL BEGIN RAISERROR(N'Asset không tồn tại.',16,1); RETURN; END

  IF @st='OK'
    EXEC sp_getapplock @Resource=N'WAREHOUSE_LOCK', @LockMode='Exclusive', @LockOwner='Session', @LockTimeout=10000;

  BEGIN TRY
    BEGIN TRAN;

      IF @st='OK'
      BEGIN
        MERGE Warehouse AS w
        USING (SELECT @atype AS asset_type_id, 1 AS cnt) s
        ON w.asset_type_id = s.asset_type_id
        WHEN MATCHED THEN UPDATE SET w.stock_qty = w.stock_qty + s.cnt
        WHEN NOT MATCHED THEN INSERT(asset_type_id, stock_qty, min_stock) VALUES(s.asset_type_id, s.cnt, 0);
      END

      DELETE FROM Asset WHERE asset_id=@asset_id;

    COMMIT;

    SELECT 'REMOVED' AS status, @asset_id AS asset_id, returned_to_warehouse = CASE WHEN @st='OK' THEN 1 ELSE 0 END;
  END TRY
  BEGIN CATCH
    IF @@TRANCOUNT>0 ROLLBACK;
    DECLARE @m NVARCHAR(4000)=ERROR_MESSAGE(); RAISERROR(@m,16,1);
  END CATCH
END
GO


-- proc lấy danh sách của ghế hoặc thiết bị khác 
CREATE OR ALTER PROCEDURE dbo.sp_GetAuditoriumAssets
  @auditorium_id INT,
  @asset_type_id INT  -- truyền id của AssetType; truyền id của 'SEAT' nếu cần ghế
AS
BEGIN
  SET NOCOUNT ON;

  IF @asset_type_id = (SELECT asset_type_id FROM AssetType WHERE name = N'SEAT')
  BEGIN
    SELECT 
      s.seat_id AS asset_id,
      at.name   AS asset_type_name,
      CONCAT(s.seat_row, s.seat_pos) AS unit_no,
      s.status,
      CAST(NULL AS DATETIME2) AS installed_at,
      'SEAT' AS asset_type
    FROM Seat s
    JOIN AssetType at ON at.asset_type_id = s.asset_type_id
    WHERE s.auditorium_id = @auditorium_id
    ORDER BY s.seat_row, s.seat_pos;
    RETURN;
  END

  SELECT 
    a.asset_id,
    at.name AS asset_type_name,
    a.unit_no,
    a.status,
    a.installed_at,
    'ASSET' AS asset_type
  FROM Asset a
  JOIN AssetType at ON at.asset_type_id = a.asset_type_id
  WHERE a.auditorium_id = @auditorium_id 
    AND a.asset_type_id = @asset_type_id
  ORDER BY a.unit_no;
END

