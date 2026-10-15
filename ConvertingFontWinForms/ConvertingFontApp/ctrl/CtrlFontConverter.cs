using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ConvertingFontApp.enums;
using ConvertingFontApp.events;
using ConvertingFontApp.mdl;
using ConvertingFontApp.util;

namespace ConvertingFontApp.ctrl
{
    public class CtrlFontConverter
    {
        public event EventHandler<ProgressEventArgs> OnProgressChanged;

        private void ReportProgress(int percent, string message, string currentFile = "", bool isCompleted = false, bool hasError = false)
        {
            OnProgressChanged?.Invoke(this, new ProgressEventArgs(percent, message, currentFile, isCompleted, hasError));
        }

        public async Task ConvertFilesAsync(List<MdlFileItem> fileItems, MdlFontConfig fontConfig, string outputDirectory)
        {
            if (fileItems == null || fileItems.Count == 0)
            {
                ReportProgress(0, "Tidak ada file yang dipilih.", isCompleted: true, hasError: true);
                return;
            }

            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Ensure fonts
            ReportProgress(5, "Memeriksa ketersediaan file font...");
            await FontResourceUtil.EnsureFontFilesAsync(fontConfig, msg => ReportProgress(5, msg));

            int total = fileItems.Count;
            for (int i = 0; i < total; i++)
            {
                var item = fileItems[i];
                int currentPercent = 10 + (int)(((i + 1) / (double)total) * 90);

                item.Status = EProcessStatus.Processing;
                item.StatusMessage = "Sedang mengonversi...";
                ReportProgress(currentPercent, $"Memproses {item.FileName}...", item.FileName);

                try
                {
                    string ext = Path.GetExtension(item.FileName);
                    string baseName = Path.GetFileNameWithoutExtension(item.FileName);
                    string outFileName = $"{baseName}_{fontConfig.DisplayName}{ext}";
                    string outFilePath = Path.Combine(outputDirectory, outFileName);

                    await Task.Run(() =>
                    {
                        switch (item.FileType)
                        {
                            case EFileType.Docx:
                                DocxUtil.ConvertFont(item.FullPath, outFilePath, fontConfig.FamilyName);
                                break;

                            case EFileType.Xml:
                                XmlUtil.ConvertFont(item.FullPath, outFilePath, fontConfig.FamilyName);
                                break;

                            case EFileType.Pdf:
                                PdfUtil.ConvertFont(item.FullPath, outFilePath, fontConfig);
                                break;

                            case EFileType.Zip:
                                ConvertZipArchive(item.FullPath, outFilePath, fontConfig);
                                break;

                            default:
                                throw new NotSupportedException($"Format file {ext} tidak didukung.");
                        }
                    });

                    item.Status = EProcessStatus.Success;
                    item.StatusMessage = "Berhasil dikonversi";
                    item.OutputFilePath = outFilePath;
                }
                catch (Exception ex)
                {
                    item.Status = EProcessStatus.Failed;
                    item.StatusMessage = $"Gagal: {ex.Message}";
                }
            }

            ReportProgress(100, "Semua proses konversi selesai!", isCompleted: true);
        }

        private void ConvertZipArchive(string zipInputPath, string zipOutputPath, MdlFontConfig fontConfig)
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "ConvertingFont_" + Guid.NewGuid().ToString("N"));
            string extractDir = Path.Combine(tempDir, "extracted");
            string convertedDir = Path.Combine(tempDir, "converted");

            try
            {
                ZipUtil.ExtractZip(zipInputPath, extractDir);
                Directory.CreateDirectory(convertedDir);

                var allFiles = Directory.GetFiles(extractDir, "*.*", SearchOption.AllDirectories);
                foreach (var file in allFiles)
                {
                    string relative = file.Substring(extractDir.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                    string dest = Path.Combine(convertedDir, relative);
                    var destSub = Path.GetDirectoryName(dest);
                    if (!Directory.Exists(destSub)) Directory.CreateDirectory(destSub);

                    string ext = Path.GetExtension(file).ToLowerInvariant();
                    if (ext == ".docx")
                    {
                        DocxUtil.ConvertFont(file, dest, fontConfig.FamilyName);
                    }
                    else if (ext == ".xml")
                    {
                        XmlUtil.ConvertFont(file, dest, fontConfig.FamilyName);
                    }
                    else if (ext == ".pdf")
                    {
                        PdfUtil.ConvertFont(file, dest, fontConfig);
                    }
                    else
                    {
                        File.Copy(file, dest, true);
                    }
                }

                ZipUtil.CreateZipFromDirectory(convertedDir, zipOutputPath);
            }
            finally
            {
                if (Directory.Exists(tempDir))
                {
                    try { Directory.Delete(tempDir, true); } catch { }
                }
            }
        }
    }
}
