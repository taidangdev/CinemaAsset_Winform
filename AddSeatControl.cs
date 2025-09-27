using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace CinameAsset
{
    public partial class AddSeatControl : UserControl
    {
        private int auditoriumId;
        private string connectionString;
        
        public event EventHandler SeatAdded;

        public AddSeatControl(int auditoriumId, string connectionString)
        {
            InitializeComponent();
            this.auditoriumId = auditoriumId;
            this.connectionString = connectionString;
        }

        private void AddSeatControl_Load(object sender, EventArgs e)
        {
            LoadAuditoriumName();
            LoadSeatStock();
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

        private void LoadSeatStock()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    
                    // Lấy số lượng ghế trong kho
                    string query = @"SELECT ISNULL(w.stock_qty, 0) as stock_qty
                                   FROM AssetType at
                                   LEFT JOIN Warehouse w ON w.asset_type_id = at.asset_type_id
                                   WHERE at.name = 'SEAT'";
                    
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        object result = cmd.ExecuteScalar();
                        int stockQty = result != null ? Convert.ToInt32(result) : 0;
                        
                        lblStockInfo.Text = $"Tồn kho: {stockQty} ghế";
                        
                        // Cập nhật max value cho NumericUpDown
                        numQuantity.Maximum = Math.Min(stockQty, 100); // Tối đa 100 ghế hoặc số tồn kho
                        
                        if (stockQty == 0)
                        {
                            numQuantity.Enabled = false;
                            btnAdd.Enabled = false;
                            lblStockInfo.ForeColor = System.Drawing.Color.Red;
                            lblStockInfo.Text += " - Không đủ ghế trong kho!";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thông tin tồn kho: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddSeats(int quantity)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    
                    using (SqlCommand cmd = new SqlCommand("sp_Auditorium_AddSeats_Auto", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@auditorium_id", auditoriumId);
                        cmd.Parameters.AddWithValue("@qty", quantity);
                        
                        cmd.ExecuteNonQuery();
                        
                        MessageBox.Show($"Đã thêm {quantity} ghế thành công!", "Thông báo", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        // Trigger event để form cha refresh danh sách
                        SeatAdded?.Invoke(this, EventArgs.Empty);
                        
                        // Đóng form
                        this.ParentForm?.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm ghế: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            int quantity = (int)numQuantity.Value;
            
            if (quantity <= 0)
            {
                MessageBox.Show("Số lượng ghế phải lớn hơn 0!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Xác nhận trước khi thêm
            DialogResult result = MessageBox.Show(
                $"Bạn có chắc chắn muốn thêm {quantity} ghế vào phòng chiếu?\n\n" +
                "Ghế sẽ được tự động sắp xếp theo thứ tự A1, A2, A3... (mỗi hàng 10 ghế)",
                "Xác nhận thêm ghế",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                AddSeats(quantity);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.ParentForm?.Close();
        }

        private void numQuantity_ValueChanged(object sender, EventArgs e)
        {
            // Cập nhật thông tin khi thay đổi số lượng
            int quantity = (int)numQuantity.Value;
            lblQuantityInfo.Text = $"Sẽ thêm {quantity} ghế";
        }
    }
}
