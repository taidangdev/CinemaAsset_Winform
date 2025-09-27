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
            this.lblTitle = new Guna.UI2.WinForms.Guna2HtmlLabel();
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
            this.guna2Panel1.SuspendLayout();
            this.guna2Panel2.SuspendLayout();
            this.guna2Panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWarehouse)).BeginInit();
            this.SuspendLayout();
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.guna2Panel1.Controls.Add(this.lblTitle);
            this.guna2Panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.guna2Panel1.Location = new System.Drawing.Point(0, 0);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Size = new System.Drawing.Size(1150, 60);
            this.guna2Panel1.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(30, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(192, 34);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Quản Lý Tồn Kho";
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
            this.guna2Panel2.Location = new System.Drawing.Point(20, 80);
            this.guna2Panel2.Name = "guna2Panel2";
            this.guna2Panel2.Size = new System.Drawing.Size(1110, 80);
            this.guna2Panel2.TabIndex = 1;
            // 
            // btnReorderSuggestion
            // 
            this.btnReorderSuggestion.BorderRadius = 8;
            this.btnReorderSuggestion.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(0)))));
            this.btnReorderSuggestion.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnReorderSuggestion.ForeColor = System.Drawing.Color.White;
            this.btnReorderSuggestion.Location = new System.Drawing.Point(850, 20);
            this.btnReorderSuggestion.Name = "btnReorderSuggestion";
            this.btnReorderSuggestion.Size = new System.Drawing.Size(120, 40);
            this.btnReorderSuggestion.TabIndex = 4;
            this.btnReorderSuggestion.Text = "Đề xuất nhập";
            this.btnReorderSuggestion.Click += new System.EventHandler(this.btnReorderSuggestion_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.BorderRadius = 8;
            this.btnRefresh.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ForeColor = System.Drawing.Color.White;
            this.btnRefresh.Location = new System.Drawing.Point(990, 20);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 40);
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
            this.chkLowStockOnly.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.chkLowStockOnly.Location = new System.Drawing.Point(450, 30);
            this.chkLowStockOnly.Name = "chkLowStockOnly";
            this.chkLowStockOnly.Size = new System.Drawing.Size(264, 25);
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
            this.cmbAssetType.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbAssetType.ItemHeight = 30;
            this.cmbAssetType.Location = new System.Drawing.Point(30, 35);
            this.cmbAssetType.Name = "cmbAssetType";
            this.cmbAssetType.Size = new System.Drawing.Size(300, 36);
            this.cmbAssetType.TabIndex = 1;
            // 
            // lblAssetType
            // 
            this.lblAssetType.BackColor = System.Drawing.Color.Transparent;
            this.lblAssetType.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblAssetType.Location = new System.Drawing.Point(30, 10);
            this.lblAssetType.Name = "lblAssetType";
            this.lblAssetType.Size = new System.Drawing.Size(107, 23);
            this.lblAssetType.TabIndex = 0;
            this.lblAssetType.Text = "Loại thiết bị:";
            // 
            // guna2Panel3
            // 
            this.guna2Panel3.BackColor = System.Drawing.Color.White;
            this.guna2Panel3.BorderRadius = 10;
            this.guna2Panel3.Controls.Add(this.dgvWarehouse);
            this.guna2Panel3.Location = new System.Drawing.Point(20, 180);
            this.guna2Panel3.Name = "guna2Panel3";
            this.guna2Panel3.Size = new System.Drawing.Size(1110, 520);
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
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvWarehouse.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvWarehouse.Location = new System.Drawing.Point(20, 20);
            this.dgvWarehouse.Name = "dgvWarehouse";
            this.dgvWarehouse.ReadOnly = true;
            this.dgvWarehouse.RowHeadersVisible = false;
            this.dgvWarehouse.RowTemplate.Height = 40;
            this.dgvWarehouse.Size = new System.Drawing.Size(1070, 480);
            this.dgvWarehouse.TabIndex = 0;
            // 
            // colAssetTypeName
            // 
            this.colAssetTypeName.HeaderText = "Loại Thiết Bị";
            this.colAssetTypeName.Name = "colAssetTypeName";
            this.colAssetTypeName.ReadOnly = true;
            this.colAssetTypeName.Width = 200;
            // 
            // colStockQty
            // 
            this.colStockQty.HeaderText = "Số Lượng Tồn";
            this.colStockQty.Name = "colStockQty";
            this.colStockQty.ReadOnly = true;
            this.colStockQty.Width = 150;
            // 
            // colMinStock
            // 
            this.colMinStock.HeaderText = "Tồn Tối Thiểu";
            this.colMinStock.Name = "colMinStock";
            this.colMinStock.ReadOnly = true;
            this.colMinStock.Width = 150;
            // 
            // colStockState
            // 
            this.colStockState.HeaderText = "Trạng Thái";
            this.colStockState.Name = "colStockState";
            this.colStockState.ReadOnly = true;
            this.colStockState.Width = 120;
            // 
            // colShortage
            // 
            this.colShortage.HeaderText = "Thiếu";
            this.colShortage.Name = "colShortage";
            this.colShortage.ReadOnly = true;
            this.colShortage.Width = 100;
            // 
            // colPctOfMin
            // 
            this.colPctOfMin.HeaderText = "% Đạt Min";
            this.colPctOfMin.Name = "colPctOfMin";
            this.colPctOfMin.ReadOnly = true;
            this.colPctOfMin.Width = 120;
            // 
            // WarehouseManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(247)))));
            this.ClientSize = new System.Drawing.Size(1150, 720);
            this.Controls.Add(this.guna2Panel3);
            this.Controls.Add(this.guna2Panel2);
            this.Controls.Add(this.guna2Panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
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
        private Guna.UI2.WinForms.Guna2HtmlLabel lblTitle;
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
    }
}
