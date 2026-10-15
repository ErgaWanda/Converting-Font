using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using ConvertingFontApp.ctrl;
using ConvertingFontApp.enums;
using ConvertingFontApp.events;
using ConvertingFontApp.mdl;

namespace ConvertingFontApp.view
{
    public partial class UcFolderConverter : UserControl
    {
        private readonly CtrlFolderConverter _ctrlFolderConverter;
        private readonly Dictionary<EFontTarget, MdlFontConfig> _fontsRegistry;

        public UcFolderConverter()
        {
            InitializeComponent();
            _ctrlFolderConverter = new CtrlFolderConverter();
            _ctrlFolderConverter.OnProgressChanged += CtrlFolderConverter_OnProgressChanged;

            _fontsRegistry = MdlFontConfig.GetRegistry();
            PopulateFontDropdown();

            string defaultOut = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ConvertingFont_Output");
            txtOutputFolder.Text = defaultOut;
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

        private void btnBrowseInput_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Pilih Folder Sumber Berisi Dokumen";
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    txtInputFolder.Text = fbd.SelectedPath;
                    lblStatus.Text = $"Folder input dipilih: {fbd.SelectedPath}";
                }
            }
        }

        private void btnBrowseOutput_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Pilih Folder Hasil Konversi";
                fbd.SelectedPath = txtOutputFolder.Text;
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    txtOutputFolder.Text = fbd.SelectedPath;
                }
            }
        }

        private void btnOpenOutput_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(txtOutputFolder.Text))
            {
                Process.Start("explorer.exe", txtOutputFolder.Text);
            }
            else
            {
                MessageBox.Show("Folder output belum ada atau belum dibuat.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            string inDir = txtInputFolder.Text.Trim();
            string outDir = txtOutputFolder.Text.Trim();

            if (string.IsNullOrEmpty(inDir) || !Directory.Exists(inDir))
            {
                MessageBox.Show("Silakan pilih folder input yang valid.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(outDir))
            {
                MessageBox.Show("Silakan tentukan folder output.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var font = GetSelectedFont();

            btnStart.Enabled = false;
            btnBrowseInput.Enabled = false;
            btnBrowseOutput.Enabled = false;
            progressBar.Value = 0;

            try
            {
                await _ctrlFolderConverter.ConvertFolderAsync(inDir, outDir, font, chkRecursive.Checked);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Terjadi kesalahan: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnStart.Enabled = true;
                btnBrowseInput.Enabled = true;
                btnBrowseOutput.Enabled = true;
            }
        }

        private void CtrlFolderConverter_OnProgressChanged(object sender, ProgressEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => CtrlFolderConverter_OnProgressChanged(sender, e)));
                return;
            }

            progressBar.Value = Math.Min(100, Math.Max(0, e.Percentage));
            lblStatus.Text = e.Message;

            if (e.IsCompleted)
            {
                MessageBox.Show(e.Message, "Konversi Folder Selesai", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
