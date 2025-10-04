USE CinemaAssetDB
GO 

------------------------------------------------------
-- 1. TẠO BẢNG TÀI KHOẢN MỚI (account - Đơn giản hóa)
------------------------------------------------------
CREATE TABLE Accounts (
    user_id INT IDENTITY PRIMARY KEY,
    FullName NVARCHAR(100) NULL, -- Tên chủ tài khoản
    username NVARCHAR(50) NOT NULL UNIQUE,
    password_hash NVARCHAR(MAX) NOT NULL, 
    role_name NVARCHAR(30) NOT NULL CHECK (role_name IN (N'QuanLy', N'NhanVien')),
    is_active BIT NOT NULL DEFAULT 1
);
GO


------------------------------------------------------
-- 2. TẠO ROLES (admin & staff)
------------------------------------------------------
    CREATE ROLE [Admin];
    CREATE ROLE [Staff];
	go

------------------------------------------------------
-- 3. QUYỀN TRUY CẬP BẢNG TÀI KHOẢN MỚI (AppUser)
------------------------------------------------------
-- admin: Toàn quyền quản lý tài khoản
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.Accounts TO [Admin];
-- NhanVien: Bị cấm hoàn toàn
DENY SELECT, INSERT, UPDATE, DELETE ON dbo.Accounts TO [Staff];
GO

------------------------------------------------------
-- 4. CẤP QUYỀN CHO ADMIN
------------------------------------------------------
-- Admin có TOÀN QUYỀN trên tất cả tables
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.Auditorium TO [Admin];
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.Seat TO [Admin];
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.Asset TO [Admin];
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.AssetType TO [Admin];
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.Vendor TO [Admin];
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.VendorCatalog TO [Admin];
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.Warehouse TO [Admin];
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.Bill TO [Admin];
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.BillItem TO [Admin];

-- Admin có quyền truy cập tất cả VIEWS
GRANT SELECT ON dbo.vw_Auditoriums_Active TO [Admin];
GRANT SELECT ON dbo.vw_AssetTypes_UI TO [Admin];

-- Admin có quyền truy cập tất cả FUNCTIONS
GRANT SELECT ON dbo.fn_RoomAssets TO [Admin];
GRANT SELECT ON dbo.fn_VendorInfo TO [Admin];
GRANT SELECT ON dbo.fn_VendorCatalog TO [Admin];
GRANT SELECT ON dbo.fn_BillSummary TO [Admin];
GRANT EXECUTE ON dbo.fn_TotalPurchase TO [Admin];
GRANT EXECUTE ON dbo.fn_GetTotalBillCount TO [Admin];
GRANT EXECUTE ON dbo.fn_IsLowStock TO [Admin];
GRANT SELECT ON dbo.fn_WarehouseReport TO [Admin];

-- Admin có quyền execute TOÀN BỘ STORED PROCEDURES
GRANT EXECUTE ON dbo.sp_RoomAssets TO [Admin];
GRANT EXECUTE ON dbo.sp_Asset_MarkBroken TO [Admin];
GRANT EXECUTE ON dbo.sp_Asset_ReplaceFromWarehouse TO [Admin];
GRANT EXECUTE ON dbo.sp_Seat_MarkBroken TO [Admin];
GRANT EXECUTE ON dbo.sp_Seat_ReplaceFromWarehouse TO [Admin];
GRANT EXECUTE ON dbo.sp_Auditorium_AddAssets TO [Admin];
GRANT EXECUTE ON dbo.sp_Auditorium_RemoveAsset TO [Admin];
GRANT EXECUTE ON dbo.sp_Seat_Delete TO [Admin];
GRANT EXECUTE ON dbo.sp_Warehouse_GetStockByType TO [Admin];
GRANT EXECUTE ON dbo.sp_Auditorium_GetName TO [Admin];
GRANT EXECUTE ON dbo.sp_Auditorium_AddSeats_Auto TO [Admin];
GRANT EXECUTE ON dbo.sp_Vendor_CreateWithCatalog TO [Admin];
GRANT EXECUTE ON dbo.sp_Vendor_UpdateFull TO [Admin];
GRANT EXECUTE ON dbo.sp_Vendor_StopCooperation TO [Admin];
GRANT EXECUTE ON dbo.usp_ReceiveBill TO [Admin];
GRANT EXECUTE ON dbo.sp_Vendor_ReceivePurchase TO [Admin];
GRANT EXECUTE ON dbo.sp_Bill_GetDetail TO [Admin];
GRANT EXECUTE ON dbo.sp_Warehouse_ReorderSuggestion TO [Admin];
GO

------------------------------------------------------
-- 5. CẤP QUYỀN CHO STAFF
------------------------------------------------------
-- Staff có quyền SELECT trên tất cả tables (xem dữ liệu)
GRANT SELECT ON dbo.Auditorium TO [Staff];
GRANT SELECT ON dbo.Seat TO [Staff];
GRANT SELECT ON dbo.Asset TO [Staff];
GRANT SELECT ON dbo.AssetType TO [Staff];
GRANT SELECT ON dbo.Vendor TO [Staff];
GRANT SELECT ON dbo.VendorCatalog TO [Staff];
GRANT SELECT ON dbo.Warehouse TO [Staff];
GRANT SELECT ON dbo.Bill TO [Staff];
GRANT SELECT ON dbo.BillItem TO [Staff];

-- Staff BỊ CẤM thêm/sửa/xóa trực tiếp tables
DENY INSERT, UPDATE, DELETE ON dbo.Auditorium TO [Staff];
DENY INSERT, UPDATE, DELETE ON dbo.Seat TO [Staff];
DENY INSERT, UPDATE, DELETE ON dbo.Asset TO [Staff];
DENY INSERT, UPDATE, DELETE ON dbo.AssetType TO [Staff];
DENY INSERT, UPDATE, DELETE ON dbo.Vendor TO [Staff];
DENY INSERT, UPDATE, DELETE ON dbo.VendorCatalog TO [Staff];
DENY INSERT, UPDATE, DELETE ON dbo.Warehouse TO [Staff];
DENY INSERT, UPDATE, DELETE ON dbo.Bill TO [Staff];
DENY INSERT, UPDATE, DELETE ON dbo.BillItem TO [Staff];

-- Staff có quyền truy cập tất cả VIEWS (giống Admin)
GRANT SELECT ON dbo.vw_Auditoriums_Active TO [Staff];
GRANT SELECT ON dbo.vw_AssetTypes_UI TO [Staff];

-- Staff có quyền truy cập tất cả FUNCTIONS (giống Admin)
GRANT SELECT ON dbo.fn_RoomAssets TO [Staff];
GRANT SELECT ON dbo.fn_VendorInfo TO [Staff];
GRANT SELECT ON dbo.fn_VendorCatalog TO [Staff];
GRANT SELECT ON dbo.fn_BillSummary TO [Staff];
GRANT EXECUTE ON dbo.fn_TotalPurchase TO [Staff];
GRANT EXECUTE ON dbo.fn_GetTotalBillCount TO [Staff];
GRANT EXECUTE ON dbo.fn_IsLowStock TO [Staff];
GRANT SELECT ON dbo.fn_WarehouseReport TO [Staff];

-- Staff chỉ được execute các procedures XEM DỮ LIỆU
GRANT EXECUTE ON dbo.sp_RoomAssets TO [Staff];
GRANT EXECUTE ON dbo.sp_Warehouse_GetStockByType TO [Staff];
GRANT EXECUTE ON dbo.sp_Auditorium_GetName TO [Staff];
GRANT EXECUTE ON dbo.sp_Bill_GetDetail TO [Staff];
GRANT EXECUTE ON dbo.sp_Warehouse_ReorderSuggestion TO [Staff];

-- Staff chỉ được ĐÁNH DẤU HỎNG thiết bị/ghế
GRANT EXECUTE ON dbo.sp_Asset_MarkBroken TO [Staff];
GRANT EXECUTE ON dbo.sp_Seat_MarkBroken TO [Staff];

-- Staff BỊ CẤM các procedures thêm/sửa/xóa/quản lý
DENY EXECUTE ON dbo.sp_Asset_ReplaceFromWarehouse TO [Staff];
DENY EXECUTE ON dbo.sp_Seat_ReplaceFromWarehouse TO [Staff];
DENY EXECUTE ON dbo.sp_Auditorium_AddAssets TO [Staff];
DENY EXECUTE ON dbo.sp_Auditorium_RemoveAsset TO [Staff];
DENY EXECUTE ON dbo.sp_Seat_Delete TO [Staff];
DENY EXECUTE ON dbo.sp_Auditorium_AddSeats_Auto TO [Staff];
DENY EXECUTE ON dbo.sp_Vendor_CreateWithCatalog TO [Staff];
DENY EXECUTE ON dbo.sp_Vendor_UpdateFull TO [Staff];
DENY EXECUTE ON dbo.sp_Vendor_StopCooperation TO [Staff];
DENY EXECUTE ON dbo.usp_ReceiveBill TO [Staff];
DENY EXECUTE ON dbo.sp_Vendor_ReceivePurchase TO [Staff];
GO


------------------------------------------------------
-- CẤP QUYỀN EXECUTE CHO QUẢN LÝ TÀI KHOẢN
------------------------------------------------------
-- Admin cần quyền tạo tài khoản
GRANT EXECUTE ON OBJECT::dbo.sp_CreateUserAccount TO [Admin];

-- Cả hai Role cần quyền thực thi hàm đăng nhập
GRANT EXECUTE ON OBJECT::dbo.fn_Login TO [Admin], [Staff];
GO









------------------------------------------------------
-- 9. TẠO PROCEDURE TẠO TÀI KHOẢN USER VÀ GÁN ROLE
------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_CreateUserAccount
    @Username NVARCHAR(50),
    @PasswordPlain NVARCHAR(MAX),  -- Mật khẩu chưa hash (dùng cho CREATE LOGIN)
    @PasswordHash NVARCHAR(MAX),   -- Mật khẩu đã hash (dùng cho INSERT Accounts)
    @FullName NVARCHAR(100),
    @RoleName NVARCHAR(30) -- 'QuanLy' hoặc 'NhanVien'
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        -- Tạo SQL Login (SỬ DỤNG @PasswordPlain)
        DECLARE @sql NVARCHAR(MAX);
        SET @sql = N'CREATE LOGIN [' + @Username + N'] WITH PASSWORD = ''' + @PasswordPlain + N'''';
        EXEC sp_executesql @sql;
        
        -- Tạo Database User
        SET @sql = N'CREATE USER [' + @Username + N'] FOR LOGIN [' + @Username + N']';
        EXEC sp_executesql @sql;
        
        -- Gán Role
        IF @RoleName = N'QuanLy'
            SET @sql = N'ALTER ROLE [Admin] ADD MEMBER [' + @Username + N']';
        ELSE IF @RoleName = N'NhanVien'
            SET @sql = N'ALTER ROLE [Staff] ADD MEMBER [' + @Username + N']';
        ELSE
        BEGIN
            RAISERROR(N'Role không hợp lệ. Chỉ chấp nhận: QuanLy, NhanVien', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        EXEC sp_executesql @sql;
        
        -- Thêm vào bảng Accounts (SỬ DỤNG @PasswordHash cho cột password_hash)
        INSERT INTO dbo.Accounts(FullName, username, password_hash, role_name, is_active)
        VALUES(@FullName, @Username, @PasswordHash, @RoleName, 1);
        
        COMMIT TRANSACTION;
        
        SELECT 'SUCCESS' AS Status, 'Tài khoản được tạo thành công' AS Message;
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END
GO

------------------------------------------------------
-- FUNCTION ĐĂNG NHẬP (fn_Login)
------------------------------------------------------
CREATE OR ALTER FUNCTION fn_Login
(
    @username NVARCHAR(50),
    @passwordHash NVARCHAR(MAX) -- Mật khẩu đã hash được gửi từ WinForms
)
RETURNS NVARCHAR(30) -- Trả về tên Role: 'QuanLy' hoặc 'NhanVien'
AS
BEGIN
    DECLARE @roleName NVARCHAR(30);
    
    -- Tìm người dùng và kiểm tra mật khẩu đã hash
    SELECT @roleName = a.role_name
    FROM dbo.Accounts a
    WHERE a.username = @username
      AND a.password_hash = @passwordHash
      AND a.is_active = 1; -- Chỉ cho phép tài khoản hoạt động đăng nhập

    -- Trả về tên Role hoặc thông báo lỗi
    RETURN ISNULL(@roleName, N'Đăng nhập thất bại');
END;
GO
