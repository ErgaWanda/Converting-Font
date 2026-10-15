using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ConvertingFontApp.mdl;
using iText.IO.Font;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Data;
using iText.Kernel.Pdf.Canvas.Parser.Listener;

namespace ConvertingFontApp.util
{
    public static class PdfUtil
    {
        private class ExtractedTextChunk
        {
            public string Text { get; set; }
            public float X { get; set; }
            public float Y { get; set; }
            public float FontSize { get; set; }
            public Color FillColor { get; set; }
            public bool IsBold { get; set; }
            public bool IsItalic { get; set; }
        }

        private class TextExtractListener : IEventListener
        {
            public List<ExtractedTextChunk> Chunks { get; } = new List<ExtractedTextChunk>();

            public void EventOccurred(IEventData data, EventType type)
            {
                if (type == EventType.RENDER_TEXT)
                {
                    var tri = (TextRenderInfo)data;
                    string text = tri.GetText();
                    if (string.IsNullOrEmpty(text)) return;

                    var baseline = tri.GetBaseline();
                    var start = baseline.GetStartPoint();
                    string fontName = "";
                    try
                    {
                        fontName = tri.GetFont()?.GetFontProgram()?.GetFontNames()?.GetFontName() ?? "";
                    }
                    catch { }
                    fontName = fontName.ToLowerInvariant();

                    Chunks.Add(new ExtractedTextChunk
                    {
                        Text = text,
                        X = start.Get(0),
                        Y = start.Get(1),
                        FontSize = tri.GetFontSize(),
                        FillColor = tri.GetFillColor(),
                        IsBold = fontName.Contains("bold"),
                        IsItalic = fontName.Contains("italic") || fontName.Contains("oblique")
                    });
                }
            }

            public ICollection<EventType> GetSupportedEvents()
            {
                return new[] { EventType.RENDER_TEXT };
            }
        }

        public static void ConvertFont(string inputFilePath, string outputFilePath, MdlFontConfig fontConfig)
        {
            var dir = Path.GetDirectoryName(outputFilePath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            string regPath = fontConfig?.Variants?.RegularPath;
            string boldPath = fontConfig?.Variants?.BoldPath;
            string italicPath = fontConfig?.Variants?.ItalicPath;
            string boldItalicPath = fontConfig?.Variants?.BoldItalicPath;

            if (string.IsNullOrEmpty(regPath) || !File.Exists(regPath))
            {
                regPath = Path.Combine(MdlFontConfig.AppFontsDirectory, "Roboto-Regular.ttf");
            }

            using (var reader = new PdfReader(inputFilePath))
            using (var writer = new PdfWriter(outputFilePath))
            using (var pdfDoc = new PdfDocument(reader, writer))
            {
                // Load fonts
                PdfFont regFont = LoadFont(regPath, iText.IO.Font.Constants.StandardFonts.HELVETICA);
                PdfFont boldFont = LoadFont(boldPath, iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD);
                PdfFont italicFont = LoadFont(italicPath, iText.IO.Font.Constants.StandardFonts.HELVETICA_OBLIQUE);
                PdfFont boldItalicFont = LoadFont(boldItalicPath, iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLDOBLIQUE);

                int pageCount = pdfDoc.GetNumberOfPages();
                for (int p = 1; p <= pageCount; p++)
                {
                    var page = pdfDoc.GetPage(p);

                    // 1. Extract all text chunks with exact coordinates & styles
                    var listener = new TextExtractListener();
                    var parser = new PdfCanvasProcessor(listener);
                    parser.ProcessPageContent(page);

                    // 2. Remove all original text (BT ... ET) from page content streams
                    // This prevents ghosting/overlapping and eliminates corrupt font dictionary errors in Acrobat
                    CleanPageContentStreams(page);

                    // 3. Draw extracted text with new target font on clean page canvas
                    if (listener.Chunks.Count > 0)
                    {
                        var canvas = new PdfCanvas(page);
                        foreach (var chunk in listener.Chunks)
                        {
                            PdfFont fontToUse = regFont;
                            if (chunk.IsBold && chunk.IsItalic && boldItalicFont != null) fontToUse = boldItalicFont;
                            else if (chunk.IsBold && boldFont != null) fontToUse = boldFont;
                            else if (chunk.IsItalic && italicFont != null) fontToUse = italicFont;

                            canvas.BeginText();
                            canvas.SetFontAndSize(fontToUse, chunk.FontSize > 0 ? chunk.FontSize : 10f);
                            if (chunk.FillColor != null)
                            {
                                canvas.SetColor(chunk.FillColor, true);
                            }
                            canvas.SetTextMatrix(chunk.X, chunk.Y);
                            canvas.ShowText(chunk.Text);
                            canvas.EndText();
                        }
                    }
                }
            }
        }

        private static PdfFont LoadFont(string fontPath, string fallbackStandard)
        {
            if (!string.IsNullOrEmpty(fontPath) && File.Exists(fontPath))
            {
                try
                {
                    return PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.FORCE_EMBEDDED);
                }
                catch
                {
                    // Ignore fallback below
                }
            }
            return PdfFontFactory.CreateFont(fallbackStandard);
        }

        private static void CleanPageContentStreams(PdfPage page)
        {
            var contentsObj = page.GetPdfObject().Get(PdfName.Contents);
            if (contentsObj == null) return;

            if (contentsObj is PdfStream singleStream)
            {
                CleanStream(singleStream);
            }
            else if (contentsObj is PdfArray streamArray)
            {
                for (int i = 0; i < streamArray.Size(); i++)
                {
                    var sObj = streamArray.Get(i);
                    if (sObj is PdfStream arrStream)
                    {
                        CleanStream(arrStream);
                    }
                }
            }
        }

        private static void CleanStream(PdfStream stream)
        {
            try
            {
                byte[] rawBytes = stream.GetBytes();
                if (rawBytes == null || rawBytes.Length == 0) return;

                string latinStr = Encoding.GetEncoding("ISO-8859-1").GetString(rawBytes);
                string cleaned = RemoveBtEtTokens(latinStr);
                stream.SetData(Encoding.GetEncoding("ISO-8859-1").GetBytes(cleaned));
            }
            catch
            {
                // In case of unhandled compression format, retain original
            }
        }

        private static string RemoveBtEtTokens(string streamText)
        {
            var sb = new StringBuilder();
            int i = 0;
            int n = streamText.Length;
            bool inText = false;

            while (i < n)
            {
                if (!inText)
                {
                    if ((i == 0 || char.IsWhiteSpace(streamText[i - 1])) &&
                        i + 1 < n && streamText[i] == 'B' && streamText[i + 1] == 'T' &&
                        (i + 2 == n || char.IsWhiteSpace(streamText[i + 2])))
                    {
                        inText = true;
                        i += 2;
                        continue;
                    }
                    sb.Append(streamText[i]);
                    i++;
                }
                else
                {
                    if (streamText[i] == '(')
                    {
                        i++;
                        int depth = 1;
                        while (i < n && depth > 0)
                        {
                            if (streamText[i] == '\\')
                            {
                                i += 2;
                            }
                            else if (streamText[i] == '(')
                            {
                                depth++;
                                i++;
                            }
                            else if (streamText[i] == ')')
                            {
                                depth--;
                                i++;
                            }
                            else
                            {
                                i++;
                            }
                        }
                    }
                    else if (streamText[i] == '<' && i + 1 < n && streamText[i + 1] != '<')
                    {
                        i++;
                        while (i < n && streamText[i] != '>')
                        {
                            i++;
                        }
                        if (i < n) i++;
                    }
                    else if ((i == 0 || char.IsWhiteSpace(streamText[i - 1])) &&
                             i + 1 < n && streamText[i] == 'E' && streamText[i + 1] == 'T' &&
                             (i + 2 == n || char.IsWhiteSpace(streamText[i + 2])))
                    {
                        inText = false;
                        i += 2;
                        sb.Append(" ");
                    }
                    else
                    {
                        i++;
                    }
                }
            }

            return sb.ToString();
        }
    }
}
