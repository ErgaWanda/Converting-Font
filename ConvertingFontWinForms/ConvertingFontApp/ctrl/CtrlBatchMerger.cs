using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ConvertingFontApp.events;
using ConvertingFontApp.mdl;
using ConvertingFontApp.util;

namespace ConvertingFontApp.ctrl
{
    public class CtrlBatchMerger
    {
        public event EventHandler<ProgressEventArgs> OnProgressChanged;

        private void ReportProgress(int percent, string message, string currentFile = "", bool isCompleted = false, bool hasError = false)
        {
            OnProgressChanged?.Invoke(this, new ProgressEventArgs(percent, message, currentFile, isCompleted, hasError));
        }

        public async Task<string> MergeAsync(string templatePath, string dataFilePath, string outputDirectory, MdlFontConfig fontConfig)
        {
            if (!File.Exists(templatePath))
                throw new FileNotFoundException("File template tidak ditemukan.", templatePath);

            if (!File.Exists(dataFilePath))
                throw new FileNotFoundException("File data tabular tidak ditemukan.", dataFilePath);

            if (!Directory.Exists(outputDirectory))
                Directory.CreateDirectory(outputDirectory);

            ReportProgress(10, "Membaca file dataset (CSV/Excel)...");
            var dt = await Task.Run(() => ExcelCsvUtil.ReadToDataTable(dataFilePath));

            if (dt.Rows.Count == 0)
                throw new InvalidOperationException("Dataset kosong atau tidak memiliki baris data.");

            string tempDir = Path.Combine(Path.GetTempPath(), "BatchMerger_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            string templateExt = Path.GetExtension(templatePath).ToLowerInvariant();
            int total = dt.Rows.Count;

            try
            {
                await Task.Run(() =>
                {
                    string templateContent = "";
                    if (templateExt == ".xml")
                    {
                        templateContent = File.ReadAllText(templatePath);
                    }

                    for (int i = 0; i < total; i++)
                    {
                        var row = dt.Rows[i];
                        int percent = 15 + (int)(((i + 1) / (double)total) * 75);

                        string idVal = null;
                        foreach (string colKey in new[] { "NO_POLIS", "NOMOR_POLIS", "POLIS", "ID", "NO", "KODE", "NIK" })
                        {
                            if (dt.Columns.Contains(colKey))
                            {
                                idVal = row[colKey]?.ToString()?.Trim();
                                break;
                            }
                        }

                        if (string.IsNullOrEmpty(idVal))
                        {
                            idVal = $"ROW_{i + 1}";
                        }

                        // Clean idVal
                        idVal = Regex.Replace(idVal, @"[^\w\-]", "_");
                        string outDocName = $"Doc_{idVal}{templateExt}";
                        string outDocPath = Path.Combine(tempDir, outDocName);

                        ReportProgress(percent, $"Menghasilkan dokumen {i + 1} dari {total} ({outDocName})...", outDocName);

                        if (templateExt == ".xml")
                        {
                            string mergedXml = templateContent;
                            foreach (DataColumn col in dt.Columns)
                            {
                                string key = col.ColumnName;
                                string val = row[col]?.ToString() ?? "";
                                mergedXml = Regex.Replace(mergedXml, $@"\{{{Regex.Escape(key)}\}}", val, RegexOptions.IgnoreCase);
                                mergedXml = Regex.Replace(mergedXml, $@"\[{Regex.Escape(key)}\]", val, RegexOptions.IgnoreCase);
                            }

                            File.WriteAllText(outDocPath, mergedXml);

                            // Apply font conversion
                            if (fontConfig != null)
                            {
                                XmlUtil.ConvertFont(outDocPath, outDocPath, fontConfig.FamilyName);
                            }
                        }
                        else if (templateExt == ".docx")
                        {
                            File.Copy(templatePath, outDocPath, true);
                            if (fontConfig != null)
                            {
                                DocxUtil.ConvertFont(outDocPath, outDocPath, fontConfig.FamilyName);
                            }
                        }
                        else if (templateExt == ".pdf")
                        {
                            File.Copy(templatePath, outDocPath, true);
                            if (fontConfig != null)
                            {
                                PdfUtil.ConvertFont(outDocPath, outDocPath, fontConfig);
                            }
                        }
                    }
                });

                // Package to ZIP
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string zipFileName = $"BatchMerged_{timestamp}.zip";
                string zipOutputPath = Path.Combine(outputDirectory, zipFileName);

                ReportProgress(95, "Mengompresi dokumen hasil merger ke ZIP...");
                await Task.Run(() => ZipUtil.CreateZipFromDirectory(tempDir, zipOutputPath));

                ReportProgress(100, $"Selesai! {total} dokumen berhasil digenerate ke {zipFileName}", zipFileName, isCompleted: true);
                return zipOutputPath;
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
