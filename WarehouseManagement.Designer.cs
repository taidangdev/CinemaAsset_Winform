namespace CinameAsset
{
    partial class WarehouseManagement
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2Panel2 = new Guna.UI2.WinForms.Guna2Panel();
            this.btnReorderSuggestion = new Guna.UI2.WinForms.Guna2Button();
            this.btnRefresh = new Guna.UI2.WinForms.Guna2Button();
            this.chkLowStockOnly = new Guna.UI2.WinForms.Guna2CheckBox();
            this.cmbAssetType = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblAssetType = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2Panel3 = new Guna.UI2.WinForms.Guna2Panel();
            this.dgvWarehouse = new Guna.UI2.WinForms.Guna2DataGridView();
            this.colAssetTypeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStockQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMinStock = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStockState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colShortage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPctOfMin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.guna2Panel1.SuspendLayout();
            this.guna2Panel2.SuspendLayout();
            this.guna2Panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWarehouse)).BeginInit();
            this.SuspendLayout();
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.guna2Panel1.Controls.Add(this.label1);
            this.guna2Panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.guna2Panel1.Location = new System.Drawing.Point(0, 0);
            this.guna2Panel1.Margin = new System.Windows.Forms.Padding(4);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Size = new System.Drawing.Size(951, 61);
            this.guna2Panel1.TabIndex = 0;
            // 
            // guna2Panel2
            // 
            this.guna2Panel2.BackColor = System.Drawing.Color.White;
            this.guna2Panel2.BorderRadius = 10;
            this.guna2Panel2.Controls.Add(this.btnReorderSuggestion);
            this.guna2Panel2.Controls.Add(this.btnRefresh);
            this.guna2Panel2.Controls.Add(this.chkLowStockOnly);
            this.guna2Panel2.Controls.Add(this.cmbAssetType);
            this.guna2Panel2.Controls.Add(this.lblAssetType);
            this.guna2Panel2.Location = new System.Drawing.Point(13, 69);
            this.guna2Panel2.Margin = new System.Windows.Forms.Padding(4);
            this.guna2Panel2.Name = "guna2Panel2";
            this.guna2Panel2.Size = new System.Drawing.Size(922, 105);
            this.guna2Panel2.TabIndex = 1;
            // 
            // btnReorderSuggestion
            // 
            this.btnReorderSuggestion.BorderRadius = 8;
            this.btnReorderSuggestion.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(0)))));
            this.btnReorderSuggestion.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReorderSuggestion.ForeColor = System.Drawing.Color.White;
            this.btnReorderSuggestion.Location = new System.Drawing.Point(441, 18);
            this.btnReorderSuggestion.Margin = new System.Windows.Forms.Padding(4);
            this.btnReorderSuggestion.Name = "btnReorderSuggestion";
            this.btnReorderSuggestion.Size = new System.Drawing.Size(179, 48);
            this.btnReorderSuggestion.TabIndex = 4;
            this.btnReorderSuggestion.Text = "Đề xuất nhập";
            this.btnReorderSuggestion.Click += new System.EventHandler(this.btnReorderSuggestion_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.BorderRadius = 8;
            this.btnRefresh.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(677, 17);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(4);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(160, 49);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "Làm mới";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // chkLowStockOnly
            // 
            this.chkLowStockOnly.AutoSize = true;
            this.chkLowStockOnly.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.chkLowStockOnly.CheckedState.BorderRadius = 0;
            this.chkLowStockOnly.CheckedState.BorderThickness = 0;
            this.chkLowStockOnly.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.chkLowStockOnly.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkLowStockOnly.Location = new System.Drawing.Point(489, 74);
            this.chkLowStockOnly.Margin = new System.Windows.Forms.Padding(4);
            this.chkLowStockOnly.Name = "chkLowStockOnly";
            this.chkLowStockOnly.Size = new System.Drawing.Size(323, 27);
            this.chkLowStockOnly.TabIndex = 2;
            this.chkLowStockOnly.Text = "Chỉ hiện thị hàng sắp hết (dưới min)";
            this.chkLowStockOnly.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.chkLowStockOnly.UncheckedState.BorderRadius = 0;
            this.chkLowStockOnly.UncheckedState.BorderThickness = 0;
            this.chkLowStockOnly.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.chkLowStockOnly.CheckedChanged += new System.EventHandler(this.chkLowStockOnly_CheckedChanged);
            // 
            // cmbAssetType
            // 
            this.cmbAssetType.BackColor = System.Drawing.Color.Transparent;
            this.cmbAssetType.BorderRadius = 8;
            this.cmbAssetType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbAssetType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAssetType.FocusedColor = System.Drawing.Color.Empty;
            this.cmbAssetType.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbAssetType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbAssetType.ItemHeight = 30;
            this.cmbAssetType.Location = new System.Drawing.Point(13, 51);
            this.cmbAssetType.Margin = new System.Windows.Forms.Padding(4);
            this.cmbAssetType.Name = "cmbAssetType";
            this.cmbAssetType.Size = new System.Drawing.Size(345, 36);
            this.cmbAssetType.TabIndex = 1;
            // 
            // lblAssetType
            // 
            this.lblAssetType.BackColor = System.Drawing.Color.Transparent;
            this.lblAssetType.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAssetType.Location = new System.Drawing.Point(48, 16);
            this.lblAssetType.Margin = new System.Windows.Forms.Padding(4);
            this.lblAssetType.Name = "lblAssetType";
            this.lblAssetType.Size = new System.Drawing.Size(109, 27);
            this.lblAssetType.TabIndex = 0;
            this.lblAssetType.Text = "Loại thiết bị:";
            // 
            // guna2Panel3
            // 
            this.guna2Panel3.BackColor = System.Drawing.Color.White;
            this.guna2Panel3.BorderRadius = 10;
            this.guna2Panel3.Controls.Add(this.dgvWarehouse);
            this.guna2Panel3.Location = new System.Drawing.Point(13, 182);
            this.guna2Panel3.Margin = new System.Windows.Forms.Padding(4);
            this.guna2Panel3.Name = "guna2Panel3";
            this.guna2Panel3.Size = new System.Drawing.Size(925, 399);
            this.guna2Panel3.TabIndex = 2;
            // 
            // dgvWarehouse
            // 
            this.dgvWarehouse.AllowUserToAddRows = false;
            this.dgvWarehouse.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgvWarehouse.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvWarehouse.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvWarehouse.ColumnHeadersHeight = 45;
            this.dgvWarehouse.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colAssetTypeName,
            this.colStockQty,
            this.colMinStock,
            this.colStockState,
            this.colShortage,
            this.colPctOfMin});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 11F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvWarehouse.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvWarehouse.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvWarehouse.Location = new System.Drawing.Point(4, 21);
            this.dgvWarehouse.Margin = new System.Windows.Forms.Padding(4);
            this.dgvWarehouse.Name = "dgvWarehouse";
            this.dgvWarehouse.ReadOnly = true;
            this.dgvWarehouse.RowHeadersVisible = false;
            this.dgvWarehouse.RowHeadersWidth = 51;
            this.dgvWarehouse.RowTemplate.Height = 40;
            this.dgvWarehouse.Size = new System.Drawing.Size(917, 374);
            this.dgvWarehouse.TabIndex = 0;
            this.dgvWarehouse.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvWarehouse.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvWarehouse.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvWarehouse.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvWarehouse.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvWarehouse.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvWarehouse.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvWarehouse.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.dgvWarehouse.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvWarehouse.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvWarehouse.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvWarehouse.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvWarehouse.ThemeStyle.HeaderStyle.Height = 45;
            this.dgvWarehouse.ThemeStyle.ReadOnly = true;
            this.dgvWarehouse.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvWarehouse.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvWarehouse.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvWarehouse.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvWarehouse.ThemeStyle.RowsStyle.Height = 40;
            this.dgvWarehouse.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvWarehouse.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            // 
            // colAssetTypeName
            // 
            this.colAssetTypeName.HeaderText = "Loại Thiết Bị";
            this.colAssetTypeName.MinimumWidth = 6;
            this.colAssetTypeName.Name = "colAssetTypeName";
            this.colAssetTypeName.ReadOnly = true;
            // 
            // colStockQty
            // 
            this.colStockQty.HeaderText = "Số Lượng Tồn";
            this.colStockQty.MinimumWidth = 6;
            this.colStockQty.Name = "colStockQty";
            this.colStockQty.ReadOnly = true;
            // 
            // colMinStock
            // 
            this.colMinStock.HeaderText = "Tồn Tối Thiểu";
            this.colMinStock.MinimumWidth = 6;
            this.colMinStock.Name = "colMinStock";
            this.colMinStock.ReadOnly = true;
            // 
            // colStockState
            // 
            this.colStockState.HeaderText = "Trạng Thái";
            this.colStockState.MinimumWidth = 6;
            this.colStockState.Name = "colStockState";
            this.colStockState.ReadOnly = true;
            // 
            // colShortage
            // 
            this.colShortage.HeaderText = "Thiếu";
            this.colShortage.MinimumWidth = 6;
            this.colShortage.Name = "colShortage";
            this.colShortage.ReadOnly = true;
            // 
            // colPctOfMin
            // 
            this.colPctOfMin.HeaderText = "% Đạt Min";
            this.colPctOfMin.MinimumWidth = 6;
            this.colPctOfMin.Name = "colPctOfMin";
            this.colPctOfMin.ReadOnly = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(256, 41);
            this.label1.TabIndex = 2;
            this.label1.Text = "Quản Lý Tồn Kho";
            // 
            // WarehouseManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(247)))));
            this.ClientSize = new System.Drawing.Size(951, 579);
            this.Controls.Add(this.guna2Panel3);
            this.Controls.Add(this.guna2Panel2);
            this.Controls.Add(this.guna2Panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "WarehouseManagement";
            this.Text = "Quản Lý Kho Hàng";
            this.Load += new System.EventHandler(this.WarehouseManagement_Load);
            this.guna2Panel1.ResumeLayout(false);
            this.guna2Panel1.PerformLayout();
            this.guna2Panel2.ResumeLayout(false);
            this.guna2Panel2.PerformLayout();
            this.guna2Panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvWarehouse)).EndInit();
            this.ResumeLayout(false);

        }

        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel2;
        private Guna.UI2.WinForms.Guna2Button btnReorderSuggestion;
        private Guna.UI2.WinForms.Guna2Button btnRefresh;
        private Guna.UI2.WinForms.Guna2CheckBox chkLowStockOnly;
        private Guna.UI2.WinForms.Guna2ComboBox cmbAssetType;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblAssetType;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel3;
        private Guna.UI2.WinForms.Guna2DataGridView dgvWarehouse;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAssetTypeName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStockQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMinStock;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStockState;
        private System.Windows.Forms.DataGridViewTextBoxColumn colShortage;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPctOfMin;
        private System.Windows.Forms.Label label1;
    }
}
