namespace CinameAsset
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.guna2Panel2 = new Guna.UI2.WinForms.Guna2Panel();
            this.btnLogout = new Guna.UI2.WinForms.Guna2Button();
            this.btnWarehouse = new Guna.UI2.WinForms.Guna2Button();
            this.btnStatistics = new Guna.UI2.WinForms.Guna2Button();
            this.btnVendors = new Guna.UI2.WinForms.Guna2Button();
            this.btnInfrastructure = new Guna.UI2.WinForms.Guna2Button();
            this.panelContent = new Guna.UI2.WinForms.Guna2Panel();
            this.btnUserManagement = new Guna.UI2.WinForms.Guna2Button();
            this.guna2Panel1.SuspendLayout();
            this.guna2Panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2Panel1.Controls.Add(this.label1);
            this.guna2Panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.guna2Panel1.Location = new System.Drawing.Point(0, 0);
            this.guna2Panel1.Margin = new System.Windows.Forms.Padding(5);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Size = new System.Drawing.Size(1089, 95);
            this.guna2Panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Location = new System.Drawing.Point(12, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(360, 38);
            this.label1.TabIndex = 1;
            this.label1.Text = "Hệ Thống Quản Lý Tài Sản";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // guna2Panel2
            // 
            this.guna2Panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(52)))), ((int)(((byte)(54)))));
            this.guna2Panel2.Controls.Add(this.btnLogout);
            this.guna2Panel2.Controls.Add(this.btnUserManagement);
            this.guna2Panel2.Controls.Add(this.btnWarehouse);
            this.guna2Panel2.Controls.Add(this.btnStatistics);
            this.guna2Panel2.Controls.Add(this.btnVendors);
            this.guna2Panel2.Controls.Add(this.btnInfrastructure);
            this.guna2Panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.guna2Panel2.Location = new System.Drawing.Point(0, 95);
            this.guna2Panel2.Margin = new System.Windows.Forms.Padding(5);
            this.guna2Panel2.Name = "guna2Panel2";
            this.guna2Panel2.Size = new System.Drawing.Size(138, 579);
            this.guna2Panel2.TabIndex = 1;
            // 
            // btnLogout
            // 
            this.btnLogout.BorderRadius = 5;
            this.btnLogout.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnLogout.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnLogout.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnLogout.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnLogout.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnLogout.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnLogout.ImageSize = new System.Drawing.Size(24, 24);
            this.btnLogout.Location = new System.Drawing.Point(0, 452);
            this.btnLogout.Margin = new System.Windows.Forms.Padding(5);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.btnLogout.Size = new System.Drawing.Size(138, 57);
            this.btnLogout.TabIndex = 6;
            this.btnLogout.Text = "Đăng xuất";
            this.btnLogout.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // btnWarehouse
            // 
            this.btnWarehouse.BorderRadius = 5;
            this.btnWarehouse.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnWarehouse.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnWarehouse.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnWarehouse.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnWarehouse.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnWarehouse.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(52)))), ((int)(((byte)(54)))));
            this.btnWarehouse.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnWarehouse.ForeColor = System.Drawing.Color.White;
            this.btnWarehouse.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.btnWarehouse.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnWarehouse.ImageSize = new System.Drawing.Size(24, 24);
            this.btnWarehouse.Location = new System.Drawing.Point(0, 276);
            this.btnWarehouse.Margin = new System.Windows.Forms.Padding(5);
            this.btnWarehouse.Name = "btnWarehouse";
            this.btnWarehouse.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.btnWarehouse.Size = new System.Drawing.Size(138, 85);
            this.btnWarehouse.TabIndex = 3;
            this.btnWarehouse.Text = "Kho Hàng";
            this.btnWarehouse.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnWarehouse.Click += new System.EventHandler(this.btnWarehouse_Click);
            // 
            // btnStatistics
            // 
            this.btnStatistics.BorderRadius = 5;
            this.btnStatistics.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnStatistics.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnStatistics.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnStatistics.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnStatistics.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnStatistics.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(52)))), ((int)(((byte)(54)))));
            this.btnStatistics.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStatistics.ForeColor = System.Drawing.Color.White;
            this.btnStatistics.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.btnStatistics.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnStatistics.ImageSize = new System.Drawing.Size(24, 24);
            this.btnStatistics.Location = new System.Drawing.Point(0, 184);
            this.btnStatistics.Margin = new System.Windows.Forms.Padding(5);
            this.btnStatistics.Name = "btnStatistics";
            this.btnStatistics.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.btnStatistics.Size = new System.Drawing.Size(138, 92);
            this.btnStatistics.TabIndex = 2;
            this.btnStatistics.Text = "Thống Kê Nhập Hàng";
            this.btnStatistics.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnStatistics.Click += new System.EventHandler(this.btnStatistics_Click);
            // 
            // btnVendors
            // 
            this.btnVendors.BorderRadius = 5;
            this.btnVendors.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnVendors.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnVendors.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnVendors.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnVendors.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnVendors.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(52)))), ((int)(((byte)(54)))));
            this.btnVendors.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVendors.ForeColor = System.Drawing.Color.White;
            this.btnVendors.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.btnVendors.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnVendors.ImageSize = new System.Drawing.Size(24, 24);
            this.btnVendors.Location = new System.Drawing.Point(0, 92);
            this.btnVendors.Margin = new System.Windows.Forms.Padding(5);
            this.btnVendors.Name = "btnVendors";
            this.btnVendors.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.btnVendors.Size = new System.Drawing.Size(138, 92);
            this.btnVendors.TabIndex = 1;
            this.btnVendors.Text = "Đối Tác";
            this.btnVendors.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnVendors.Click += new System.EventHandler(this.btnVendors_Click);
            // 
            // btnInfrastructure
            // 
            this.btnInfrastructure.BorderRadius = 5;
            this.btnInfrastructure.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnInfrastructure.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnInfrastructure.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnInfrastructure.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnInfrastructure.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnInfrastructure.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInfrastructure.ForeColor = System.Drawing.Color.White;
            this.btnInfrastructure.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.btnInfrastructure.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnInfrastructure.ImageSize = new System.Drawing.Size(24, 24);
            this.btnInfrastructure.Location = new System.Drawing.Point(0, 0);
            this.btnInfrastructure.Margin = new System.Windows.Forms.Padding(5);
            this.btnInfrastructure.Name = "btnInfrastructure";
            this.btnInfrastructure.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.btnInfrastructure.Size = new System.Drawing.Size(138, 92);
            this.btnInfrastructure.TabIndex = 0;
            this.btnInfrastructure.Text = "Thông Tin Hạ Tầng";
            this.btnInfrastructure.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnInfrastructure.Click += new System.EventHandler(this.btnInfrastructure_Click);
            // 
            // panelContent
            // 
            this.panelContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(138, 95);
            this.panelContent.Margin = new System.Windows.Forms.Padding(5);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(951, 579);
            this.panelContent.TabIndex = 2;
            // 
            // btnUserManagement
            // 
            this.btnUserManagement.BorderRadius = 5;
            this.btnUserManagement.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnUserManagement.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnUserManagement.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnUserManagement.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnUserManagement.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnUserManagement.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUserManagement.ForeColor = System.Drawing.Color.White;
            this.btnUserManagement.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnUserManagement.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnUserManagement.ImageSize = new System.Drawing.Size(24, 24);
            this.btnUserManagement.Location = new System.Drawing.Point(0, 509);
            this.btnUserManagement.Margin = new System.Windows.Forms.Padding(5);
            this.btnUserManagement.Name = "btnUserManagement";
            this.btnUserManagement.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.btnUserManagement.Size = new System.Drawing.Size(138, 70);
            this.btnUserManagement.TabIndex = 5;
            this.btnUserManagement.Text = "Quản lý tài khoản";
            this.btnUserManagement.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnUserManagement.Click += new System.EventHandler(this.btnUserManagement_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1089, 674);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.guna2Panel2);
            this.Controls.Add(this.guna2Panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hệ Thống Quản Lý Tài Sản Rạp Chiếu Phim";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.guna2Panel1.ResumeLayout(false);
            this.guna2Panel1.PerformLayout();
            this.guna2Panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel2;
        private Guna.UI2.WinForms.Guna2Button btnInfrastructure;
        private Guna.UI2.WinForms.Guna2Button btnVendors;
        private Guna.UI2.WinForms.Guna2Button btnStatistics;
        private Guna.UI2.WinForms.Guna2Button btnWarehouse;
        private Guna.UI2.WinForms.Guna2Panel panelContent;
        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2Button btnLogout;
        private Guna.UI2.WinForms.Guna2Button btnUserManagement;
    }
}
