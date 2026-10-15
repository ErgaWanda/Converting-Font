namespace ConvertingFontApp.view
{
    partial class FormMain
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
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.btnOpenFontFolder = new System.Windows.Forms.Button();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlNav = new System.Windows.Forms.Panel();
            this.btnNavFolder = new System.Windows.Forms.Button();
            this.btnNavMerger = new System.Windows.Forms.Button();
            this.btnNavConverter = new System.Windows.Forms.Button();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatusApp = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnlHeader.SuspendLayout();
            this.pnlNav.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.pnlHeader.Controls.Add(this.btnOpenFontFolder);
            this.pnlHeader.Controls.Add(this.lblSubtitle);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Padding = new System.Windows.Forms.Padding(20, 15, 20, 15);
            this.pnlHeader.Size = new System.Drawing.Size(964, 76);
            this.pnlHeader.TabIndex = 0;
            // 
            // btnOpenFontFolder
            // 
            this.btnOpenFontFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenFontFolder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.btnOpenFontFolder.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(65)))), ((int)(((byte)(85)))));
            this.btnOpenFontFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenFontFolder.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnOpenFontFolder.ForeColor = System.Drawing.Color.White;
            this.btnOpenFontFolder.Location = new System.Drawing.Point(824, 20);
            this.btnOpenFontFolder.Name = "btnOpenFontFolder";
            this.btnOpenFontFolder.Size = new System.Drawing.Size(120, 32);
            this.btnOpenFontFolder.TabIndex = 2;
            this.btnOpenFontFolder.Text = "📁 Aset Font";
            this.btnOpenFontFolder.UseVisualStyleBackColor = false;
            this.btnOpenFontFolder.Click += new System.EventHandler(this.btnOpenFontFolder_Click);
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblSubtitle.Location = new System.Drawing.Point(20, 44);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(465, 15);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "Precision Typography & Document Personalization Engine (Word DOCX, PDF, XML, ZIP)" +
    "";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(18, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(471, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Tools Converting Font && Batch Mapping Merger";
            // 
            // pnlNav
            // 
            this.pnlNav.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.pnlNav.Controls.Add(this.btnNavFolder);
            this.pnlNav.Controls.Add(this.btnNavMerger);
            this.pnlNav.Controls.Add(this.btnNavConverter);
            this.pnlNav.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlNav.Location = new System.Drawing.Point(0, 76);
            this.pnlNav.Name = "pnlNav";
            this.pnlNav.Padding = new System.Windows.Forms.Padding(16, 6, 16, 6);
            this.pnlNav.Size = new System.Drawing.Size(964, 46);
            this.pnlNav.TabIndex = 1;
            // 
            // btnNavFolder
            // 
            this.btnNavFolder.BackColor = System.Drawing.Color.Transparent;
            this.btnNavFolder.FlatAppearance.BorderSize = 0;
            this.btnNavFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavFolder.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnNavFolder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.btnNavFolder.Location = new System.Drawing.Point(400, 6);
            this.btnNavFolder.Name = "btnNavFolder";
            this.btnNavFolder.Size = new System.Drawing.Size(185, 34);
            this.btnNavFolder.TabIndex = 2;
            this.btnNavFolder.Text = "📂 Konversi Folder";
            this.btnNavFolder.UseVisualStyleBackColor = false;
            this.btnNavFolder.Click += new System.EventHandler(this.btnNavFolder_Click);
            // 
            // btnNavMerger
            // 
            this.btnNavMerger.BackColor = System.Drawing.Color.Transparent;
            this.btnNavMerger.FlatAppearance.BorderSize = 0;
            this.btnNavMerger.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavMerger.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnNavMerger.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.btnNavMerger.Location = new System.Drawing.Point(205, 6);
            this.btnNavMerger.Name = "btnNavMerger";
            this.btnNavMerger.Size = new System.Drawing.Size(190, 34);
            this.btnNavMerger.TabIndex = 1;
            this.btnNavMerger.Text = "📊 Batch Mapping Merger";
            this.btnNavMerger.UseVisualStyleBackColor = false;
            this.btnNavMerger.Click += new System.EventHandler(this.btnNavMerger_Click);
            // 
            // btnNavConverter
            // 
            this.btnNavConverter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.btnNavConverter.FlatAppearance.BorderSize = 0;
            this.btnNavConverter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavConverter.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnNavConverter.ForeColor = System.Drawing.Color.White;
            this.btnNavConverter.Location = new System.Drawing.Point(16, 6);
            this.btnNavConverter.Name = "btnNavConverter";
            this.btnNavConverter.Size = new System.Drawing.Size(185, 34);
            this.btnNavConverter.TabIndex = 0;
            this.btnNavConverter.Text = "📄 Konversi Dokumen";
            this.btnNavConverter.UseVisualStyleBackColor = false;
            this.btnNavConverter.Click += new System.EventHandler(this.btnNavConverter_Click);
            // 
            // pnlContent
            // 
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(0, 122);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(964, 539);
            this.pnlContent.TabIndex = 2;
            // 
            // statusStrip
            // 
            this.statusStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatusApp});
            this.statusStrip.Location = new System.Drawing.Point(0, 661);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(964, 22);
            this.statusStrip.TabIndex = 3;
            this.statusStrip.Text = "statusStrip1";
            // 
            // lblStatusApp
            // 
            this.lblStatusApp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(116)))), ((int)(((byte)(139)))));
            this.lblStatusApp.Name = "lblStatusApp";
            this.lblStatusApp.Size = new System.Drawing.Size(273, 17);
            this.lblStatusApp.Text = "Aplikasi Siap | Arsitektur MVC .NET Framework 4.8";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(964, 683);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.pnlNav);
            this.Controls.Add(this.pnlHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(900, 650);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tools Converting Font & Batch Mapping Merger";
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlNav.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Button btnOpenFontFolder;
        private System.Windows.Forms.Panel pnlNav;
        private System.Windows.Forms.Button btnNavConverter;
        private System.Windows.Forms.Button btnNavMerger;
        private System.Windows.Forms.Button btnNavFolder;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusApp;
    }
}
