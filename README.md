# Tools Converting Font & Batch Mapping Merger

[![Python](https://img.shields.io/badge/Python-3.10+-3776AB?style=for-the-badge&logo=python&logoColor=white)](https://www.python.org/)
[![FastAPI](https://img.shields.io/badge/FastAPI-0.110+-009688?style=for-the-badge&logo=fastapi&logoColor=white)](https://fastapi.tiangolo.com/)
[![TailwindCSS](https://img.shields.io/badge/Tailwind_CSS-38B2AC?style=for-the-badge&logo=tailwind-css&logoColor=white)](https://tailwindcss.com/)
[![PyMuPDF](https://img.shields.io/badge/PyMuPDF-1.24+-D32F2F?style=for-the-badge&logo=adobe-acrobat-reader&logoColor=white)](https://pymupdf.readthedocs.io/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?style=for-the-badge)](https://opensource.org/licenses/MIT)

**Tools Converting Font** adalah portal otomatisasi dokumen tingkat lanjut yang dirancang khusus untuk standarisasi tipografi dan personalisasi dokumen massal. Aplikasi ini menyediakan mesin konversi font presisi tinggi (mengubah font dokumen seperti Arial, Helvetica, dan Times Roman menjadi **Roboto resmi Google**) untuk format **PDF, DOCX, dan XML**, serta dilengkapi fitur **Batch Mapping Merger** untuk menggabungkan master template dengan dataset nasabah (CSV/Excel).

---

## 🌟 Fitur Utama

### 1. 🔤 Precision Font Converter (PDF, DOCX, XML, ZIP)
- **Konversi Font ke Google Roboto**:
  - Otomatis mendownload dan menerapkan varian resmi keluarga font Roboto (`Regular`, `Bold`, `Italic`, `BoldItalic`).
- **PDF Engine Berpresisi Tinggi (High-Fidelity)**:
  - **Deduplikasi Ghost / Overlapping Spans**: Mengeliminasi teks tersembunyi atau ganda yang tertumpuk di layer background.
  - **Auto-Fit Font Scaling**: Menyesuaikan skala ukuran teks secara proporsional agar tidak melebihi batas koordinat garis/kontainer asli dokumen.
  - **Preservasi Spasi & Alignment**: Menjaga perataan paragraf rata kanan-kiri (*justified text*), spasi antar kata, dan indentasi daftar bernomor (*numbered lists*).
  - **Vector Drawing untuk Simbol Khusus**: Mengonversi simbol non-standar (seperti bullet persegi `▪`) ke objek vektor presisi agar tidak terjadi karakter rusak (*tofu/glitch*).
- **DOCX & XML Transformation**:
  - Merestrukturisasi elemen styling `w:rFonts`, font styles, atribut, dan teks node XML secara menyeluruh.
- **Dukungan Multi-File & ZIP Archive**:
  - Konversi banyak dokumen sekaligus melalui multi-select upload atau satu file ZIP (struktur folder di dalam ZIP tetap dipertahankan secara utuh).

### 2. 🗂️ Batch Mapping Merger
- **Otomatisasi Personalisasi Dokumen**:
  - Menggabungkan 1 Master Template (**XML** atau **PDF**) dengan data tabular (**CSV** atau **Excel** `.xlsx`/`.xls`).
- **Live Data Preview**:
  - Pratinjau interaktif 5 baris pertama data nasabah beserta jumlah total baris sebelum proses generate dijalankan.
- **Kemasan ZIP Otomatis**:
  - Seluruh dokumen yang ter-generate secara otomatis dibundel menjadi file `.zip` siap unduh.

### 3. 💻 CLI Batch Folder Converter (`convert_folder.py`)
- Script CLI mandiri untuk mengonversi seluruh file dalam suatu folder lokal/jaringan secara offline tanpa perlu membuka peramban web.
- Mendukung pemindaian rekursif ke seluruh subfolder.

### 4. 🎨 Modern & Responsive User Interface
- Antarmuka web modern berbasis **Tailwind CSS**, tipografi **Plus Jakarta Sans**, dan **Lucide Icons**.
- Drag & Drop zone interaktif, indikator proses real-time, tab navigasi intuitif, dan tombol unduh file sampel instan (*PDF, DOCX, XML, CSV*).

---

## 🏗️ Arsitektur & Teknologi

| Komponen | Teknologi | Keterangan |
| :--- | :--- | :--- |
| **Backend Framework** | [FastAPI](https://fastapi.tiangolo.com/) | REST API asinkron berkecepatan tinggi |
| **PDF Processing Engine** | [PyMuPDF (fitz)](https://pymupdf.readthedocs.io/) | Ekstraksi span, redaksi koordinat teks, dan injeksi font vektor |
| **DOCX Processing** | [python-docx](https://python-docx.readthedocs.io/) | Manipulasi OOXML `w:rFonts` dan run styling |
| **Data Processing** | [pandas](https://pandas.pydata.org/) & [openpyxl](https://openpyxl.readthedocs.io/) | Parser CSV dan Excel untuk batch merger |
| **Frontend** | HTML5, Tailwind CSS (CDN), Lucide Icons | Antarmuka web responsif dan interaktif |
| **Template Engine** | Jinja2 | Rendering antarmuka web |
| **Server** | Uvicorn | ASGI Web Server |

---

## 📁 Struktur Direktori

```plaintext
Converting Font/
│
├── assets/
│   ├── fonts/               # Penyimpanan font Google Roboto (.ttf) otomatis
│   │   ├── Roboto-Regular.ttf
│   │   ├── Roboto-Bold.ttf
│   │   ├── Roboto-Italic.ttf
│   │   └── Roboto-BoldItalic.ttf
│   └── samples/             # Sampel dokumen untuk pengujian & unduhan di web
│       ├── data_nasabah.csv
│       ├── template.docx
│       ├── template.pdf
│       └── template.xml
│
├── templates/
│   └── index.html           # Tampilan antarmuka web (UI)
│
├── convert_folder.py        # CLI script konversi batch folder lokal
├── main.py                  # Aplikasi utama FastAPI & core engine konversi
├── requirements.txt         # Daftar dependensi Python
├── test_app.py              # Unit & integration test suite
└── README.md                # Dokumentasi proyek
```

---

## 🚀 Panduan Instalasi & Menjalankan Aplikasi

### Prasyarat
- **Python 3.10+** sudah terpasang di sistem.
- Koneksi internet pada saat pertama kali menjalankan server (untuk mengunduh aset font resmi Roboto secara otomatis ke folder `assets/fonts/`).

### 1. Klon Repositori
```bash
git clone https://github.com/ErgaWanda/Converting-Font.git
cd "Converting-Font"
```

### 2. Buat & Aktifkan Virtual Environment (Direkomendasikan)
- **Windows (PowerShell):**
  ```powershell
  python -m venv venv
  .\venv\Scripts\Activate.ps1
  ```
- **Linux / macOS:**
  ```bash
  python3 -m venv venv
  source venv/bin/activate
  ```

### 3. Instal Dependensi
```bash
pip install -r requirements.txt
```

### 4. Jalankan Web Server
Gunakan perintah berikut:
```bash
python main.py
```
atau menggunakan `uvicorn` langsung:
```bash
uvicorn main:app --host 127.0.0.1 --port 8000 --reload
```

Buka peramban (browser) dan akses alamat:
👉 **[http://127.0.0.1:8000](http://127.0.0.1:8000)**

Dokumentasi API Swagger interaktif dapat diakses di:
👉 **[http://127.0.0.1:8000/docs](http://127.0.0.1:8000/docs)**

---

## 🛠️ Penggunaan

### A. Melalui Antarmuka Web (UI)

#### 1. Tab "Konversi Font"
1. Pilih atau tarik (*drag and drop*) dokumen berformat `.xml`, `.docx`, `.pdf`, atau kumpulan file dalam arsip `.zip`.
2. Klik tombol **"Mulai Konversi Font"**.
3. Sistem akan memproses dan mengunduh file hasil konversi dengan awalan nama `Roboto_...` (atau `.zip` jika mengunggah banyak file).

#### 2. Tab "Batch Mapping Merger"
1. Unggah **File Master Template** (`.xml` atau `.pdf`).
2. Unggah **File Data Nasabah** (`.csv` atau `.xlsx`/`.xls`).
3. Sistem akan menampilkan pratinjau tabel 5 baris pertama data nasabah.
4. Klik **"Proses Batch Mapping"** untuk mengunduh seluruh dokumen yang telah dipersonalisasi dalam 1 file `.zip`.

---

### B. Melalui Command Line Interface (CLI)

Gunakan script `convert_folder.py` untuk mengonversi seluruh file dalam suatu folder secara instan:

```bash
# Konversi semua file XML & DOCX dalam folder 'input_docs' (rekursif)
python convert_folder.py "C:\Path\To\input_docs"

# Menentukan folder output khusus
python convert_folder.py "C:\Path\To\input_docs" -o "C:\Path\To\output_roboto"

# Hanya konversi di folder utama (non-rekursif / abaikan sub-folder)
python convert_folder.py "C:\Path\To\input_docs" --no-recursive
```

---

## 🔌 Dokumentasi REST API

| Method | Endpoint | Deskripsi | Parameter / Payload | Response |
| :--- | :--- | :--- | :--- | :--- |
| `GET` | `/` | Menampilkan antarmuka web | - | `HTML` |
| `POST` | `/api/convert-font` | Mengonversi file XML, DOCX, PDF, atau ZIP ke font Roboto | `file`: UploadFile atau `files`: List[UploadFile] | File Dokumen / ZIP Streaming |
| `POST` | `/api/preview-data` | Pratinjau 5 baris data CSV/Excel | `data_file`: UploadFile | JSON (`columns`, `rows`, `total_rows`) |
| `POST` | `/api/batch-merge` | Menggabungkan template XML/PDF dengan data nasabah | `template_file`: UploadFile, `data_file`: UploadFile | ZIP Streaming file hasil merge |
| `GET` | `/api/download-sample/{type}` | Mengunduh file sampel (`pdf`, `xml`, `docx`, `csv`) | Path parameter `{type}` | File Attachment |

---

## 🧪 Pengujian (Testing)

Proyek ini telah dilengkapi dengan unit test dan integration test komprehensif yang memvalidasi presisi konversi font, preservasi layout, penghapusan overlapping text, vector bullets, auto-fit scaling, serta seluruh endpoint API:

```bash
python test_app.py
```

Output pengujian yang berhasil:
```plaintext
XML Font Conversion OK: 13 replaced.
DOCX Font Conversion OK: 11 replaced.
PDF Fonts in output: [(5, 'OBVXVV+Roboto Bold', 'Roboto-Bold'), (11, 'AEYGHS+Roboto Regular', 'Roboto-Regular')]
PDF Font Conversion OK: 13 spans processed.
Times PDF Font Conversion OK: No Times font remains in output.
Bullet Symbol PDF Conversion OK: Square vector bullets rendered properly and Arial replaced by Roboto.
Overlapping Span Deduplication OK: Ghost/covered text suppressed successfully.
Inline Spacing and Space Preservation OK: No merged words, Bold and Regular styles preserved.
Auto-Fit Font Scaling OK: Font size scaled to fit original line boundaries.
Justified Text Alignment OK: Word spacing preserved on justified paragraphs.
Numbered List Indentation Alignment OK: First line body aligns with continuation indent.
XML Batch ZIP OK.
PDF Batch ZIP OK.
GET / OK
POST /api/convert-font (Single, ZIP, and Multi-file) OK
POST /api/preview-data OK
POST /api/batch-merge (XML) OK
POST /api/batch-merge (PDF) OK
GET /api/download-sample/pdf OK
GET /api/download-sample/xml OK
GET /api/download-sample/docx OK
GET /api/download-sample/csv OK
ALL TESTS PASSED!
```

---

## 📄 Lisensi

Didistribusikan di bawah lisensi MIT. Lihat file `LICENSE` untuk rincian lebih lanjut.
