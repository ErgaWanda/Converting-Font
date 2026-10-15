using System.IO;
using ConvertingFontApp.enums;

namespace ConvertingFontApp.mdl
{
    public class MdlFileItem
    {
        public string FileName { get; set; }
        public string FullPath { get; set; }
        public EFileType FileType { get; set; }
        public long FileSizeBytes { get; set; }
        public string FileSizeFormatted => FormatBytes(FileSizeBytes);
        public EProcessStatus Status { get; set; } = EProcessStatus.Pending;
        public string StatusMessage { get; set; } = "Menunggu...";
        public string OutputFilePath { get; set; }

        public static MdlFileItem FromFilePath(string path)
        {
            var fi = new FileInfo(path);
            var ext = fi.Extension.ToLowerInvariant();
            var item = new MdlFileItem
            {
                FileName = fi.Name,
                FullPath = fi.FullName,
                FileSizeBytes = fi.Exists ? fi.Length : 0,
                Status = EProcessStatus.Pending,
                StatusMessage = "Siap diproses"
            };

            switch (ext)
            {
                case ".docx":
                    item.FileType = EFileType.Docx;
                    break;
                case ".pdf":
                    item.FileType = EFileType.Pdf;
                    break;
                case ".xml":
                    item.FileType = EFileType.Xml;
                    break;
                case ".zip":
                    item.FileType = EFileType.Zip;
                    break;
                case ".csv":
                    item.FileType = EFileType.Csv;
                    break;
                case ".xlsx":
                case ".xls":
                    item.FileType = EFileType.Excel;
                    break;
                default:
                    item.FileType = EFileType.Unknown;
                    break;
            }

            return item;
        }

        private static string FormatBytes(long bytes)
        {
            if (bytes < 1024) return $"{bytes} B";
            if (bytes < 1024 * 1024) return $"{(bytes / 1024.0):F1} KB";
            return $"{(bytes / (1024.0 * 1024.0)):F2} MB";
        }
    }
}
