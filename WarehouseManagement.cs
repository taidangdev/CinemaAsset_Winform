using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace CinameAsset
{
    public partial class WarehouseManagement : Form
    {
        private string connectionString;

        public WarehouseManagement(string connectionString)
        {
            InitializeComponent();
            this.connectionString = connectionString;
        }

        private void WarehouseManagement_Load(object sender, EventArgs e)
        {
            LoadAssetTypes();
            LoadWarehouseData();
            this.cmbAssetType.SelectionChangeCommitted += cmbAssetType_SelectionChangeCommitted;
        }
        private void cmbAssetType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            LoadWarehouseData();
        }

        private void LoadAssetTypes()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT asset_type_id, name FROM AssetType ORDER BY name";
                    
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    
                    // Thêm placeholder
                    var r = dt.NewRow();
                    r["asset_type_id"] = -1;
                    r["name"] = "-- Tất cả loại thiết bị --";
                    dt.Rows.InsertAt(r, 0);
                    
                    // Thêm cột display_text
                    dt.Columns.Add("display_text", typeof(string));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            dt.Rows[i]["display_text"] = "-- Tất cả loại thiết bị --";
                        }
                        else
                        {
                            string assetTypeName = dt.Rows[i]["name"].ToString();
                            dt.Rows[i]["display_text"] = GetAssetTypeDisplayName(assetTypeName);
                        }
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

        private void LoadWarehouseData()
        {
            try
            {
                bool onlyLowStock = chkLowStockOnly.Checked;
                int? assetTypeId = null;
                
                int selectedId = GetSelectedInt(cmbAssetType);
                if (selectedId != -1)
                {
                    assetTypeId = selectedId;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.fn_WarehouseReport(@only_low, @asset_type_id, @name_like)", conn))
                    {
                        cmd.Parameters.AddWithValue("@only_low", onlyLowStock ? (object)1 : DBNull.Value);
                        cmd.Parameters.AddWithValue("@asset_type_id", assetTypeId.HasValue ? (object)assetTypeId.Value : DBNull.Value);
                        cmd.Parameters.AddWithValue("@name_like", DBNull.Value);
                        
                        SqlDataReader reader = cmd.ExecuteReader();
                        
                        dgvWarehouse.Rows.Clear();
                        
                        while (reader.Read())
                        {
                            int rowIndex = dgvWarehouse.Rows.Add();
                            DataGridViewRow row = dgvWarehouse.Rows[rowIndex];
                            
                            string stockState = reader["stock_state"].ToString();
                            
                            row.Cells["colAssetTypeName"].Value = GetAssetTypeDisplayName(reader["asset_type_name"].ToString());
                            row.Cells["colStockQty"].Value = reader["stock_qty"];
                            row.Cells["colMinStock"].Value = reader["min_stock"];
                            row.Cells["colStockState"].Value = stockState == "LOW" ? "Sắp hết" : "Đủ";
                            row.Cells["colShortage"].Value = reader["shortage"];
                            row.Cells["colPctOfMin"].Value = Convert.ToDecimal(reader["pct_of_min"]).ToString("F1") + "%";
                            
                            // Tô đỏ các dòng có trạng thái LOW
                            if (stockState == "LOW")
                            {
                                row.DefaultCellStyle.BackColor = Color.FromArgb(255, 235, 238);
                                row.DefaultCellStyle.ForeColor = Color.FromArgb(211, 47, 47);
                                row.Cells["colStockState"].Style.BackColor = Color.FromArgb(244, 67, 54);
                                row.Cells["colStockState"].Style.ForeColor = Color.White;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu tồn kho: {ex.Message}", "Lỗi", 
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

        // Helper an toàn khi lấy int từ SelectedValue
        private int GetSelectedInt(ComboBox cb)
        {
            return (cb.SelectedValue != null && int.TryParse(cb.SelectedValue.ToString(), out var v)) ? v : -1;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadWarehouseData();
        }

        private void chkLowStockOnly_CheckedChanged(object sender, EventArgs e)
        {
            LoadWarehouseData();
        }

        private void btnReorderSuggestion_Click(object sender, EventArgs e)
        {
            ShowReorderSuggestion();
        }

        private void ShowReorderSuggestion()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    
                    using (SqlCommand cmd = new SqlCommand("sp_Warehouse_ReorderSuggestion", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        
                        SqlDataReader reader = cmd.ExecuteReader();
                        
                        string suggestion = "=== ĐỀ XUẤT NHẬP KHO ===\n\n";
                        bool hasLowStock = false;
                        
                        while (reader.Read())
                        {
                            hasLowStock = true;
                            string assetTypeName = GetAssetTypeDisplayName(reader["asset_type_name"].ToString());
                            int stockQty = Convert.ToInt32(reader["stock_qty"]);
                            int minStock = Convert.ToInt32(reader["min_stock"]);
                            int shortage = Convert.ToInt32(reader["shortage"]);
                            string vendorName = reader["vendor_name"] != DBNull.Value ? reader["vendor_name"].ToString() : "Không có đối tác";
                            
                            suggestion += $"• {assetTypeName}:\n";
                            suggestion += $"  - Tồn hiện tại: {stockQty}\n";
                            suggestion += $"  - Tồn tối thiểu: {minStock}\n";
                            suggestion += $"  - Cần nhập: {shortage}\n";
                            suggestion += $"  - Đối tác: {vendorName}\n\n";
                        }
                        
                        if (!hasLowStock)
                        {
                            suggestion += "Tất cả mặt hàng đều đủ tồn kho tối thiểu.\nKhông cần nhập thêm hàng.";
                        }
                        
                        // Hiển thị trong MessageBox hoặc form riêng
                        MessageBox.Show(suggestion, "Đề Xuất Nhập Kho", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tạo đề xuất nhập kho: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
