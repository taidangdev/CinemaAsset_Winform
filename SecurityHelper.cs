using System.Security.Cryptography;
using System.Text;

namespace CinameAsset
{
    /// <summary>
    /// Static class xử lý bảo mật và hashing mật khẩu.
    /// Tích hợp với hệ thống RBAC SQL Server đã thiết lập.
    /// </summary>
    public static class SecurityHelper
    {
        /// <summary>
        /// Tạo chuỗi hash SHA256 từ mật khẩu plaintext.
        /// Dùng cho việc lưu trữ vào DB và xác thực bằng fn_Login.
        /// </summary>
        /// <param name="password">Mật khẩu plaintext từ người dùng nhập.</param>
        /// <returns>Chuỗi hash SHA256 (hex string) để lưu vào cột password_hash.</returns>
        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                return string.Empty;

            using (SHA256 sha256 = SHA256.Create())
            {
                // Chuyển password thành byte array
                byte[] inputBytes = Encoding.UTF8.GetBytes(password);
                
                // Tính hash SHA256
                byte[] hashBytes = sha256.ComputeHash(inputBytes);
                
                // Chuyển byte array thành chuỗi hex
                StringBuilder hexBuilder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    hexBuilder.Append(hashBytes[i].ToString("x2"));
                }
                
                return hexBuilder.ToString();
            }
        }

        /// <summary>
        /// Xác minh mật khẩu bằng cách so sánh hash.
        /// Dùng để double-check trước khi gọi fn_Login.
        /// </summary>
        /// <param name="inputPassword">Mật khẩu người dùng nhập.</param>
        /// <param name="storedHash">Hash đã lưu trong database.</param>
        /// <returns>True nếu mật khẩu khớp.</returns>
        public static bool VerifyPassword(string inputPassword, string storedHash)
        {
            if (string.IsNullOrEmpty(inputPassword) || string.IsNullOrEmpty(storedHash))
                return false;

            string inputHash = HashPassword(inputPassword);
            return string.Equals(inputHash, storedHash, System.StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Tạo salt ngẫu nhiên để tăng cường bảo mật (tùy chọn nâng cao).
        /// Có thể dùng trong tương lai để salt + hash password.
        /// </summary>
        /// <param name="size">Kích thước salt (bytes).</param>
        /// <returns>Chuỗi salt base64.</returns>
        public static string GenerateSalt(int size = 32)
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] saltBytes = new byte[size];
                rng.GetBytes(saltBytes);
                return System.Convert.ToBase64String(saltBytes);
            }
        }
    }
}
