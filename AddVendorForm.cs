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

        private void LoadAssetTypes()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT asset_type_id, name FROM AssetType ORDER BY name";
                    
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        
                        clbAssetTypes.Items.Clear();
                        
                        while (reader.Read())
                        {
                            AssetTypeItem item = new AssetTypeItem
                            {
                                AssetTypeId = Convert.ToInt32(reader["asset_type_id"]),
                                Name = reader["name"].ToString()
                            };
                            
                            clbAssetTypes.Items.Add(item);
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

    public class AssetTypeItem
    {
        public int AssetTypeId { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return GetDisplayName(Name);
        }

        private string GetDisplayName(string assetTypeName)
        {
            switch (assetTypeName)
            {
                case "SCREEN": return "Màn hình";
                case "SPEAKER": return "Loa";
                case "AIR_CON": return "Máy lạnh";
                case "SEAT": return "Ghế";
                default: return assetTypeName;
            }
        }
    }
}
