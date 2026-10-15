# ConvertingFontApp - Windows Forms (.NET Framework 4.8)

Aplikasi desktop Windows Forms untuk konversi font dokumen dan personalisasi massal (Batch Mapping Merger), dibangun dengan pola arsitektur **MVC (Model - View - Controller)** yang mengadopsi standar arsitektur pada project **AIAFDMC**.

---

## 🏗️ Struktur Arsitektur MVC

```plaintext
ConvertingFontApp/
│
├── ConvertingFontApp.sln          # Solution file (Dapat dibuka di VS 2017 & VS 2022)
│
└── ConvertingFontApp/
    ├── ConvertingFontApp.csproj   # Project WinForms (.NET Framework 4.8)
    ├── App.config
    ├── Program.cs                 # Main Entry Point
    │
    ├── mdl/                       # MODEL: Struktur data & State
    │   ├── MdlFontConfig.cs       # Konfigurasi font (Roboto, Open Sans, Montserrat, Arial, Times)
    │   ├── MdlFileItem.cs         # Model antrian file konversi & status proses
    │   └── MdlMergerRow.cs        # Model baris data dataset CSV/Excel
    │
    ├── view/                      # VIEW: Antarmuka Pengguna (UI)
    │   ├── FormMain.cs            # Form Utama Window induk aplikasi
    │   ├── FormMain.Designer.cs
    │   ├── UcFontConverter.cs     # Tab 1: Drag & Drop Converter (DOCX, PDF, XML, ZIP)
    │   ├── UcBatchMerger.cs       # Tab 2: Batch Mapping Merger (Template + CSV/Excel)
    │   └── UcFolderConverter.cs   # Tab 3: Konversi Folder Batch Lokal
    │
    ├── ctrl/                      # CONTROLLER: Pengatur Alur & Logika Bisnis
    │   ├── CtrlFontConverter.cs   # Orkestrasi konversi dokumen dan ZIP
    │   ├── CtrlBatchMerger.cs     # Orkestrasi mapping data nasabah & export ZIP
    │   └── CtrlFolderConverter.cs # Orkestrasi pemindaian dan konversi folder rekursif
    │
    ├── util/                      # UTILITY: Mesin Pengolah Dokumen
    │   ├── DocxUtil.cs            # Modifikasi font Word (DocumentFormat.OpenXml)
    │   ├── XmlUtil.cs             # Modifikasi font XML (System.Xml.Linq XDocument)
    │   ├── PdfUtil.cs             # Manipulasi font PDF (iText 7)
    │   ├── ExcelCsvUtil.cs        # Parser CSV (CsvHelper) & Excel (ClosedXML)
    │   ├── ZipUtil.cs             # Ekstrak & kompresi ZIP (System.IO.Compression)
    │   └── FontResourceUtil.cs    # Pengecekan & auto-download file font TTF
    │
    ├── enums/                     # ENUMS: Tipe Data Enumerasi
    │   ├── EFileType.cs           # Unknown, Docx, Pdf, Xml, Zip, Csv, Excel
    │   ├── EFontTarget.cs         # Roboto, OpenSans, Montserrat, Arial, TimesNewRoman
    │   └── EProcessStatus.cs      # Pending, Processing, Success, Failed, Skipped
    │
    ├── events/                    # EVENTS: Notifikasi Antar Lapisan
    │   └── ProgressEventArgs.cs   # Event data untuk update ProgressBar & status UI
    │
    └── Assets/
        └── Fonts/                 # File TrueType Font (.ttf) offline bawaan
```

---

## 🚀 Cara Menjalankan & Membuka Project

### 1. Buka di Visual Studio (VS 2017 / VS 2022)
1. Double-click file `ConvertingFontApp.sln`.
2. Tekan **F5** atau klik tombol **Start** warna hijau untuk menjalankan aplikasi dalam mode Debug.
3. Anda bisa membuka file Form di Designer (`FormMain.cs`, `UcFontConverter.cs`, dll) untuk mengedit tata letak UI secara visual.

### 2. Menjalankan Langsung File `.exe`
Aplikasi sudah berhasil di-build dan siap digunakan tanpa perlu membuka Visual Studio:
- Lokasi: `ConvertingFontApp\bin\Debug\ConvertingFontApp.exe`
- Buka folder tersebut dan klik dua kali `ConvertingFontApp.exe`.

---

## 🌟 Fitur Utama Aplikasi

1. **Tab 1: Konversi Dokumen (Single & Batch)**:
   - Area **Drag & Drop** file (PDF, DOCX, XML, ZIP) atau tombol *Pilih File...*.
   - Pilihan target font: **Roboto** (Google Font), **Open Sans**, **Montserrat**, **Arial**, **Times New Roman**.
   - Grid daftar antrian file dengan informasi format, ukuran, dan status real-time.
   - Pilihan folder output dan tombol *Buka Folder*.
   - Progress bar dan pesan status proses.

2. **Tab 2: Batch Mapping Merger**:
   - Pemilihan file Master Template (XML, PDF, DOCX).
   - Pemilihan file Dataset nasabah (CSV / Excel `.xlsx`/`.xls`).
   - **Live Preview Data**: Menampilkan pratinjau tabel 10 baris pertama beserta total jumlah baris dan kolom.
   - Tombol **Generate & Export ZIP**: Otomatis memproduksi dokumen personal tiap nasabah dan membundelnya dalam format file `.zip`.

3. **Tab 3: Konversi Folder Batch**:
   - Pemilihan folder sumber input dan folder tujuan output.
   - Checkbox opsi **Proses seluruh sub-folder secara rekursif**.
   - Menjaga struktur hirarki direktori folder secara utuh.
