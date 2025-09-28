using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace CinameAsset
{
    public partial class PurchaseForm : Form
    {
        private string connectionString;
        private List<PurchaseItem> purchaseItems = new List<PurchaseItem>();
        public event EventHandler PurchaseCompleted;

        public PurchaseForm(string connectionString)
        {
            InitializeComponent();
            this.connectionString = connectionString;
        }

        private void PurchaseForm_Load(object sender, EventArgs e)
        {
            LoadVendors();
            UpdateTotal();
        }

        private void LoadVendors()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT vendor_id, name FROM dbo.vw_VendorActiveWithCatalog";
                    
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    
                    // Thêm placeholder
                    var r = dt.NewRow();
                    r["vendor_id"] = -1;
                    r["name"] = "-- Chọn đối tác --";
                    dt.Rows.InsertAt(r, 0);
                    
                    // Gán datasource
                    cmbVendor.DataSource = dt;
                    cmbVendor.DisplayMember = "name";
                    cmbVendor.ValueMember = "vendor_id";
                    cmbVendor.DropDownStyle = ComboBoxStyle.DropDownList;
                    
                    cmbVendor.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách đối tác: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbVendor_SelectedIndexChanged(object sender, EventArgs e)
        {
            int vendorId = GetSelectedInt(cmbVendor);
            if (vendorId != -1)
            {
                LoadAssetTypes(vendorId);
            }
            else
            {
                cmbAssetType.DataSource = null;
                cmbAssetType.Items.Clear();
            }
        }

        private void LoadAssetTypes(int vendorId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT asset_type_id, asset_type_display AS [display]
                                        FROM dbo.vw_VendorCatalogActive
                                        WHERE vendor_id = @vendor_id
                                        ORDER BY asset_type_display;";
                    
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.SelectCommand.Parameters.AddWithValue("@vendor_id", vendorId);
                    da.Fill(dt);
                    
                    // Thêm placeholder
                    var r = dt.NewRow();
                    r["asset_type_id"] = -1;
                    r["display"] = "-- Chọn loại thiết bị --";
                    dt.Rows.InsertAt(r, 0);         
                    
                    // Gán datasource
                    cmbAssetType.DataSource = dt;
                    cmbAssetType.DisplayMember = "display";
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

        // Helper an toàn khi lấy int từ SelectedValue
        private int GetSelectedInt(ComboBox cb)
        {
            return (cb.SelectedValue != null && int.TryParse(cb.SelectedValue.ToString(), out var v)) ? v : -1;
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            if (ValidateItem())
            {
                AddItemToList();
            }
        }

        private bool ValidateItem()
        {
            int assetTypeId = GetSelectedInt(cmbAssetType);
            if (assetTypeId == -1)
            {
                MessageBox.Show("Vui lòng chọn loại thiết bị!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (numQuantity.Value <= 0)
            {
                MessageBox.Show("Số lượng phải lớn hơn 0!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (numUnitCost.Value <= 0)
            {
                MessageBox.Show("Đơn giá phải lớn hơn 0!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void AddItemToList()
        {
            int assetTypeId = GetSelectedInt(cmbAssetType);
            string assetTypeName = cmbAssetType.Text;
            
            PurchaseItem item = new PurchaseItem
            {
                AssetTypeId = assetTypeId,
                AssetTypeName = assetTypeName,
                Quantity = (int)numQuantity.Value,
                UnitCost = numUnitCost.Value,
                LineTotal = numQuantity.Value * numUnitCost.Value
            };

            purchaseItems.Add(item);
            RefreshItemsList();
            ClearItemInputs();
        }

        private void RefreshItemsList()
        {
            dgvItems.Rows.Clear();
            
            foreach (var item in purchaseItems)
            {
                int rowIndex = dgvItems.Rows.Add();
                DataGridViewRow row = dgvItems.Rows[rowIndex];
                
                row.Cells["colAssetType"].Value = item.AssetTypeName;
                row.Cells["colQuantity"].Value = item.Quantity;
                row.Cells["colUnitCost"].Value = item.UnitCost.ToString("N0");
                row.Cells["colLineTotal"].Value = item.LineTotal.ToString("N0");
            }
            
            UpdateTotal();
        }

        private void ClearItemInputs()
        {
            cmbAssetType.SelectedIndex = 0;
            numQuantity.Value = 1;
            numUnitCost.Value = 0;
        }

        private void UpdateTotal()
        {
            decimal total = 0;
            foreach (var item in purchaseItems)
            {
                total += item.LineTotal;
            }
            lblTotalAmount.Text = total.ToString("N0") + " VNĐ";
        }

        private void btnRemoveItem_Click(object sender, EventArgs e)
        {
            if (dgvItems.SelectedRows.Count > 0)
            {
                int selectedIndex = dgvItems.SelectedRows[0].Index;
                purchaseItems.RemoveAt(selectedIndex);
                RefreshItemsList();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn dòng cần xóa!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidatePurchase())
            {
                SavePurchase();
            }
        }

        private bool ValidatePurchase()
        {
            int vendorId = GetSelectedInt(cmbVendor);
            if (vendorId == -1)
            {
                MessageBox.Show("Vui lòng chọn đối tác!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (purchaseItems.Count == 0)
            {
                MessageBox.Show("Vui lòng thêm ít nhất một mặt hàng!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void SavePurchase()
        {
            try
            {
                int vendorId = GetSelectedInt(cmbVendor);

                // Tạo JSON cho danh sách mặt hàng
                List<object> items = new List<object>();
                foreach (var item in purchaseItems)
                {
                    items.Add(new 
                    { 
                        asset_type_id = item.AssetTypeId, 
                        qty = item.Quantity, 
                        unit_cost = item.UnitCost 
                    });
                }
                string itemsJson = JsonConvert.SerializeObject(items);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    
                    using (SqlCommand cmd = new SqlCommand("sp_Vendor_ReceivePurchase", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        
                        cmd.Parameters.AddWithValue("@vendor_id", vendorId);
                        cmd.Parameters.AddWithValue("@ItemsJson", itemsJson);
                        
                        SqlParameter billIdParam = new SqlParameter("@BillId", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(billIdParam);
                        
                        cmd.ExecuteNonQuery();
                        
                        int newBillId = Convert.ToInt32(billIdParam.Value);
                        
                        MessageBox.Show($"Đã lưu hóa đơn nhập hàng thành công! (ID: {newBillId})", "Thông báo", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        PurchaseCompleted?.Invoke(this, EventArgs.Empty);
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu hóa đơn: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

    public class PurchaseItem
    {
        public int AssetTypeId { get; set; }
        public string AssetTypeName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public decimal LineTotal { get; set; }
    }

}
