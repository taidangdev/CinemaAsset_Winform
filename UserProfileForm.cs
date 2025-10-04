using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace CinameAsset
{
    public partial class UserProfileForm : Form
    {
        private string connectionString;

        public UserProfileForm()
        {
            InitializeComponent();
            
            // Kiểm tra đăng nhập
            if (!SessionManager.IsLoggedIn)
            {
                MessageBox.Show("Vui lòng đăng nhập trước!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            connectionString = SessionManager.GetConnectionString();
        }

        private void UserProfileForm_Load(object sender, EventArgs e)
        {
            // Load thông tin user khi form khởi động
            lblCurrentUser.Text = $"Người dùng: {SessionManager.Username}";
            lblRole.Text = $"Vai trò: {(SessionManager.IsAdmin ? "Quản lý" : "Nhân viên")}";
            lblRole.ForeColor = SessionManager.IsAdmin ? Color.FromArgb(220, 53, 69) : Color.FromArgb(40, 167, 69);
            
            txtUsername.Text = SessionManager.Username;
            txtFullName.Text = SessionManager.FullName ?? "";
            
            ShowMessage("Bạn có thể cập nhật thông tin cá nhân và đổi mật khẩu tại đây.", Color.Gray);
        }

        private void btnUpdateInfo_Click(object sender, EventArgs e)
        {
            try
            {
                string newFullName = txtFullName.Text.Trim();
                
                if (string.IsNullOrEmpty(newFullName))
                {
                    ShowMessage("Vui lòng nhập họ tên!", Color.Red);
                    txtFullName.Focus();
                    return;
                }

                if (newFullName == SessionManager.FullName)
                {
                    ShowMessage("Thông tin chưa thay đổi.", Color.Orange);
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE dbo.Accounts SET FullName = @FullName WHERE username = @Username";
                    
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@FullName", newFullName);
                        cmd.Parameters.AddWithValue("@Username", SessionManager.Username);
                        
                        int rowsAffected = cmd.ExecuteNonQuery();
                        
                        if (rowsAffected > 0)
                        {
                            // Cập nhật session với thông tin mới
                            SessionManager.SetSession(SessionManager.Username, "", SessionManager.RoleName, newFullName);
                            
                            ShowMessage("Cập nhật thông tin thành công!", Color.Green);
                        }
                        else
                        {
                            ShowMessage("Không thể cập nhật thông tin!", Color.Red);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"Lỗi cập nhật thông tin: {ex.Message}", Color.Red);
            }
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            try
            {
                string currentPassword = txtCurrentPassword.Text;
                string newPassword = txtNewPassword.Text;
                string confirmPassword = txtConfirmPassword.Text;

                // Validation
                if (string.IsNullOrEmpty(currentPassword))
                {
                    ShowMessage("Vui lòng nhập mật khẩu hiện tại!", Color.Red);
                    txtCurrentPassword.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(newPassword))
                {
                    ShowMessage("Vui lòng nhập mật khẩu mới!", Color.Red);
                    txtNewPassword.Focus();
                    return;
                }

                if (newPassword.Length < 6)
                {
                    ShowMessage("Mật khẩu mới phải có ít nhất 6 ký tự!", Color.Red);
                    txtNewPassword.Focus();
                    return;
                }

                if (newPassword != confirmPassword)
                {
                    ShowMessage("Xác nhận mật khẩu không khớp!", Color.Red);
                    txtConfirmPassword.Focus();
                    return;
                }

                if (currentPassword == newPassword)
                {
                    ShowMessage("Mật khẩu mới phải khác mật khẩu hiện tại!", Color.Red);
                    txtNewPassword.Focus();
                    return;
                }

                // Verify current password
                string currentPasswordHash = SecurityHelper.HashPassword(currentPassword);
                string newPasswordHash = SecurityHelper.HashPassword(newPassword);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    
                    // Check current password
                    string checkQuery = "SELECT COUNT(*) FROM dbo.Accounts WHERE username = @Username AND password_hash = @CurrentPasswordHash";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@Username", SessionManager.Username);
                        checkCmd.Parameters.AddWithValue("@CurrentPasswordHash", currentPasswordHash);
                        
                        int count = (int)checkCmd.ExecuteScalar();
                        if (count == 0)
                        {
                            ShowMessage("Mật khẩu hiện tại không đúng!", Color.Red);
                            txtCurrentPassword.Clear();
                            txtCurrentPassword.Focus();
                            return;
                        }
                    }

                    // Update password
                    string updateQuery = "UPDATE dbo.Accounts SET password_hash = @NewPasswordHash WHERE username = @Username";
                    using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
                    {
                        updateCmd.Parameters.AddWithValue("@NewPasswordHash", newPasswordHash);
                        updateCmd.Parameters.AddWithValue("@Username", SessionManager.Username);
                        
                        int rowsAffected = updateCmd.ExecuteNonQuery();
                        
                        if (rowsAffected > 0)
                        {
                            ShowMessage("Đổi mật khẩu thành công!", Color.Green);
                            
                            // Clear password fields
                            txtCurrentPassword.Clear();
                            txtNewPassword.Clear();
                            txtConfirmPassword.Clear();
                        }
                        else
                        {
                            ShowMessage("Không thể đổi mật khẩu!", Color.Red);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"Lỗi đổi mật khẩu: {ex.Message}", Color.Red);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ShowMessage(string message, Color color)
        {
            lblMessage.Text = message;
            lblMessage.ForeColor = color;
        }
    }
}
