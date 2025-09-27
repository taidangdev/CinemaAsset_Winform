-- Auditorium : lưu thông tin phòng chiếu
CREATE TABLE Auditorium (
  auditorium_id INT IDENTITY PRIMARY KEY,
  name          NVARCHAR(100) NOT NULL,
  is_active     BIT NOT NULL DEFAULT 1,
  created_at    DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);
CREATE UNIQUE INDEX UQ_Auditorium_Name ON Auditorium(name);


-- Seat: lưu thông tin của ghế trong từng phòng chiếu
CREATE TABLE Seat (
  seat_id       INT IDENTITY PRIMARY KEY,
  auditorium_id INT NOT NULL REFERENCES Auditorium(auditorium_id),
  asset_type_id INT NOT NULL REFERENCES AssetType(asset_type_id), -- FK 1: loại ghế
  seat_row      NVARCHAR(10) NOT NULL,      -- A, B, C...
  seat_pos      INT NOT NULL CHECK (seat_pos >= 1), 
  status        VARCHAR(15) NOT NULL DEFAULT 'OK'
                 CHECK (status IN ('OK','BROKEN')),

  -- Không trùng vị trí ghế trong cùng phòng
  CONSTRAINT UQ_Seat_RowPos_InRoom UNIQUE (auditorium_id, seat_row, seat_pos)
);
CREATE INDEX IX_Seat_Auditorium ON Seat(auditorium_id, seat_row, seat_pos);


-- Vendor : lưu thông tin đối tác
CREATE TABLE Vendor (
  vendor_id  INT IDENTITY PRIMARY KEY,
  name       NVARCHAR(200) NOT NULL,
  phone      NVARCHAR(30)  NULL,
  email      NVARCHAR(120) NULL,
  address    NVARCHAR(300) NULL,
  is_active  BIT NOT NULL DEFAULT 1
);
CREATE UNIQUE INDEX UQ_Vendor_Name ON Vendor(name);


-- AssetType (mỗi loại gắn đúng 1 vendor)
CREATE TABLE AssetType (
  asset_type_id INT IDENTITY PRIMARY KEY,
  name          NVARCHAR(100) NOT NULL,     -- SCREEN/SPEAKER/AIR_CON/SEAT/...
);
CREATE UNIQUE INDEX UQ_AssetType_Name ON AssetType(name);

/* ===== VendorCatalog: vendor nào cung cấp loại hàng nào ===== */
CREATE TABLE VendorCatalog (
  vendor_id     INT NOT NULL REFERENCES Vendor(vendor_id),
  asset_type_id INT NOT NULL REFERENCES AssetType(asset_type_id),
  is_active     BIT NOT NULL DEFAULT 1,
  CONSTRAINT PK_VendorCatalog PRIMARY KEY (vendor_id, asset_type_id)
);


-- Asset (thiết bị lắp đặt)
CREATE TABLE Asset (
  asset_id       INT IDENTITY PRIMARY KEY,
  asset_type_id  INT NOT NULL REFERENCES AssetType(asset_type_id),
  auditorium_id  INT NOT NULL REFERENCES Auditorium(auditorium_id),
  unit_no        INT NOT NULL CHECK (unit_no >= 1),
  status         VARCHAR(20) NOT NULL DEFAULT 'OK'
                 CHECK (status IN ('OK','BROKEN')),
  installed_at   DATETIME2 NULL,
  CONSTRAINT UQ_Asset_InRoom UNIQUE (auditorium_id, asset_type_id, unit_no)
);

-- Warehouse (mỗi loại 1 dòng tồn)
CREATE TABLE Warehouse (
  asset_type_id   INT PRIMARY KEY REFERENCES AssetType(asset_type_id),
  stock_qty       INT NOT NULL DEFAULT 0 CHECK (stock_qty >= 0),
  min_stock       INT NOT NULL DEFAULT 0 CHECK (min_stock >= 0)
);

-- Bill (hóa đơn nhập)
CREATE TABLE Bill (
  bill_id      INT IDENTITY PRIMARY KEY,
  bill_no      NVARCHAR(50) NOT NULL,
  bill_date    DATE NOT NULL,
  vendor_id    INT NOT NULL REFERENCES Vendor(vendor_id),
  total_amount DECIMAL(18,2) NULL,
  created_at   DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);
CREATE UNIQUE INDEX UQ_Bill_No ON Bill(bill_no);

CREATE TABLE BillItem (
  bill_item_id  INT IDENTITY PRIMARY KEY,
  bill_id       INT NOT NULL REFERENCES Bill(bill_id),
  asset_type_id INT NOT NULL REFERENCES AssetType(asset_type_id),
  qty           INT NOT NULL CHECK (qty > 0),
  unit_cost     DECIMAL(18,2) NOT NULL CHECK (unit_cost >= 0)
);
CREATE INDEX IX_BillItem_Bill ON BillItem(bill_id);
GO

-- Nhanh hơn khi nhìn chi tiết bill
CREATE INDEX IX_BillItem_Bill_AType ON BillItem(bill_id, asset_type_id);

-- Kho tra cứu nhanh hàng sắp dưới định mức
CREATE INDEX IX_Warehouse_MinStock ON Warehouse(asset_type_id) INCLUDE (stock_qty, min_stock);

-- Tăng tốc duyệt asset trong phòng
CREATE INDEX IX_Asset_AudType ON Asset(auditorium_id, asset_type_id, unit_no);






/* ===== TRIGGER ===== */
-- 1) BillItem phải là mặt hàng do vendor của Bill cung cấp (kiểm theo VendorCatalog)
CREATE OR ALTER TRIGGER trg_BillItem_ValidateVendorCatalog
ON BillItem
AFTER INSERT, UPDATE
AS
BEGIN
  SET NOCOUNT ON;

  IF EXISTS (
    SELECT 1
    FROM inserted i
    JOIN Bill b ON b.bill_id = i.bill_id
    LEFT JOIN VendorCatalog vc
      ON vc.vendor_id = b.vendor_id
     AND vc.asset_type_id = i.asset_type_id
     AND vc.is_active = 1
    WHERE vc.vendor_id IS NULL
  )
  BEGIN
    RAISERROR (N'Vendor trong Bill không cung cấp loại hàng của BillItem.', 16, 1);
    ROLLBACK TRANSACTION;
    RETURN;
  END
END
GO


-- tự động tính tổng tiền trong BillItem và đồng bộ lên Bill


CREATE OR ALTER TRIGGER trg_BillItem_RecalcTotal
ON BillItem
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
  SET NOCOUNT ON;

  ;WITH affected AS (
    SELECT bill_id FROM inserted
    UNION
    SELECT bill_id FROM deleted
  )
  UPDATE b
  SET b.total_amount = ISNULL(x.sum_amount, 0)
  FROM Bill b
  JOIN affected a ON a.bill_id = b.bill_id
  OUTER APPLY (
     SELECT SUM(qty * unit_cost) AS sum_amount
     FROM BillItem bi
     WHERE bi.bill_id = b.bill_id
  ) x;
END
GO


-- INSERT: normalize seat_row
CREATE OR ALTER TRIGGER trg_Seat_Normalize_Insert
ON Seat
INSTEAD OF INSERT
AS
BEGIN
  SET NOCOUNT ON;

  INSERT INTO Seat(auditorium_id, asset_type_id, seat_row, seat_pos, status)
  SELECT  i.auditorium_id,
          i.asset_type_id,
          UPPER(LTRIM(RTRIM(i.seat_row))) AS seat_row,
          i.seat_pos,
          i.status
  FROM inserted i;
END
GO

-- UPDATE: normalize seat_row nếu có cập nhật
CREATE OR ALTER TRIGGER trg_Seat_Normalize_Update
ON Seat
INSTEAD OF UPDATE
AS
BEGIN
  SET NOCOUNT ON;

  -- Cập nhật theo dữ liệu đưa vào; luôn chuẩn hóa seat_row
  UPDATE s
  SET s.auditorium_id = ISNULL(i.auditorium_id, s.auditorium_id),
      s.asset_type_id = ISNULL(i.asset_type_id, s.asset_type_id),
      s.seat_row      = UPPER(LTRIM(RTRIM(ISNULL(i.seat_row, s.seat_row)))),
      s.seat_pos      = ISNULL(i.seat_pos, s.seat_pos),
      s.status        = ISNULL(i.status, s.status)
  FROM Seat s
  JOIN inserted i ON i.seat_id = s.seat_id;
END
GO





---------INSERT DATA ----------
/* ========= AUDITORIUM ========= */
INSERT INTO Auditorium(name) VALUES
 (N'Room Alpha'),
 (N'Room Beta'),
 (N'Room Gamma');

/* ========= ASSET TYPE =========
   Gợi ý: tách loại ghế thành 2 loại để Seat tham chiếu (SEAT_STD / SEAT_VIP) */
INSERT INTO AssetType(name) VALUES
 (N'SEAT'),
 (N'SCREEN'),
 (N'SPEAKER'),
 (N'AIR_CON');

/* Lấy id nhanh (chỉ để tham khảo) */
SELECT * FROM AssetType;

/* ========= VENDOR ========= */
INSERT INTO Vendor(name, phone, email, address) VALUES
 (N'SeatingPro',    N'0901-234-567', N'sales@seatingpro.com', N'123 Lê Lợi, Q1'),
 (N'SilverScreen',  N'0902-345-678', N'hello@silverscreen.vn', N'45 Hai Bà Trưng, Q1'),
 (N'DolbySound Co', N'0903-456-789', N'contact@dolbysound.com', N'88 Pasteur, Q3'),
 (N'CoolAir VN',    N'0904-567-890', N'support@coolair.vn', N'12 Nguyễn Huệ, Q1');

SELECT * FROM Vendor;
/* ========= VENDOR CATALOG =========*/
-- SeatingPro: SEAT + SCREEN
INSERT INTO VendorCatalog(vendor_id, asset_type_id, is_active)
SELECT v.vendor_id, a.asset_type_id, 1
FROM Vendor v
JOIN AssetType a ON a.name IN (N'SEAT', N'SCREEN')
WHERE v.name = N'SeatingPro';

-- SilverScreen: SCREEN
INSERT INTO VendorCatalog(vendor_id, asset_type_id, is_active)
SELECT v.vendor_id, a.asset_type_id, 1
FROM Vendor v
JOIN AssetType a ON a.name = N'SCREEN'
WHERE v.name = N'SilverScreen';

-- DolbySound Co: SPEAKER
INSERT INTO VendorCatalog(vendor_id, asset_type_id, is_active)
SELECT v.vendor_id, a.asset_type_id, 1
FROM Vendor v
JOIN AssetType a ON a.name = N'SPEAKER'
WHERE v.name = N'DolbySound Co';

-- CoolAir VN: AIR_CON
INSERT INTO VendorCatalog(vendor_id, asset_type_id, is_active)
SELECT v.vendor_id, a.asset_type_id, 1
FROM Vendor v
JOIN AssetType a ON a.name = N'AIR_CON'
WHERE v.name = N'CoolAir VN';

SELECT * FROM VendorCatalog ORDER BY vendor_id, asset_type_id;


/* ========= WAREHOUSE =========
   Khởi tạo tồn kho và min_stock */

INSERT INTO Warehouse(asset_type_id, stock_qty, min_stock)
SELECT asset_type_id,
       CASE name
            WHEN N'SEAT'   THEN 100
            WHEN N'SCREEN' THEN 5
            WHEN N'SPEAKER'THEN 10
            WHEN N'AIR_CON'THEN 3
       END,
       CASE name
            WHEN N'SEAT'   THEN 20
            WHEN N'SCREEN' THEN 1
            WHEN N'SPEAKER'THEN 4
            WHEN N'AIR_CON'THEN 1
       END
FROM AssetType;

SELECT * FROM Warehouse ORDER BY asset_type_id;


/* ========= SEAT =========
   Tạo một ít ghế cho Room Alpha (chuẩn hoá seat_row nhờ trigger) */
DECLARE @SEAT INT = (SELECT asset_type_id FROM AssetType WHERE name = N'SEAT');
DECLARE @Alpha INT = (SELECT auditorium_id FROM Auditorium WHERE name = N'Room Alpha');

-- Test seat_row: trigger sẽ chuẩn hóa thành "A", "B"
INSERT INTO Seat(auditorium_id, asset_type_id, seat_row, seat_pos, status) VALUES
(@Alpha, @SEAT, N'a ', 1, 'OK'),
(@Alpha, @SEAT, N'A',  2, 'OK'),
(@Alpha, @SEAT, N' b', 1, 'OK'),
(@Alpha, @SEAT, N'B',  2, 'OK');

SELECT * FROM Seat WHERE auditorium_id =(SELECT auditorium_id FROM Auditorium WHERE name = N'Room Alpha') ORDER BY seat_row, seat_pos;





