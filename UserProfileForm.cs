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
        
        private GroupBox groupBoxInfo;
        private GroupBox groupBoxChangePassword;
        private Label lblCurrentUser;
        private Label lblRole;
        private TextBox txtFullName;
        private TextBox txtUsername;
        private TextBox txtCurrentPassword;
        private TextBox txtNewPassword;
        private TextBox txtConfirmPassword;
        private Button btnUpdateInfo;
        private Button btnChangePassword;
        private Button btnClose;
        private Label lblFullName;
        private Label lblUsername;
        private Label lblCurrentPassword;
        private Label lblNewPassword;
        private Label lblConfirmPassword;
        private Label lblMessage;

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

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form properties
            this.Text = "Thông tin cá nhân - Cinema Asset Management";
            this.Size = new Size(500, 550);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // GroupBox thông tin cá nhân
            this.groupBoxInfo = new GroupBox();
            this.groupBoxInfo.Text = "Thông tin cá nhân";
            this.groupBoxInfo.Location = new Point(20, 20);
            this.groupBoxInfo.Size = new Size(440, 200);
            this.groupBoxInfo.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);

            // Current user info (read-only)
            this.lblCurrentUser = new Label();
            this.lblCurrentUser.Text = $"Người dùng: {SessionManager.Username}";
            this.lblCurrentUser.Location = new Point(20, 30);
            this.lblCurrentUser.Size = new Size(400, 20);
            this.lblCurrentUser.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            this.lblCurrentUser.ForeColor = Color.FromArgb(54, 162, 235);

            this.lblRole = new Label();
            this.lblRole.Text = $"Vai trò: {(SessionManager.IsAdmin ? "Quản lý" : "Nhân viên")}";
            this.lblRole.Location = new Point(20, 55);
            this.lblRole.Size = new Size(400, 20);
            this.lblRole.Font = new Font("Microsoft Sans Serif", 10F);
            this.lblRole.ForeColor = SessionManager.IsAdmin ? Color.FromArgb(220, 53, 69) : Color.FromArgb(40, 167, 69);

            // Username (read-only)
            this.lblUsername = new Label();
            this.lblUsername.Text = "Tên đăng nhập:";
            this.lblUsername.Location = new Point(20, 85);
            this.lblUsername.Size = new Size(120, 20);
            this.lblUsername.Font = new Font("Microsoft Sans Serif", 9F);

            this.txtUsername = new TextBox();
            this.txtUsername.Location = new Point(150, 85);
            this.txtUsername.Size = new Size(260, 23);
            this.txtUsername.ReadOnly = true;
            this.txtUsername.BackColor = Color.LightGray;
            this.txtUsername.Text = SessionManager.Username;

            // Full name (editable)
            this.lblFullName = new Label();
            this.lblFullName.Text = "Họ và tên:";
            this.lblFullName.Location = new Point(20, 115);
            this.lblFullName.Size = new Size(120, 20);
            this.lblFullName.Font = new Font("Microsoft Sans Serif", 9F);

            this.txtFullName = new TextBox();
            this.txtFullName.Location = new Point(150, 115);
            this.txtFullName.Size = new Size(260, 23);
            this.txtFullName.Text = SessionManager.FullName;

            // Update info button
            this.btnUpdateInfo = new Button();
            this.btnUpdateInfo.Text = "Cập nhật thông tin";
            this.btnUpdateInfo.Location = new Point(150, 150);
            this.btnUpdateInfo.Size = new Size(130, 35);
            this.btnUpdateInfo.BackColor = Color.FromArgb(40, 167, 69);
            this.btnUpdateInfo.ForeColor = Color.White;
            this.btnUpdateInfo.FlatStyle = FlatStyle.Flat;
            this.btnUpdateInfo.Click += new EventHandler(this.btnUpdateInfo_Click);

            // Add controls to group box
            this.groupBoxInfo.Controls.Add(this.lblCurrentUser);
            this.groupBoxInfo.Controls.Add(this.lblRole);
            this.groupBoxInfo.Controls.Add(this.lblUsername);
            this.groupBoxInfo.Controls.Add(this.txtUsername);
            this.groupBoxInfo.Controls.Add(this.lblFullName);
            this.groupBoxInfo.Controls.Add(this.txtFullName);
            this.groupBoxInfo.Controls.Add(this.btnUpdateInfo);

            // GroupBox đổi mật khẩu
            this.groupBoxChangePassword = new GroupBox();
            this.groupBoxChangePassword.Text = "Đổi mật khẩu";
            this.groupBoxChangePassword.Location = new Point(20, 240);
            this.groupBoxChangePassword.Size = new Size(440, 200);
            this.groupBoxChangePassword.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);

            // Current password
            this.lblCurrentPassword = new Label();
            this.lblCurrentPassword.Text = "Mật khẩu hiện tại:";
            this.lblCurrentPassword.Location = new Point(20, 30);
            this.lblCurrentPassword.Size = new Size(120, 20);
            this.lblCurrentPassword.Font = new Font("Microsoft Sans Serif", 9F);

            this.txtCurrentPassword = new TextBox();
            this.txtCurrentPassword.Location = new Point(150, 30);
            this.txtCurrentPassword.Size = new Size(260, 23);
            this.txtCurrentPassword.UseSystemPasswordChar = true;

            // New password
            this.lblNewPassword = new Label();
            this.lblNewPassword.Text = "Mật khẩu mới:";
            this.lblNewPassword.Location = new Point(20, 65);
            this.lblNewPassword.Size = new Size(120, 20);
            this.lblNewPassword.Font = new Font("Microsoft Sans Serif", 9F);

            this.txtNewPassword = new TextBox();
            this.txtNewPassword.Location = new Point(150, 65);
            this.txtNewPassword.Size = new Size(260, 23);
            this.txtNewPassword.UseSystemPasswordChar = true;

            // Confirm password
            this.lblConfirmPassword = new Label();
            this.lblConfirmPassword.Text = "Xác nhận mật khẩu:";
            this.lblConfirmPassword.Location = new Point(20, 100);
            this.lblConfirmPassword.Size = new Size(120, 20);
            this.lblConfirmPassword.Font = new Font("Microsoft Sans Serif", 9F);

            this.txtConfirmPassword = new TextBox();
            this.txtConfirmPassword.Location = new Point(150, 100);
            this.txtConfirmPassword.Size = new Size(260, 23);
            this.txtConfirmPassword.UseSystemPasswordChar = true;

            // Change password button
            this.btnChangePassword = new Button();
            this.btnChangePassword.Text = "Đổi mật khẩu";
            this.btnChangePassword.Location = new Point(150, 140);
            this.btnChangePassword.Size = new Size(130, 35);
            this.btnChangePassword.BackColor = Color.FromArgb(54, 162, 235);
            this.btnChangePassword.ForeColor = Color.White;
            this.btnChangePassword.FlatStyle = FlatStyle.Flat;
            this.btnChangePassword.Click += new EventHandler(this.btnChangePassword_Click);

            // Add controls to password group box
            this.groupBoxChangePassword.Controls.Add(this.lblCurrentPassword);
            this.groupBoxChangePassword.Controls.Add(this.txtCurrentPassword);
            this.groupBoxChangePassword.Controls.Add(this.lblNewPassword);
            this.groupBoxChangePassword.Controls.Add(this.txtNewPassword);
            this.groupBoxChangePassword.Controls.Add(this.lblConfirmPassword);
            this.groupBoxChangePassword.Controls.Add(this.txtConfirmPassword);
            this.groupBoxChangePassword.Controls.Add(this.btnChangePassword);

            // Message label
            this.lblMessage = new Label();
            this.lblMessage.Location = new Point(20, 450);
            this.lblMessage.Size = new Size(440, 30);
            this.lblMessage.Font = new Font("Microsoft Sans Serif", 9F);
            this.lblMessage.ForeColor = Color.Red;
            this.lblMessage.Text = "";

            // Close button
            this.btnClose = new Button();
            this.btnClose.Text = "Đóng";
            this.btnClose.Location = new Point(385, 485);
            this.btnClose.Size = new Size(75, 30);
            this.btnClose.BackColor = Color.Gray;
            this.btnClose.ForeColor = Color.White;
            this.btnClose.FlatStyle = FlatStyle.Flat;
            this.btnClose.Click += new EventHandler(this.btnClose_Click);

            // Add controls to form
            this.Controls.Add(this.groupBoxInfo);
            this.Controls.Add(this.groupBoxChangePassword);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.btnClose);

            this.ResumeLayout(false);
            this.PerformLayout();
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

        private void UserProfileForm_Load(object sender, EventArgs e)
        {
            // Load thông tin user khi form khởi động
            txtFullName.Text = SessionManager.FullName ?? "";
            ShowMessage("Bạn có thể cập nhật thông tin cá nhân và đổi mật khẩu tại đây.", Color.Gray);
        }
    }
}
