namespace ConvertingFontApp.view
{
    partial class UcBatchMerger
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
            this.pnlTop = new System.Windows.Forms.Panel();
            this.cmbTargetFont = new System.Windows.Forms.ComboBox();
            this.lblFont = new System.Windows.Forms.Label();
            this.btnBrowseData = new System.Windows.Forms.Button();
            this.txtDataPath = new System.Windows.Forms.TextBox();
            this.lblData = new System.Windows.Forms.Label();
            this.btnBrowseTemplate = new System.Windows.Forms.Button();
            this.txtTemplatePath = new System.Windows.Forms.TextBox();
            this.lblTemplate = new System.Windows.Forms.Label();
            this.pnlPreviewHeader = new System.Windows.Forms.Panel();
            this.lblPreviewInfo = new System.Windows.Forms.Label();
            this.lblPreviewTitle = new System.Windows.Forms.Label();
            this.dgvPreview = new System.Windows.Forms.DataGridView();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnOpenOutput = new System.Windows.Forms.Button();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnBrowseOutput = new System.Windows.Forms.Button();
            this.txtOutputDir = new System.Windows.Forms.TextBox();
            this.lblOutputDir = new System.Windows.Forms.Label();
            this.pnlTop.SuspendLayout();
            this.pnlPreviewHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPreview)).BeginInit();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.White;
            this.pnlTop.Controls.Add(this.cmbTargetFont);
            this.pnlTop.Controls.Add(this.lblFont);
            this.pnlTop.Controls.Add(this.btnBrowseData);
            this.pnlTop.Controls.Add(this.txtDataPath);
            this.pnlTop.Controls.Add(this.lblData);
            this.pnlTop.Controls.Add(this.btnBrowseTemplate);
            this.pnlTop.Controls.Add(this.txtTemplatePath);
            this.pnlTop.Controls.Add(this.lblTemplate);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Padding = new System.Windows.Forms.Padding(16);
            this.pnlTop.Size = new System.Drawing.Size(860, 130);
            this.pnlTop.TabIndex = 0;
            // 
            // cmbTargetFont
            // 
            this.cmbTargetFont.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTargetFont.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cmbTargetFont.FormattingEnabled = true;
            this.cmbTargetFont.Location = new System.Drawing.Point(155, 88);
            this.cmbTargetFont.Name = "cmbTargetFont";
            this.cmbTargetFont.Size = new System.Drawing.Size(200, 25);
            this.cmbTargetFont.TabIndex = 7;
            // 
            // lblFont
            // 
            this.lblFont.AutoSize = true;
            this.lblFont.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblFont.Location = new System.Drawing.Point(16, 91);
            this.lblFont.Name = "lblFont";
            this.lblFont.Size = new System.Drawing.Size(109, 17);
            this.lblFont.TabIndex = 6;
            this.lblFont.Text = "Target Tipografi:";
            // 
            // btnBrowseData
            // 
            this.btnBrowseData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseData.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.btnBrowseData.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.btnBrowseData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseData.Location = new System.Drawing.Point(744, 48);
            this.btnBrowseData.Name = "btnBrowseData";
            this.btnBrowseData.Size = new System.Drawing.Size(95, 27);
            this.btnBrowseData.TabIndex = 5;
            this.btnBrowseData.Text = "Pilih Data...";
            this.btnBrowseData.UseVisualStyleBackColor = false;
            this.btnBrowseData.Click += new System.EventHandler(this.btnBrowseData_Click);
            // 
            // txtDataPath
            // 
            this.txtDataPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDataPath.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtDataPath.Location = new System.Drawing.Point(155, 50);
            this.txtDataPath.Name = "txtDataPath";
            this.txtDataPath.ReadOnly = true;
            this.txtDataPath.Size = new System.Drawing.Size(575, 24);
            this.txtDataPath.TabIndex = 4;
            // 
            // lblData
            // 
            this.lblData.AutoSize = true;
            this.lblData.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblData.Location = new System.Drawing.Point(16, 53);
            this.lblData.Name = "lblData";
            this.lblData.Size = new System.Drawing.Size(125, 17);
            this.lblData.TabIndex = 3;
            this.lblData.Text = "Data (CSV/Excel):";
            // 
            // btnBrowseTemplate
            // 
            this.btnBrowseTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseTemplate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.btnBrowseTemplate.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.btnBrowseTemplate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseTemplate.Location = new System.Drawing.Point(744, 13);
            this.btnBrowseTemplate.Name = "btnBrowseTemplate";
            this.btnBrowseTemplate.Size = new System.Drawing.Size(95, 27);
            this.btnBrowseTemplate.TabIndex = 2;
            this.btnBrowseTemplate.Text = "Pilih Template...";
            this.btnBrowseTemplate.UseVisualStyleBackColor = false;
            this.btnBrowseTemplate.Click += new System.EventHandler(this.btnBrowseTemplate_Click);
            // 
            // txtTemplatePath
            // 
            this.txtTemplatePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTemplatePath.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtTemplatePath.Location = new System.Drawing.Point(155, 15);
            this.txtTemplatePath.Name = "txtTemplatePath";
            this.txtTemplatePath.ReadOnly = true;
            this.txtTemplatePath.Size = new System.Drawing.Size(575, 24);
            this.txtTemplatePath.TabIndex = 1;
            // 
            // lblTemplate
            // 
            this.lblTemplate.AutoSize = true;
            this.lblTemplate.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblTemplate.Location = new System.Drawing.Point(16, 18);
            this.lblTemplate.Name = "lblTemplate";
            this.lblTemplate.Size = new System.Drawing.Size(126, 17);
            this.lblTemplate.TabIndex = 0;
            this.lblTemplate.Text = "Master Template:";
            // 
            // pnlPreviewHeader
            // 
            this.pnlPreviewHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.pnlPreviewHeader.Controls.Add(this.lblPreviewInfo);
            this.pnlPreviewHeader.Controls.Add(this.lblPreviewTitle);
            this.pnlPreviewHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlPreviewHeader.Location = new System.Drawing.Point(0, 130);
            this.pnlPreviewHeader.Name = "pnlPreviewHeader";
            this.pnlPreviewHeader.Padding = new System.Windows.Forms.Padding(16, 8, 16, 8);
            this.pnlPreviewHeader.Size = new System.Drawing.Size(860, 36);
            this.pnlPreviewHeader.TabIndex = 1;
            // 
            // lblPreviewInfo
            // 
            this.lblPreviewInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPreviewInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblPreviewInfo.Location = new System.Drawing.Point(450, 9);
            this.lblPreviewInfo.Name = "lblPreviewInfo";
            this.lblPreviewInfo.Size = new System.Drawing.Size(394, 20);
            this.lblPreviewInfo.TabIndex = 1;
            this.lblPreviewInfo.Text = "Belum ada dataset yang dimuat.";
            this.lblPreviewInfo.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblPreviewTitle
            // 
            this.lblPreviewTitle.AutoSize = true;
            this.lblPreviewTitle.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblPreviewTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.lblPreviewTitle.Location = new System.Drawing.Point(16, 9);
            this.lblPreviewTitle.Name = "lblPreviewTitle";
            this.lblPreviewTitle.Size = new System.Drawing.Size(186, 17);
            this.lblPreviewTitle.TabIndex = 0;
            this.lblPreviewTitle.Text = "📊 Pratinjau Dataset Nasabah";
            // 
            // dgvPreview
            // 
            this.dgvPreview.AllowUserToAddRows = false;
            this.dgvPreview.AllowUserToDeleteRows = false;
            this.dgvPreview.BackgroundColor = System.Drawing.Color.White;
            this.dgvPreview.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvPreview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPreview.Location = new System.Drawing.Point(0, 166);
            this.dgvPreview.Name = "dgvPreview";
            this.dgvPreview.ReadOnly = true;
            this.dgvPreview.RowHeadersVisible = false;
            this.dgvPreview.Size = new System.Drawing.Size(860, 274);
            this.dgvPreview.TabIndex = 2;
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.Color.White;
            this.pnlBottom.Controls.Add(this.btnOpenOutput);
            this.pnlBottom.Controls.Add(this.btnGenerate);
            this.pnlBottom.Controls.Add(this.progressBar);
            this.pnlBottom.Controls.Add(this.lblStatus);
            this.pnlBottom.Controls.Add(this.btnBrowseOutput);
            this.pnlBottom.Controls.Add(this.txtOutputDir);
            this.pnlBottom.Controls.Add(this.lblOutputDir);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 440);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Padding = new System.Windows.Forms.Padding(16, 12, 16, 12);
            this.pnlBottom.Size = new System.Drawing.Size(860, 150);
            this.pnlBottom.TabIndex = 3;
            // 
            // btnOpenOutput
            // 
            this.btnOpenOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenOutput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.btnOpenOutput.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.btnOpenOutput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenOutput.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnOpenOutput.Location = new System.Drawing.Point(744, 15);
            this.btnOpenOutput.Name = "btnOpenOutput";
            this.btnOpenOutput.Size = new System.Drawing.Size(95, 27);
            this.btnOpenOutput.TabIndex = 6;
            this.btnOpenOutput.Text = "Buka Folder";
            this.btnOpenOutput.UseVisualStyleBackColor = false;
            this.btnOpenOutput.Click += new System.EventHandler(this.btnOpenOutput_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.btnGenerate.FlatAppearance.BorderSize = 0;
            this.btnGenerate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerate.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnGenerate.ForeColor = System.Drawing.Color.White;
            this.btnGenerate.Location = new System.Drawing.Point(645, 90);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(194, 38);
            this.btnGenerate.TabIndex = 5;
            this.btnGenerate.Text = "🚀 Generate & Export ZIP";
            this.btnGenerate.UseVisualStyleBackColor = false;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(19, 90);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(605, 22);
            this.progressBar.TabIndex = 4;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.ForeColor = System.Drawing.Color.DimGray;
            this.lblStatus.Location = new System.Drawing.Point(16, 60);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(262, 17);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "Status: Menunggu template dan dataset...";
            // 
            // btnBrowseOutput
            // 
            this.btnBrowseOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseOutput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.btnBrowseOutput.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.btnBrowseOutput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseOutput.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnBrowseOutput.Location = new System.Drawing.Point(645, 15);
            this.btnBrowseOutput.Name = "btnBrowseOutput";
            this.btnBrowseOutput.Size = new System.Drawing.Size(90, 27);
            this.btnBrowseOutput.TabIndex = 2;
            this.btnBrowseOutput.Text = "Pilih...";
            this.btnBrowseOutput.UseVisualStyleBackColor = false;
            this.btnBrowseOutput.Click += new System.EventHandler(this.btnBrowseOutput_Click);
            // 
            // txtOutputDir
            // 
            this.txtOutputDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutputDir.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtOutputDir.Location = new System.Drawing.Point(120, 16);
            this.txtOutputDir.Name = "txtOutputDir";
            this.txtOutputDir.Size = new System.Drawing.Size(515, 24);
            this.txtOutputDir.TabIndex = 1;
            // 
            // lblOutputDir
            // 
            this.lblOutputDir.AutoSize = true;
            this.lblOutputDir.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblOutputDir.Location = new System.Drawing.Point(16, 19);
            this.lblOutputDir.Name = "lblOutputDir";
            this.lblOutputDir.Size = new System.Drawing.Size(96, 17);
            this.lblOutputDir.TabIndex = 0;
            this.lblOutputDir.Text = "Folder Output:";
            // 
            // UcBatchMerger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.dgvPreview);
            this.Controls.Add(this.pnlPreviewHeader);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlTop);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UcBatchMerger";
            this.Size = new System.Drawing.Size(860, 590);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlPreviewHeader.ResumeLayout(false);
            this.pnlPreviewHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPreview)).EndInit();
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblTemplate;
        private System.Windows.Forms.TextBox txtTemplatePath;
        private System.Windows.Forms.Button btnBrowseTemplate;
        private System.Windows.Forms.Label lblData;
        private System.Windows.Forms.TextBox txtDataPath;
        private System.Windows.Forms.Button btnBrowseData;
        private System.Windows.Forms.Label lblFont;
        private System.Windows.Forms.ComboBox cmbTargetFont;
        private System.Windows.Forms.Panel pnlPreviewHeader;
        private System.Windows.Forms.Label lblPreviewTitle;
        private System.Windows.Forms.Label lblPreviewInfo;
        private System.Windows.Forms.DataGridView dgvPreview;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Label lblOutputDir;
        private System.Windows.Forms.TextBox txtOutputDir;
        private System.Windows.Forms.Button btnBrowseOutput;
        private System.Windows.Forms.Button btnOpenOutput;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button btnGenerate;
    }
}
