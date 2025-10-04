using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CinameAsset
{
    public partial class PurchaseStatistics : Form
    {
        private string connectionString;

        public PurchaseStatistics(string connectionString)
        {
            InitializeComponent();
            this.connectionString = connectionString;
        }

        private void PurchaseStatistics_Load(object sender, EventArgs e)
        {
            InitializeDatePickers();
            LoadVendors();
            LoadBillStatistics();
        }

        private void InitializeDatePickers()
        {
            // Mặc định từ đầu tháng đến hôm nay
            dtpFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpTo.Value = DateTime.Now;
        }

        private void LoadVendors()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT DISTINCT vendor_id, vendor_name AS name FROM dbo.fn_VendorCatalog(NULL) ORDER BY vendor_name";
                    
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    
                    // Thêm placeholder
                    var r = dt.NewRow();
                    r["vendor_id"] = -1;
                    r["name"] = "-- Tất cả đối tác --";
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

        private void LoadBillStatistics()
        {
            try
            {
                DateTime? dateFrom = dtpFrom.Value.Date;
                DateTime? dateTo = dtpTo.Value.Date;
                int? vendorId = null;
                
                int selectedId = GetSelectedInt(cmbVendor);
                if (selectedId != -1)
                {
                    vendorId = selectedId;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    
                    // === BƯỚC MỚI: TÍNH TỔNG SỐ HÓA ĐƠN ===
                    using (SqlCommand countCmd = new SqlCommand("SELECT dbo.fn_GetTotalBillCount(@date_from, @date_to, @vendor_id)", conn))
                    {
                        countCmd.Parameters.AddWithValue("@date_from", dateFrom.HasValue ? (object)dateFrom.Value : DBNull.Value);
                        countCmd.Parameters.AddWithValue("@date_to", dateTo.HasValue ? (object)dateTo.Value : DBNull.Value);
                        countCmd.Parameters.AddWithValue("@vendor_id", vendorId.HasValue ? (object)vendorId.Value : DBNull.Value);

                        var result = countCmd.ExecuteScalar();
                        int totalCount = Convert.ToInt32(result);
                        lblTotalBillsCount.Text = $"Tổng số hóa đơn: {totalCount}";
                    }
                    
                    using (SqlCommand cmd = new SqlCommand("sp_PurchaseStats_ListAndTotal", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        
                        cmd.Parameters.AddWithValue("@date_from", dateFrom.HasValue ? (object)dateFrom.Value : DBNull.Value);
                        cmd.Parameters.AddWithValue("@date_to", dateTo.HasValue ? (object)dateTo.Value : DBNull.Value);
                        cmd.Parameters.AddWithValue("@vendor_id", vendorId.HasValue ? (object)vendorId.Value : DBNull.Value);
                        
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Result Set 1: Danh sách hóa đơn
                            dgvBills.Rows.Clear();
                            
                            while (reader.Read())
                            {
                                int rowIndex = dgvBills.Rows.Add();
                                DataGridViewRow row = dgvBills.Rows[rowIndex];
                                
                                row.Cells["colBillNo"].Value = reader["bill_no"];
                                row.Cells["colBillDate"].Value = Convert.ToDateTime(reader["bill_date"]).ToString("dd/MM/yyyy");
                                row.Cells["colVendorName"].Value = reader["vendor_name"];
                                row.Cells["colTotalAmount"].Value = Convert.ToDecimal(reader["total_amount"]).ToString("N0") + " VNĐ";
                                
                                // Lưu bill_id để dùng khi click Detail
                                row.Tag = reader["bill_id"];
                            }
                            
                            // Result Set 2: Tổng chi
                            if (reader.NextResult() && reader.Read())
                            {
                                decimal totalSpent = reader["total_spent"] != DBNull.Value ? 
                                    Convert.ToDecimal(reader["total_spent"]) : 0;
                                lblTotalSpent.Text = totalSpent.ToString("N0") + " VNĐ";
                            }
                            else
                            {
                                lblTotalSpent.Text = "0 VNĐ";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thống kê: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void dgvBills_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // Kiểm tra nếu click vào cột "Chi tiết"
            if (e.ColumnIndex == dgvBills.Columns["colDetail"].Index)
            {
                DataGridViewRow row = dgvBills.Rows[e.RowIndex];
                int billId = Convert.ToInt32(row.Tag);
                
                // Mở form chi tiết hóa đơn
                BillDetailForm detailForm = new BillDetailForm(connectionString, billId);
                detailForm.ShowDialog(this);
            }
        }

        // Helper an toàn khi lấy int từ SelectedValue
        private int GetSelectedInt(ComboBox cb)
        {
            return (cb.SelectedValue != null && int.TryParse(cb.SelectedValue.ToString(), out var v)) ? v : -1;
        }

        private void guna2Panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnRefresh_Click_1(object sender, EventArgs e)
        {
            LoadBillStatistics();
        }
    }
}
