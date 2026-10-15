using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ConvertingFontApp.enums;
using ConvertingFontApp.events;
using ConvertingFontApp.mdl;
using ConvertingFontApp.util;

namespace ConvertingFontApp.ctrl
{
    public class CtrlFolderConverter
    {
        public event EventHandler<ProgressEventArgs> OnProgressChanged;

        private void ReportProgress(int percent, string message, string currentFile = "", bool isCompleted = false, bool hasError = false)
        {
            OnProgressChanged?.Invoke(this, new ProgressEventArgs(percent, message, currentFile, isCompleted, hasError));
        }

        public async Task ConvertFolderAsync(string inputFolder, string outputFolder, MdlFontConfig fontConfig, bool recursive)
        {
            if (!Directory.Exists(inputFolder))
                throw new DirectoryNotFoundException($"Folder input '{inputFolder}' tidak ditemukan.");

            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            ReportProgress(5, "Memindai file dalam folder...");
            var searchOpt = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            var supportedExtensions = new[] { ".docx", ".pdf", ".xml", ".zip" };

            var files = Directory.GetFiles(inputFolder, "*.*", searchOpt)
                                 .Where(f => supportedExtensions.Contains(Path.GetExtension(f).ToLowerInvariant()))
                                 .ToList();

            if (files.Count == 0)
            {
                ReportProgress(100, "Tidak ada file yang didukung (DOCX, PDF, XML, ZIP) di folder ini.", isCompleted: true);
                return;
            }

            ReportProgress(10, $"Ditemukan {files.Count} file. Memeriksa ketersediaan font...");
            await FontResourceUtil.EnsureFontFilesAsync(fontConfig, msg => ReportProgress(10, msg));

            int total = files.Count;
            for (int i = 0; i < total; i++)
            {
                var file = files[i];
                var fileName = Path.GetFileName(file);
                int percent = 10 + (int)(((i + 1) / (double)total) * 90);

                ReportProgress(percent, $"Mengonversi {fileName} ({i + 1}/{total})...", fileName);

                string relative = file.Substring(inputFolder.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                string destFilePath = Path.Combine(outputFolder, relative);
                var destDir = Path.GetDirectoryName(destFilePath);
                if (!Directory.Exists(destDir))
                    Directory.CreateDirectory(destDir);

                string ext = Path.GetExtension(file).ToLowerInvariant();

                await Task.Run(() =>
                {
                    try
                    {
                        if (ext == ".docx")
                        {
                            DocxUtil.ConvertFont(file, destFilePath, fontConfig.FamilyName);
                        }
                        else if (ext == ".xml")
                        {
                            XmlUtil.ConvertFont(file, destFilePath, fontConfig.FamilyName);
                        }
                        else if (ext == ".pdf")
                        {
                            PdfUtil.ConvertFont(file, destFilePath, fontConfig);
                        }
                        else if (ext == ".zip")
                        {
                            var ctrlConv = new CtrlFontConverter();
                            var item = MdlFileItem.FromFilePath(file);
                            // Convert single zip
                            var singleItemList = new System.Collections.Generic.List<MdlFileItem> { item };
                            ctrlConv.ConvertFilesAsync(singleItemList, fontConfig, destDir).Wait();
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error converting {fileName}: {ex.Message}");
                    }
                });
            }

            ReportProgress(100, $"Selesai! {total} file telah diproses ke folder output.", isCompleted: true);
        }
    }
}
