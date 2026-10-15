using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ConvertingFontApp.ctrl;
using ConvertingFontApp.enums;
using ConvertingFontApp.events;
using ConvertingFontApp.mdl;

namespace ConvertingFontApp.view
{
    public partial class UcFontConverter : UserControl
    {
        private readonly CtrlFontConverter _ctrlConverter;
        private readonly Dictionary<EFontTarget, MdlFontConfig> _fontsRegistry;
        private readonly List<MdlFileItem> _fileList = new List<MdlFileItem>();

        public UcFontConverter()
        {
            InitializeComponent();
            _ctrlConverter = new CtrlFontConverter();
            _ctrlConverter.OnProgressChanged += CtrlConverter_OnProgressChanged;

            _fontsRegistry = MdlFontConfig.GetRegistry();
            PopulateFontDropdown();

            // Default output directory
            string defaultOut = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ConvertingFont_Output");
            txtOutputDir.Text = defaultOut;
        }

        private void PopulateFontDropdown()
        {
            cmbTargetFont.Items.Clear();
            foreach (var kvp in _fontsRegistry)
            {
                cmbTargetFont.Items.Add(kvp.Value.DisplayName);
            }

            if (cmbTargetFont.Items.Count > 0)
            {
                cmbTargetFont.SelectedIndex = 0; // Roboto default
            }
        }

        private MdlFontConfig GetSelectedFont()
        {
            string selectedName = cmbTargetFont.SelectedItem?.ToString();
            foreach (var kvp in _fontsRegistry)
            {
                if (kvp.Value.DisplayName == selectedName)
                    return kvp.Value;
            }
            return _fontsRegistry[EFontTarget.Roboto];
        }

        private void cmbTargetFont_SelectedIndexChanged(object sender, EventArgs e)
        {
            var font = GetSelectedFont();
            if (font != null)
            {
                lblFontDesc.Text = $"[{font.Badge}] {font.Description}";
            }
        }

        private void pnlDropZone_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void pnlDropZone_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                AddFilesToQueue(files);
            }
        }

        private void btnBrowseFiles_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Title = "Pilih Dokumen untuk Dikonversi";
                ofd.Filter = "Semua Dokumen yang Didukung (*.docx;*.pdf;*.xml;*.zip)|*.docx;*.pdf;*.xml;*.zip|Word Document (*.docx)|*.docx|PDF (*.pdf)|*.pdf|XML (*.xml)|*.xml|ZIP Archive (*.zip)|*.zip";
                ofd.Multiselect = true;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    AddFilesToQueue(ofd.FileNames);
                }
            }
        }

        private void AddFilesToQueue(string[] filePaths)
        {
            if (filePaths == null) return;

            foreach (var path in filePaths)
            {
                if (File.Exists(path))
                {
                    // Check duplicate
                    if (_fileList.Exists(f => string.Equals(f.FullPath, path, StringComparison.OrdinalIgnoreCase)))
                        continue;

                    var item = MdlFileItem.FromFilePath(path);
                    if (item.FileType == EFileType.Docx || item.FileType == EFileType.Pdf || item.FileType == EFileType.Xml || item.FileType == EFileType.Zip)
                    {
                        _fileList.Add(item);
                    }
                }
            }

            RefreshGrid();
        }

        private void RefreshGrid()
        {
            dgvFiles.Rows.Clear();
            foreach (var item in _fileList)
            {
                int rowIndex = dgvFiles.Rows.Add(item.FileName, item.FileType.ToString().ToUpperInvariant(), item.FileSizeFormatted, item.StatusMessage);
                var row = dgvFiles.Rows[rowIndex];

                if (item.Status == EProcessStatus.Success)
                    row.Cells[3].Style.ForeColor = Color.Green;
                else if (item.Status == EProcessStatus.Failed)
                    row.Cells[3].Style.ForeColor = Color.Red;
                else if (item.Status == EProcessStatus.Processing)
                    row.Cells[3].Style.ForeColor = Color.DarkOrange;
                else
                    row.Cells[3].Style.ForeColor = Color.Black;
            }

            lblStatus.Text = $"Total antrian: {_fileList.Count} file.";
        }

        private void btnClearList_Click(object sender, EventArgs e)
        {
            _fileList.Clear();
            RefreshGrid();
            progressBar.Value = 0;
            lblStatus.Text = "Antrian telah dibersihkan.";
        }

        private void btnBrowseOutput_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Pilih Folder Hasil Konversi";
                fbd.SelectedPath = txtOutputDir.Text;
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    txtOutputDir.Text = fbd.SelectedPath;
                }
            }
        }

        private void btnOpenOutput_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(txtOutputDir.Text))
            {
                Process.Start("explorer.exe", txtOutputDir.Text);
            }
            else
            {
                MessageBox.Show("Folder output belum ada atau belum dibuat.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void btnConvert_Click(object sender, EventArgs e)
        {
            if (_fileList.Count == 0)
            {
                MessageBox.Show("Silakan pilih file terlebih dahulu.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string outDir = txtOutputDir.Text.Trim();
            if (string.IsNullOrEmpty(outDir))
            {
                MessageBox.Show("Silakan tentukan folder output.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var font = GetSelectedFont();

            btnConvert.Enabled = false;
            btnBrowseFiles.Enabled = false;
            btnClearList.Enabled = false;
            progressBar.Value = 0;

            try
            {
                await _ctrlConverter.ConvertFilesAsync(_fileList, font, outDir);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Terjadi kesalahan: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnConvert.Enabled = true;
                btnBrowseFiles.Enabled = true;
                btnClearList.Enabled = true;
                RefreshGrid();
            }
        }

        private void CtrlConverter_OnProgressChanged(object sender, ProgressEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => CtrlConverter_OnProgressChanged(sender, e)));
                return;
            }

            progressBar.Value = Math.Min(100, Math.Max(0, e.Percentage));
            lblStatus.Text = e.Message;
            RefreshGrid();

            if (e.IsCompleted)
            {
                MessageBox.Show(e.Message, "Konversi Selesai", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
