using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CinameAsset
{
    public partial class VendorManagement : Form
    {
        private string connectionString;

        public VendorManagement(string connectionString)
        {
            InitializeComponent();
            this.connectionString = connectionString;
        }

        private void VendorManagement_Load(object sender, EventArgs e)
        {
            LoadVendors();
        }

        private void LoadVendors()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM vw_VendorActiveWithCatalog ORDER BY name";
                    
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        
                        dgvVendors.Rows.Clear();
                        
                        while (reader.Read())
                        {
                            int rowIndex = dgvVendors.Rows.Add();
                            DataGridViewRow row = dgvVendors.Rows[rowIndex];
                            
                            row.Cells["colVendorId"].Value = reader["vendor_id"];
                            row.Cells["colVendorName"].Value = reader["name"];
                            row.Cells["colPhone"].Value = reader["phone"] ?? "";
                            row.Cells["colEmail"].Value = reader["email"] ?? "";
                            row.Cells["colAddress"].Value = reader["address"] ?? "";
                            row.Cells["colAssetTypes"].Value = reader["asset_types"] ?? "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách đối tác: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddVendor_Click(object sender, EventArgs e)
        {
            AddVendorForm addVendorForm = new AddVendorForm(connectionString);
            addVendorForm.VendorAdded += (s, args) => LoadVendors();
            addVendorForm.ShowDialog(this);
        }

        private void btnPurchase_Click(object sender, EventArgs e)
        {
            PurchaseForm purchaseForm = new PurchaseForm(connectionString);
            purchaseForm.PurchaseCompleted += (s, args) => { /* Có thể refresh nếu cần */ };
            purchaseForm.ShowDialog(this);
        }

        private void dgvVendors_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (e.ColumnIndex == dgvVendors.Columns["colStopCooperation"].Index)
            {
                DataGridViewRow row = dgvVendors.Rows[e.RowIndex];
                int vendorId = Convert.ToInt32(row.Cells["colVendorId"].Value);
                string vendorName = row.Cells["colVendorName"].Value.ToString();

                DialogResult result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn dừng hợp tác với đối tác '{vendorName}'?", 
                    "Xác nhận dừng hợp tác", 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    StopCooperation(vendorId);
                }
            }
        }

        private void StopCooperation(int vendorId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    
                    using (SqlCommand cmd = new SqlCommand("sp_Vendor_StopCooperation", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@vendor_id", vendorId);
                        
                        cmd.ExecuteNonQuery();
                        
                        MessageBox.Show("Đã dừng hợp tác với đối tác thành công!", "Thông báo", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        LoadVendors(); // Refresh danh sách
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi dừng hợp tác: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
