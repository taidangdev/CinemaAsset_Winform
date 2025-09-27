namespace CinameAsset
{
    partial class BillDetailForm
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
            this.lblTotalAmount = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblTotalLabel = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblVendorName = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblVendorLabel = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblBillDate = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblDateLabel = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblBillNo = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblBillNoLabel = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2Panel3 = new Guna.UI2.WinForms.Guna2Panel();
            this.dgvItems = new Guna.UI2.WinForms.Guna2DataGridView();
            this.colAssetTypeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnitCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLineTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblItemsTitle = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.btnClose = new Guna.UI2.WinForms.Guna2Button();
            this.guna2Panel1.SuspendLayout();
            this.guna2Panel2.SuspendLayout();
            this.guna2Panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).BeginInit();
            this.SuspendLayout();
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(81)))), ((int)(((byte)(181)))));
            this.guna2Panel1.Controls.Add(this.lblTitle);
            this.guna2Panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.guna2Panel1.Location = new System.Drawing.Point(0, 0);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Size = new System.Drawing.Size(900, 80);
            this.guna2Panel1.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(30, 25);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(154, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Chi Tiết Hóa Đơn";
            // 
            // guna2Panel2
            // 
            this.guna2Panel2.BackColor = System.Drawing.Color.White;
            this.guna2Panel2.BorderRadius = 10;
            this.guna2Panel2.Controls.Add(this.lblTotalAmount);
            this.guna2Panel2.Controls.Add(this.lblTotalLabel);
            this.guna2Panel2.Controls.Add(this.lblVendorName);
            this.guna2Panel2.Controls.Add(this.lblVendorLabel);
            this.guna2Panel2.Controls.Add(this.lblBillDate);
            this.guna2Panel2.Controls.Add(this.lblDateLabel);
            this.guna2Panel2.Controls.Add(this.lblBillNo);
            this.guna2Panel2.Controls.Add(this.lblBillNoLabel);
            this.guna2Panel2.Location = new System.Drawing.Point(20, 80);
            this.guna2Panel2.Name = "guna2Panel2";
            this.guna2Panel2.Size = new System.Drawing.Size(860, 140);
            this.guna2Panel2.TabIndex = 1;
            // 
            // lblTotalAmount
            // 
            this.lblTotalAmount.BackColor = System.Drawing.Color.Transparent;
            this.lblTotalAmount.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTotalAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(81)))), ((int)(((byte)(181)))));
            this.lblTotalAmount.Location = new System.Drawing.Point(550, 75);
            this.lblTotalAmount.Name = "lblTotalAmount";
            this.lblTotalAmount.Size = new System.Drawing.Size(13, 27);
            this.lblTotalAmount.TabIndex = 7;
            this.lblTotalAmount.Text = "0";
            // 
            // lblTotalLabel
            // 
            this.lblTotalLabel.BackColor = System.Drawing.Color.Transparent;
            this.lblTotalLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTotalLabel.Location = new System.Drawing.Point(450, 78);
            this.lblTotalLabel.Name = "lblTotalLabel";
            this.lblTotalLabel.Size = new System.Drawing.Size(78, 23);
            this.lblTotalLabel.TabIndex = 6;
            this.lblTotalLabel.Text = "Tổng tiền:";
            // 
            // lblVendorName
            // 
            this.lblVendorName.BackColor = System.Drawing.Color.Transparent;
            this.lblVendorName.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblVendorName.Location = new System.Drawing.Point(550, 25);
            this.lblVendorName.Name = "lblVendorName";
            this.lblVendorName.Size = new System.Drawing.Size(11, 23);
            this.lblVendorName.TabIndex = 5;
            this.lblVendorName.Text = "-";
            // 
            // lblVendorLabel
            // 
            this.lblVendorLabel.BackColor = System.Drawing.Color.Transparent;
            this.lblVendorLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblVendorLabel.Location = new System.Drawing.Point(450, 25);
            this.lblVendorLabel.Name = "lblVendorLabel";
            this.lblVendorLabel.Size = new System.Drawing.Size(60, 23);
            this.lblVendorLabel.TabIndex = 4;
            this.lblVendorLabel.Text = "Đối tác:";
            // 
            // lblBillDate
            // 
            this.lblBillDate.BackColor = System.Drawing.Color.Transparent;
            this.lblBillDate.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblBillDate.Location = new System.Drawing.Point(150, 75);
            this.lblBillDate.Name = "lblBillDate";
            this.lblBillDate.Size = new System.Drawing.Size(11, 23);
            this.lblBillDate.TabIndex = 3;
            this.lblBillDate.Text = "-";
            // 
            // lblDateLabel
            // 
            this.lblDateLabel.BackColor = System.Drawing.Color.Transparent;
            this.lblDateLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblDateLabel.Location = new System.Drawing.Point(30, 75);
            this.lblDateLabel.Name = "lblDateLabel";
            this.lblDateLabel.Size = new System.Drawing.Size(41, 23);
            this.lblDateLabel.TabIndex = 2;
            this.lblDateLabel.Text = "Ngày:";
            // 
            // lblBillNo
            // 
            this.lblBillNo.BackColor = System.Drawing.Color.Transparent;
            this.lblBillNo.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblBillNo.Location = new System.Drawing.Point(150, 25);
            this.lblBillNo.Name = "lblBillNo";
            this.lblBillNo.Size = new System.Drawing.Size(11, 23);
            this.lblBillNo.TabIndex = 1;
            this.lblBillNo.Text = "-";
            // 
            // lblBillNoLabel
            // 
            this.lblBillNoLabel.BackColor = System.Drawing.Color.Transparent;
            this.lblBillNoLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblBillNoLabel.Location = new System.Drawing.Point(30, 25);
            this.lblBillNoLabel.Name = "lblBillNoLabel";
            this.lblBillNoLabel.Size = new System.Drawing.Size(88, 23);
            this.lblBillNoLabel.TabIndex = 0;
            this.lblBillNoLabel.Text = "Số hóa đơn:";
            // 
            // guna2Panel3
            // 
            this.guna2Panel3.BackColor = System.Drawing.Color.White;
            this.guna2Panel3.BorderRadius = 10;
            this.guna2Panel3.Controls.Add(this.dgvItems);
            this.guna2Panel3.Controls.Add(this.lblItemsTitle);
            this.guna2Panel3.Location = new System.Drawing.Point(20, 220);
            this.guna2Panel3.Name = "guna2Panel3";
            this.guna2Panel3.Size = new System.Drawing.Size(860, 380);
            this.guna2Panel3.TabIndex = 2;
            // 
            // dgvItems
            // 
            this.dgvItems.AllowUserToAddRows = false;
            this.dgvItems.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgvItems.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(81)))), ((int)(((byte)(181)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvItems.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvItems.ColumnHeadersHeight = 40;
            this.dgvItems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colAssetTypeName,
            this.colQuantity,
            this.colUnitCost,
            this.colLineTotal});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 11F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvItems.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvItems.Location = new System.Drawing.Point(20, 50);
            this.dgvItems.Name = "dgvItems";
            this.dgvItems.ReadOnly = true;
            this.dgvItems.RowHeadersVisible = false;
            this.dgvItems.RowTemplate.Height = 35;
            this.dgvItems.Size = new System.Drawing.Size(820, 280);
            this.dgvItems.TabIndex = 1;
            // 
            // colAssetTypeName
            // 
            this.colAssetTypeName.HeaderText = "Loại Thiết Bị";
            this.colAssetTypeName.Name = "colAssetTypeName";
            this.colAssetTypeName.ReadOnly = true;
            this.colAssetTypeName.Width = 200;
            // 
            // colQuantity
            // 
            this.colQuantity.HeaderText = "Số Lượng";
            this.colQuantity.Name = "colQuantity";
            this.colQuantity.ReadOnly = true;
            this.colQuantity.Width = 120;
            // 
            // colUnitCost
            // 
            this.colUnitCost.HeaderText = "Đơn Giá";
            this.colUnitCost.Name = "colUnitCost";
            this.colUnitCost.ReadOnly = true;
            this.colUnitCost.Width = 150;
            // 
            // colLineTotal
            // 
            this.colLineTotal.HeaderText = "Thành Tiền";
            this.colLineTotal.Name = "colLineTotal";
            this.colLineTotal.ReadOnly = true;
            this.colLineTotal.Width = 150;
            // 
            // lblItemsTitle
            // 
            this.lblItemsTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblItemsTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblItemsTitle.Location = new System.Drawing.Point(20, 15);
            this.lblItemsTitle.Name = "lblItemsTitle";
            this.lblItemsTitle.Size = new System.Drawing.Size(181, 27);
            this.lblItemsTitle.TabIndex = 0;
            this.lblItemsTitle.Text = "Danh sách mặt hàng:";
            // 
            // btnClose
            // 
            this.btnClose.BorderRadius = 8;
            this.btnClose.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(81)))), ((int)(((byte)(181)))));
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(740, 630);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(140, 45);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Đóng";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // BillDetailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(247)))));
            this.ClientSize = new System.Drawing.Size(900, 700);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.guna2Panel3);
            this.Controls.Add(this.guna2Panel2);
            this.Controls.Add(this.guna2Panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BillDetailForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Chi Tiết Hóa Đơn";
            this.Load += new System.EventHandler(this.BillDetailForm_Load);
            this.guna2Panel1.ResumeLayout(false);
            this.guna2Panel1.PerformLayout();
            this.guna2Panel2.ResumeLayout(false);
            this.guna2Panel2.PerformLayout();
            this.guna2Panel3.ResumeLayout(false);
            this.guna2Panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).EndInit();
            this.ResumeLayout(false);
        }

        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblTitle;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel2;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblTotalAmount;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblTotalLabel;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblVendorName;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblVendorLabel;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblBillDate;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblDateLabel;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblBillNo;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblBillNoLabel;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel3;
        private Guna.UI2.WinForms.Guna2DataGridView dgvItems;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAssetTypeName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnitCost;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLineTotal;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblItemsTitle;
        private Guna.UI2.WinForms.Guna2Button btnClose;
    }
}
