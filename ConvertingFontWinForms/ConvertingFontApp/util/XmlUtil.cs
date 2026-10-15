using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace ConvertingFontApp.util
{
    public static class XmlUtil
    {
        private static readonly string[] KnownFonts = new[]
        {
            "ArialMT", "Arial-BoldMT", "Arial-ItalicMT", "Arial-BoldItalicMT",
            "Arial", "Calibri", "Times New Roman", "TimesNewRoman", "Times",
            "Neo Sans Pro", "NeoSansPro", "Neo Sans Intel", "NeoSans Intel",
            "Neo-Sans-Pro", "Neo-Sans", "Neo Sans", "NeoSans",
            "Segoe UI", "SegoeUI", "Helvetica", "Tahoma", "Verdana",
            "Aptos", "Cambria", "Garamond"
        };

        private static readonly string[] FontAttrNames = new[]
        {
            "family", "font-family", "fontfamily", "font_family",
            "face", "typeface", "font", "font-name", "fontname", "font_name"
        };

        public static int ConvertFont(string inputFilePath, string outputFilePath, string targetFontFamily)
        {
            if (string.IsNullOrWhiteSpace(targetFontFamily))
                targetFontFamily = "Roboto";

            var xmlStr = File.ReadAllText(inputFilePath, Encoding.UTF8);
            int count = 0;

            try
            {
                var doc = XDocument.Parse(xmlStr);
                foreach (var elem in doc.Descendants())
                {
                    // Check attributes
                    foreach (var attr in elem.Attributes())
                    {
                        var localName = attr.Name.LocalName.ToLowerInvariant();
                        bool isFontAttr = Array.Exists(FontAttrNames, f => f == localName);
                        if (isFontAttr)
                        {
                            if (!string.Equals(attr.Value, targetFontFamily, StringComparison.OrdinalIgnoreCase))
                            {
                                attr.Value = targetFontFamily;
                                count++;
                            }
                        }
                        else
                        {
                            string val = attr.Value;
                            bool modified = false;
                            foreach (var font in KnownFonts)
                            {
                                if (val.IndexOf(font, StringComparison.OrdinalIgnoreCase) >= 0)
                                {
                                    val = Regex.Replace(val, $@"\b{Regex.Escape(font)}\b", targetFontFamily, RegexOptions.IgnoreCase);
                                    modified = true;
                                }
                            }
                            if (modified)
                            {
                                attr.Value = val;
                                count++;
                            }
                        }
                    }

                    // Check element text
                    if (!elem.HasElements && !string.IsNullOrEmpty(elem.Value))
                    {
                        string val = elem.Value;
                        bool modified = false;
                        foreach (var font in KnownFonts)
                        {
                            if (val.IndexOf(font, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                val = Regex.Replace(val, $@"\b{Regex.Escape(font)}\b", targetFontFamily, RegexOptions.IgnoreCase);
                                modified = true;
                            }
                        }
                        if (modified)
                        {
                            elem.Value = val;
                            count++;
                        }
                    }
                }

                var dir = Path.GetDirectoryName(outputFilePath);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                doc.Save(outputFilePath);
            }
            catch
            {
                // Fallback using Regex
                string pattern = @"(?i)\b(Arial|Calibri|Times New Roman|Neo[\s\-_]?Sans(?:[\s\-_]?Pro|[\s\-_]?Intel)?|NeoSansPro|NeoSans|Segoe UI|Helvetica)\b";
                string replaced = Regex.Replace(xmlStr, pattern, m =>
                {
                    count++;
                    return targetFontFamily;
                });

                var dir = Path.GetDirectoryName(outputFilePath);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                File.WriteAllText(outputFilePath, replaced, Encoding.UTF8);
            }

            return count;
        }
    }
}
