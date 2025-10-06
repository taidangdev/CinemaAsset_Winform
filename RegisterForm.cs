using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace CinameAsset
{
    public partial class RegisterForm : Form
    {
        // Connection String có quyền cao để tạo SQL Login (cần thay đổi theo môi trường)
        private const string AdminConnectionString = "Data Source=(local);Initial Catalog=CinemaAssetDB;User ID=sa;Password=1234;";
        
        private TextBox txtUsername;
        private TextBox txtPassword;
        private TextBox txtFullName;
        private ComboBox cboRole;
        private Button btnRegister;
        private Button btnSwitchToLogin;
        private Label lblMessage;
        private Label lblTitle;
        private Label lblUsername;
        private Label lblPassword;
        private Label lblFullName;
        private Label lblRole;

        public RegisterForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblFullName = new System.Windows.Forms.Label();
            this.txtFullName = new System.Windows.Forms.TextBox();
            this.lblRole = new System.Windows.Forms.Label();
            this.cboRole = new System.Windows.Forms.ComboBox();
            this.btnRegister = new System.Windows.Forms.Button();
            this.btnSwitchToLogin = new System.Windows.Forms.Button();
            this.lblMessage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(162)))), ((int)(((byte)(235)))));
            this.lblTitle.Location = new System.Drawing.Point(120, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(307, 31);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "ĐĂNG KÝ TÀI KHOẢN";
            // 
            // lblUsername
            // 
            this.lblUsername.Location = new System.Drawing.Point(30, 70);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(100, 20);
            this.lblUsername.TabIndex = 1;
            this.lblUsername.Text = "Tên đăng nhập: *";
            // 
            // txtUsername
            // 
            this.txtUsername.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtUsername.Location = new System.Drawing.Point(30, 90);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(380, 26);
            this.txtUsername.TabIndex = 2;
            // 
            // lblPassword
            // 
            this.lblPassword.Location = new System.Drawing.Point(30, 120);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(100, 20);
            this.lblPassword.TabIndex = 3;
            this.lblPassword.Text = "Mật khẩu: *";
            // 
            // txtPassword
            // 
            this.txtPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtPassword.Location = new System.Drawing.Point(30, 140);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(380, 26);
            this.txtPassword.TabIndex = 4;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // lblFullName
            // 
            this.lblFullName.Location = new System.Drawing.Point(30, 170);
            this.lblFullName.Name = "lblFullName";
            this.lblFullName.Size = new System.Drawing.Size(100, 20);
            this.lblFullName.TabIndex = 5;
            this.lblFullName.Text = "Họ và tên: *";
            // 
            // txtFullName
            // 
            this.txtFullName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.txtFullName.Location = new System.Drawing.Point(30, 190);
            this.txtFullName.Name = "txtFullName";
            this.txtFullName.Size = new System.Drawing.Size(380, 26);
            this.txtFullName.TabIndex = 6;
            // 
            // lblRole
            // 
            this.lblRole.Location = new System.Drawing.Point(30, 220);
            this.lblRole.Name = "lblRole";
            this.lblRole.Size = new System.Drawing.Size(100, 20);
            this.lblRole.TabIndex = 7;
            this.lblRole.Text = "Vai trò: *";
            // 
            // cboRole
            // 
            this.cboRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRole.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.cboRole.Items.AddRange(new object[] {
            "QuanLy",
            "NhanVien"});
            this.cboRole.Location = new System.Drawing.Point(30, 240);
            this.cboRole.Name = "cboRole";
            this.cboRole.Size = new System.Drawing.Size(380, 28);
            this.cboRole.TabIndex = 8;
            // 
            // btnRegister
            // 
            this.btnRegister.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnRegister.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRegister.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnRegister.ForeColor = System.Drawing.Color.White;
            this.btnRegister.Location = new System.Drawing.Point(120, 280);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(100, 35);
            this.btnRegister.TabIndex = 9;
            this.btnRegister.Text = "Đăng ký";
            this.btnRegister.UseVisualStyleBackColor = false;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            // 
            // btnSwitchToLogin
            // 
            this.btnSwitchToLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(162)))), ((int)(((byte)(235)))));
            this.btnSwitchToLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSwitchToLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnSwitchToLogin.ForeColor = System.Drawing.Color.White;
            this.btnSwitchToLogin.Location = new System.Drawing.Point(240, 280);
            this.btnSwitchToLogin.Name = "btnSwitchToLogin";
            this.btnSwitchToLogin.Size = new System.Drawing.Size(100, 35);
            this.btnSwitchToLogin.TabIndex = 10;
            this.btnSwitchToLogin.Text = "Đăng nhập";
            this.btnSwitchToLogin.UseVisualStyleBackColor = false;
            this.btnSwitchToLogin.Click += new System.EventHandler(this.btnSwitchToLogin_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.lblMessage.ForeColor = System.Drawing.Color.Red;
            this.lblMessage.Location = new System.Drawing.Point(30, 325);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(380, 40);
            this.lblMessage.TabIndex = 11;
            // 
            // RegisterForm
            // 
            this.ClientSize = new System.Drawing.Size(470, 353);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblFullName);
            this.Controls.Add(this.txtFullName);
            this.Controls.Add(this.lblRole);
            this.Controls.Add(this.cboRole);
            this.Controls.Add(this.btnRegister);
            this.Controls.Add(this.btnSwitchToLogin);
            this.Controls.Add(this.lblMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RegisterForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Đăng ký tài khoản - Cinema Asset Management";
            this.Load += new System.EventHandler(this.RegisterForm_Load_1);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                // BƯỚC 1: Lấy dữ liệu và validation
                string username = txtUsername.Text.Trim();
                string passwordPlain = txtPassword.Text;
                string fullName = txtFullName.Text.Trim();
                string roleName = cboRole.SelectedItem?.ToString();

                // Validate required fields
                if (string.IsNullOrEmpty(username))
                {
                    ShowMessage("Vui lòng nhập tên đăng nhập!", Color.Red);
                    txtUsername.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(passwordPlain))
                {
                    ShowMessage("Vui lòng nhập mật khẩu!", Color.Red);
                    txtPassword.Focus();
                    return;
                }

                if (passwordPlain.Length < 6)
                {
                    ShowMessage("Mật khẩu phải có ít nhất 6 ký tự!", Color.Red);
                    txtPassword.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(fullName))
                {
                    ShowMessage("Vui lòng nhập họ tên!", Color.Red);
                    txtFullName.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(roleName))
                {
                    ShowMessage("Vui lòng chọn vai trò!", Color.Red);
                    cboRole.Focus();
                    return;
                }

                // BƯỚC 2: HASHING (Client-side)
                string passwordHash = SecurityHelper.HashPassword(passwordPlain);

                // BƯỚC 3: Disable button và hiển thị progress
                btnRegister.Enabled = false;
                btnRegister.Text = "Đang xử lý...";
                this.Cursor = Cursors.WaitCursor;
                ShowMessage("Đang tạo tài khoản...", Color.Blue);

                // BƯỚC 4: Gọi stored procedure tạo tài khoản
                using (SqlConnection conn = new SqlConnection(AdminConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_CreateUserAccount", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 60; // Tăng timeout vì CREATE LOGIN có thể mất thời gian

                        // 5 tham số theo stored procedure đã định nghĩa
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@PasswordPlain", passwordPlain);
                        cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
                        cmd.Parameters.AddWithValue("@FullName", fullName);
                        cmd.Parameters.AddWithValue("@RoleName", roleName);

                        conn.Open();
                        
                        // Execute và đọc kết quả
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string status = reader["Status"].ToString();
                                string message = reader["Message"].ToString();

                                if (status == "SUCCESS")
                                {
                                    ShowMessage($"✓ {message}", Color.Green);
                                    
                                    // Clear form sau khi thành công
                                    ClearForm();
                                    
                                    // Hiển thị thông báo chuyển đăng nhập
                                    MessageBox.Show(
                                        $"Tài khoản '{username}' đã được tạo thành công!\n\n" +
                                        "Bạn có thể chuyển sang đăng nhập bằng nút 'Đăng nhập' bên dưới.",
                                        "Đăng ký thành công",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
                                }
                                else
                                {
                                    ShowMessage($"Lỗi: {message}", Color.Red);
                                }
                            }
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                // Xử lý lỗi SQL cụ thể
                string errorMessage = "Lỗi SQL: ";
                switch (sqlEx.Number)
                {
                    case 15405: // Login đã tồn tại
                        errorMessage += "Tên đăng nhập đã tồn tại trong hệ thống.";
                        break;
                    case 18456: // Login failed
                        errorMessage += "Lỗi xác thực. Kiểm tra connection string.";
                        break;
                    case 2: // Cannot open database
                        errorMessage += "Không thể kết nối database. Kiểm tra server.";
                        break;
                    default:
                        errorMessage += sqlEx.Message;
                        break;
                }
                ShowMessage(errorMessage, Color.Red);
            }
            catch (Exception ex)
            {
                ShowMessage($"Lỗi hệ thống: {ex.Message}", Color.Red);
            }
            finally
            {
                // Reset button state
                btnRegister.Enabled = true;
                btnRegister.Text = "Đăng ký";
                this.Cursor = Cursors.Default;
            }
        }

        private void btnSwitchToLogin_Click(object sender, EventArgs e)
        {
            // Chuyển sang LoginForm
            this.Hide();
            
            LoginForm loginForm = new LoginForm();
            loginForm.FormClosed += (s, args) => this.Show(); // Quay lại RegisterForm nếu LoginForm đóng
            loginForm.Show();
        }

        private void ShowMessage(string message, Color color)
        {
            lblMessage.Text = message;
            lblMessage.ForeColor = color;
            Application.DoEvents(); // Refresh UI ngay lập tức
        }

        private void ClearForm()
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtFullName.Clear();
            cboRole.SelectedIndex = 0;
            txtUsername.Focus();
        }

        private void RegisterForm_Load(object sender, EventArgs e)
        {
            // Focus vào username khi form load
            txtUsername.Focus();
            
            // Hiển thị thông tin hướng dẫn
            ShowMessage("Tạo tài khoản đầu tiên cho hệ thống Cinema Asset Management", Color.Gray);
        }

        // Override ProcessDialogKey để xử lý phím tắt
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                btnRegister_Click(this, EventArgs.Empty);
                return true;
            }
            else if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            return base.ProcessDialogKey(keyData);
        }

        private void RegisterForm_Load_1(object sender, EventArgs e)
        {

        }
    }
}
