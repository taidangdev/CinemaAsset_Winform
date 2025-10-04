using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CinameAsset
{
    public partial class UserManagementForm : Form
    {
        private string connectionString;

        public UserManagementForm()
        {
            InitializeComponent();
            connectionString = SessionManager.GetConnectionString();
            
            // Kiểm tra quyền Admin
            if (!SessionManager.IsAdmin)
            {
                MessageBox.Show("Bạn không có quyền truy cập chức năng này!", "Từ chối truy cập", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            // THÊM SỰ KIỆN XỬ LÝ NHẤP VÀO CỘT NÚT
            this.dgvUsers.CellContentClick += new DataGridViewCellEventHandler(this.dgvUsers_CellContentClick);
        }

        private void InitializeComponent()
        {
            this.groupBoxAdd = new System.Windows.Forms.GroupBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtFullName = new System.Windows.Forms.TextBox();
            this.cmbRole = new System.Windows.Forms.ComboBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblFullName = new System.Windows.Forms.Label();
            this.lblRole = new System.Windows.Forms.Label();
            
            this.groupBoxManage = new System.Windows.Forms.GroupBox();
            this.dgvUsers = new System.Windows.Forms.DataGridView();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            
            this.groupBoxAdd.SuspendLayout();
            this.groupBoxManage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
            this.SuspendLayout();
            
            // groupBoxAdd
            this.groupBoxAdd.Controls.Add(this.btnAdd);
            this.groupBoxAdd.Controls.Add(this.cmbRole);
            this.groupBoxAdd.Controls.Add(this.txtFullName);
            this.groupBoxAdd.Controls.Add(this.txtPassword);
            this.groupBoxAdd.Controls.Add(this.txtUsername);
            this.groupBoxAdd.Controls.Add(this.lblRole);
            this.groupBoxAdd.Controls.Add(this.lblFullName);
            this.groupBoxAdd.Controls.Add(this.lblPassword);
            this.groupBoxAdd.Controls.Add(this.lblUsername);
            this.groupBoxAdd.Location = new System.Drawing.Point(12, 12);
            this.groupBoxAdd.Name = "groupBoxAdd";
            this.groupBoxAdd.Size = new System.Drawing.Size(760, 150);
            this.groupBoxAdd.TabIndex = 0;
            this.groupBoxAdd.TabStop = false;
            this.groupBoxAdd.Text = "Thêm tài khoản mới";
            
            // Labels and TextBoxes for Add User
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(20, 30);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(85, 13);
            this.lblUsername.TabIndex = 0;
            this.lblUsername.Text = "Tên đăng nhập:";
            
            this.txtUsername.Location = new System.Drawing.Point(20, 50);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(150, 20);
            this.txtUsername.TabIndex = 1;
            
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(200, 30);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(58, 13);
            this.lblPassword.TabIndex = 2;
            this.lblPassword.Text = "Mật khẩu:";
            
            this.txtPassword.Location = new System.Drawing.Point(200, 50);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(150, 20);
            this.txtPassword.TabIndex = 3;
            this.txtPassword.UseSystemPasswordChar = true;
            
            this.lblFullName.AutoSize = true;
            this.lblFullName.Location = new System.Drawing.Point(380, 30);
            this.lblFullName.Name = "lblFullName";
            this.lblFullName.Size = new System.Drawing.Size(61, 13);
            this.lblFullName.TabIndex = 4;
            this.lblFullName.Text = "Họ và tên:";
            
            this.txtFullName.Location = new System.Drawing.Point(380, 50);
            this.txtFullName.Name = "txtFullName";
            this.txtFullName.Size = new System.Drawing.Size(200, 20);
            this.txtFullName.TabIndex = 5;
            
            this.lblRole.AutoSize = true;
            this.lblRole.Location = new System.Drawing.Point(600, 30);
            this.lblRole.Name = "lblRole";
            this.lblRole.Size = new System.Drawing.Size(44, 13);
            this.lblRole.TabIndex = 6;
            this.lblRole.Text = "Vai trò:";
            
            this.cmbRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRole.Items.AddRange(new object[] { "QuanLy", "NhanVien" });
            this.cmbRole.Location = new System.Drawing.Point(600, 50);
            this.cmbRole.Name = "cmbRole";
            this.cmbRole.Size = new System.Drawing.Size(120, 21);
            this.cmbRole.TabIndex = 7;
            
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(320, 90);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(120, 35);
            this.btnAdd.TabIndex = 8;
            this.btnAdd.Text = "Thêm tài khoản";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            
            // groupBoxManage
            this.groupBoxManage.Controls.Add(this.btnDelete);
            this.groupBoxManage.Controls.Add(this.btnRefresh);
            this.groupBoxManage.Controls.Add(this.dgvUsers);
            this.groupBoxManage.Location = new System.Drawing.Point(12, 180);
            this.groupBoxManage.Name = "groupBoxManage";
            this.groupBoxManage.Size = new System.Drawing.Size(760, 350);
            this.groupBoxManage.TabIndex = 1;
            this.groupBoxManage.TabStop = false;
            this.groupBoxManage.Text = "Quản lý tài khoản";
            
            // dgvUsers
            this.dgvUsers.AllowUserToAddRows = false;
            this.dgvUsers.AllowUserToDeleteRows = false;
            this.dgvUsers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvUsers.Location = new System.Drawing.Point(20, 25);
            this.dgvUsers.MultiSelect = false;
            this.dgvUsers.Name = "dgvUsers";
            this.dgvUsers.ReadOnly = true;
            this.dgvUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsers.Size = new System.Drawing.Size(720, 280);
            this.dgvUsers.TabIndex = 0;
            
            this.btnRefresh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(162)))), ((int)(((byte)(235)))));
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(550, 315);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(90, 30);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.Text = "Làm mới";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Location = new System.Drawing.Point(650, 315);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(90, 30);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "Xóa";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            
            // UserManagementForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.groupBoxManage);
            this.Controls.Add(this.groupBoxAdd);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "UserManagementForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quản lý tài khoản - Cinema Asset Management";
            this.Load += new System.EventHandler(this.UserManagementForm_Load);
            
            this.groupBoxAdd.ResumeLayout(false);
            this.groupBoxAdd.PerformLayout();
            this.groupBoxManage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.GroupBox groupBoxAdd;
        private System.Windows.Forms.GroupBox groupBoxManage;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtFullName;
        private System.Windows.Forms.ComboBox cmbRole;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblFullName;
        private System.Windows.Forms.Label lblRole;
        private System.Windows.Forms.DataGridView dgvUsers;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnDelete;

        private void UserManagementForm_Load(object sender, EventArgs e)
        {
            // Thiết lập giá trị mặc định
            cmbRole.SelectedIndex = 1; // NhanVien
            LoadUsers();
            
            // Ẩn nút Delete bên ngoài vì đã có cột nút Xóa trong grid
            btnDelete.Visible = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(txtUsername.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    MessageBox.Show("Vui lòng nhập mật khẩu!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtFullName.Text))
                {
                    MessageBox.Show("Vui lòng nhập họ tên!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cmbRole.SelectedIndex == -1)
                {
                    MessageBox.Show("Vui lòng chọn vai trò!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Lấy thông tin
                string username = txtUsername.Text.Trim();
                string passwordPlain = txtPassword.Text;
                string passwordHash = SecurityHelper.HashPassword(passwordPlain);
                string fullName = txtFullName.Text.Trim();
                string roleName = cmbRole.SelectedItem.ToString();

                // Gọi stored procedure tạo tài khoản
                // Sử dụng connection string có quyền cao (sa) để tạo SQL Login
                using (SqlConnection conn = new SqlConnection("Data Source=(local);Initial Catalog=CinemaAssetDB;User ID=sa;Password=1234;"))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_CreateUserAccount", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@PasswordPlain", passwordPlain);
                        cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
                        cmd.Parameters.AddWithValue("@FullName", fullName);
                        cmd.Parameters.AddWithValue("@RoleName", roleName);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string status = reader["Status"].ToString();
                                string message = reader["Message"].ToString();
                                
                                if (status == "SUCCESS")
                                {
                                    MessageBox.Show("Tạo tài khoản thành công!", "Thành công", 
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    
                                    // Clear form
                                    ClearAddForm();
                                    LoadUsers();
                                }
                                else
                                {
                                    MessageBox.Show($"Lỗi: {message}", "Thất bại", 
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tạo tài khoản: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvUsers.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn tài khoản cần xóa!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataGridViewRow selectedRow = dgvUsers.SelectedRows[0];
                string username = selectedRow.Cells["username"].Value.ToString();
                string fullName = selectedRow.Cells["FullName"].Value.ToString();

                // Không cho phép xóa tài khoản hiện tại
                if (username == SessionManager.Username)
                {
                    MessageBox.Show("Không thể xóa tài khoản đang đăng nhập!", "Cảnh báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa tài khoản:\n{fullName} ({username})?", 
                    "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Sử dụng connection string có quyền cao (sa) để xóa SQL Login
                    using (SqlConnection conn = new SqlConnection("Data Source=(local);Initial Catalog=CinemaAssetDB;User ID=sa;Password=1234;"))
                    {
                        conn.Open();
                        
                        // Gọi stored procedure để xóa cả SQL Login và record trong Accounts
                        using (SqlCommand cmd = new SqlCommand("sp_DeleteUserAccount_WithLogin", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Username", username);
                            
                            cmd.ExecuteNonQuery();
                            
                            MessageBox.Show("Xóa tài khoản thành công!", "Thành công", 
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadUsers();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xóa tài khoản: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadUsers();
        }

        private void LoadUsers()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT user_id, FullName, username, role_name, is_active,
                               CASE WHEN is_active = 1 THEN N'Hoạt động' ELSE N'Tạm khóa' END AS status_text
                        FROM dbo.Accounts 
                        ORDER BY role_name, FullName";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvUsers.DataSource = dt;

                    // Tùy chỉnh cột
                    if (dgvUsers.Columns.Count > 0)
                    {
                        dgvUsers.Columns["user_id"].HeaderText = "ID";
                        dgvUsers.Columns["user_id"].Width = 50;
                        dgvUsers.Columns["FullName"].HeaderText = "Họ và tên";
                        dgvUsers.Columns["username"].HeaderText = "Tên đăng nhập";
                        dgvUsers.Columns["role_name"].HeaderText = "Vai trò";
                        dgvUsers.Columns["is_active"].Visible = false;
                        dgvUsers.Columns["status_text"].HeaderText = "Trạng thái";
                        dgvUsers.Columns["status_text"].Width = 100;

                        // THÊM CỘT NÚT XÓA MỚI
                        if (dgvUsers.Columns["colActionDelete"] == null)
                        {
                            DataGridViewButtonColumn deleteBtn = new DataGridViewButtonColumn();
                            deleteBtn.Name = "colActionDelete";
                            deleteBtn.HeaderText = "Thao tác";
                            deleteBtn.Text = "Xóa";
                            deleteBtn.UseColumnTextForButtonValue = true;
                            deleteBtn.Width = 80;
                            dgvUsers.Columns.Add(deleteBtn);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh sách tài khoản: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearAddForm()
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtFullName.Clear();
            cmbRole.SelectedIndex = 1; // NhanVien
            txtUsername.Focus();
        }

        // Xử lý sự kiện nhấp vào cột nút trong DataGridView
        private void dgvUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Chỉ xử lý khi nhấp vào cột "Xóa" và không phải header
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && dgvUsers.Columns[e.ColumnIndex].Name == "colActionDelete")
            {
                try
                {
                    // 1. Đảm bảo dòng được chọn
                    dgvUsers.ClearSelection();
                    dgvUsers.Rows[e.RowIndex].Selected = true;
                    
                    // 2. Gọi logic xóa tài khoản (tái sử dụng hàm btnDelete_Click)
                    btnDelete_Click(sender, e);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa: {ex.Message}", "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
