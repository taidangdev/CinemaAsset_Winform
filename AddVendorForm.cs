using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace CinameAsset
{
    public partial class AddVendorForm : Form
    {
        private string connectionString;
        public event EventHandler VendorAdded;

        public AddVendorForm(string connectionString)
        {
            InitializeComponent();
            this.connectionString = connectionString;
        }

        private void AddVendorForm_Load(object sender, EventArgs e)
        {
            LoadAssetTypes();
        }


        public class AssetTypeItem
        {
            public int AssetTypeId { get; set; }   // từ [key]
            public string Display { get; set; }    // từ [display]

            public override string ToString() => Display; // CheckedListBox dùng ToString() để hiển thị
        }


        private void LoadAssetTypes()
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand(
                    "SELECT [key], [display] FROM dbo.vw_AssetTypes_UI ORDER BY [display];", conn))
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        clbAssetTypes.Items.Clear();

                        while (reader.Read())
                        {
                            var item = new AssetTypeItem
                            {
                                AssetTypeId = reader.GetInt32(0),              // [key]
                                Display = reader.GetString(1)              // [display]
                            };

                            // Add từng item; CheckedListBox sẽ hiển thị theo ToString()
                            clbAssetTypes.Items.Add(item, false);
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
                    
                    using (SqlCommand cmd = new SqlCommand("sp_Vendor_CreateWithCatalog", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        
                        cmd.Parameters.AddWithValue("@name", txtName.Text.Trim());
                        cmd.Parameters.AddWithValue("@phone", string.IsNullOrWhiteSpace(txtPhone.Text) ? (object)DBNull.Value : txtPhone.Text.Trim());
                        cmd.Parameters.AddWithValue("@email", string.IsNullOrWhiteSpace(txtEmail.Text) ? (object)DBNull.Value : txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@address", string.IsNullOrWhiteSpace(txtAddress.Text) ? (object)DBNull.Value : txtAddress.Text.Trim());
                        cmd.Parameters.AddWithValue("@AssetTypesJson", assetTypesJson);
                        
                        SqlParameter vendorIdParam = new SqlParameter("@vendor_id", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(vendorIdParam);
                        
                        cmd.ExecuteNonQuery();
                        
                        int newVendorId = Convert.ToInt32(vendorIdParam.Value);
                        
                        MessageBox.Show($"Đã thêm đối tác mới thành công! (ID: {newVendorId})", "Thông báo", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        VendorAdded?.Invoke(this, EventArgs.Empty);
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm đối tác: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblName_Click(object sender, EventArgs e)
        {

        }

        private void lblAddress_Click(object sender, EventArgs e)
        {

        }
    }
}
