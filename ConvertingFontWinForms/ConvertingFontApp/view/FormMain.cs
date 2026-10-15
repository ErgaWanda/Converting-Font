using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ConvertingFontApp.mdl;

namespace ConvertingFontApp.view
{
    public partial class FormMain : Form
    {
        private UcFontConverter _ucConverter;
        private UcBatchMerger _ucMerger;
        private UcFolderConverter _ucFolder;

        public FormMain()
        {
            InitializeComponent();
            InitializeUserControls();
            ShowConverterView();
        }

        private void InitializeUserControls()
        {
            _ucConverter = new UcFontConverter { Dock = DockStyle.Fill };
            _ucMerger = new UcBatchMerger { Dock = DockStyle.Fill };
            _ucFolder = new UcFolderConverter { Dock = DockStyle.Fill };

            pnlContent.Controls.Add(_ucConverter);
            pnlContent.Controls.Add(_ucMerger);
            pnlContent.Controls.Add(_ucFolder);
        }

        private void SetActiveNavButton(Button activeButton)
        {
            Color activeBg = Color.FromArgb(37, 99, 235);
            Color activeFg = Color.White;
            Color inactiveBg = Color.Transparent;
            Color inactiveFg = Color.FromArgb(71, 85, 105);

            foreach (Control c in pnlNav.Controls)
            {
                if (c is Button btn)
                {
                    if (btn == activeButton)
                    {
                        btn.BackColor = activeBg;
                        btn.ForeColor = activeFg;
                    }
                    else
                    {
                        btn.BackColor = inactiveBg;
                        btn.ForeColor = inactiveFg;
                    }
                }
            }
        }

        private void ShowConverterView()
        {
            _ucConverter.BringToFront();
            SetActiveNavButton(btnNavConverter);
            lblStatusApp.Text = "Modul: Konversi Dokumen (Single & Batch File)";
        }

        private void ShowMergerView()
        {
            _ucMerger.BringToFront();
            SetActiveNavButton(btnNavMerger);
            lblStatusApp.Text = "Modul: Batch Mapping Merger (Template + CSV/Excel)";
        }

        private void ShowFolderView()
        {
            _ucFolder.BringToFront();
            SetActiveNavButton(btnNavFolder);
            lblStatusApp.Text = "Modul: Konversi Folder Batch Lokal";
        }

        private void btnNavConverter_Click(object sender, EventArgs e)
        {
            ShowConverterView();
        }

        private void btnNavMerger_Click(object sender, EventArgs e)
        {
            ShowMergerView();
        }

        private void btnNavFolder_Click(object sender, EventArgs e)
        {
            ShowFolderView();
        }

        private void btnOpenFontFolder_Click(object sender, EventArgs e)
        {
            string fontsDir = MdlFontConfig.AppFontsDirectory;
            if (Directory.Exists(fontsDir))
            {
                Process.Start("explorer.exe", fontsDir);
            }
            else
            {
                MessageBox.Show("Folder font belum tersedia.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
