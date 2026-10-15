namespace ConvertingFontApp.view
{
    partial class UcFontConverter
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
            this.lblFontDesc = new System.Windows.Forms.Label();
            this.cmbTargetFont = new System.Windows.Forms.ComboBox();
            this.lblTargetFont = new System.Windows.Forms.Label();
            this.pnlDropZone = new System.Windows.Forms.Panel();
            this.btnClearList = new System.Windows.Forms.Button();
            this.btnBrowseFiles = new System.Windows.Forms.Button();
            this.lblDropHint = new System.Windows.Forms.Label();
            this.dgvFiles = new System.Windows.Forms.DataGridView();
            this.colFileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFileType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFileSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnOpenOutput = new System.Windows.Forms.Button();
            this.btnConvert = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnBrowseOutput = new System.Windows.Forms.Button();
            this.txtOutputDir = new System.Windows.Forms.TextBox();
            this.lblOutputDir = new System.Windows.Forms.Label();
            this.pnlTop.SuspendLayout();
            this.pnlDropZone.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFiles)).BeginInit();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.White;
            this.pnlTop.Controls.Add(this.lblFontDesc);
            this.pnlTop.Controls.Add(this.cmbTargetFont);
            this.pnlTop.Controls.Add(this.lblTargetFont);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Padding = new System.Windows.Forms.Padding(16, 12, 16, 12);
            this.pnlTop.Size = new System.Drawing.Size(860, 68);
            this.pnlTop.TabIndex = 0;
            // 
            // lblFontDesc
            // 
            this.lblFontDesc.AutoSize = true;
            this.lblFontDesc.ForeColor = System.Drawing.Color.DimGray;
            this.lblFontDesc.Location = new System.Drawing.Point(340, 22);
            this.lblFontDesc.Name = "lblFontDesc";
            this.lblFontDesc.Size = new System.Drawing.Size(343, 17);
            this.lblFontDesc.TabIndex = 2;
            this.lblFontDesc.Text = "Font resmi Google dengan keterbacaan tinggi dan seimbang.";
            // 
            // cmbTargetFont
            // 
            this.cmbTargetFont.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTargetFont.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cmbTargetFont.FormattingEnabled = true;
            this.cmbTargetFont.Location = new System.Drawing.Point(135, 18);
            this.cmbTargetFont.Name = "cmbTargetFont";
            this.cmbTargetFont.Size = new System.Drawing.Size(185, 25);
            this.cmbTargetFont.TabIndex = 1;
            this.cmbTargetFont.SelectedIndexChanged += new System.EventHandler(this.cmbTargetFont_SelectedIndexChanged);
            // 
            // lblTargetFont
            // 
            this.lblTargetFont.AutoSize = true;
            this.lblTargetFont.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblTargetFont.Location = new System.Drawing.Point(16, 21);
            this.lblTargetFont.Name = "lblTargetFont";
            this.lblTargetFont.Size = new System.Drawing.Size(109, 17);
            this.lblTargetFont.TabIndex = 0;
            this.lblTargetFont.Text = "Target Tipografi:";
            // 
            // pnlDropZone
            // 
            this.pnlDropZone.AllowDrop = true;
            this.pnlDropZone.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.pnlDropZone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlDropZone.Controls.Add(this.btnClearList);
            this.pnlDropZone.Controls.Add(this.btnBrowseFiles);
            this.pnlDropZone.Controls.Add(this.lblDropHint);
            this.pnlDropZone.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDropZone.Location = new System.Drawing.Point(0, 68);
            this.pnlDropZone.Name = "pnlDropZone";
            this.pnlDropZone.Size = new System.Drawing.Size(860, 95);
            this.pnlDropZone.TabIndex = 1;
            this.pnlDropZone.DragDrop += new System.Windows.Forms.DragEventHandler(this.pnlDropZone_DragDrop);
            this.pnlDropZone.DragEnter += new System.Windows.Forms.DragEventHandler(this.pnlDropZone_DragEnter);
            // 
            // btnClearList
            // 
            this.btnClearList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.btnClearList.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.btnClearList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearList.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnClearList.Location = new System.Drawing.Point(744, 30);
            this.btnClearList.Name = "btnClearList";
            this.btnClearList.Size = new System.Drawing.Size(95, 34);
            this.btnClearList.TabIndex = 2;
            this.btnClearList.Text = "Bersihkan";
            this.btnClearList.UseVisualStyleBackColor = false;
            this.btnClearList.Click += new System.EventHandler(this.btnClearList_Click);
            // 
            // btnBrowseFiles
            // 
            this.btnBrowseFiles.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(99)))), ((int)(((byte)(235)))));
            this.btnBrowseFiles.FlatAppearance.BorderSize = 0;
            this.btnBrowseFiles.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseFiles.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnBrowseFiles.ForeColor = System.Drawing.Color.White;
            this.btnBrowseFiles.Location = new System.Drawing.Point(475, 30);
            this.btnBrowseFiles.Name = "btnBrowseFiles";
            this.btnBrowseFiles.Size = new System.Drawing.Size(120, 34);
            this.btnBrowseFiles.TabIndex = 1;
            this.btnBrowseFiles.Text = "Pilih File...";
            this.btnBrowseFiles.UseVisualStyleBackColor = false;
            this.btnBrowseFiles.Click += new System.EventHandler(this.btnBrowseFiles_Click);
            // 
            // lblDropHint
            // 
            this.lblDropHint.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblDropHint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblDropHint.Location = new System.Drawing.Point(20, 20);
            this.lblDropHint.Name = "lblDropHint";
            this.lblDropHint.Size = new System.Drawing.Size(430, 52);
            this.lblDropHint.TabIndex = 0;
            this.lblDropHint.Text = "📥 Tarik dan letakkan file ke area ini (PDF, DOCX, XML, ZIP)\r\natau gunakan tombol" +
    " di samping untuk memilih file:";
            // 
            // dgvFiles
            // 
            this.dgvFiles.AllowUserToAddRows = false;
            this.dgvFiles.AllowUserToDeleteRows = false;
            this.dgvFiles.BackgroundColor = System.Drawing.Color.White;
            this.dgvFiles.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFiles.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colFileName,
            this.colFileType,
            this.colFileSize,
            this.colStatus});
            this.dgvFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvFiles.Location = new System.Drawing.Point(0, 163);
            this.dgvFiles.Name = "dgvFiles";
            this.dgvFiles.ReadOnly = true;
            this.dgvFiles.RowHeadersVisible = false;
            this.dgvFiles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvFiles.Size = new System.Drawing.Size(860, 277);
            this.dgvFiles.TabIndex = 2;
            // 
            // colFileName
            // 
            this.colFileName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colFileName.HeaderText = "Nama File";
            this.colFileName.Name = "colFileName";
            this.colFileName.ReadOnly = true;
            // 
            // colFileType
            // 
            this.colFileType.HeaderText = "Tipe";
            this.colFileType.Name = "colFileType";
            this.colFileType.ReadOnly = true;
            this.colFileType.Width = 80;
            // 
            // colFileSize
            // 
            this.colFileSize.HeaderText = "Ukuran";
            this.colFileSize.Name = "colFileSize";
            this.colFileSize.ReadOnly = true;
            // 
            // colStatus
            // 
            this.colStatus.HeaderText = "Status";
            this.colStatus.Name = "colStatus";
            this.colStatus.ReadOnly = true;
            this.colStatus.Width = 220;
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.Color.White;
            this.pnlBottom.Controls.Add(this.btnOpenOutput);
            this.pnlBottom.Controls.Add(this.btnConvert);
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
            // btnConvert
            // 
            this.btnConvert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConvert.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.btnConvert.FlatAppearance.BorderSize = 0;
            this.btnConvert.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConvert.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnConvert.ForeColor = System.Drawing.Color.White;
            this.btnConvert.Location = new System.Drawing.Point(674, 90);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(165, 38);
            this.btnConvert.TabIndex = 5;
            this.btnConvert.Text = "⚡ Mulai Konversi";
            this.btnConvert.UseVisualStyleBackColor = false;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(19, 90);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(635, 22);
            this.progressBar.TabIndex = 4;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.ForeColor = System.Drawing.Color.DimGray;
            this.lblStatus.Location = new System.Drawing.Point(16, 60);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(130, 17);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "Status: Siap konversi.";
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
            // UcFontConverter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.dgvFiles);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlDropZone);
            this.Controls.Add(this.pnlTop);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UcFontConverter";
            this.Size = new System.Drawing.Size(860, 590);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlDropZone.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFiles)).EndInit();
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblTargetFont;
        private System.Windows.Forms.ComboBox cmbTargetFont;
        private System.Windows.Forms.Label lblFontDesc;
        private System.Windows.Forms.Panel pnlDropZone;
        private System.Windows.Forms.Label lblDropHint;
        private System.Windows.Forms.Button btnBrowseFiles;
        private System.Windows.Forms.Button btnClearList;
        private System.Windows.Forms.DataGridView dgvFiles;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFileType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFileSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Label lblOutputDir;
        private System.Windows.Forms.TextBox txtOutputDir;
        private System.Windows.Forms.Button btnBrowseOutput;
        private System.Windows.Forms.Button btnOpenOutput;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button btnConvert;
    }
}
