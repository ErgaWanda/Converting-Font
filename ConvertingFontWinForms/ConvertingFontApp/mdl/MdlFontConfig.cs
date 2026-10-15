using System;
using System.Collections.Generic;
using System.IO;
using ConvertingFontApp.enums;

namespace ConvertingFontApp.mdl
{
    public class MdlFontVariant
    {
        public string RegularPath { get; set; }
        public string BoldPath { get; set; }
        public string ItalicPath { get; set; }
        public string BoldItalicPath { get; set; }

        public string RegularUrl { get; set; }
        public string BoldUrl { get; set; }
        public string ItalicUrl { get; set; }
        public string BoldItalicUrl { get; set; }

        public bool IsComplete()
        {
            return File.Exists(RegularPath) &&
                   File.Exists(BoldPath) &&
                   File.Exists(ItalicPath) &&
                   File.Exists(BoldItalicPath);
        }
    }

    public class MdlFontConfig
    {
        public EFontTarget Target { get; set; }
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string FamilyName { get; set; }
        public string Badge { get; set; }
        public string Description { get; set; }
        public MdlFontVariant Variants { get; set; }

        public static string AppFontsDirectory
        {
            get
            {
                var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Fonts");
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                return dir;
            }
        }

        public static Dictionary<EFontTarget, MdlFontConfig> GetRegistry()
        {
            var baseFontsDir = AppFontsDirectory;
            var winFonts = Environment.GetFolderPath(Environment.SpecialFolder.Fonts);

            return new Dictionary<EFontTarget, MdlFontConfig>
            {
                {
                    EFontTarget.Roboto,
                    new MdlFontConfig
                    {
                        Target = EFontTarget.Roboto,
                        Id = "roboto",
                        DisplayName = "Roboto",
                        FamilyName = "Roboto",
                        Badge = "Google Font (Bawaan)",
                        Description = "Font resmi standar dengan keterbacaan tinggi dan proporsi seimbang.",
                        Variants = new MdlFontVariant
                        {
                            RegularPath = Path.Combine(baseFontsDir, "Roboto-Regular.ttf"),
                            BoldPath = Path.Combine(baseFontsDir, "Roboto-Bold.ttf"),
                            ItalicPath = Path.Combine(baseFontsDir, "Roboto-Italic.ttf"),
                            BoldItalicPath = Path.Combine(baseFontsDir, "Roboto-BoldItalic.ttf"),
                            RegularUrl = "https://raw.githubusercontent.com/googlefonts/roboto/main/src/hinted/Roboto-Regular.ttf",
                            BoldUrl = "https://raw.githubusercontent.com/googlefonts/roboto/main/src/hinted/Roboto-Bold.ttf",
                            ItalicUrl = "https://raw.githubusercontent.com/googlefonts/roboto/main/src/hinted/Roboto-Italic.ttf",
                            BoldItalicUrl = "https://raw.githubusercontent.com/googlefonts/roboto/main/src/hinted/Roboto-BoldItalic.ttf"
                        }
                    }
                },
                {
                    EFontTarget.OpenSans,
                    new MdlFontConfig
                    {
                        Target = EFontTarget.OpenSans,
                        Id = "opensans",
                        DisplayName = "Open Sans",
                        FamilyName = "Open Sans",
                        Badge = "Google Font",
                        Description = "Font modern humanis sans-serif yang bersih dan mudah dibaca.",
                        Variants = new MdlFontVariant
                        {
                            RegularPath = Path.Combine(baseFontsDir, "OpenSans-Regular.ttf"),
                            BoldPath = Path.Combine(baseFontsDir, "OpenSans-Bold.ttf"),
                            ItalicPath = Path.Combine(baseFontsDir, "OpenSans-Italic.ttf"),
                            BoldItalicPath = Path.Combine(baseFontsDir, "OpenSans-BoldItalic.ttf"),
                            RegularUrl = "https://raw.githubusercontent.com/googlefonts/opensans/main/fonts/ttf/OpenSans-Regular.ttf",
                            BoldUrl = "https://raw.githubusercontent.com/googlefonts/opensans/main/fonts/ttf/OpenSans-Bold.ttf",
                            ItalicUrl = "https://raw.githubusercontent.com/googlefonts/opensans/main/fonts/ttf/OpenSans-Italic.ttf",
                            BoldItalicUrl = "https://raw.githubusercontent.com/googlefonts/opensans/main/fonts/ttf/OpenSans-BoldItalic.ttf"
                        }
                    }
                },
                {
                    EFontTarget.Montserrat,
                    new MdlFontConfig
                    {
                        Target = EFontTarget.Montserrat,
                        Id = "montserrat",
                        DisplayName = "Montserrat",
                        FamilyName = "Montserrat",
                        Badge = "Google Font",
                        Description = "Font geometris sans-serif kontemporer terinspirasi tipografi urban.",
                        Variants = new MdlFontVariant
                        {
                            RegularPath = Path.Combine(baseFontsDir, "Montserrat-Regular.ttf"),
                            BoldPath = Path.Combine(baseFontsDir, "Montserrat-Bold.ttf"),
                            ItalicPath = Path.Combine(baseFontsDir, "Montserrat-Italic.ttf"),
                            BoldItalicPath = Path.Combine(baseFontsDir, "Montserrat-BoldItalic.ttf"),
                            RegularUrl = "https://raw.githubusercontent.com/JulietaUla/Montserrat/master/fonts/ttf/Montserrat-Regular.ttf",
                            BoldUrl = "https://raw.githubusercontent.com/JulietaUla/Montserrat/master/fonts/ttf/Montserrat-Bold.ttf",
                            ItalicUrl = "https://raw.githubusercontent.com/JulietaUla/Montserrat/master/fonts/ttf/Montserrat-Italic.ttf",
                            BoldItalicUrl = "https://raw.githubusercontent.com/JulietaUla/Montserrat/master/fonts/ttf/Montserrat-BoldItalic.ttf"
                        }
                    }
                },
                {
                    EFontTarget.Arial,
                    new MdlFontConfig
                    {
                        Target = EFontTarget.Arial,
                        Id = "arial",
                        DisplayName = "Arial",
                        FamilyName = "Arial",
                        Badge = "System Font",
                        Description = "Font sans-serif klasik universal yang kompatibel di semua platform.",
                        Variants = new MdlFontVariant
                        {
                            RegularPath = Path.Combine(winFonts, "arial.ttf"),
                            BoldPath = Path.Combine(winFonts, "arialbd.ttf"),
                            ItalicPath = Path.Combine(winFonts, "ariali.ttf"),
                            BoldItalicPath = Path.Combine(winFonts, "arialbi.ttf")
                        }
                    }
                },
                {
                    EFontTarget.TimesNewRoman,
                    new MdlFontConfig
                    {
                        Target = EFontTarget.TimesNewRoman,
                        Id = "times",
                        DisplayName = "Times New Roman",
                        FamilyName = "Times New Roman",
                        Badge = "System Font",
                        Description = "Font serif formal klasik standar untuk surat dan dokumen legal.",
                        Variants = new MdlFontVariant
                        {
                            RegularPath = Path.Combine(winFonts, "times.ttf"),
                            BoldPath = Path.Combine(winFonts, "timesbd.ttf"),
                            ItalicPath = Path.Combine(winFonts, "timesi.ttf"),
                            BoldItalicPath = Path.Combine(winFonts, "timesbi.ttf")
                        }
                    }
                }
            };
        }
    }
}
