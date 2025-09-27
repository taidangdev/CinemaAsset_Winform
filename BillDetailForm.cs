using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CinameAsset
{
    public partial class BillDetailForm : Form
    {
        private string connectionString;
        private int billId;

        public BillDetailForm(string connectionString, int billId)
        {
            InitializeComponent();
            this.connectionString = connectionString;
            this.billId = billId;
        }

        private void BillDetailForm_Load(object sender, EventArgs e)
        {
            LoadBillDetail();
        }

        private void LoadBillDetail()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    
                    using (SqlCommand cmd = new SqlCommand("sp_Bill_GetDetail", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@bill_id", billId);
                        
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Result Set 1: Bill Header
                            if (reader.Read())
                            {
                                lblBillNo.Text = reader["bill_no"].ToString();
                                lblBillDate.Text = Convert.ToDateTime(reader["bill_date"]).ToString("dd/MM/yyyy");
                                lblVendorName.Text = reader["vendor_name"].ToString();
                                lblTotalAmount.Text = Convert.ToDecimal(reader["total_amount"]).ToString("N0") + " VNĐ";
                            }
                            
                            // Result Set 2: Bill Items
                            if (reader.NextResult())
                            {
                                dgvItems.Rows.Clear();
                                
                                while (reader.Read())
                                {
                                    int rowIndex = dgvItems.Rows.Add();
                                    DataGridViewRow row = dgvItems.Rows[rowIndex];
                                    
                                    row.Cells["colAssetTypeName"].Value = GetAssetTypeDisplayName(reader["asset_type_name"].ToString());
                                    row.Cells["colQuantity"].Value = reader["qty"];
                                    row.Cells["colUnitCost"].Value = Convert.ToDecimal(reader["unit_cost"]).ToString("N0") + " VNĐ";
                                    row.Cells["colLineTotal"].Value = Convert.ToDecimal(reader["line_total"]).ToString("N0") + " VNĐ";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải chi tiết hóa đơn: {ex.Message}", "Lỗi", 
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
