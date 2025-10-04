using System;
using System.Data.SqlClient;

namespace CinameAsset
{
    /// <summary>
    /// Static class quản lý phiên đăng nhập và kết nối động theo mô hình RBAC.
    /// Tạo connection string cá nhân hóa để áp dụng quyền SQL Server cho từng user.
    /// </summary>
    public static class SessionManager
    {
        #region Properties - Thông tin phiên hiện tại
        
        /// <summary>
        /// Tên đăng nhập hiện tại (SQL Login name).
        /// </summary>
        public static string Username { get; private set; }
        
        /// <summary>
        /// Tên đầy đủ của người dùng từ bảng Accounts.
        /// </summary>
        public static string FullName { get; private set; }
        
        /// <summary>
        /// Vai trò hiện tại: 'QuanLy' (Admin) hoặc 'NhanVien' (Staff).
        /// </summary>
        public static string RoleName { get; private set; }
        
        /// <summary>
        /// Chuỗi kết nối cá nhân hóa sử dụng SQL Login của user hiện tại.
        /// Đảm bảo áp dụng đúng quyền RBAC đã thiết lập.
        /// </summary>
        public static string CurrentUserConnectionString { get; private set; }

        /// <summary>
        /// Kiểm tra xem có phiên đăng nhập hợp lệ hay không.
        /// </summary>
        public static bool IsLoggedIn => !string.IsNullOrEmpty(Username) && 
                                        !string.IsNullOrEmpty(CurrentUserConnectionString);

        /// <summary>
        /// Kiểm tra người dùng hiện tại có phải Admin không.
        /// </summary>
        public static bool IsAdmin => RoleName == "QuanLy";

        /// <summary>
        /// Kiểm tra người dùng hiện tại có phải Staff không.
        /// </summary>
        public static bool IsStaff => RoleName == "NhanVien";

        #endregion

        #region Configuration - Cấu hình kết nối

        // TODO: Thay đổi các giá trị này theo cấu hình SQL Server của bạn
        private const string ServerName = "(local)";  // Hoặc tên SQL Server của bạn
        private const string DatabaseName = "CinemaAssetDB";
        
        // Connection string để xác thực ban đầu (dùng tài khoản có quyền gọi fn_Login)
        // TODO: Cấu hình tài khoản auth trong App.config hoặc appsettings
        private const string AuthConnectionString = 
            "Data Source=(local);Initial Catalog=CinemaAssetDB;Integrated Security=True;";

        #endregion

        #region Public Methods

        /// <summary>
        /// Xác thực đăng nhập và thiết lập phiên làm việc.
        /// </summary>
        /// <param name="username">Tên đăng nhập.</param>
        /// <param name="password">Mật khẩu plaintext.</param>
        /// <returns>True nếu đăng nhập thành công.</returns>
        public static bool Login(string username, string password)
        {
            try
            {
                // Hash mật khẩu để xác thực
                string passwordHash = SecurityHelper.HashPassword(password);
                
                // Gọi function fn_Login để xác thực
                string roleName = AuthenticateUser(username, passwordHash);
                
                if (roleName == "Đăng nhập thất bại")
                {
                    return false;
                }

                // Lấy thông tin chi tiết user từ bảng Accounts
                var userInfo = GetUserInfo(username, passwordHash);
                if (userInfo == null)
                {
                    return false;
                }

                // Thiết lập phiên làm việc
                SetSession(username, password, roleName, userInfo.FullName);
                
                return true;
            }
            catch (Exception ex)
            {
                // Log error nếu cần
                System.Windows.Forms.MessageBox.Show($"Lỗi đăng nhập: {ex.Message}", 
                    "Lỗi", System.Windows.Forms.MessageBoxButtons.OK, 
                    System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// Đăng xuất và xóa phiên làm việc.
        /// </summary>
        public static void Logout()
        {
            ClearSession();
        }

        /// <summary>
        /// Lấy connection string cho user hiện tại.
        /// Đảm bảo áp dụng đúng quyền RBAC.
        /// </summary>
        /// <returns>Connection string hoặc null nếu chưa đăng nhập.</returns>
        public static string GetConnectionString()
        {
            if (!IsLoggedIn)
            {
                throw new InvalidOperationException("Chưa đăng nhập. Vui lòng đăng nhập trước.");
            }
            
            return CurrentUserConnectionString;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Thiết lập phiên làm việc sau khi xác thực thành công.
        /// </summary>
        public static void SetSession(string username, string passwordPlaintext, 
                                     string roleName, string fullName)
        {
            Username = username;
            RoleName = roleName;
            FullName = fullName;

            // Tạo connection string cá nhân hóa với SQL Login của user
            CurrentUserConnectionString = 
                $"Data Source={ServerName};Initial Catalog={DatabaseName};" +
                $"User ID={username};Password={passwordPlaintext};" +
                $"Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;";
        }

        /// <summary>
        /// Xóa phiên làm việc.
        /// </summary>
        private static void ClearSession()
        {
            Username = null;
            RoleName = null;
            FullName = null;
            CurrentUserConnectionString = null;
        }

        /// <summary>
        /// Xác thực user bằng function fn_Login.
        /// </summary>
        private static string AuthenticateUser(string username, string passwordHash)
        {
            using (var conn = new SqlConnection(AuthConnectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT dbo.fn_Login(@username, @passwordHash)", conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@passwordHash", passwordHash);
                    
                    var result = cmd.ExecuteScalar();
                    return result?.ToString() ?? "Đăng nhập thất bại";
                }
            }
        }

        /// <summary>
        /// Lấy thông tin chi tiết user từ bảng Accounts.
        /// </summary>
        private static UserInfo GetUserInfo(string username, string passwordHash)
        {
            using (var conn = new SqlConnection(AuthConnectionString))
            {
                conn.Open();
                string query = @"
                    SELECT FullName, role_name 
                    FROM dbo.Accounts 
                    WHERE username = @username 
                      AND password_hash = @passwordHash 
                      AND is_active = 1";
                
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@passwordHash", passwordHash);
                    
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new UserInfo
                            {
                                FullName = reader["FullName"]?.ToString() ?? username,
                                RoleName = reader["role_name"]?.ToString()
                            };
                        }
                    }
                }
            }
            return null;
        }

        #endregion

        #region Helper Classes

        /// <summary>
        /// Class chứa thông tin user.
        /// </summary>
        private class UserInfo
        {
            public string FullName { get; set; }
            public string RoleName { get; set; }
        }

        #endregion
    }
}
