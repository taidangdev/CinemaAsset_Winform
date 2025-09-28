namespace CinameAsset
{
    partial class InfrastructureManagement
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.guna2Panel2 = new Guna.UI2.WinForms.Guna2Panel();
            this.cmbAssetType = new Guna.UI2.WinForms.Guna2ComboBox();
            this.cmbAuditorium = new Guna.UI2.WinForms.Guna2ComboBox();
            this.guna2Panel3 = new Guna.UI2.WinForms.Guna2Panel();
            this.btnAddSeat = new Guna.UI2.WinForms.Guna2Button();
            this.btnAddAsset = new Guna.UI2.WinForms.Guna2Button();
            this.dgvAssets = new Guna.UI2.WinForms.Guna2DataGridView();
            this.colAssetId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAssetType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnitNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInstalledAt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEdit = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colDelete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.guna2Panel1.SuspendLayout();
            this.guna2Panel2.SuspendLayout();
            this.guna2Panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAssets)).BeginInit();
            this.SuspendLayout();
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.BackColor = System.Drawing.Color.LightSkyBlue;
            this.guna2Panel1.Controls.Add(this.label1);
            this.guna2Panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.guna2Panel1.Location = new System.Drawing.Point(0, 0);
            this.guna2Panel1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Size = new System.Drawing.Size(951, 71);
            this.guna2Panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label1.Location = new System.Drawing.Point(11, 22);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(215, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "Thông tin hạ tầng";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // guna2Panel2
            // 
            this.guna2Panel2.BackColor = System.Drawing.Color.Transparent;
            this.guna2Panel2.BorderRadius = 10;
            this.guna2Panel2.Controls.Add(this.label3);
            this.guna2Panel2.Controls.Add(this.label2);
            this.guna2Panel2.Controls.Add(this.cmbAssetType);
            this.guna2Panel2.Controls.Add(this.cmbAuditorium);
            this.guna2Panel2.FillColor = System.Drawing.Color.White;
            this.guna2Panel2.Location = new System.Drawing.Point(11, 81);
            this.guna2Panel2.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.guna2Panel2.Name = "guna2Panel2";
            this.guna2Panel2.ShadowDecoration.BorderRadius = 10;
            this.guna2Panel2.ShadowDecoration.Depth = 20;
            this.guna2Panel2.ShadowDecoration.Enabled = true;
            this.guna2Panel2.Size = new System.Drawing.Size(926, 93);
            this.guna2Panel2.TabIndex = 1;
            // 
            // cmbAssetType
            // 
            this.cmbAssetType.BackColor = System.Drawing.Color.Transparent;
            this.cmbAssetType.BorderRadius = 8;
            this.cmbAssetType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbAssetType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAssetType.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbAssetType.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbAssetType.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.cmbAssetType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbAssetType.ItemHeight = 30;
            this.cmbAssetType.Location = new System.Drawing.Point(492, 42);
            this.cmbAssetType.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.cmbAssetType.Name = "cmbAssetType";
            this.cmbAssetType.Size = new System.Drawing.Size(389, 36);
            this.cmbAssetType.TabIndex = 3;
            // 
            // cmbAuditorium
            // 
            this.cmbAuditorium.BackColor = System.Drawing.Color.Transparent;
            this.cmbAuditorium.BorderRadius = 8;
            this.cmbAuditorium.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbAuditorium.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAuditorium.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbAuditorium.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbAuditorium.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.cmbAuditorium.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbAuditorium.ItemHeight = 30;
            this.cmbAuditorium.Location = new System.Drawing.Point(27, 42);
            this.cmbAuditorium.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.cmbAuditorium.Name = "cmbAuditorium";
            this.cmbAuditorium.Size = new System.Drawing.Size(389, 36);
            this.cmbAuditorium.TabIndex = 1;
            // 
            // guna2Panel3
            // 
            this.guna2Panel3.BackColor = System.Drawing.Color.Transparent;
            this.guna2Panel3.BorderColor = System.Drawing.Color.White;
            this.guna2Panel3.BorderRadius = 10;
            this.guna2Panel3.Controls.Add(this.btnAddSeat);
            this.guna2Panel3.Controls.Add(this.btnAddAsset);
            this.guna2Panel3.Controls.Add(this.dgvAssets);
            this.guna2Panel3.FillColor = System.Drawing.Color.White;
            this.guna2Panel3.Location = new System.Drawing.Point(11, 184);
            this.guna2Panel3.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.guna2Panel3.Name = "guna2Panel3";
            this.guna2Panel3.ShadowDecoration.BorderRadius = 10;
            this.guna2Panel3.ShadowDecoration.Depth = 20;
            this.guna2Panel3.ShadowDecoration.Enabled = true;
            this.guna2Panel3.Size = new System.Drawing.Size(926, 381);
            this.guna2Panel3.TabIndex = 2;
            this.guna2Panel3.Paint += new System.Windows.Forms.PaintEventHandler(this.guna2Panel3_Paint);
            // 
            // btnAddSeat
            // 
            this.btnAddSeat.BorderRadius = 8;
            this.btnAddSeat.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnAddSeat.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnAddSeat.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnAddSeat.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnAddSeat.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnAddSeat.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddSeat.ForeColor = System.Drawing.Color.White;
            this.btnAddSeat.Location = new System.Drawing.Point(22, 16);
            this.btnAddSeat.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.btnAddSeat.Name = "btnAddSeat";
            this.btnAddSeat.Size = new System.Drawing.Size(224, 45);
            this.btnAddSeat.TabIndex = 2;
            this.btnAddSeat.Text = "Thêm Ghế Vào Phòng";
            this.btnAddSeat.Click += new System.EventHandler(this.btnAddSeat_Click);
            // 
            // btnAddAsset
            // 
            this.btnAddAsset.BorderRadius = 8;
            this.btnAddAsset.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnAddAsset.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnAddAsset.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnAddAsset.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnAddAsset.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnAddAsset.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddAsset.ForeColor = System.Drawing.Color.White;
            this.btnAddAsset.Location = new System.Drawing.Point(279, 16);
            this.btnAddAsset.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.btnAddAsset.Name = "btnAddAsset";
            this.btnAddAsset.Size = new System.Drawing.Size(205, 45);
            this.btnAddAsset.TabIndex = 1;
            this.btnAddAsset.Text = "Lắp Thêm Thiết Bị";
            this.btnAddAsset.Click += new System.EventHandler(this.btnAddAsset_Click);
            // 
            // dgvAssets
            // 
            this.dgvAssets.AllowUserToAddRows = false;
            this.dgvAssets.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgvAssets.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvAssets.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvAssets.ColumnHeadersHeight = 40;
            this.dgvAssets.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgvAssets.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colAssetId,
            this.colAssetType,
            this.colUnitNo,
            this.colStatus,
            this.colInstalledAt,
            this.colEdit,
            this.colDelete});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvAssets.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvAssets.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvAssets.Location = new System.Drawing.Point(5, 71);
            this.dgvAssets.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.dgvAssets.Name = "dgvAssets";
            this.dgvAssets.ReadOnly = true;
            this.dgvAssets.RowHeadersVisible = false;
            this.dgvAssets.RowHeadersWidth = 51;
            this.dgvAssets.RowTemplate.Height = 35;
            this.dgvAssets.Size = new System.Drawing.Size(916, 305);
            this.dgvAssets.TabIndex = 0;
            this.dgvAssets.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvAssets.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvAssets.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvAssets.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvAssets.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvAssets.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvAssets.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvAssets.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.dgvAssets.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvAssets.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvAssets.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvAssets.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgvAssets.ThemeStyle.HeaderStyle.Height = 40;
            this.dgvAssets.ThemeStyle.ReadOnly = true;
            this.dgvAssets.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvAssets.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvAssets.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvAssets.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvAssets.ThemeStyle.RowsStyle.Height = 35;
            this.dgvAssets.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvAssets.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvAssets.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAssets_CellClick);
            // 
            // colAssetId
            // 
            this.colAssetId.HeaderText = "ID";
            this.colAssetId.MinimumWidth = 6;
            this.colAssetId.Name = "colAssetId";
            this.colAssetId.ReadOnly = true;
            this.colAssetId.Visible = false;
            // 
            // colAssetType
            // 
            this.colAssetType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colAssetType.HeaderText = "Loại Thiết Bị";
            this.colAssetType.MinimumWidth = 6;
            this.colAssetType.Name = "colAssetType";
            this.colAssetType.ReadOnly = true;
            // 
            // colUnitNo
            // 
            this.colUnitNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.colUnitNo.DefaultCellStyle = dataGridViewCellStyle3;
            this.colUnitNo.HeaderText = "Số Thứ Tự";
            this.colUnitNo.MinimumWidth = 6;
            this.colUnitNo.Name = "colUnitNo";
            this.colUnitNo.ReadOnly = true;
            // 
            // colStatus
            // 
            this.colStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colStatus.HeaderText = "Trạng Thái";
            this.colStatus.MinimumWidth = 6;
            this.colStatus.Name = "colStatus";
            this.colStatus.ReadOnly = true;
            // 
            // colInstalledAt
            // 
            this.colInstalledAt.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colInstalledAt.HeaderText = "Ngày Lắp Đặt";
            this.colInstalledAt.MinimumWidth = 6;
            this.colInstalledAt.Name = "colInstalledAt";
            this.colInstalledAt.ReadOnly = true;
            // 
            // colEdit
            // 
            this.colEdit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colEdit.HeaderText = "Sửa";
            this.colEdit.MinimumWidth = 6;
            this.colEdit.Name = "colEdit";
            this.colEdit.ReadOnly = true;
            this.colEdit.Text = "Cập Nhật";
            this.colEdit.UseColumnTextForButtonValue = true;
            this.colEdit.Width = 46;
            // 
            // colDelete
            // 
            this.colDelete.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colDelete.HeaderText = "Xóa";
            this.colDelete.MinimumWidth = 6;
            this.colDelete.Name = "colDelete";
            this.colDelete.ReadOnly = true;
            this.colDelete.Text = "Xóa";
            this.colDelete.UseColumnTextForButtonValue = true;
            this.colDelete.Width = 46;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(23, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 22);
            this.label2.TabIndex = 6;
            this.label2.Text = "Phòng chiếu:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(488, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(119, 22);
            this.label3.TabIndex = 7;
            this.label3.Text = "Thoại thiết bị:";
            // 
            // InfrastructureManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(951, 579);
            this.ControlBox = false;
            this.Controls.Add(this.guna2Panel3);
            this.Controls.Add(this.guna2Panel2);
            this.Controls.Add(this.guna2Panel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.MaximizeBox = false;
            this.Name = "InfrastructureManagement";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quản Lý Thông Tin Hạ Tầng";
            this.Load += new System.EventHandler(this.InfrastructureManagement_Load);
            this.guna2Panel1.ResumeLayout(false);
            this.guna2Panel1.PerformLayout();
            this.guna2Panel2.ResumeLayout(false);
            this.guna2Panel2.PerformLayout();
            this.guna2Panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAssets)).EndInit();
            this.ResumeLayout(false);

        }


        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel2;
        private Guna.UI2.WinForms.Guna2ComboBox cmbAuditorium;
        private Guna.UI2.WinForms.Guna2ComboBox cmbAssetType;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel3;
        private Guna.UI2.WinForms.Guna2DataGridView dgvAssets;
        private Guna.UI2.WinForms.Guna2Button btnAddSeat;
        private Guna.UI2.WinForms.Guna2Button btnAddAsset;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAssetId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAssetType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnitNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInstalledAt;
        private System.Windows.Forms.DataGridViewButtonColumn colEdit;
        private System.Windows.Forms.DataGridViewButtonColumn colDelete;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
    }
}
