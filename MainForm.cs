using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace CinameAsset
{
    public partial class MainForm : Form
    {
        private string connectionString = "Server=localhost;Database=CinemaAssetDB;User Id=sa;Password=1234;";
        private Form currentChildForm = null;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Mặc định hiển thị trang Thông tin hạ tầng
            ShowInfrastructureForm();
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

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }
    }
}
