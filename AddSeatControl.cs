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
        private readonly int seatTypeId;

        public event EventHandler SeatAdded;

        public AddSeatControl(int auditoriumId, string connectionString, int seatTypeId)
        {
            InitializeComponent();
            this.auditoriumId = auditoriumId;
            // Sử dụng connection string động từ SessionManager thay vì tham số
            this.connectionString = SessionManager.GetConnectionString();
            this.seatTypeId = seatTypeId;
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
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand("dbo.sp_Auditorium_GetName", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@auditorium_id", SqlDbType.Int).Value = auditoriumId;
                    conn.Open();
                    var result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        label4.Text = result.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải tên phòng chiếu: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSeatStock()
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand("dbo.sp_Warehouse_GetStockByType", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@asset_type_id", SqlDbType.Int).Value = seatTypeId;

                    conn.Open();
                    var result = cmd.ExecuteScalar();
                    int stockQty = (result == null || result == DBNull.Value) ? 0 : Convert.ToInt32(result);

                    lblStockInfo.Text = $"Tồn kho: {stockQty} ghế";
                    numQuantity.Maximum = Math.Min(stockQty, 100);

                    bool outOfStock = stockQty <= 0;
                    numQuantity.Enabled = !outOfStock;
                    btnAdd.Enabled = !outOfStock;
                    if (outOfStock)
                    {
                        lblStockInfo.ForeColor = System.Drawing.Color.Red;
                        lblStockInfo.Text += " - Không đủ ghế trong kho!";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thông tin tồn kho: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddSeats(int quantity)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand("dbo.sp_Auditorium_AddSeats_Auto", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@auditorium_id", SqlDbType.Int).Value = auditoriumId;
                    cmd.Parameters.Add("@qty", SqlDbType.Int).Value = quantity;

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show($"Đã thêm {quantity} ghế thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    SeatAdded?.Invoke(this, EventArgs.Empty);
                    this.ParentForm?.Close();
                }
            }
            catch (SqlException sqlEx)
            {
                // Lỗi 229: Permission Denied - RBAC chặn Staff thực hiện thao tác
                if (sqlEx.Number == 229 || sqlEx.Message.Contains("permission was denied"))
                {
                    MessageBox.Show(
                        "LỖI PHÂN QUYỀN: Tài khoản của bạn không có quyền thực hiện thao tác quản lý này.",
                        "Truy cập bị từ chối",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Stop);
                }
                else
                {
                    MessageBox.Show($"Lỗi CSDL: {sqlEx.Message}", "Lỗi SQL", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi hệ thống: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }
    }
}
