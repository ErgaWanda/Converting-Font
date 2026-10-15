namespace ConvertingFontApp.view
{
    partial class UcFolderConverter
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
            this.chkRecursive = new System.Windows.Forms.CheckBox();
            this.cmbTargetFont = new System.Windows.Forms.ComboBox();
            this.lblFont = new System.Windows.Forms.Label();
            this.btnBrowseOutput = new System.Windows.Forms.Button();
            this.txtOutputFolder = new System.Windows.Forms.TextBox();
            this.lblOutput = new System.Windows.Forms.Label();
            this.btnBrowseInput = new System.Windows.Forms.Button();
            this.txtInputFolder = new System.Windows.Forms.TextBox();
            this.lblInput = new System.Windows.Forms.Label();
            this.pnlMiddle = new System.Windows.Forms.Panel();
            this.lblInstructions = new System.Windows.Forms.Label();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnOpenOutput = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblStatus = new System.Windows.Forms.Label();
            this.pnlTop.SuspendLayout();
            this.pnlMiddle.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.White;
            this.pnlTop.Controls.Add(this.chkRecursive);
            this.pnlTop.Controls.Add(this.cmbTargetFont);
            this.pnlTop.Controls.Add(this.lblFont);
            this.pnlTop.Controls.Add(this.btnBrowseOutput);
            this.pnlTop.Controls.Add(this.txtOutputFolder);
            this.pnlTop.Controls.Add(this.lblOutput);
            this.pnlTop.Controls.Add(this.btnBrowseInput);
            this.pnlTop.Controls.Add(this.txtInputFolder);
            this.pnlTop.Controls.Add(this.lblInput);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Padding = new System.Windows.Forms.Padding(16);
            this.pnlTop.Size = new System.Drawing.Size(860, 160);
            this.pnlTop.TabIndex = 0;
            // 
            // chkRecursive
            // 
            this.chkRecursive.AutoSize = true;
            this.chkRecursive.Checked = true;
            this.chkRecursive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRecursive.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.chkRecursive.Location = new System.Drawing.Point(375, 90);
            this.chkRecursive.Name = "chkRecursive";
            this.chkRecursive.Size = new System.Drawing.Size(262, 21);
            this.chkRecursive.TabIndex = 8;
            this.chkRecursive.Text = "Proses seluruh sub-folder secara rekursif";
            this.chkRecursive.UseVisualStyleBackColor = true;
            // 
            // cmbTargetFont
            // 
            this.cmbTargetFont.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTargetFont.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cmbTargetFont.FormattingEnabled = true;
            this.cmbTargetFont.Location = new System.Drawing.Point(145, 88);
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
            // btnBrowseOutput
            // 
            this.btnBrowseOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseOutput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.btnBrowseOutput.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.btnBrowseOutput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseOutput.Location = new System.Drawing.Point(744, 48);
            this.btnBrowseOutput.Name = "btnBrowseOutput";
            this.btnBrowseOutput.Size = new System.Drawing.Size(95, 27);
            this.btnBrowseOutput.TabIndex = 5;
            this.btnBrowseOutput.Text = "Pilih...";
            this.btnBrowseOutput.UseVisualStyleBackColor = false;
            this.btnBrowseOutput.Click += new System.EventHandler(this.btnBrowseOutput_Click);
            // 
            // txtOutputFolder
            // 
            this.txtOutputFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutputFolder.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtOutputFolder.Location = new System.Drawing.Point(145, 50);
            this.txtOutputFolder.Name = "txtOutputFolder";
            this.txtOutputFolder.Size = new System.Drawing.Size(585, 24);
            this.txtOutputFolder.TabIndex = 4;
            // 
            // lblOutput
            // 
            this.lblOutput.AutoSize = true;
            this.lblOutput.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblOutput.Location = new System.Drawing.Point(16, 53);
            this.lblOutput.Name = "lblOutput";
            this.lblOutput.Size = new System.Drawing.Size(96, 17);
            this.lblOutput.TabIndex = 3;
            this.lblOutput.Text = "Folder Output:";
            // 
            // btnBrowseInput
            // 
            this.btnBrowseInput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseInput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.btnBrowseInput.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.btnBrowseInput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseInput.Location = new System.Drawing.Point(744, 13);
            this.btnBrowseInput.Name = "btnBrowseInput";
            this.btnBrowseInput.Size = new System.Drawing.Size(95, 27);
            this.btnBrowseInput.TabIndex = 2;
            this.btnBrowseInput.Text = "Pilih...";
            this.btnBrowseInput.UseVisualStyleBackColor = false;
            this.btnBrowseInput.Click += new System.EventHandler(this.btnBrowseInput_Click);
            // 
            // txtInputFolder
            // 
            this.txtInputFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInputFolder.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtInputFolder.Location = new System.Drawing.Point(145, 15);
            this.txtInputFolder.Name = "txtInputFolder";
            this.txtInputFolder.Size = new System.Drawing.Size(585, 24);
            this.txtInputFolder.TabIndex = 1;
            // 
            // lblInput
            // 
            this.lblInput.AutoSize = true;
            this.lblInput.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblInput.Location = new System.Drawing.Point(16, 18);
            this.lblInput.Name = "lblInput";
            this.lblInput.Size = new System.Drawing.Size(86, 17);
            this.lblInput.TabIndex = 0;
            this.lblInput.Text = "Folder Input:";
            // 
            // pnlMiddle
            // 
            this.pnlMiddle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.pnlMiddle.Controls.Add(this.lblInstructions);
            this.pnlMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMiddle.Location = new System.Drawing.Point(0, 160);
            this.pnlMiddle.Name = "pnlMiddle";
            this.pnlMiddle.Padding = new System.Windows.Forms.Padding(24);
            this.pnlMiddle.Size = new System.Drawing.Size(860, 280);
            this.pnlMiddle.TabIndex = 1;
            // 
            // lblInstructions
            // 
            this.lblInstructions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblInstructions.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblInstructions.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(85)))), ((int)(((byte)(105)))));
            this.lblInstructions.Location = new System.Drawing.Point(24, 24);
            this.lblInstructions.Name = "lblInstructions";
            this.lblInstructions.Size = new System.Drawing.Size(812, 232);
            this.lblInstructions.TabIndex = 0;
            this.lblInstructions.Text = "💡 Panduan Konversi Folder Batch:\r\n\r\n1. Pilih folder yang berisi dokumen yang ing" +
    "in Anda konversi (PDF, DOCX, XML, ZIP).\r\n2. Tentukan folder tujuan penyimpanan hasil konversi.\r\n3. Centang opsi 'Proses seluruh sub-folder secara rekursif' jika Anda ingin struktur folder dipertahankan secara utuh.\r\n4. Klik tombol 'Mulai Konversi Folder' di bawah untuk memulai proses batch otomatis.";
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.Color.White;
            this.pnlBottom.Controls.Add(this.btnOpenOutput);
            this.pnlBottom.Controls.Add(this.btnStart);
            this.pnlBottom.Controls.Add(this.progressBar);
            this.pnlBottom.Controls.Add(this.lblStatus);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 440);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Padding = new System.Windows.Forms.Padding(16, 12, 16, 12);
            this.pnlBottom.Size = new System.Drawing.Size(860, 150);
            this.pnlBottom.TabIndex = 2;
            // 
            // btnOpenOutput
            // 
            this.btnOpenOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenOutput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(249)))));
            this.btnOpenOutput.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.btnOpenOutput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenOutput.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnOpenOutput.Location = new System.Drawing.Point(734, 20);
            this.btnOpenOutput.Name = "btnOpenOutput";
            this.btnOpenOutput.Size = new System.Drawing.Size(105, 30);
            this.btnOpenOutput.TabIndex = 6;
            this.btnOpenOutput.Text = "Buka Folder";
            this.btnOpenOutput.UseVisualStyleBackColor = false;
            this.btnOpenOutput.Click += new System.EventHandler(this.btnOpenOutput_Click);
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(185)))), ((int)(((byte)(129)))));
            this.btnStart.FlatAppearance.BorderSize = 0;
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnStart.ForeColor = System.Drawing.Color.White;
            this.btnStart.Location = new System.Drawing.Point(620, 90);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(219, 38);
            this.btnStart.TabIndex = 5;
            this.btnStart.Text = "⚡ Mulai Konversi Folder";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(19, 90);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(585, 22);
            this.progressBar.TabIndex = 4;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.ForeColor = System.Drawing.Color.DimGray;
            this.lblStatus.Location = new System.Drawing.Point(16, 60);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(215, 17);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "Status: Menunggu pilihan folder...";
            // 
            // UcFolderConverter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pnlMiddle);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlTop);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UcFolderConverter";
            this.Size = new System.Drawing.Size(860, 590);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlMiddle.ResumeLayout(false);
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblInput;
        private System.Windows.Forms.TextBox txtInputFolder;
        private System.Windows.Forms.Button btnBrowseInput;
        private System.Windows.Forms.Label lblOutput;
        private System.Windows.Forms.TextBox txtOutputFolder;
        private System.Windows.Forms.Button btnBrowseOutput;
        private System.Windows.Forms.Label lblFont;
        private System.Windows.Forms.ComboBox cmbTargetFont;
        private System.Windows.Forms.CheckBox chkRecursive;
        private System.Windows.Forms.Panel pnlMiddle;
        private System.Windows.Forms.Label lblInstructions;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnOpenOutput;
    }
}
