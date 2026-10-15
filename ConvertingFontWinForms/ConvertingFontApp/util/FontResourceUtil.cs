using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using ConvertingFontApp.enums;
using ConvertingFontApp.mdl;

namespace ConvertingFontApp.util
{
    public static class FontResourceUtil
    {
        public static async Task EnsureFontFilesAsync(MdlFontConfig config, Action<string> logAction = null)
        {
            if (config == null || config.Variants == null) return;

            var v = config.Variants;
            var filesToEnsure = new[]
            {
                new { Path = v.RegularPath, Url = v.RegularUrl, Name = "Regular" },
                new { Path = v.BoldPath, Url = v.BoldUrl, Name = "Bold" },
                new { Path = v.ItalicPath, Url = v.ItalicUrl, Name = "Italic" },
                new { Path = v.BoldItalicPath, Url = v.BoldItalicUrl, Name = "BoldItalic" }
            };

            foreach (var item in filesToEnsure)
            {
                if (!File.Exists(item.Path))
                {
                    if (!string.IsNullOrEmpty(item.Url))
                    {
                        var dir = Path.GetDirectoryName(item.Path);
                        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }

                        logAction?.Invoke($"Mengunduh font {config.DisplayName} ({item.Name})...");
                        using (var client = new WebClient())
                        {
                            await client.DownloadFileTaskAsync(new Uri(item.Url), item.Path);
                        }
                        logAction?.Invoke($"Font {config.DisplayName} ({item.Name}) siap.");
                    }
                }
            }
        }
    }
}
