using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace CinameAsset
{
    public partial class AddAssetControl : UserControl
    {
        private int auditoriumId;
        private string connectionString;
        
        public event EventHandler AssetAdded;

        public AddAssetControl(int auditoriumId, string connectionString)
        {
            InitializeComponent();
            this.auditoriumId = auditoriumId;
            this.connectionString = connectionString;
        }

        private void AddAssetControl_Load(object sender, EventArgs e)
        {
            LoadAuditoriumName();
            LoadAssetTypes();
        }

        private void LoadAuditoriumName()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT name FROM Auditorium WHERE auditorium_id = @auditorium_id";
                    
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@auditorium_id", auditoriumId);
                        object result = cmd.ExecuteScalar();
                        
                        if (result != null)
                        {
                            lblAuditoriumName.Text = result.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải tên phòng chiếu: {ex.Message}", "Lỗi", 
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
                    
                    // Chỉ lấy các loại thiết bị khác ghế (ghế được quản lý riêng trong bảng Seat)
                    string query = @"SELECT at.asset_type_id, at.name, w.stock_qty
                                   FROM AssetType at
                                   LEFT JOIN Warehouse w ON w.asset_type_id = at.asset_type_id
                                   WHERE at.name != 'SEAT'
                                   ORDER BY at.name";
                    
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    
                    // Thêm placeholder
                    var r = dt.NewRow();
                    r["asset_type_id"] = -1;
                    r["name"] = "-- Chọn loại thiết bị --";
                    r["stock_qty"] = 0;
                    dt.Rows.InsertAt(r, 0);
                    
                    // Cập nhật display text cho các dòng khác
                    for (int i = 1; i < dt.Rows.Count; i++)
                    {
                        string assetTypeName = dt.Rows[i]["name"].ToString();
                        int stockQty = dt.Rows[i]["stock_qty"] != DBNull.Value ? Convert.ToInt32(dt.Rows[i]["stock_qty"]) : 0;
                        string displayText = $"{GetAssetTypeDisplayName(assetTypeName)} (Kho: {stockQty})";
                        
                        // Thêm cột display_text
                        if (!dt.Columns.Contains("display_text"))
                        {
                            dt.Columns.Add("display_text", typeof(string));
                        }
                        dt.Rows[i]["display_text"] = displayText;
                    }
                    
                    // Gán datasource
                    cmbAssetType.DataSource = dt;
                    cmbAssetType.DisplayMember = "display_text";
                    cmbAssetType.ValueMember = "asset_type_id";
                    cmbAssetType.DropDownStyle = ComboBoxStyle.DropDownList;
                    
                    cmbAssetType.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách loại thiết bị: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetAssetTypeDisplayName(string assetTypeName)
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

        private void AddAssets(int assetTypeId, int quantity)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    
                    // Tạo JSON cho danh sách thiết bị cần thêm
                    var items = new[] { new { asset_type_id = assetTypeId, qty = quantity } };
                    string itemsJson = JsonConvert.SerializeObject(items);
                    
                    using (SqlCommand cmd = new SqlCommand("sp_Auditorium_AddAssets", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@auditorium_id", auditoriumId);
                        cmd.Parameters.AddWithValue("@ItemsJson", itemsJson);
                        
                        cmd.ExecuteNonQuery();
                        
                        MessageBox.Show($"Đã thêm {quantity} thiết bị thành công!", "Thông báo", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        // Trigger event để form cha refresh danh sách
                        AssetAdded?.Invoke(this, EventArgs.Empty);
                        
                        // Đóng form
                        this.ParentForm?.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm thiết bị: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.ParentForm?.Close();
        }

        private void guna2Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            int assetTypeId = GetSelectedInt(cmbAssetType);
            if (assetTypeId != -1)
            {
                int quantity = (int)numQuantity.Value;
                AddAssets(assetTypeId, quantity);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn loại thiết bị!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Helper an toàn khi lấy int từ SelectedValue
        private int GetSelectedInt(ComboBox cb)
        {
            return (cb.SelectedValue != null && int.TryParse(cb.SelectedValue.ToString(), out var v)) ? v : -1;
        }
    }
}
