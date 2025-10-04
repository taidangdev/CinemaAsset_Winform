using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace CinameAsset
{
    public partial class EditVendorForm : Form
    {
        private string connectionString;
        private int vendorId;
        public event EventHandler VendorUpdated;

        public EditVendorForm(string connectionString, int vendorId)
        {
            InitializeComponent();
            this.connectionString = connectionString;
            this.vendorId = vendorId;
        }

        private void EditVendorForm_Load(object sender, EventArgs e)
        {
            LoadVendorData();
            LoadAssetTypes();
        }

        public class AssetTypeItem
        {
            public int AssetTypeId { get; set; }
            public string Display { get; set; }
            public override string ToString() => Display;
        }

        private void LoadVendorData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM dbo.fn_VendorInfo(@vendor_id)";
                    
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@vendor_id", vendorId);
                        
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtName.Text = reader["name"].ToString();
                                txtPhone.Text = reader["phone"]?.ToString() ?? "";
                                txtEmail.Text = reader["email"]?.ToString() ?? "";
                                txtAddress.Text = reader["address"]?.ToString() ?? "";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thông tin đối tác: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadAssetTypes()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    
                    // Lấy tất cả asset types
                    string queryAllTypes = "SELECT [key], [display] FROM dbo.vw_AssetTypes_UI ORDER BY [display]";
                    using (SqlCommand cmd = new SqlCommand(queryAllTypes, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            clbAssetTypes.Items.Clear();
                            
                            while (reader.Read())
                            {
                                var item = new AssetTypeItem
                                {
                                    AssetTypeId = reader.GetInt32(0),
                                    Display = reader.GetString(1)
                                };
                                clbAssetTypes.Items.Add(item, false);
                            }
                        }
                    }
                    
                    // Lấy asset types hiện tại của vendor và check vào checkbox
                    string queryVendorTypes = "SELECT asset_type_id FROM dbo.fn_VendorCatalog(@vendor_id)";
                    using (SqlCommand cmd = new SqlCommand(queryVendorTypes, conn))
                    {
                        cmd.Parameters.AddWithValue("@vendor_id", vendorId);
                        
                        List<int> vendorAssetTypeIds = new List<int>();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                vendorAssetTypeIds.Add(reader.GetInt32(0));
                            }
                        }
                        
                        // Check các checkbox tương ứng
                        for (int i = 0; i < clbAssetTypes.Items.Count; i++)
                        {
                            AssetTypeItem item = (AssetTypeItem)clbAssetTypes.Items[i];
                            if (vendorAssetTypeIds.Contains(item.AssetTypeId))
                            {
                                clbAssetTypes.SetItemChecked(i, true);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách loại hàng: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                SaveVendor();
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên đối tác!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return false;
            }

            if (clbAssetTypes.CheckedItems.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một loại hàng cung cấp!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void SaveVendor()
        {
            try
            {
                // Tạo JSON cho danh sách loại hàng
                List<object> assetTypes = new List<object>();
                foreach (AssetTypeItem item in clbAssetTypes.CheckedItems)
                {
                    assetTypes.Add(new { asset_type_id = item.AssetTypeId });
                }
                string assetTypesJson = JsonConvert.SerializeObject(assetTypes);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    
                    using (SqlCommand cmd = new SqlCommand("sp_Vendor_UpdateFull", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        
                        cmd.Parameters.AddWithValue("@vendor_id", vendorId);
                        cmd.Parameters.AddWithValue("@name", txtName.Text.Trim());
                        cmd.Parameters.AddWithValue("@phone", string.IsNullOrWhiteSpace(txtPhone.Text) ? (object)DBNull.Value : txtPhone.Text.Trim());
                        cmd.Parameters.AddWithValue("@email", string.IsNullOrWhiteSpace(txtEmail.Text) ? (object)DBNull.Value : txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@address", string.IsNullOrWhiteSpace(txtAddress.Text) ? (object)DBNull.Value : txtAddress.Text.Trim());
                        cmd.Parameters.AddWithValue("@AssetTypesJson", assetTypesJson);
                        
                        cmd.ExecuteNonQuery();
                        
                        MessageBox.Show("Đã cập nhật thông tin đối tác thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        VendorUpdated?.Invoke(this, EventArgs.Empty);
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật đối tác: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
