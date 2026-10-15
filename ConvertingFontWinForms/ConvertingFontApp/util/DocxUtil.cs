using System;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace ConvertingFontApp.util
{
    public static class DocxUtil
    {
        public static void ConvertFont(string inputFilePath, string outputFilePath, string targetFontName)
        {
            if (string.IsNullOrWhiteSpace(targetFontName))
                targetFontName = "Roboto";

            var dir = Path.GetDirectoryName(outputFilePath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            File.Copy(inputFilePath, outputFilePath, true);

            using (var doc = WordprocessingDocument.Open(outputFilePath, true))
            {
                // 1. Process Main Document Part
                if (doc.MainDocumentPart != null)
                {
                    ApplyFontToContainer(doc.MainDocumentPart.Document, targetFontName);

                    // 2. Process Styles
                    if (doc.MainDocumentPart.StyleDefinitionsPart != null)
                    {
                        var styles = doc.MainDocumentPart.StyleDefinitionsPart.Styles;
                        if (styles != null)
                        {
                            foreach (var runFonts in styles.Descendants<RunFonts>())
                            {
                                UpdateRunFonts(runFonts, targetFontName);
                            }

                            // Also default docDefaults
                            if (styles.DocDefaults?.RunPropertiesDefault?.RunPropertiesBaseStyle != null)
                            {
                                var rPr = styles.DocDefaults.RunPropertiesDefault.RunPropertiesBaseStyle;
                                var rFonts = rPr.GetFirstChild<RunFonts>() ?? rPr.AppendChild(new RunFonts());
                                UpdateRunFonts(rFonts, targetFontName);
                            }
                        }
                        doc.MainDocumentPart.StyleDefinitionsPart.Styles.Save();
                    }

                    // 3. Process Headers
                    foreach (var header in doc.MainDocumentPart.HeaderParts)
                    {
                        if (header.Header != null)
                        {
                            ApplyFontToContainer(header.Header, targetFontName);
                            header.Header.Save();
                        }
                    }

                    // 4. Process Footers
                    foreach (var footer in doc.MainDocumentPart.FooterParts)
                    {
                        if (footer.Footer != null)
                        {
                            ApplyFontToContainer(footer.Footer, targetFontName);
                            footer.Footer.Save();
                        }
                    }

                    doc.MainDocumentPart.Document.Save();
                }
            }
        }

        private static void ApplyFontToContainer(DocumentFormat.OpenXml.OpenXmlElement container, string targetFontName)
        {
            if (container == null) return;

            // Existing RunFonts elements
            foreach (var runFonts in container.Descendants<RunFonts>())
            {
                UpdateRunFonts(runFonts, targetFontName);
            }

            // Runs that have RunProperties but no RunFonts
            foreach (var run in container.Descendants<Run>())
            {
                var rPr = run.RunProperties;
                if (rPr == null)
                {
                    rPr = new RunProperties();
                    run.PrependChild(rPr);
                }

                var rFonts = rPr.GetFirstChild<RunFonts>();
                if (rFonts == null)
                {
                    rFonts = new RunFonts();
                    rPr.AppendChild(rFonts);
                }
                UpdateRunFonts(rFonts, targetFontName);
            }
        }

        private static void UpdateRunFonts(RunFonts runFonts, string targetFontName)
        {
            if (runFonts == null) return;
            runFonts.Ascii = targetFontName;
            runFonts.HighAnsi = targetFontName;
            runFonts.ComplexScript = targetFontName;
            runFonts.EastAsia = targetFontName;
        }
    }
}
