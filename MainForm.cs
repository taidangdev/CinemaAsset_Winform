using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace CinameAsset
{
    public partial class MainForm : Form
    {
        private string connectionString;
        private Form currentChildForm = null;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Kiểm tra đăng nhập
            if (!SessionManager.IsLoggedIn)
            {
                MessageBox.Show("Vui lòng đăng nhập trước!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            // Sử dụng connection string cá nhân hóa
            connectionString = SessionManager.GetConnectionString();
            
            // Áp dụng phân quyền UI
            ApplyRoleBasedPermissions();
            
            // Hiển thị thông tin user
            DisplayUserInfo();
            
            // Mặc định hiển thị trang Thông tin hạ tầng
            ShowInfrastructureForm();
        }

        private void ApplyRoleBasedPermissions()
        {
            
            if (SessionManager.IsStaff)
            {
                // Staff: Ẩn các chức năng quản lý Admin
                btnUserManagement.Visible = false;
                
                // Staff chỉ được xem, không được thêm/sửa/xóa
                // Có thể thêm logic để disable các button Add/Edit trong forms
            }
            else if (SessionManager.IsAdmin)
            {
                // Admin: Hiển thị đầy đủ chức năng
                btnUserManagement.Visible = true;
            }
            
            // Debug info trong console
            System.Diagnostics.Debug.WriteLine($"RBAC Applied: User={SessionManager.Username}, Role={SessionManager.RoleName}, IsAdmin={SessionManager.IsAdmin}");
        }

        private void DisplayUserInfo()
        {
            // Hiển thị thông tin user trên title bar hoặc label
            this.Text = $"Cinema Asset Management - {SessionManager.FullName} ({SessionManager.RoleName})";
        }

        private void OpenChildForm(Form childForm)
        {
            // Đóng form con hiện tại nếu có
            if (currentChildForm != null)
            {
                currentChildForm.Close();
                currentChildForm.Dispose();
            }

            // Thiết lập form con mới
            currentChildForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            
            // Thêm vào panel content
            panelContent.Controls.Clear();
            panelContent.Controls.Add(childForm);
            childForm.Show();
        }

        private void ResetButtonColors()
        {
            // Reset tất cả button về màu mặc định
            btnInfrastructure.FillColor = Color.FromArgb(45, 52, 54);
            btnVendors.FillColor = Color.FromArgb(45, 52, 54);
            btnStatistics.FillColor = Color.FromArgb(45, 52, 54);
            btnWarehouse.FillColor = Color.FromArgb(45, 52, 54);
            if (SessionManager.IsAdmin) 
                btnUserManagement.FillColor = Color.FromArgb(45, 52, 54);
        }

        private void SetActiveButton(Guna2Button activeButton)
        {
            ResetButtonColors();
            activeButton.FillColor = Color.FromArgb(94, 148, 255);
        }

        private void btnInfrastructure_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnInfrastructure);
            ShowInfrastructureForm();
        }

        private void btnVendors_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnVendors);
            ShowVendorsForm();
        }

        private void btnStatistics_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnStatistics);
            ShowStatisticsForm();
        }

        private void btnWarehouse_Click(object sender, EventArgs e)
        {
            SetActiveButton(btnWarehouse);
            ShowWarehouseForm();
        }

        private void ShowInfrastructureForm()
        {
            InfrastructureManagement infrastructureForm = new InfrastructureManagement();
            OpenChildForm(infrastructureForm);
        }

        private void ShowVendorsForm()
        {
            VendorManagement vendorForm = new VendorManagement(connectionString);
            OpenChildForm(vendorForm);
        }

        private void ShowStatisticsForm()
        {
            PurchaseStatistics statisticsForm = new PurchaseStatistics(connectionString);
            OpenChildForm(statisticsForm);
        }

        private void ShowWarehouseForm()
        {
            WarehouseManagement warehouseForm = new WarehouseManagement(connectionString);
            OpenChildForm(warehouseForm);
        }

        private void ShowUserManagementForm()
        {
            // Chỉ Admin mới được truy cập
            if (SessionManager.IsAdmin)
            {
                UserManagementForm userForm = new UserManagementForm();
                OpenChildForm(userForm);
            }
            else
            {
                MessageBox.Show("Bạn không có quyền truy cập chức năng này!", "Từ chối truy cập", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Event handler cho button User Management
        private void btnUserManagement_Click(object sender, EventArgs e)
        {
            // Reset button colors và set active
            ResetButtonColors();
            btnUserManagement.FillColor = Color.FromArgb(220, 53, 69); // Red color for admin
            
            ShowUserManagementForm();
        }

        private void ShowUserProfileForm()
        {
            UserProfileForm profileForm = new UserProfileForm();
            OpenChildForm(profileForm);
        }


        // Helper method để show RBAC status cho debugging
        public void ShowRBACStatus()
        {
            string message = $"RBAC STATUS:\n" +
                           $"User: {SessionManager.Username}\n" +
                           $"Full Name: {SessionManager.FullName}\n" +
                           $"Role: {SessionManager.RoleName}\n" +
                           $"Is Admin: {SessionManager.IsAdmin}\n" +
                           $"Is Staff: {SessionManager.IsStaff}\n\n" +
                           $"Available Features:\n" +
                           $"✓ User Profile (All users)\n" +
                           $"{(SessionManager.IsAdmin ? "✓" : "✗")} User Management (Admin only)\n" +
                           $"{(SessionManager.IsAdmin ? "✓" : "✗")} Full CRUD Operations (Admin only)\n" +
                           $"{(SessionManager.IsStaff ? "✓" : "✗")} View Data + Mark Broken (Staff only)";

            MessageBox.Show(message, "RBAC Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Thêm chức năng đăng xuất
        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận đăng xuất", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Đăng xuất
                SessionManager.Logout();
                
                // Đóng MainForm và hiển thị LoginForm
                this.Hide();
                
                LoginForm loginForm = new LoginForm();
                loginForm.Show();
                
                // Đóng MainForm khi LoginForm đóng
                loginForm.FormClosed += (s, args) => this.Close();
            }
        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
