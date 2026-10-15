using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConvertingFontApp.ctrl;
using ConvertingFontApp.enums;
using ConvertingFontApp.events;
using ConvertingFontApp.mdl;
using ConvertingFontApp.util;

namespace ConvertingFontApp.view
{
    public partial class UcBatchMerger : UserControl
    {
        private readonly CtrlBatchMerger _ctrlMerger;
        private readonly Dictionary<EFontTarget, MdlFontConfig> _fontsRegistry;
        private DataTable _currentDataset;

        public UcBatchMerger()
        {
            InitializeComponent();
            _ctrlMerger = new CtrlBatchMerger();
            _ctrlMerger.OnProgressChanged += CtrlMerger_OnProgressChanged;

            _fontsRegistry = MdlFontConfig.GetRegistry();
            PopulateFontDropdown();

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
                cmbTargetFont.SelectedIndex = 0;
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

        private void btnBrowseTemplate_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Title = "Pilih Master Template (XML, PDF, DOCX)";
                ofd.Filter = "Master Template (*.xml;*.pdf;*.docx)|*.xml;*.pdf;*.docx|XML Template (*.xml)|*.xml|PDF Template (*.pdf)|*.pdf|Word Template (*.docx)|*.docx";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtTemplatePath.Text = ofd.FileName;
                }
            }
        }

        private async void btnBrowseData_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Title = "Pilih File Dataset Nasabah (CSV, Excel)";
                ofd.Filter = "Dataset (*.csv;*.xlsx;*.xls)|*.csv;*.xlsx;*.xls|CSV (*.csv)|*.csv|Excel Spreadsheet (*.xlsx;*.xls)|*.xlsx;*.xls";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtDataPath.Text = ofd.FileName;
                    await LoadDataPreviewAsync(ofd.FileName);
                }
            }
        }

        private async Task LoadDataPreviewAsync(string filePath)
        {
            lblPreviewInfo.Text = "Memuat dataset...";
            try
            {
                _currentDataset = await Task.Run(() => ExcelCsvUtil.ReadToDataTable(filePath));
                
                // Show up to 10 rows in preview
                var previewDt = _currentDataset.Clone();
                int rowsToTake = Math.Min(10, _currentDataset.Rows.Count);
                for (int i = 0; i < rowsToTake; i++)
                {
                    previewDt.ImportRow(_currentDataset.Rows[i]);
                }

                dgvPreview.DataSource = previewDt;
                lblPreviewInfo.Text = $"Total data: {_currentDataset.Rows.Count:N0} baris | {_currentDataset.Columns.Count} kolom (Menampilkan {rowsToTake} baris pertama)";
                lblStatus.Text = "Dataset berhasil dimuat. Siap melakukan generate.";
            }
            catch (Exception ex)
            {
                lblPreviewInfo.Text = "Gagal memuat dataset.";
                MessageBox.Show($"Gagal membaca file data: {ex.Message}", "Error Dataset", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBrowseOutput_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Pilih Folder Hasil Batch Merger";
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

        private async void btnGenerate_Click(object sender, EventArgs e)
        {
            string templatePath = txtTemplatePath.Text.Trim();
            string dataPath = txtDataPath.Text.Trim();
            string outDir = txtOutputDir.Text.Trim();

            if (string.IsNullOrEmpty(templatePath) || !File.Exists(templatePath))
            {
                MessageBox.Show("Silakan pilih file Master Template yang valid.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(dataPath) || !File.Exists(dataPath))
            {
                MessageBox.Show("Silakan pilih file Dataset nasabah (CSV/Excel) yang valid.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(outDir))
            {
                MessageBox.Show("Silakan tentukan folder output.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var font = GetSelectedFont();

            btnGenerate.Enabled = false;
            btnBrowseTemplate.Enabled = false;
            btnBrowseData.Enabled = false;
            progressBar.Value = 0;

            try
            {
                string zipResult = await _ctrlMerger.MergeAsync(templatePath, dataPath, outDir, font);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Terjadi kesalahan: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnGenerate.Enabled = true;
                btnBrowseTemplate.Enabled = true;
                btnBrowseData.Enabled = true;
            }
        }

        private void CtrlMerger_OnProgressChanged(object sender, ProgressEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => CtrlMerger_OnProgressChanged(sender, e)));
                return;
            }

            progressBar.Value = Math.Min(100, Math.Max(0, e.Percentage));
            lblStatus.Text = e.Message;

            if (e.IsCompleted)
            {
                MessageBox.Show(e.Message, "Batch Merger Selesai", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
