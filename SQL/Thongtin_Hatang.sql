USE CinemaAssetDB
GO


---------ĐANG DÙNG CHO WINFORM ---------


--VIEW ĐỂ LOAD CÁC PHÒNG CHIẾU PHIM CÒN HOẠT ĐỘNG
CREATE OR ALTER VIEW dbo.vw_Auditoriums_Active AS
SELECT auditorium_id, name
FROM dbo.Auditorium
WHERE is_active = 1;
GO

--VIEW ĐỂ LOAD LOẠI THIẾT BỊ TRONG PHÒNG CHIẾU 
CREATE OR ALTER VIEW dbo.vw_AssetTypes_UI AS
SELECT 
    at.asset_type_id AS [key],   -- INT, làm ValueMember
    CASE at.name 
        WHEN N'SEAT'    THEN N'Ghế'
        WHEN N'SCREEN'  THEN N'Màn hình'
        WHEN N'SPEAKER' THEN N'Loa'
        WHEN N'AIR_CON' THEN N'Máy lạnh'
        ELSE at.name
    END AS [display]
FROM dbo.AssetType at;



-- ITVF TRẢ VỀ CÁC THIẾT BỊ THEO PHÒNG  THEO LOẠI
CREATE OR ALTER FUNCTION dbo.fn_RoomAssets
(
    @auditorium_id INT,
    @asset_type_id INT
)
RETURNS TABLE
AS
RETURN
WITH SeatType AS (
    SELECT asset_type_id AS seat_type_id
    FROM dbo.AssetType
    WHERE name = N'SEAT'
)
-- Nếu chọn SEAT -> trả ghế
SELECT 
    s.seat_id                                   AS asset_id,
    s.asset_type_id,
    ui.display                                  AS asset_type_display,
    CONCAT(s.seat_row, s.seat_pos)              AS unit_no,
    s.status,
    CAST(NULL AS DATETIME2)                     AS installed_at,
    CAST('SEAT' AS VARCHAR(5))                  AS kind
FROM dbo.Seat s
CROSS JOIN SeatType st
JOIN dbo.vw_AssetTypes_UI ui ON ui.[key] = s.asset_type_id
WHERE s.auditorium_id = @auditorium_id
  AND @asset_type_id = st.seat_type_id

UNION ALL

-- Nếu KHÔNG phải SEAT -> trả Asset theo loại
SELECT
    a.asset_id,
    a.asset_type_id,
    ui.display                                  AS asset_type_display,
    CAST(a.unit_no AS NVARCHAR(50))             AS unit_no,
    a.status,
    a.installed_at,
    CAST('ASSET' AS VARCHAR(5))                 AS kind
FROM dbo.Asset a
CROSS JOIN SeatType st
JOIN dbo.vw_AssetTypes_UI ui ON ui.[key] = a.asset_type_id
WHERE a.auditorium_id = @auditorium_id
  AND a.asset_type_id = @asset_type_id
  AND @asset_type_id <> st.seat_type_id;

--PROCEDURE HIỂN THỊ DANH SÁCH TÀI SẢN THEO LOẠI THEO PHÒNG
CREATE OR ALTER PROCEDURE dbo.sp_RoomAssets
    @auditorium_id INT,
    @asset_type_id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM dbo.fn_RoomAssets(@auditorium_id, @asset_type_id)
    ORDER BY kind, unit_no;
END
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

-- LẤY TỒN KHO THEO LOẠI 
CREATE OR ALTER PROCEDURE dbo.sp_Warehouse_GetStockByType
    @asset_type_id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ISNULL(w.stock_qty, 0) AS stock_qty
    FROM dbo.Warehouse w
    WHERE w.asset_type_id = @asset_type_id;
END
GO

-- LẤY TÊN PHÒNG CHIẾU 
CREATE OR ALTER PROCEDURE dbo.sp_Auditorium_GetName
    @auditorium_id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT name
    FROM dbo.Auditorium
    WHERE auditorium_id = @auditorium_id;
END
GO





/* ============================================================
   PROC: sp_Auditorium_AddSeats_Auto
   Mục đích:
     - Tự động thêm @qty ghế vào phòng @auditorium_id
     - Quy ước: tối đa 10 hàng (A..J), mỗi hàng 10 ghế → capacity = 100
     - Ghế mới được đánh số tuyến tính theo thứ tự A1, A2, ... B1, B2, ...
     - Trừ kho Warehouse theo loại SEAT
     - Trả về danh sách ghế của phòng sau khi thêm (vw_AuditoriumSeats)
   Ghi chú:
     - Dùng sp_getapplock để khóa logic theo phòng & kho trong phiên
     - Có kiểm tra tồn kho & sức chứa
   ============================================================ */
CREATE OR ALTER PROCEDURE dbo.sp_Auditorium_AddSeats_Auto
    @auditorium_id INT,
    @qty           INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    -- Validate input
    IF @qty IS NULL OR @qty <= 0
    BEGIN
        RAISERROR(N'Số lượng phải > 0.', 16, 1);
        RETURN;
    END

    DECLARE @seat_type_id INT = (
        SELECT TOP (1) asset_type_id
        FROM dbo.AssetType
        WHERE name = N'SEAT'
    );

    IF @seat_type_id IS NULL
    BEGIN
        RAISERROR(N'Không tìm thấy loại SEAT trong AssetType.', 16, 1);
        RETURN;
    END

    -- Cấu hình phòng
    DECLARE @MaxRows  INT = 10;   -- A..J
    DECLARE @PerRow   INT = 10;   -- 1..10
    DECLARE @Capacity INT = @MaxRows * @PerRow; -- 100

    -- Khoá logic
    DECLARE @lockAud INT, @lockWh INT;
    DECLARE @resAud NVARCHAR(64) = N'AUD_' + CONVERT(NVARCHAR(20), @auditorium_id);

    EXEC @lockAud = sys.sp_getapplock
        @Resource    = @resAud,
        @LockMode    = N'Exclusive',
        @LockOwner   = N'Session',
        @LockTimeout = 10000;

    IF @lockAud < 0
    BEGIN
        RAISERROR(N'Không lấy được khóa phòng chiếu.', 16, 1);
        RETURN;
    END

    EXEC @lockWh = sys.sp_getapplock
        @Resource    = N'WAREHOUSE_LOCK',
        @LockMode    = N'Exclusive',
        @LockOwner   = N'Session',
        @LockTimeout = 10000;

    IF @lockWh < 0
    BEGIN
        RAISERROR(N'Không lấy được khóa kho.', 16, 1);
        RETURN;
    END

    BEGIN TRY
        BEGIN TRAN;

        -- Chỗ còn trống
        DECLARE @current INT = (
            SELECT COUNT(*)
            FROM dbo.Seat
            WHERE auditorium_id = @auditorium_id
        );
        DECLARE @remain  INT = @Capacity - @current;

        IF @remain <= 0
        BEGIN
            RAISERROR(N'Phòng đã đủ %d ghế.', 16, 1, @Capacity);
            ROLLBACK TRAN; RETURN;
        END

        IF @qty > @remain
        BEGIN
            RAISERROR(N'Không đủ chỗ. Còn trống: %d ghế.', 16, 1, @remain);
            ROLLBACK TRAN; RETURN;
        END

        -- Tìm số tuyến tính lớn nhất
        DECLARE @maxLinear INT;

        ;WITH SeatsLinear AS
        (
            SELECT ((ASCII(LEFT(UPPER(LTRIM(RTRIM(seat_row))), 1)) - ASCII('A')) * @PerRow)
                   + seat_pos AS linear_no
            FROM dbo.Seat
            WHERE auditorium_id = @auditorium_id
        )
        SELECT @maxLinear = ISNULL(MAX(linear_no), 0)
        FROM SeatsLinear;

        DECLARE @startNo INT = @maxLinear + 1;
        DECLARE @endNo   INT = @startNo + @qty - 1;

        IF @endNo > @Capacity
        BEGIN
            RAISERROR(N'Không đủ chỗ đến hết phòng (tối đa %d ghế).', 16, 1, @Capacity);
            ROLLBACK TRAN; RETURN;
        END

        -- Trừ kho
        UPDATE dbo.Warehouse
        SET stock_qty = stock_qty - @qty
        WHERE asset_type_id = @seat_type_id
          AND stock_qty    >= @qty;

        IF @@ROWCOUNT = 0
        BEGIN
            DECLARE @stock INT = (
                SELECT ISNULL(stock_qty, 0)
                FROM dbo.Warehouse
                WHERE asset_type_id = @seat_type_id
            );
            RAISERROR(N'Kho không đủ ghế. Tồn hiện tại: %d', 16, 1, @stock);
            ROLLBACK TRAN; RETURN;
        END

        -- Chèn ghế mới
        ;WITH tally AS
        (
            SELECT TOP (@qty) ROW_NUMBER() OVER (ORDER BY (SELECT 1)) AS rn
            FROM sys.all_objects
        ),
        to_insert AS
        (
            SELECT
                n       = @startNo + (rn - 1),
                row_idx = ((@startNo + (rn - 1) - 1) / @PerRow),
                pos     = ((@startNo + (rn - 1) - 1) % @PerRow) + 1
            FROM tally
        )
        INSERT INTO dbo.Seat (auditorium_id, asset_type_id, seat_row, seat_pos, status)
        SELECT
            @auditorium_id,
            @seat_type_id,
            CHAR(ASCII('A') + row_idx),  -- A..J
            pos,
            'OK'
        FROM to_insert;

        COMMIT;

        -- Trả kết quả cho UI (không dùng view khác ngoài vw_AssetTypes_UI)
        SELECT 
            s.seat_id,
            s.auditorium_id,
            s.asset_type_id,
            ui.display                  AS asset_type_display,
            s.seat_row,
            s.seat_pos,
            CONCAT(s.seat_row, s.seat_pos) AS unit_no,
            s.status
        FROM dbo.Seat s
        JOIN dbo.vw_AssetTypes_UI ui ON ui.[key] = s.asset_type_id
        WHERE s.auditorium_id = @auditorium_id
        ORDER BY s.seat_row, s.seat_pos;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK;
        DECLARE @m NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@m, 16, 1);
    END CATCH
END
GO

---------ĐANG DÙNG CHO WINFORM ---------