use CinemaAssetDB
go


-- Function duy nhất thay thế cho vw_WarehouseStatus và fn_WarehouseFilter
CREATE OR ALTER FUNCTION dbo.fn_WarehouseReport
(
  @only_low BIT        = NULL,          -- 1: chỉ LOW, 0: tất cả
  @asset_type_id INT   = NULL,          -- lọc theo loại
  @name_like NVARCHAR(100) = NULL       -- tìm theo tên (LIKE)
)
RETURNS TABLE
AS
RETURN
(
  SELECT
    w.asset_type_id,
    at.name AS asset_type_name,
    w.stock_qty,
    w.min_stock,
    
    -- Tính toán stock_state (sử dụng fn_IsLowStock như cũ)
    CASE 
      WHEN dbo.fn_IsLowStock(w.asset_type_id) = 1 THEN N'LOW' 
      ELSE N'OK' 
    END AS stock_state,
    
    -- Tính thiếu
    CASE 
      WHEN w.stock_qty < w.min_stock THEN (w.min_stock - w.stock_qty) 
      ELSE 0 
    END AS shortage,
    
    -- Tính % đạt so với min
    CAST(CASE 
          WHEN w.min_stock = 0 THEN 100.0
          ELSE 100.0 * w.stock_qty / NULLIF(w.min_stock,0)
        END AS DECIMAL(20,2)) AS pct_of_min
  FROM dbo.Warehouse w
  JOIN dbo.AssetType at ON at.asset_type_id = w.asset_type_id
  
  -- ÁP DỤNG LỌC (Logic từ fn_WarehouseFilter)
  WHERE (@only_low IS NULL OR 
         (@only_low = 1 AND (CASE WHEN dbo.fn_IsLowStock(w.asset_type_id) = 1 THEN N'LOW' ELSE N'OK' END) = N'LOW') OR 
         (@only_low = 0))
    AND (@asset_type_id IS NULL OR w.asset_type_id = @asset_type_id)
    AND (@name_like IS NULL OR at.name LIKE N'%' + @name_like + N'%')
);
GO




--Procedure: Đề xuất nhập kho
CREATE OR ALTER PROCEDURE dbo.sp_Warehouse_ReorderSuggestion
AS
BEGIN
    SET NOCOUNT ON;

    /*
      Logic:
      - Lấy tất cả mặt hàng trong Warehouse có stock_qty < min_stock
      - Tính thiếu = min_stock - stock_qty
      - Gợi ý số lượng nhập = thiếu (có thể cộng thêm % buffer nếu bạn muốn)
      - Lấy danh sách vendor có thể cung cấp từ VendorCatalog
    */

    SELECT
        at.asset_type_id,
        at.name AS asset_type_name,
        w.stock_qty,
        w.min_stock,
        (w.min_stock - w.stock_qty) AS shortage,
        v.vendor_id,
        v.name AS vendor_name
    FROM Warehouse w
    JOIN AssetType at ON at.asset_type_id = w.asset_type_id
    LEFT JOIN VendorCatalog vc 
           ON vc.asset_type_id = w.asset_type_id AND vc.is_active = 1
    LEFT JOIN Vendor v 
           ON v.vendor_id = vc.vendor_id AND v.is_active = 1
    WHERE w.stock_qty < w.min_stock
    ORDER BY at.name, v.name;
END
GO



