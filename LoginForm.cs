using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CinameAsset
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.SuspendLayout();
            
            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(80, 30);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(240, 26);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "CINEMA ASSET LOGIN";
            
            // lblUsername
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(50, 80);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(88, 13);
            this.lblUsername.TabIndex = 1;
            this.lblUsername.Text = "Tên đăng nhập:";
            
            // txtUsername
            this.txtUsername.Location = new System.Drawing.Point(50, 100);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(300, 20);
            this.txtUsername.TabIndex = 2;
            
            // lblPassword
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(50, 140);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(58, 13);
            this.lblPassword.TabIndex = 3;
            this.lblPassword.Text = "Mật khẩu:";
            
            // txtPassword
            this.txtPassword.Location = new System.Drawing.Point(50, 160);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(300, 20);
            this.txtPassword.TabIndex = 4;
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPassword_KeyPress);
            
            // btnLogin
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(162)))), ((int)(((byte)(235)))));
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.Location = new System.Drawing.Point(150, 200);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(100, 35);
            this.btnLogin.TabIndex = 5;
            this.btnLogin.Text = "Đăng nhập";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            
            // LoginForm
            this.AcceptButton = this.btnLogin;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 280);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Đăng nhập - Cinema Asset Management";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblTitle;

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(txtUsername.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtUsername.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    MessageBox.Show("Vui lòng nhập mật khẩu!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPassword.Focus();
                    return;
                }

                // Disable button và hiển thị loading
                btnLogin.Enabled = false;
                btnLogin.Text = "Đang đăng nhập...";
                this.Cursor = Cursors.WaitCursor;

                string username = txtUsername.Text.Trim();
                string password = txtPassword.Text;

                // Thực hiện đăng nhập qua SessionManager
                bool loginSuccess = SessionManager.Login(username, password);

                if (loginSuccess)
                {
                    // Đăng nhập thành công
                    MessageBox.Show($"Đăng nhập thành công!\nChào mừng {SessionManager.FullName},{SessionManager.RoleName}", 
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Ẩn LoginForm và mở MainForm
                    this.Hide();
                    
                    MainForm mainForm = new MainForm();
                    mainForm.FormClosed += (s, args) => this.Show(); // Hiển thị lại LoginForm khi MainForm đóng
                    mainForm.Show();

                    // Clear password và reset form
                    txtPassword.Clear();
                    txtUsername.Clear();
                }
                else
                {
                    // Đăng nhập thất bại
                    MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!", 
                        "Đăng nhập thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi đăng nhập: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Reset button state
                btnLogin.Enabled = true;
                btnLogin.Text = "Đăng nhập";
                this.Cursor = Cursors.Default;
            }
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Nhấn Enter để đăng nhập
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnLogin_Click(sender, e);
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            // Focus vào username khi form load
            txtUsername.Focus();
        }
    }
}
