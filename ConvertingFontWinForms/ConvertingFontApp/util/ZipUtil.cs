using System;
using System.IO;
using System.IO.Compression;

namespace ConvertingFontApp.util
{
    public static class ZipUtil
    {
        public static void ExtractZip(string zipFilePath, string targetDirectory)
        {
            if (!Directory.Exists(targetDirectory))
                Directory.CreateDirectory(targetDirectory);

            ZipFile.ExtractToDirectory(zipFilePath, targetDirectory);
        }

        public static void CreateZipFromDirectory(string sourceDirectory, string destinationZipFilePath)
        {
            var destDir = Path.GetDirectoryName(destinationZipFilePath);
            if (!string.IsNullOrEmpty(destDir) && !Directory.Exists(destDir))
                Directory.CreateDirectory(destDir);

            if (File.Exists(destinationZipFilePath))
                File.Delete(destinationZipFilePath);

            ZipFile.CreateFromDirectory(sourceDirectory, destinationZipFilePath, CompressionLevel.Optimal, false);
        }
    }
}
