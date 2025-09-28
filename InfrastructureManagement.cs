using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace CinameAsset
{
    public partial class InfrastructureManagement : Form
    {
        private string connectionString = "Server=localhost;Database=CinemaAssetDB;User Id=sa;Password=1234;";

        public InfrastructureManagement()
        {
            InitializeComponent();
            this.cmbAuditorium.SelectionChangeCommitted += cmbAuditorium_SelectionChangeCommitted;
            this.cmbAssetType.SelectionChangeCommitted += cmbAssetType_SelectionChangeCommitted;
            cmbAuditorium.DropDownStyle = ComboBoxStyle.DropDownList;
            CacheSeatTypeId();

        }
        // Cờ chặn event khi đang bind
        private bool _isBinding = false;

        // Helper an toàn khi lấy int từ SelectedValue
        private int GetSelectedInt(ComboBox cb)
        {
            return (cb.SelectedValue != null && int.TryParse(cb.SelectedValue.ToString(), out var v)) ? v : -1;
        }
        private void cmbAuditorium_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (_isBinding) return;
            // Bỏ qua placeholder
            if (GetSelectedInt(cmbAuditorium) == -1) return;
            LoadAssets();
        }


        //lấy id của ghế trong bảng assetType
        private int _seatTypeId = -1;

        private void CacheSeatTypeId()
        {
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("SELECT asset_type_id FROM AssetType WHERE name = N'SEAT';", conn))
            {
                conn.Open();
                var obj = cmd.ExecuteScalar();
                if (obj != null) _seatTypeId = Convert.ToInt32(obj);
            }
        }


        private void cmbAssetType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (_isBinding) return;
            if (GetSelectedInt(cmbAssetType) == -1) { dgvAssets.Rows.Clear(); return; }
            if (GetSelectedInt(cmbAuditorium) == -1) { dgvAssets.Rows.Clear(); return; }
            LoadAssets();
        }


        private void InfrastructureManagement_Load(object sender, EventArgs e)
        {
            LoadAuditoriums();
            LoadAssetTypes();
            LoadAssets();
            dgvAssets.Columns["colInstalledAt"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";

        }

        private void LoadAuditoriums()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT * FROM dbo.vw_Auditoriums_Active ORDER BY name";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    // Thêm placeholder
                    var r = dt.NewRow();
                    r["auditorium_id"] = -1;
                    r["name"] = "-- Chọn phòng chiếu --";
                    dt.Rows.InsertAt(r, 0);

                    // Gán datasource trực tiếp
                    cmbAuditorium.DataSource = dt;
                    cmbAuditorium.DisplayMember = "name";           // hiển thị tên
                    cmbAuditorium.ValueMember = "auditorium_id";  // giữ id để dùng sau

                    // Thêm lựa chọn mặc định
                    cmbAuditorium.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách phòng chiếu: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadAssetTypes()
        {
            
            try
            {
                _isBinding = true;

                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand("SELECT * FROM dbo.vw_AssetTypes_UI ORDER BY [display];", conn))
                {
                    conn.Open();
                    using (var rd = cmd.ExecuteReader())
                    {
                        var dt = new DataTable();
                        dt.Load(rd);

                        // placeholder
                        var row = dt.NewRow();
                        row["key"] = -1;
                        row["display"] = "-- Chọn loại thiết bị --";
                        dt.Rows.InsertAt(row, 0);


                        cmbAssetType.DataSource = null;
                        cmbAssetType.DisplayMember = "display";
                        cmbAssetType.ValueMember = "key";
                        cmbAssetType.DropDownStyle = ComboBoxStyle.DropDownList;
                        cmbAssetType.DataSource = dt;

                        cmbAssetType.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách loại thiết bị: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isBinding = false;
            }
        }

        private void LoadAssets()
        {
            
            int auditoriumId = GetSelectedInt(cmbAuditorium);
           int typeId = GetSelectedInt(cmbAssetType);

            if (auditoriumId == -1 || typeId == -1)
            {
                dgvAssets.Rows.Clear();
                return;
            }

            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    using(var cmd = new SqlCommand("dbo.sp_RoomAssets",conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@auditorium_id",SqlDbType.Int).Value=auditoriumId;
                        cmd.Parameters.Add("@asset_type_id",SqlDbType.Int).Value = typeId;

                        conn.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            dgvAssets.Rows.Clear();

                            while (reader.Read())
                            {
                                int rowIndex = dgvAssets.Rows.Add();
                                var row = dgvAssets.Rows[rowIndex];

                                // Các cột lấy từ proc
                                row.Cells["colAssetId"].Value = reader["asset_id"];
                                row.Cells["colAssetType"].Value = reader["asset_type_display"]?.ToString(); // tên Việt hoá từ view UI
                                row.Cells["colUnitNo"].Value = reader["unit_no"]?.ToString();

                                string status = reader["status"]?.ToString();
                                row.Cells["colStatus"].Value = (status == "OK") ? "Hoạt động" : "Hỏng";

                                if (status == "BROKEN")
                                {
                                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 230, 230);
                                    row.DefaultCellStyle.ForeColor = Color.DarkRed;
                                }
                                else
                                {
                                    row.DefaultCellStyle.BackColor = Color.FromArgb(230, 255, 230);
                                    row.DefaultCellStyle.ForeColor = Color.DarkGreen;
                                }

                                if (reader["installed_at"] != DBNull.Value)
                                {
                                    DateTime installedAt = Convert.ToDateTime(reader["installed_at"]);
                                    //row.Cells["colInstalledAt"].Value = installedAt.ToString("dd/MM/yyyy HH:mm");
                                    row.Cells["colInstalledAt"].Value = installedAt;
                                }
                                else
                                {
                                    row.Cells["colInstalledAt"].Value = "N/A";
                                }

                                // kind: "SEAT" hoặc "ASSET" để biết loại record
                                row.Tag = reader["kind"]?.ToString();
                            }
                        }
                    }                                                                         
                }
               
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách thiết bị: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        

        private void btnAddAsset_Click(object sender, EventArgs e)
        {
            int auditoriumId = GetSelectedInt(cmbAuditorium);
            if (auditoriumId == -1)
            {
                MessageBox.Show("Vui lòng chọn phòng chiếu trước!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            AddAssetControl addAssetControl = new AddAssetControl(auditoriumId, connectionString);
            addAssetControl.AssetAdded += (s, args) => LoadAssets(); // Refresh danh sách sau khi thêm

            Form addAssetForm = new Form
            {
                Text = "Lắp Thêm Thiết Bị",
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,

                AutoScaleMode = AutoScaleMode.Dpi,        // scale đúng theo DPI
                AutoSize = true,                          // form tự resize theo nội dung
                AutoSizeMode = AutoSizeMode.GrowAndShrink // không cho resize nhỏ hơn nội dung
            };

            addAssetForm.Controls.Add(addAssetControl);
            addAssetForm.ShowDialog(this);
        }

        private void btnAddSeat_Click(object sender, EventArgs e)
        {
            int auditoriumId = GetSelectedInt(cmbAuditorium);
            if (auditoriumId == -1)
            {
                MessageBox.Show("Vui lòng chọn phòng chiếu trước!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            AddSeatControl addSeatControl = new AddSeatControl(auditoriumId,connectionString, _seatTypeId);
            addSeatControl.SeatAdded += (s, args) => LoadAssets(); // Refresh danh sách sau khi thêm

            Form addSeatForm = new Form
            {
                Text = "Thêm Ghế Vào Phòng",
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,

                AutoScaleMode = AutoScaleMode.Dpi,        // scale đúng theo DPI
                AutoSize = true,                          // form tự resize theo nội dung
                AutoSizeMode = AutoSizeMode.GrowAndShrink // không cho resize nhỏ hơn nội dung
            };

            addSeatForm.Controls.Add(addSeatControl);
            addSeatForm.ShowDialog(this);
        }

        private void dgvAssets_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvAssets.Rows[e.RowIndex];
            int assetId = Convert.ToInt32(row.Cells["colAssetId"].Value);
            string assetType = row.Tag?.ToString();
            string currentStatus = row.Cells["colStatus"].Value.ToString();

            if (e.ColumnIndex == dgvAssets.Columns["colEdit"].Index)
            {
                // Cập nhật trạng thái thiết bị
                UpdateAssetStatus(assetId, assetType, currentStatus);
            }
            else if (e.ColumnIndex == dgvAssets.Columns["colDelete"].Index)
            {
                // Xóa thiết bị
                DeleteAsset(assetId, assetType);
            }
        }

        private void UpdateAssetStatus(int assetId, string assetType, string currentStatus)
        {
            try
            {
                string message = currentStatus == "Hoạt động" ? 
                    "Bạn có muốn đánh dấu thiết bị này là hỏng?" : 
                    "Bạn có muốn thay thế thiết bị này từ kho?";

                DialogResult result = MessageBox.Show(message, "Xác nhận", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        using (var transaction = conn.BeginTransaction())
                        {
                            try
                            {
                                string procedureName = assetType == "SEAT"
                                    ? (currentStatus == "Hoạt động" ? "sp_Seat_MarkBroken" : "sp_Seat_ReplaceFromWarehouse")
                                    : (currentStatus == "Hoạt động" ? "sp_Asset_MarkBroken" : "sp_Asset_ReplaceFromWarehouse");

                                using (SqlCommand cmd = new SqlCommand(procedureName, conn, transaction))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue(
                                        assetType == "SEAT" ? "@seat_id" : "@asset_id", 
                                        assetId
                                    );
                                    cmd.ExecuteNonQuery();
                                }

                                transaction.Commit();
                                MessageBox.Show("Cập nhật trạng thái thành công!", "Thông báo", 
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadAssets();
                            }
                            catch (Exception)
                            {
                                transaction.Rollback();
                                throw;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật trạng thái: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteAsset(int assetId, string assetType)
        {
            try
            {
                DialogResult result = MessageBox.Show(
                    "Bạn có chắc chắn muốn xóa thiết bị này?\nThiết bị còn tốt sẽ được trả về kho.", 
                    "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        using (var transaction = conn.BeginTransaction())
                        {
                            try
                            {
                                string query = assetType == "SEAT"
                                    ? "sp_Seat_Delete"
                                    : "sp_Asset_Delete";

                                using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue(
                                        assetType == "SEAT" ? "@seat_id" : "@asset_id", 
                                        assetId
                                    );
                                    cmd.ExecuteNonQuery();
                                }

                                transaction.Commit();
                                MessageBox.Show("Xóa thiết bị thành công!", "Thông báo", 
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadAssets();
                            }
                            catch (Exception)
                            {
                                transaction.Rollback();
                                throw;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa thiết bị: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2Panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }

}
