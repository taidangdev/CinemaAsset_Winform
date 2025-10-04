use CinemaAssetDB
go

--1 TVF: lọc mềm theo khoảng ngày & vendor (dùng cho bộ lọc UI)
CREATE OR ALTER FUNCTION dbo.fn_BillSummary
(
  @date_from DATE = NULL,
  @date_to   DATE = NULL,
  @vendor_id INT  = NULL
)
RETURNS TABLE
AS
RETURN
(
  SELECT b.bill_id, b.bill_no, b.bill_date,
         v.name AS vendor_name, b.total_amount, b.created_at
  FROM Bill b
  JOIN Vendor v ON v.vendor_id = b.vendor_id
  WHERE (@date_from IS NULL OR b.bill_date >= @date_from)
    AND (@date_to   IS NULL OR b.bill_date <= @date_to)
    AND (@vendor_id IS NULL OR b.vendor_id = @vendor_id)
);
GO

-- 2 TVF: tổng số tiền đã nhập trong kỳ (để đổ vào textbox Total)

CREATE OR ALTER FUNCTION dbo.fn_TotalPurchase
(
  @date_from DATE = NULL,
  @date_to   DATE = NULL,
  @vendor_id INT  = NULL
)
RETURNS TABLE
AS
RETURN
(
  SELECT SUM(b.total_amount) AS total_spent
  FROM Bill b
  WHERE (@date_from IS NULL OR b.bill_date >= @date_from)
    AND (@date_to   IS NULL OR b.bill_date <= @date_to)
    AND (@vendor_id IS NULL OR b.vendor_id = @vendor_id)
);
GO

--3) (tiện cho WinForms) Proc trả 2 result sets: danh sách + tổng
CREATE OR ALTER PROCEDURE dbo.sp_PurchaseStats_ListAndTotal
  @date_from DATE = NULL,
  @date_to   DATE = NULL,
  @vendor_id INT  = NULL
AS
BEGIN
  SET NOCOUNT ON;
  -- RS1: danh sách hóa đơn
  SELECT * FROM dbo.fn_BillSummary(@date_from, @date_to, @vendor_id) ORDER BY created_at DESC;
  -- RS2: tổng chi
  SELECT total_spent FROM dbo.fn_TotalPurchase(@date_from, @date_to, @vendor_id);
END
GO


--4) (tiện cho nút) Proc trả chi tiết 1 bill (2 RS: header + items) 
--Trong tab “Thống kê nhập hàng” bạn có DataGridView hiển thị danh sách Bill (bill_no, ngày, vendor, total_amount).

--Khi người dùng bấm nút “Chi tiết” ở cuối mỗi dòng, bạn cần hiện thêm thông tin:

--Thông tin hóa đơn (header: số, ngày, vendor, tổng tiền).

--Danh sách mặt hàng trong hóa đơn đó (items: loại hàng, số lượng, đơn giá, thành tiền).
CREATE OR ALTER PROCEDURE dbo.sp_Bill_GetDetail
  @bill_id INT
AS
BEGIN
  SET NOCOUNT ON;
  -- RS1: header
  SELECT b.bill_id, b.bill_no, b.bill_date, v.name AS vendor_name, b.total_amount, b.created_at
  FROM Bill b JOIN Vendor v ON v.vendor_id = b.vendor_id
  WHERE b.bill_id = @bill_id;
  -- RS2: items
  SELECT bi.bill_item_id, at.name AS asset_type_name, bi.qty, bi.unit_cost, (bi.qty*bi.unit_cost) AS line_total
  FROM BillItem bi JOIN AssetType at ON at.asset_type_id = bi.asset_type_id
  WHERE bi.bill_id = @bill_id
  ORDER BY bi.bill_item_id;
END
GO

-- tính tổng số hóa đơn theo lọc
CREATE OR ALTER FUNCTION dbo.fn_GetTotalBillCount
(
  @date_from DATE = NULL,
  @date_to   DATE = NULL,
  @vendor_id INT  = NULL
)
RETURNS INT
AS
BEGIN
    DECLARE @Count INT;
    
    SELECT @Count = COUNT(b.bill_id)
    FROM dbo.Bill b
    WHERE (@date_from IS NULL OR b.bill_date >= @date_from)
      AND (@date_to   IS NULL OR b.bill_date <= @date_to)
      AND (@vendor_id IS NULL OR b.vendor_id = @vendor_id);

    RETURN ISNULL(@Count, 0);
END
GO


--Kiểm tra trạng thái tồn kho thấp (LOW) cho một loại hàng.
CREATE OR ALTER FUNCTION dbo.fn_IsLowStock(@asset_type_id INT)
RETURNS BIT
AS
BEGIN
    DECLARE @IsLow BIT;

    SELECT @IsLow = CASE 
                        WHEN w.stock_qty < w.min_stock THEN 1 
                        ELSE 0 
                    END
    FROM dbo.Warehouse w
    WHERE w.asset_type_id = @asset_type_id;

    RETURN ISNULL(@IsLow, 0);
END
GO


