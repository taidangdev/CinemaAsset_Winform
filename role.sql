use CinemaAssetDB
go 



CREATE ROLE QuanLy;
GO
CREATE ROLE NhanVien;




-- Phân quyền
GO
CREATE ROLE QuanLy;
GO
CREATE ROLE NhanVien;

-- Xem thành viên của 1 ROLE
EXEC sp_helprolemember N'QuanLy'


-- Gán quyền cho ROLE QuanLy
GRANT SELECT,INSERT,UPDATE,DELETE ON dbo.MOVIE TO QuanLy;
GRANT SELECT,INSERT,UPDATE,DELETE ON dbo.Genre TO QuanLy;
GRANT SELECT ON dbo.vw_AllMovies TO QuanLy;
GRANT SELECT ON dbo.vw_StaffMovies TO QuanLy;
GRANT SELECT ON dbo.fn_SearchMovieByRoleStatus TO QuanLy;
GRANT EXECUTE ON OBJECT::sp_AddMovieWithBill TO QuanLy;
GRANT EXECUTE ON OBJECT::fn_SearchMovieByRoleStatus TO QuanLy;
GRANT EXECUTE ON OBJECT::sp_DeleteMovieWithTransaction TO QuanLy;
GRANT EXECUTE ON OBJECT::sp_FilterShowTimes TO QuanLy;


GRANT SELECT ON dbo.ACCOUNTS TO QuanLy;
-- Gán quyền cho ROLE NhanVien
GRANT SELECT ON dbo.MOVIE TO NhanVien;
DENY SELECT, INSERT, UPDATE, DELETE ON dbo.MOVIE TO NhanVien;
DENY SELECT, INSERT, UPDATE, DELETE ON dbo.ACCOUNTS TO NhanVien;
GRANT SELECT ON dbo.fn_SearchMovieByRoleStatus TO NhanVien;
GRANT EXECUTE ON OBJECT::sp_FilterShowTimes TO NhanVien;


-- Procedure tạo LOGIN + USER + gán ROLE
GO
CREATE OR ALTER PROCEDURE sp_CreateSQLLogin
    @Username NVARCHAR(50),
    @Password NVARCHAR(MAX),
    @Role NVARCHAR(30)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        DECLARE @sqlString NVARCHAR(MAX);

        -- Kiểm tra login đã tồn tại
        IF EXISTS (SELECT 1 FROM sys.server_principals WHERE name = @Username)
        BEGIN
            RAISERROR(N'Login [%s] đã tồn tại!', 16, 1, @Username);
            RETURN;
        END

        -- Tạo LOGIN
        SET @sqlString = N'CREATE LOGIN ' + QUOTENAME(@Username) +
                         N' WITH PASSWORD = N''' + REPLACE(@Password, '''', '''''') +
                         N''', DEFAULT_DATABASE = [CinemaManagement], CHECK_EXPIRATION = OFF, CHECK_POLICY = OFF';
        EXEC (@sqlString);

        -- Tạo USER trong DB
        SET @sqlString = N'CREATE USER ' + QUOTENAME(@Username) +
                         N' FOR LOGIN ' + QUOTENAME(@Username);
        EXEC (@sqlString);

        -- Gán ROLE
        IF (@Role = N'Nhân viên')
            SET @sqlString = N'ALTER ROLE [NhanVien] ADD MEMBER ' + QUOTENAME(@Username);
        ELSE IF (@Role = N'Quản lý')
            SET @sqlString = N'ALTER ROLE [QuanLy] ADD MEMBER ' + QUOTENAME(@Username);

        EXEC (@sqlString);
    END TRY
    BEGIN CATCH
        DECLARE @err NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@err, 16, 1);
    END CATCH
END
GO

-- Procedure tạo STAFF + ACCOUNT (transaction)
GO
CREATE OR ALTER PROCEDURE sp_CreateAccount
    @FullName NVARCHAR(100),
    @Birth SMALLDATETIME,
    @Gender NVARCHAR(100),
    @Email NVARCHAR(100),
    @PhoneNumber NVARCHAR(20),
    @Salary INT,
    @Role NVARCHAR(30),
    @Username NVARCHAR(40),
    @PasswordHash NVARCHAR(MAX),   -- mật khẩu đã hash (lưu DB)
    @PasswordPlain NVARCHAR(MAX)   -- mật khẩu chưa hash (tạo LOGIN)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
BEGIN TRANSACTION;

        DECLARE @newStaffId INT;

        -- Thêm STAFF
        INSERT INTO STAFF (FullName, Birth, Gender, Email, PhoneNumber, Salary, Role, NgayVaoLam, ImageSource)
        VALUES (@FullName, @Birth, @Gender, @Email, @PhoneNumber, @Salary, @Role, GETDATE(), NULL);

        SET @newStaffId = SCOPE_IDENTITY();

        -- Thêm ACCOUNT (lưu password hash)
        INSERT INTO ACCOUNTS (Username, Password, Staff_Id)
        VALUES (@Username, @PasswordHash, @newStaffId);

        COMMIT TRANSACTION;

        -- Gọi procedure tạo LOGIN/USER với password plaintext
        EXEC sp_CreateSQLLogin @Username, @PasswordPlain, @Role;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        DECLARE @err NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@err, 16, 1);
    END CATCH
END

-- Trigger xóa tài khoản (Chưa đụng đến)
GO
CREATE OR ALTER TRIGGER [dbo].[trg_DeleteSQLAccount]
ON [dbo].[ACCOUNTS]
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @username NVARCHAR(50);
    DECLARE @sqlString NVARCHAR(MAX);

    -- Lấy username vừa bị xóa
    SELECT @username = d.Username
    FROM deleted d;

    -- Xóa USER trong DB
    SET @sqlString = 'DROP USER IF EXISTS [' + @username + ']';
    EXEC (@sqlString);

    -- Xóa LOGIN trong server
    SET @sqlString = 'DROP LOGIN IF EXISTS [' + @username + ']';
    EXEC (@sqlString);

	DELETE s
    FROM STAFF s
    INNER JOIN deleted d ON s.Id = d.Staff_Id;
END;


-- Function đăng nhập
GO
CREATE OR ALTER FUNCTION fn_Login
(
    @username NVARCHAR(50),
    @password NVARCHAR(100)
)
RETURNS NVARCHAR(100)
AS
BEGIN
    DECLARE @role NVARCHAR(50);
    DECLARE @isValid BIT;

    -- Kiểm tra username và password có hợp lệ không
    SELECT @isValid = CASE
        WHEN EXISTS (
            SELECT 1
            FROM ACCOUNTS a
            WHERE a.Username = @username
              AND a.Password = @password
        )
        THEN 1
        ELSE 0
    END;

    IF @isValid = 1
    BEGIN
        -- Nếu đăng nhập hợp lệ, lấy role từ sys.database_principals
        SELECT @role = dp.name
        FROM sys.database_role_members AS drm
        JOIN sys.database_principals AS dp 
            ON drm.role_principal_id = dp.principal_id
        JOIN sys.database_principals AS dp2 
            ON drm.member_principal_id = dp2.principal_id
        WHERE dp2.name = @username;

        -- Nếu user không thuộc vai trò nào thì trả về thông báo
        IF @role IS NULL
            SET @role = N'User này không có role nào';
    END
    ELSE
    BEGIN
        -- Nếu đăng nhập không hợp lệ
        SET @role = N'Đăng nhập thất bại';
    END

    RETURN @role;
END;
GO