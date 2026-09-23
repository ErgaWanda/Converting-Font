import io
import re
import os
import zipfile
import urllib.request
import xml.etree.ElementTree as ET
from typing import Optional

from fastapi import FastAPI, File, UploadFile, HTTPException, Form
from fastapi.responses import HTMLResponse, StreamingResponse, FileResponse, Response
from fastapi.staticfiles import StaticFiles
from fastapi.templating import Jinja2Templates
from starlette.requests import Request
import pandas as pd
import docx
from docx.oxml.ns import qn
import pymupdf
import pymupdf as fitz

app = FastAPI(
    title="Tools Converting Font",
    description="Portal Otomatisasi Dokumen: Font Converter & Batch Mapping Merger (XML, DOCX, & PDF)",
    version="1.3.0"
)


BASE_DIR = os.path.dirname(os.path.abspath(__file__))
TEMPLATES_DIR = os.path.join(BASE_DIR, "templates")
SAMPLES_DIR = os.path.join(BASE_DIR, "assets", "samples")
FONTS_DIR = os.path.join(BASE_DIR, "assets", "fonts")

os.makedirs(TEMPLATES_DIR, exist_ok=True)
os.makedirs(SAMPLES_DIR, exist_ok=True)
os.makedirs(FONTS_DIR, exist_ok=True)

templates = Jinja2Templates(directory=TEMPLATES_DIR)

ROBOTO_FONTS = {
    'regular': os.path.join(FONTS_DIR, 'Roboto-Regular.ttf'),
    'bold': os.path.join(FONTS_DIR, 'Roboto-Bold.ttf'),
    'italic': os.path.join(FONTS_DIR, 'Roboto-Italic.ttf'),
    'bolditalic': os.path.join(FONTS_DIR, 'Roboto-BoldItalic.ttf')
}

def ensure_roboto_fonts():
    urls = {
        'Roboto-Regular.ttf': 'https://raw.githubusercontent.com/googlefonts/roboto/main/src/hinted/Roboto-Regular.ttf',
        'Roboto-Bold.ttf': 'https://raw.githubusercontent.com/googlefonts/roboto/main/src/hinted/Roboto-Bold.ttf',
        'Roboto-Italic.ttf': 'https://raw.githubusercontent.com/googlefonts/roboto/main/src/hinted/Roboto-Italic.ttf',
        'Roboto-BoldItalic.ttf': 'https://raw.githubusercontent.com/googlefonts/roboto/main/src/hinted/Roboto-BoldItalic.ttf'
    }
    for filename, url in urls.items():
        dest = os.path.join(FONTS_DIR, filename)
        if not os.path.exists(dest) or os.path.getsize(dest) == 0:
            try:
                req = urllib.request.Request(url, headers={'User-Agent': 'Mozilla/5.0'})
                with urllib.request.urlopen(req, timeout=10) as resp:
                    data = resp.read()
                    with open(dest, 'wb') as f:
                        f.write(data)
            except Exception as e:
                print(f"Warning: Gagal mengunduh {filename}: {e}")

ensure_roboto_fonts()


def convert_xml_arial_to_roboto(xml_bytes: bytes) -> tuple[bytes, int]:
    count = 0
    try:
        xml_str = xml_bytes.decode('utf-8')
    except UnicodeDecodeError:
        xml_str = xml_bytes.decode('latin-1', errors='replace')

    try:
        root = ET.fromstring(xml_str)
        for elem in root.iter():
            for key, val in list(elem.attrib.items()):
                if val and re.search(r'\barial\b', val, re.IGNORECASE):
                    new_val = re.sub(r'\barial\b', 'Roboto', val, flags=re.IGNORECASE)
                    elem.attrib[key] = new_val
                    count += 1
                elif val and 'Arial' in val:
                    new_val = val.replace('ArialMT', 'Roboto-Regular').replace('Arial-BoldMT', 'Roboto-Bold').replace('Arial', 'Roboto')
                    elem.attrib[key] = new_val
                    count += 1

            if elem.text and re.search(r'\barial\b', elem.text, re.IGNORECASE):
                elem.text = re.sub(r'\barial\b', 'Roboto', elem.text, flags=re.IGNORECASE)
                count += 1

            if elem.tail and re.search(r'\barial\b', elem.tail, re.IGNORECASE):
                elem.tail = re.sub(r'\barial\b', 'Roboto', elem.tail, flags=re.IGNORECASE)
                count += 1

        out_stream = io.BytesIO()
        tree = ET.ElementTree(root)
        tree.write(out_stream, encoding='utf-8', xml_declaration=True)
        converted_xml = out_stream.getvalue()

    except Exception:
        converted_str, count = re.subn(r'(?i)\bArial\b', 'Roboto', xml_str)
        converted_xml = converted_str.encode('utf-8')

    converted_str = converted_xml.decode('utf-8', errors='replace')
    extra_subs, extra_count = re.subn(r'(?i)\bArial\b', 'Roboto', converted_str)
    if extra_count > 0:
        converted_xml = extra_subs.encode('utf-8')
        count += extra_count

    return converted_xml, count


def convert_rtf_arial_to_roboto(rtf_bytes: bytes) -> tuple[bytes, int]:
    """Konversi referensi font Arial ke Roboto dalam dokumen RTF.

    Menggunakan dua lapis penggantian:
    1. Regex bertarget pada blok \\fonttbl untuk mengganti deklarasi font secara presisi.
    2. Regex fallback global untuk menangkap sisa referensi Arial di luar fonttbl.

    PENTING: Encode balik dengan encoding yang sama agar file tidak membengkak.
    RTF dengan data binary (gambar) akan membengkak 2-8x jika di-encode ulang ke UTF-8.
    """
    count = 0

    # Deteksi encoding RTF dan catat encoding yang berhasil dipakai
    # PENTING: harus encode balik dengan encoding yang SAMA agar tidak ada ekspansi ukuran
    encoding_used = 'latin-1'  # default RTF: ANSI/Latin-1
    try:
        # Coba UTF-8 dulu (hanya berhasil jika file memang pure UTF-8, jarang untuk RTF)
        rtf_str = rtf_bytes.decode('utf-8')
        # Verifikasi ini benar-benar UTF-8 bukan latin-1 yang kebetulan valid
        # Jika ada karakter multibyte UTF-8, ini memang UTF-8
        if any(ord(c) > 0xFF for c in rtf_str):
            encoding_used = 'utf-8'
        else:
            # Semua karakter masuk latin-1 range — decode ulang sebagai latin-1 agar encode balik aman
            rtf_str = rtf_bytes.decode('latin-1')
            encoding_used = 'latin-1'
    except UnicodeDecodeError:
        try:
            rtf_str = rtf_bytes.decode('latin-1')
            encoding_used = 'latin-1'
        except UnicodeDecodeError:
            rtf_str = rtf_bytes.decode('cp1252', errors='replace')
            encoding_used = 'cp1252'

    # Validasi bahwa ini memang file RTF
    if not rtf_str.strip().startswith('{\\rtf'):
        raise ValueError("File bukan dokumen RTF yang valid.")

    # --- Lapis 1: Ganti nama font dalam blok \fonttbl ---
    # Contoh: {\f0\fswiss\fcharset0 Arial;} -> {\f0\fswiss\fcharset0 Roboto;}
    # Tangani berbagai varian nama: Arial, ArialMT, Arial-BoldMT, Arial Bold, Arial Narrow, dll.
    arial_font_pattern = re.compile(
        r'(\\f\d+[^;{]*?)\s+(Arial(?:-\w+|\s+\w+)*)\s*;',
        re.IGNORECASE
    )

    def replace_in_fonttbl(m):
        nonlocal count
        original_name = m.group(2)
        lower = original_name.lower()
        if 'bold' in lower and 'italic' in lower:
            new_name = 'Roboto Bold Italic'
        elif 'boldmt' in lower or 'bold' in lower:
            new_name = 'Roboto Bold'
        elif 'italic' in lower or 'oblique' in lower:
            new_name = 'Roboto Italic'
        elif 'narrow' in lower:
            new_name = 'Roboto Condensed'
        else:
            new_name = 'Roboto'
        count += 1
        return f"{m.group(1)} {new_name};"

    # Temukan blok \fonttbl dan ganti di dalamnya
    fonttbl_pattern = re.compile(r'(\{\\fonttbl)(.*?)(\})', re.DOTALL)

    def process_fonttbl_block(m):
        prefix = m.group(1)
        body = arial_font_pattern.sub(replace_in_fonttbl, m.group(2))
        suffix = m.group(3)
        return prefix + body + suffix

    rtf_str = fonttbl_pattern.sub(process_fonttbl_block, rtf_str)

    # --- Lapis 2: Regex fallback global ---
    rtf_str, extra_subs = re.subn(r'\bArial\b', 'Roboto', rtf_str, flags=re.IGNORECASE)
    count += extra_subs

    # Encode balik dengan encoding YANG SAMA saat decode
    # Ini krusial: RTF latin-1 → utf-8 akan mengembangkan setiap byte 0x80-0xFF menjadi 2 byte
    result_bytes = rtf_str.encode(encoding_used, errors='replace')

    return result_bytes, count



def convert_doc_arial_to_roboto(doc_bytes: bytes) -> tuple[bytes, int]:
    """Konversi font Arial ke Roboto dalam file .doc (Word 97-2003 binary format).

    Strategi berlapis:
    0. Cek apakah isi file sebenarnya RTF — banyak .doc lama menyimpan konten RTF.
    1. Coba buka sebagai .docx (OOXML) — beberapa .doc sebenarnya OOXML yang salah ekstensi.
    2. Fallback: cari dan ganti nama font Arial di binary stream (ASCII & UTF-16 LE).
    """
    # --- Lapis 0: Deteksi RTF di dalam file .doc ---
    # Banyak file Word 97-2003 (.doc) sebenarnya adalah RTF yang disimpan dengan ekstensi salah.
    # Cek magic bytes: RTF selalu dimulai dengan "{\rtf"
    for encoding in ('utf-8', 'latin-1', 'cp1252'):
        try:
            peek = doc_bytes[:20].decode(encoding)
            if peek.strip().startswith('{\\rtf'):
                # Ini file RTF — proses sebagai RTF
                return convert_rtf_arial_to_roboto(doc_bytes)
            break
        except UnicodeDecodeError:
            continue

    # --- Lapis 1: Coba buka sebagai DOCX (OOXML) ---
    # Beberapa file .doc sebenarnya adalah Office Open XML (ZIP-based) yang salah diberi ekstensi
    try:
        converted_bytes, count = convert_docx_arial_to_roboto(doc_bytes)
        return converted_bytes, count
    except Exception:
        pass

    # --- Lapis 2: Binary stream replacement ---
    # File .doc menyimpan nama font sebagai string ASCII dan UTF-16 LE di dalam binary stream.
    count = 0
    arial_variants = [
        (b'Arial-BoldItalicMT', b'Roboto-BoldItalic '),  # 18 char
        (b'Arial-BoldMT',       b'Roboto-Bold  '),        # 12 char
        (b'Arial-ItalicMT',     b'Roboto-Italic '),       # 14 char
        (b'ArialMT',            b'Roboto '),              # 7 char
        (b'Arial Unicode MS',   b'Roboto          '),     # 16 char
        (b'Arial Narrow',       b'Roboto Cond.'),         # 12 char
        (b'Arial Bold',         b'Roboto Bold'),          # 10 char
        (b'Arial Italic',       b'Roboto Italic'),        # 12 char
    ]

    result = doc_bytes
    for old, new in arial_variants:
        if len(old) != len(new):
            continue
        n = result.count(old)
        if n > 0:
            result = result.replace(old, new)
            count += n

    # Ganti sisa "Arial" standalone dalam konteks null-byte (ASCII dalam binary stream)
    arial_bin_pat = re.compile(b'(?<=\x00)Arial(?=[\x00\x20])', re.IGNORECASE)
    result, n = arial_bin_pat.subn(b'Robot', result)
    count += n

    # Ganti dalam UTF-16 LE (wide-char strings yang umum di Office binary formats)
    arial_wide = 'Arial'.encode('utf-16-le')
    roboto_wide = 'Robot'.encode('utf-16-le')  # sama panjang: 5 char × 2 byte = 10 byte
    n_wide = result.count(arial_wide)
    if n_wide > 0:
        result = result.replace(arial_wide, roboto_wide)
        count += n_wide

    return result, count



def convert_docx_arial_to_roboto(docx_bytes: bytes) -> tuple[bytes, int]:
    doc = docx.Document(io.BytesIO(docx_bytes))
    count = 0

    def process_run(run):
        nonlocal count
        changed = False

        if run.font.name and 'arial' in run.font.name.lower():
            run.font.name = 'Roboto'
            changed = True

        rPr = run._r.get_or_add_rPr()
        rFonts = rPr.find(qn('w:rFonts'))
        if rFonts is not None:
            for attr in ['ascii', 'hAnsi', 'cs', 'eastAsia']:
                val = rFonts.get(qn(f'w:{attr}'))
                if val and 'arial' in val.lower():
                    rFonts.set(qn(f'w:{attr}'), 'Roboto')
                    changed = True
        else:
            if changed or (run.font.name == 'Roboto'):
                rFonts_elem = docx.oxml.OxmlElement('w:rFonts')
                rFonts_elem.set(qn('w:ascii'), 'Roboto')
                rFonts_elem.set(qn('w:hAnsi'), 'Roboto')
                rFonts_elem.set(qn('w:cs'), 'Roboto')
                rPr.append(rFonts_elem)

        if changed:
            count += 1

    for style in doc.styles:
        try:
            if hasattr(style, 'font') and style.font.name and 'arial' in style.font.name.lower():
                style.font.name = 'Roboto'
                count += 1
        except Exception:
            pass

    for p in doc.paragraphs:
        for run in p.runs:
            process_run(run)

    for table in doc.tables:
        for row in table.rows:
            for cell in row.cells:
                for p in cell.paragraphs:
                    for run in p.runs:
                        process_run(run)

    for section in doc.sections:
        for header_type in [section.header, section.first_page_header, section.even_page_header]:
            if header_type is not None:
                for p in header_type.paragraphs:
                    for run in p.runs:
                        process_run(run)
                for table in header_type.tables:
                    for row in table.rows:
                        for cell in row.cells:
                            for p in cell.paragraphs:
                                for run in p.runs:
                                    process_run(run)

        for footer_type in [section.footer, section.first_page_footer, section.even_page_footer]:
            if footer_type is not None:
                for p in footer_type.paragraphs:
                    for run in p.runs:
                        process_run(run)
                for table in footer_type.tables:
                    for row in table.rows:
                        for cell in row.cells:
                            for p in cell.paragraphs:
                                for run in p.runs:
                                    process_run(run)

    out_stream = io.BytesIO()
    doc.save(out_stream)
    return out_stream.getvalue(), count


ROUND_BULLETS = {
    '\uf0b7', '\uf06c', '\uf06d', '\u25cf', '\u25cb', '\u25ef',
    '\u2022', '\u00b7', '\u2219', '\u2023'
}
SQUARE_BULLETS = {
    '\uf0a7', '\uf06e', '\uf071', '\uf0de', '\uf0d8', '\uf0a8',
    '\u25aa', '\u25ab', '\u25a0', '\u25a1', '\u25fe', '\u25fc',
    '\u25c6', '\u25c7', '\xa7', '\x00'
}
ALL_BULLETS = ROUND_BULLETS | SQUARE_BULLETS | {
    '\u00bb', '\u00ab', '\u2013', '\u2014', '\u25b8', '\u25b9',
    '\u25ba', '\u25bb', '\u25c0', '\u25c1', '\u25c2', '\u25c3',
    '-', '*', '>'
}

PDF_DELIMITERS = set(' \t\r\n\x00\x0c()<>[]{}/%')

def _is_square_bullet(text: str, font_str: str = '') -> bool:
    if text in SQUARE_BULLETS:
        return True
    if len(text) == 1:
        code = ord(text)
        if 0xF000 <= code <= 0xF0FF and text not in ROUND_BULLETS:
            return True
        if 'wingdings' in font_str and (text in ['n', '\xa7', 'q'] or text not in ROUND_BULLETS):
            return True
    return False

def _is_bullet_span(text: str, font_str: str = '') -> bool:
    t = text.strip()
    if not t:
        return False
    if t in ALL_BULLETS:
        return True
    if _is_square_bullet(t, font_str):
        return True
    if len(t) == 1 and not t.isalnum():
        return True
    if re.match(r'^\(?[0-9a-zA-Z]{1,3}[\.\)]$', t):
        return True
    return False

def _remove_text_from_content_stream(stream_text: str) -> str:
    out = []
    i = 0
    n = len(stream_text)
    in_text = False
    bt_non_str = []
    while i < n:
        if not in_text:
            if (i == 0 or stream_text[i-1] in PDF_DELIMITERS) and stream_text[i:i+2] == 'BT' and (i+2 == n or stream_text[i+2] in PDF_DELIMITERS):
                in_text = True
                bt_non_str = []
                i += 2
                continue
            out.append(stream_text[i])
            i += 1
        else:
            if stream_text[i] == '(':
                i += 1
                depth = 1
                while i < n and depth > 0:
                    if stream_text[i] == '\\':
                        i += 2
                    elif stream_text[i] == '(':
                        depth += 1
                        i += 1
                    elif stream_text[i] == ')':
                        depth -= 1
                        i += 1
                    else:
                        i += 1
                continue
            elif stream_text[i] == '<' and (i+1 < n and stream_text[i+1] != '<'):
                i += 1
                while i < n and stream_text[i] != '>':
                    i += 1
                if i < n:
                    i += 1
                continue
            elif (i == 0 or stream_text[i-1] in PDF_DELIMITERS) and stream_text[i:i+2] == 'ET' and (i+2 == n or stream_text[i+2] in PDF_DELIMITERS):
                in_text = False
                non_str_content = ''.join(bt_non_str)
                state_ops = re.findall(r'(/[\w\#]+)\s+(cs|CS|gs)\b', non_str_content)
                if state_ops:
                    out.append(' ' + ' '.join(f'{name} {op}' for name, op in state_ops) + ' ')
                color_ops = re.findall(r'((?:[0-9\.\+\-]+\s+){1,4}(?:k|K|rg|RG|g|G))\b', non_str_content)
                if color_ops:
                    out.append(' ' + ' '.join(color_ops) + ' ')
                i += 2
                continue
            else:
                bt_non_str.append(stream_text[i])
                i += 1
    return ''.join(out)

def _clear_font_dict(obj_str: str) -> str:
    m = re.search(r'/Font\s*<<', obj_str)
    if not m:
        return obj_str
    dict_start = m.end() - 2
    i = dict_start + 2
    depth = 1
    n = len(obj_str)
    while i < n and depth > 0:
        if obj_str[i:i+2] == '<<':
            depth += 1
            i += 2
        elif obj_str[i:i+2] == '>>':
            depth -= 1
            i += 2
        else:
            i += 1
    if depth == 0:
        return obj_str[:dict_start] + '<<>>' + obj_str[i:]
    return obj_str

def convert_pdf_all_to_roboto(pdf_bytes: bytes) -> tuple[bytes, int]:
    reg_path = ROBOTO_FONTS['regular']
    bold_path = ROBOTO_FONTS['bold']
    italic_path = ROBOTO_FONTS['italic']
    bolditalic_path = ROBOTO_FONTS['bolditalic']

    font_cache = {}
    def get_font_obj(path):
        if path and path not in font_cache and os.path.exists(path):
            font_cache[path] = fitz.Font(fontfile=path)
        return font_cache.get(path)

    doc = fitz.open(stream=pdf_bytes, filetype="pdf")
    total_spans_converted = 0

    for page in doc:
        try:
            page.clean_contents(sanitize=False)
        except Exception:
            pass

    all_pages_elements = []
    for page in doc:
        d = page.get_text("dict")
        raw_spans = []
        for b in d.get("blocks", []):
            if b.get("type") != 0:
                continue
            for line in b.get("lines", []):
                for s in line.get("spans", []):
                    if s.get("text", "").strip():
                        raw_spans.append(s)

        superseded_ids = set()
        for i, s1 in enumerate(raw_spans):
            t1 = s1.get("text", "").strip().lower()
            r1 = fitz.Rect(s1["bbox"])
            for s2 in raw_spans[i+1:]:
                t2 = s2.get("text", "").strip().lower()
                if t1 == t2 or (t1 in t2 and len(t1) > 5) or (t2 in t1 and len(t2) > 5):
                    r2 = fitz.Rect(s2["bbox"])
                    if r1.intersects(r2):
                        intersect_area = fitz.Rect(r1).intersect(r2).get_area()
                        min_area = min(r1.get_area(), r2.get_area())
                        if min_area > 0 and (intersect_area / min_area) > 0.4:
                            superseded_ids.add(id(s1))
                            break

        page_pix = page.get_pixmap(dpi=72)
        pix_w, pix_h = page_pix.width, page_pix.height
        pix_samples = page_pix.samples
        for s in raw_spans:
            if id(s) in superseded_ids:
                continue
            bx0 = max(0, min(pix_w - 1, int(s["bbox"][0])))
            by0 = max(0, min(pix_h - 1, int(s["bbox"][1])))
            bx1 = max(0, min(pix_w - 1, int(s["bbox"][2])))
            by1 = max(0, min(pix_h - 1, int(s["bbox"][3])))
            if bx1 > bx0 and by1 > by0:
                s_cols = set()
                for py in range(by0, by1 + 1):
                    for px in range(bx0, bx1 + 1):
                        p_idx = (py * pix_w + px) * 3
                        s_cols.add((pix_samples[p_idx], pix_samples[p_idx+1], pix_samples[p_idx+2]))
                        if len(s_cols) > 2:
                            break
                    if len(s_cols) > 2:
                        break
                if len(s_cols) <= 2:
                    superseded_ids.add(id(s))

        x1_counts = {}
        for b in d.get("blocks", []):
            if b.get("type") != 0:
                continue
            for l in b.get("lines", []):
                txt = " ".join(s.get("text", "") for s in l.get("spans", []) if id(s) not in superseded_ids).strip()
                if len(txt.split()) >= 3 and not txt.endswith(":"):
                    x1_val = round(l["bbox"][2], 0)
                    x1_counts[x1_val] = x1_counts.get(x1_val, 0) + 1
        common_margins = [k for k, v in x1_counts.items() if v >= 4]

        all_page_groups = []
        for b in d.get("blocks", []):
            if b.get("type") != 0:
                continue
            for l in b.get("lines", []):
                sps = [s for s in l.get("spans", []) if s.get("text", "").strip() and id(s) not in superseded_ids]
                if sps:
                    sps.sort(key=lambda s: s["origin"][0])
                    all_page_groups.append({
                        "baseline_y": round(sps[0]["origin"][1], 1),
                        "bbox": tuple(l["bbox"]),
                        "spans": sps
                    })

        all_page_groups.sort(key=lambda g: (g["baseline_y"], g["bbox"][0]))

        page_bullet_indents = {}
        for gi, g in enumerate(all_page_groups):
            sps = g["spans"]
            t0 = sps[0].get("text", "").strip()
            if _is_bullet_span(t0, sps[0].get("font", "")) and len(sps) >= 2:
                p_key = round(sps[0]["origin"][0], 0)
                for next_g in all_page_groups[gi+1:]:
                    nt0 = next_g["spans"][0].get("text", "").strip()
                    if _is_bullet_span(nt0, next_g["spans"][0].get("font", "")):
                        break
                    cont_x0 = round(next_g["bbox"][0], 1)
                    if cont_x0 > sps[0]["bbox"][2] + 1.0:
                        page_bullet_indents.setdefault(p_key, []).append(cont_x0)
                        break

        page_std_indent = {}
        for p_key, c_list in page_bullet_indents.items():
            if c_list:
                freq = {}
                for cv in c_list:
                    freq[cv] = freq.get(cv, 0) + 1
                page_std_indent[p_key] = max(freq.items(), key=lambda item: item[1])[0]

        elements_to_draw = []
        for b in d.get("blocks", []):
            if b.get("type") != 0:
                continue
            lines = b.get("lines", [])
            if not lines:
                continue

            grouped_lines = []
            for l in lines:
                spans = [s for s in l.get("spans", []) if s.get("text", "").strip() and id(s) not in superseded_ids]
                if not spans:
                    continue
                base_y = round(spans[0]["origin"][1], 1)
                merged = False
                for g in grouped_lines:
                    if abs(g["baseline_y"] - base_y) <= 1.2:
                        h_gap = max(0, max(g["bbox"][0], l["bbox"][0]) - min(g["bbox"][2], l["bbox"][2]))
                        if h_gap <= 15.0:
                            g["lines"].append(l)
                            g["spans"].extend(spans)
                            g["bbox"] = (
                                min(g["bbox"][0], l["bbox"][0]),
                                min(g["bbox"][1], l["bbox"][1]),
                                max(g["bbox"][2], l["bbox"][2]),
                                max(g["bbox"][3], l["bbox"][3])
                            )
                            merged = True
                            break
                if not merged:
                    grouped_lines.append({
                        "baseline_y": base_y,
                        "lines": [l],
                        "spans": list(spans),
                        "bbox": tuple(l["bbox"])
                    })

            for g in grouped_lines:
                g["spans"].sort(key=lambda s: s["origin"][0])

            if not grouped_lines:
                continue

            block_max_x1 = max(g["bbox"][2] for g in grouped_lines)
            aligned_right_count = sum(1 for g in grouped_lines if abs(g["bbox"][2] - block_max_x1) <= 2.5)

            for gi, g in enumerate(grouped_lines):
                spans = g["spans"]
                if not spans:
                    continue

                prefix_span = None
                body_spans = spans
                if len(spans) >= 2:
                    s0 = spans[0]
                    t0 = s0.get("text", "").strip()
                    is_bullet = _is_bullet_span(t0, s0.get("font", ""))
                    if is_bullet:
                        bi = 1
                        while bi < len(spans) and not spans[bi].get("text", "").strip():
                            bi += 1
                        if bi < len(spans) and (spans[bi]["origin"][0] - s0["bbox"][2]) > 2.0:
                            prefix_span = s0
                            body_spans = spans[bi:]

                orig_body_x0 = body_spans[0]["origin"][0] if body_spans else g["bbox"][0]
                body_x0 = orig_body_x0

                if prefix_span and body_spans:
                    p_key = round(prefix_span["origin"][0], 0)
                    cand_indent = None
                    for next_g in grouped_lines[gi+1:]:
                        n_sps = next_g["spans"]
                        if not n_sps:
                            continue
                        nt0 = n_sps[0].get("text", "").strip()
                        if _is_bullet_span(nt0, n_sps[0].get("font", "")):
                            break
                        c_x0 = round(next_g["bbox"][0], 1)
                        if c_x0 > prefix_span["bbox"][2] + 1.0:
                            cand_indent = c_x0
                            break
                    if cand_indent is None:
                        cand_indent = page_std_indent.get(p_key)

                    if cand_indent is not None and cand_indent > prefix_span["bbox"][2] + 1.0:
                        if abs(cand_indent - orig_body_x0) <= 12.0:
                            body_x0 = cand_indent

                target_x1 = g["bbox"][2]
                target_w = target_x1 - body_x0
                is_right_aligned = (aligned_right_count >= 2 and abs(target_x1 - block_max_x1) <= 2.5) or any(abs(target_x1 - cm) <= 2.5 for cm in common_margins)


                words_data = []
                for s in body_spans:
                    stext = s.get("text", "")
                    if not stext:
                        continue
                    sz = s.get("size", 10.0)
                    c = s.get("color", 0)
                    cr, cg, cb = (((c >> 16) & 255) / 255.0, ((c >> 8) & 255) / 255.0, (c & 255) / 255.0)
                    color = (0.0, 0.0, 0.0) if (cr < 0.22 and cg < 0.22 and cb < 0.22) else (cr, cg, cb)
                    flags = s.get("flags", 0)
                    font_str = s.get("font", "").lower()
                    is_bold = bool(flags & 16) or any(w in font_str for w in ['bold', 'medium', 'semibold', 'semi-bold', 'demi', 'black', 'heavy'])
                    is_italic = bool(flags & 2) or 'italic' in font_str or 'oblique' in font_str
                    if is_bold and is_italic and os.path.exists(bolditalic_path):
                        fn, ff = 'Roboto-BoldItalic', bolditalic_path
                    elif is_bold and os.path.exists(bold_path):
                        fn, ff = 'Roboto-Bold', bold_path
                    elif is_italic and os.path.exists(italic_path):
                        fn, ff = 'Roboto-Italic', italic_path
                    elif os.path.exists(reg_path):
                        fn, ff = 'Roboto-Regular', reg_path
                    else:
                        fn, ff = 'helv', None
                    sf_obj = get_font_obj(ff)
                    for rb in ROUND_BULLETS:
                        stext = stext.replace(rb, '\u2022')
                    words = [w for w in stext.split() if w]
                    for w in words:
                        w_len = sf_obj.text_length(w, fontsize=sz) if sf_obj else sz * 0.5 * len(w)
                        words_data.append({
                            'word': w, 'fn': fn, 'ff': ff, 'sz': sz, 'color': color,
                            'w': w_len, 'y': s['origin'][1], 'f_obj': sf_obj
                        })

                has_square = any(_is_square_bullet(s.get("text", "").strip(), s.get("font", "").lower()) for s in body_spans)
                total_words = len(words_data)
                total_words_w = sum(wd['w'] for wd in words_data)
                default_sp = words_data[0]['f_obj'].text_length(' ', fontsize=words_data[0]['sz']) if words_data else 2.0


                body_scale = 1.0
                if total_words >= 2 and target_w > 0:
                    gaps = total_words - 1
                    natural_w = total_words_w + gaps * default_sp
                    if natural_w > target_w * 1.005:
                        body_scale = max(0.85, target_w / natural_w)
                        for wd in words_data:
                            wd['sz'] *= body_scale
                            wd['w'] = wd['f_obj'].text_length(wd['word'], fontsize=wd['sz']) if wd['f_obj'] else wd['sz'] * 0.5 * len(wd['word'])
                        total_words_w = sum(wd['w'] for wd in words_data)
                        default_sp = words_data[0]['f_obj'].text_length(' ', fontsize=words_data[0]['sz']) if words_data else 2.0

                full_line_txt = " ".join(wd['word'] for wd in words_data).strip()
                is_label = full_line_txt.endswith(":") or full_line_txt.endswith("?")
                is_centered_block = False
                if len(grouped_lines) >= 2:
                    centers = [(g_line["bbox"][0] + g_line["bbox"][2]) / 2.0 for g_line in grouped_lines]
                    widths = [g_line["bbox"][2] - g_line["bbox"][0] for g_line in grouped_lines]
                    if (max(centers) - min(centers) <= 2.5) and (max(widths) - min(widths) >= 4.0):
                        is_centered_block = True
                can_justify = False
                if is_right_aligned and not has_square and not is_label and not is_centered_block and total_words >= 4 and target_w > total_words_w:
                    gaps = total_words - 1
                    gap_w = (target_w - total_words_w) / gaps
                    if default_sp * 0.5 <= gap_w <= default_sp * 12.0:
                        can_justify = True

                def add_prefix_span(p_span):
                    p_txt = p_span.get("text", "").strip()
                    p_orig = fitz.Point(p_span["origin"])
                    p_sz = p_span.get("size", 10.0)
                    p_c = p_span.get("color", 0)
                    p_cr, p_cg, p_cb = (((p_c >> 16) & 255) / 255.0, ((p_c >> 8) & 255) / 255.0, (p_c & 255) / 255.0)
                    p_col = (0.0, 0.0, 0.0) if (p_cr < 0.22 and p_cg < 0.22 and p_cb < 0.22) else (p_cr, p_cg, p_cb)
                    p_flags = p_span.get("flags", 0)
                    p_fstr = p_span.get("font", "").lower()
                    if _is_square_bullet(p_txt, p_fstr):
                        bw = p_sz * 0.38
                        bh = p_sz * 0.38
                        r = fitz.Rect(p_orig.x, p_orig.y - p_sz * 0.48, p_orig.x + bw, p_orig.y - p_sz * 0.48 + bh)
                        elements_to_draw.append(('rect', r, p_col))
                    else:
                        for rb in ROUND_BULLETS:
                            p_txt = p_txt.replace(rb, '\u2022')
                        p_bold = bool(p_flags & 16) or any(w in p_fstr for w in ['bold', 'medium', 'semibold', 'semi-bold', 'demi', 'black', 'heavy'])
                        p_italic = bool(p_flags & 2) or 'italic' in p_fstr or 'oblique' in p_fstr
                        if p_bold and p_italic and os.path.exists(bolditalic_path):
                            p_fn, p_ff = 'Roboto-BoldItalic', bolditalic_path
                        elif p_bold and os.path.exists(bold_path):
                            p_fn, p_ff = 'Roboto-Bold', bold_path
                        elif p_italic and os.path.exists(italic_path):
                            p_fn, p_ff = 'Roboto-Italic', italic_path
                        elif os.path.exists(reg_path):
                            p_fn, p_ff = 'Roboto-Regular', reg_path
                        else:
                            p_fn, p_ff = 'helv', None
                        elements_to_draw.append(('text', p_orig, p_txt, p_fn, p_ff, p_sz, p_col))

                if can_justify:
                    if prefix_span:
                        add_prefix_span(prefix_span)
                    curr_x = body_x0
                    gaps = total_words - 1
                    gap_w = (target_w - total_words_w) / gaps
                    for i, wd in enumerate(words_data):
                        elements_to_draw.append(('text', fitz.Point(curr_x, wd['y']), wd['word'], wd['fn'], wd['ff'], wd['sz'], wd['color']))
                        if i < gaps:
                            gap_start = curr_x + wd['w']
                            sp_x = gap_start + (gap_w - default_sp) / 2.0
                            elements_to_draw.append(('text', fitz.Point(sp_x, wd['y']), ' ', wd['fn'], wd['ff'], wd['sz'], wd['color']))
                        curr_x += wd['w'] + gap_w
                    continue


                if prefix_span:
                    add_prefix_span(prefix_span)
                    draw_spans = body_spans
                    avail_w = target_w
                    base_start_x = body_x0
                else:
                    draw_spans = spans
                    avail_w = g["bbox"][2] - g["bbox"][0]
                    base_start_x = g["bbox"][0]

                rob_body_w = 0.0
                for s in draw_spans:
                    stext = s.get("text", "")
                    if not stext:
                        continue
                    for rb in ROUND_BULLETS:
                        stext = stext.replace(rb, '\u2022')
                    s_sz = s.get("size", 10.0)
                    s_flags = s.get("flags", 0)
                    s_fstr = s.get("font", "").lower()
                    s_bold = bool(s_flags & 16) or any(w in s_fstr for w in ['bold', 'medium', 'semibold', 'semi-bold', 'demi', 'black', 'heavy'])
                    s_italic = bool(s_flags & 2) or 'italic' in s_fstr or 'oblique' in s_fstr
                    if s_bold and s_italic and os.path.exists(bolditalic_path):
                        s_ff = bolditalic_path
                    elif s_bold and os.path.exists(bold_path):
                        s_ff = bold_path
                    elif s_italic and os.path.exists(italic_path):
                        s_ff = italic_path
                    elif os.path.exists(reg_path):
                        s_ff = reg_path
                    else:
                        s_ff = None
                    sf_obj = get_font_obj(s_ff)
                    clean_st = stext.strip()
                    if clean_st and _is_square_bullet(clean_st, s_fstr):
                        rob_body_w += s_sz * 0.38
                    else:
                        rob_body_w += sf_obj.text_length(stext, fontsize=s_sz) if sf_obj else s_sz * 0.5 * len(stext)

                line_scale = 1.0
                if avail_w > 0 and rob_body_w > avail_w * 1.005:
                    line_scale = max(0.85, avail_w / rob_body_w)

                body_shift_x = (body_x0 - orig_body_x0) if prefix_span else 0.0
                prev_end_x = 0
                prev_orig_x1 = 0
                for s in draw_spans:
                    text = s.get("text", "")
                    if not text:
                        continue
                    origin = fitz.Point(s["origin"][0] + body_shift_x, s["origin"][1])
                    size = s["size"] * line_scale
                    c = s.get("color", 0)
                    cr, cg, cb = (((c >> 16) & 255) / 255.0, ((c >> 8) & 255) / 255.0, (c & 255) / 255.0)
                    color = (0.0, 0.0, 0.0) if (cr < 0.22 and cg < 0.22 and cb < 0.22) else (cr, cg, cb)
                    flags = s.get("flags", 0)
                    font_str = s.get("font", "").lower()
                    is_bold = bool(flags & 16) or any(w in font_str for w in ['bold', 'medium', 'semibold', 'semi-bold', 'demi', 'black', 'heavy'])
                    is_italic = bool(flags & 2) or 'italic' in font_str or 'oblique' in font_str

                    if is_bold and is_italic and os.path.exists(bolditalic_path):
                        fn, ff = 'Roboto-BoldItalic', bolditalic_path
                    elif is_bold and os.path.exists(bold_path):
                        fn, ff = 'Roboto-Bold', bold_path
                    elif is_italic and os.path.exists(italic_path):
                        fn, ff = 'Roboto-Italic', italic_path
                    elif os.path.exists(reg_path):
                        fn, ff = 'Roboto-Regular', reg_path
                    else:
                        fn, ff = 'helv', None

                    f_obj = get_font_obj(ff)
                    for rb in ROUND_BULLETS:
                        text = text.replace(rb, '\u2022')

                    clean_t = text.strip()
                    if clean_t and _is_square_bullet(clean_t, font_str):
                        bw = size * 0.38
                        bh = size * 0.38
                        r = fitz.Rect(origin.x, origin.y - size * 0.48, origin.x + bw, origin.y - size * 0.48 + bh)
                        elements_to_draw.append(('rect', r, color))
                        prev_end_x = origin.x + bw
                        prev_orig_x1 = s["bbox"][2]
                        continue

                    if clean_t and any(_is_square_bullet(ch, font_str) for ch in text):
                        curr_x = origin.x
                        curr_y = origin.y
                        buf = ''
                        for ch in text:
                            if _is_square_bullet(ch, font_str):
                                if buf:
                                    elements_to_draw.append(('text', fitz.Point(curr_x, curr_y), buf, fn, ff, size, color))
                                    curr_x += f_obj.text_length(buf, fontsize=size) if f_obj else size * 0.5 * len(buf)
                                    buf = ''
                                bw = size * 0.38
                                bh = size * 0.38
                                r = fitz.Rect(curr_x, curr_y - size * 0.48, curr_x + bw, curr_y - size * 0.48 + bh)
                                elements_to_draw.append(('rect', r, color))
                                curr_x += bw + size * 0.15
                            else:
                                buf += ch
                        if buf:
                            elements_to_draw.append(('text', fitz.Point(curr_x, curr_y), buf, fn, ff, size, color))
                            curr_x += f_obj.text_length(buf, fontsize=size) if f_obj else size * 0.5 * len(buf)
                        prev_end_x = curr_x
                        prev_orig_x1 = s["bbox"][2]
                        continue

                    sp_char_w = f_obj.text_length(' ', fontsize=size) if f_obj else size * 0.25
                    orig_gap = (s["bbox"][0] - prev_orig_x1) if prev_orig_x1 > 0 else 999.0
                    if prev_end_x > 0:
                        if orig_gap <= 0.5:
                            origin = fitz.Point(prev_end_x, origin.y)
                        elif origin.x < prev_end_x:
                            origin = fitz.Point(prev_end_x, origin.y)
                        elif origin.x > prev_end_x + sp_char_w * 1.2:
                            span_gap = origin.x - prev_end_x
                            sp_x = prev_end_x + (span_gap - sp_char_w) / 2.0
                            elements_to_draw.append(('text', fitz.Point(sp_x, origin.y), ' ', fn, ff, size, color))

                    span_w = f_obj.text_length(text, fontsize=size) if f_obj else size * 0.5 * len(text)
                    prev_end_x = origin.x + span_w
                    prev_orig_x1 = s["bbox"][2]
                    elements_to_draw.append(('text', origin, text, fn, ff, size, color))

        all_pages_elements.append(elements_to_draw)


    for xref in range(1, doc.xref_length()):
        try:
            obj_str = doc.xref_object(xref, compressed=False)
            if '/Subtype /Form' in obj_str or '/Subtype/Form' in obj_str:
                raw_bytes = doc.xref_stream(xref)
                if raw_bytes and b'BT' in raw_bytes:
                    m = re.search(r'/Resources\s+(\d+)\s+0\s+R', obj_str)
                    if m:
                        res_xref = int(m.group(1))
                        res_obj = doc.xref_object(res_xref, compressed=False)
                        new_res = _clear_font_dict(res_obj)
                        if new_res != res_obj:
                            doc.update_object(res_xref, new_res)
                    elif '/Font' in obj_str:
                        new_obj = _clear_font_dict(obj_str)
                        if new_obj != obj_str:
                            doc.update_object(xref, new_obj)

                    raw_data = raw_bytes.decode('latin-1', errors='replace')
                    clean_data = _remove_text_from_content_stream(raw_data)
                    doc.update_stream(xref, clean_data.encode('latin-1'))
        except Exception:
            pass


    for page_idx, page in enumerate(doc):
        try:
            page.clean_contents(sanitize=False)
        except Exception:
            pass
        contents = page.get_contents()
        for cx in contents:
            try:
                raw_data = doc.xref_stream(cx).decode('latin-1', errors='replace')
                clean_data = _remove_text_from_content_stream(raw_data)
                doc.update_stream(cx, clean_data.encode('latin-1'))
            except Exception:
                pass

        try:
            p_obj = doc.xref_object(page.xref, compressed=False)
            m = re.search(r'/Resources\s+(\d+)\s+0\s+R', p_obj)
            if m:
                res_xref = int(m.group(1))
                res_obj = doc.xref_object(res_xref, compressed=False)
                new_res = _clear_font_dict(res_obj)
                if new_res != res_obj:
                    doc.update_object(res_xref, new_res)
            elif '/Font' in p_obj:
                new_p_obj = _clear_font_dict(p_obj)
                if new_p_obj != p_obj:
                    doc.update_object(page.xref, new_p_obj)
        except Exception:
            pass

        for item in all_pages_elements[page_idx]:
            try:
                if item[0] == 'rect':
                    page.draw_rect(item[1], color=None, fill=item[2], overlay=True)
                    total_spans_converted += 1
                elif item[0] == 'text':
                    _, orig, txt, fn, ff, sz, col = item
                    if ff:
                        page.insert_text(orig, txt, fontname=fn, fontfile=ff, fontsize=sz, color=col, overlay=True)
                    else:
                        page.insert_text(orig, txt, fontname=fn, fontsize=sz, color=col, overlay=True)
                    total_spans_converted += 1
            except Exception:
                continue

    for page in doc:
        try:
            page.clean_contents(sanitize=False)
        except Exception:
            pass

    try:
        doc.subset_fonts()
    except Exception:
        pass

    for x in range(1, doc.xref_length()):
        try:
            s_bytes = doc.xref_stream(x)
            if s_bytes and b'begincmap' in s_bytes:
                s = s_bytes.decode('latin-1', errors='replace')
                def fix_hex(m):
                    val = m.group(1)
                    if len(val) % 2 != 0:
                        if len(val) == 5:
                            cp = int(val, 16)
                            return '<' + chr(cp).encode('utf-16be').hex() + '>'
                        return '<0' + val + '>'
                    return '<' + val + '>'
                new_s = re.sub(r'<([0-9a-fA-F]+)>', fix_hex, s)
                if new_s != s:
                    doc.update_stream(x, new_s.encode('latin-1'))
        except Exception:
            pass

    out_pdf = doc.tobytes(garbage=4, deflate=True, clean=False)
    doc.close()
    return out_pdf, total_spans_converted


def load_dataset(file_bytes: bytes, filename: str) -> pd.DataFrame:
    ext = filename.lower().split('.')[-1]
    if ext == 'csv':
        try:
            df = pd.read_csv(io.BytesIO(file_bytes), dtype=str, keep_default_na=False, encoding='utf-8')
        except UnicodeDecodeError:
            df = pd.read_csv(io.BytesIO(file_bytes), dtype=str, keep_default_na=False, encoding='latin-1')
    elif ext in ['xlsx', 'xls']:
        df = pd.read_excel(io.BytesIO(file_bytes), dtype=str, keep_default_na=False)
    else:
        raise HTTPException(status_code=400, detail="Format file data nasabah harus berupa CSV atau Excel (.xlsx, .xls)")

    df.columns = [str(c).strip() for c in df.columns]
    return df


def get_document_clean_names(row: pd.Series, idx: int, columns: list[str]) -> tuple[str, str]:
    id_val = None
    for key in ['NO_POLIS', 'NOMOR_POLIS', 'POLIS', 'ID', 'NO']:
        for c in columns:
            if c.upper() == key:
                id_val = str(row[c]).strip()
                break
        if id_val:
            break

    name_val = None
    for key in ['NAMA', 'NAMA_NASABAH', 'PEMEGANG_POLIS']:
        for c in columns:
            if c.upper() == key:
                name_val = str(row[c]).strip()
                break
        if name_val:
            break

    clean_id = re.sub(r'[^a-zA-Z0-9_-]', '_', id_val) if id_val else f"NASABAH_{idx+1:04d}"
    clean_name = ("_" + re.sub(r'[^a-zA-Z0-9_-]', '_', name_val)) if name_val else ""
    return clean_id, clean_name


def generate_batch_xml_zip(template_bytes: bytes, df: pd.DataFrame) -> tuple[io.BytesIO, int]:
    try:
        template_str = template_bytes.decode('utf-8')
    except UnicodeDecodeError:
        template_str = template_bytes.decode('latin-1', errors='replace')

    zip_buffer = io.BytesIO()
    total_generated = 0

    with zipfile.ZipFile(zip_buffer, mode='w', compression=zipfile.ZIP_DEFLATED) as zf:
        for idx, row in df.iterrows():
            rendered_xml = template_str

            for col in df.columns:
                val = str(row[col]) if row[col] is not None else ""
                patterns = [
                    re.compile(r'\{\{\s*' + re.escape(col) + r'\s*\}\}', re.IGNORECASE),
                    re.compile(r'\{' + re.escape(col) + r'\}', re.IGNORECASE),
                    re.compile(r'\[' + re.escape(col) + r'\]', re.IGNORECASE),
                ]
                for pattern in patterns:
                    rendered_xml = pattern.sub(val, rendered_xml)

            clean_id, clean_name = get_document_clean_names(row, idx, list(df.columns))
            filename = f"DOC_{idx+1:04d}_{clean_id}{clean_name}.xml"

            zf.writestr(filename, rendered_xml.encode('utf-8'))
            total_generated += 1

    zip_buffer.seek(0)
    return zip_buffer, total_generated


def generate_batch_pdf_zip(template_bytes: bytes, df: pd.DataFrame) -> tuple[io.BytesIO, int]:
    reg_path = ROBOTO_FONTS['regular']
    zip_buffer = io.BytesIO()
    total_generated = 0


    normalized_template, _ = convert_pdf_all_to_roboto(template_bytes)

    with zipfile.ZipFile(zip_buffer, mode='w', compression=zipfile.ZIP_DEFLATED) as zf:
        for idx, row in df.iterrows():
            doc = pymupdf.open(stream=normalized_template, filetype="pdf")

            for page in doc:
                replacements_to_draw = []

                for col in df.columns:
                    val = str(row[col]) if row[col] is not None else ""
                    placeholder_variants = [
                        f"{{{{{col}}}}}",
                        f"{{{{ {col} }}}}",
                        f"{{{col}}}",
                        f"[{col}]"
                    ]

                    for p_var in placeholder_variants:
                        rects = page.search_for(p_var)
                        for r in rects:
                            page.add_redact_annot(r, fill=None)
                            replacements_to_draw.append((r, val))

                page.apply_redactions(images=0, graphics=0, text=0)

                for r, val in replacements_to_draw:
                    point = pymupdf.Point(r.x0, r.y1 - 2)
                    if os.path.exists(reg_path):
                        page.insert_text(point, val, fontname="Roboto-Regular", fontfile=reg_path, fontsize=10, color=(0.0, 0.17, 0.42))
                    else:
                        page.insert_text(point, val, fontname="helv", fontsize=10, color=(0.0, 0.17, 0.42))

            clean_id, clean_name = get_document_clean_names(row, idx, list(df.columns))
            filename = f"DOC_{idx+1:04d}_{clean_id}{clean_name}.pdf"

            try:
                doc.subset_fonts()
            except Exception:
                pass

            zf.writestr(filename, doc.tobytes(garbage=4, deflate=True))
            doc.close()
            total_generated += 1

    zip_buffer.seek(0)
    return zip_buffer, total_generated


@app.get("/", response_class=HTMLResponse)
async def home(request: Request):
    response = templates.TemplateResponse(request=request, name="index.html")
    response.headers["Cache-Control"] = "no-cache, no-store, must-revalidate"
    response.headers["Pragma"] = "no-cache"
    response.headers["Expires"] = "0"
    return response


@app.get("/favicon.ico", include_in_schema=False)
async def favicon():
    svg_icon = (
        '<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 100 100">'
        '<rect width="100" height="100" rx="24" fill="#2563EB"/>'
        '<text x="50" y="65" font-family="system-ui, sans-serif" font-weight="bold" '
        'font-size="52" fill="#FFFFFF" text-anchor="middle">F</text>'
        '</svg>'
    )
    return Response(content=svg_icon, media_type="image/svg+xml")


def process_single_file_content(file_bytes: bytes, filename: str) -> tuple[bytes, str, int]:
    ext = filename.lower().split('.')[-1]
    if ext == 'xml':
        converted_bytes, replacements = convert_xml_arial_to_roboto(file_bytes)
        media_type = "application/xml"
    elif ext == 'docx':
        converted_bytes, replacements = convert_docx_arial_to_roboto(file_bytes)
        media_type = "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
    elif ext == 'pdf':
        converted_bytes, replacements = convert_pdf_all_to_roboto(file_bytes)
        media_type = "application/pdf"
    elif ext == 'rtf':
        converted_bytes, replacements = convert_rtf_arial_to_roboto(file_bytes)
        media_type = "application/rtf"
    elif ext == 'doc':
        converted_bytes, replacements = convert_doc_arial_to_roboto(file_bytes)
        media_type = "application/msword"
    else:
        raise HTTPException(
            status_code=400,
            detail=f"Tipe file '{filename}' tidak didukung. Format yang didukung: .xml, .docx, .doc, .pdf, .rtf, atau .zip."
        )
    return converted_bytes, media_type, replacements


@app.post("/api/convert-font")
async def convert_font_endpoint(
    file: Optional[UploadFile] = File(None),
    files: Optional[list[UploadFile]] = File(None)
):
    upload_list: list[UploadFile] = []
    if files:
        upload_list.extend(files)
    if file:
        upload_list.append(file)

    if not upload_list:
        raise HTTPException(status_code=400, detail="Tidak ada file yang diunggah.")


    if len(upload_list) == 1 and not (upload_list[0].filename or "").lower().endswith('.zip'):
        single_file = upload_list[0]
        filename = single_file.filename or "document"
        file_bytes = await single_file.read()
        if not file_bytes:
            raise HTTPException(status_code=400, detail="File kosong.")

        converted_bytes, media_type, replacements = process_single_file_content(file_bytes, filename)
        out_name = f"Roboto_{filename}"

        return StreamingResponse(
            io.BytesIO(converted_bytes),
            media_type=media_type,
            headers={
                "Content-Disposition": f'attachment; filename="{out_name}"',
                "X-Replacements-Count": str(replacements),
                "Access-Control-Expose-Headers": "Content-Disposition, X-Replacements-Count"
            }
        )


    out_zip_buffer = io.BytesIO()
    total_converted = 0
    total_replacements = 0
    used_names = set()

    def get_unique_zip_entry(name: str) -> str:
        base = name
        counter = 1
        while name in used_names:
            parts = base.rsplit('.', 1)
            if len(parts) == 2:
                name = f"{parts[0]}_{counter}.{parts[1]}"
            else:
                name = f"{base}_{counter}"
            counter += 1
        used_names.add(name)
        return name

    with zipfile.ZipFile(out_zip_buffer, mode='w', compression=zipfile.ZIP_DEFLATED) as zf:
        for ufile in upload_list:
            fname = ufile.filename or "file"
            fbytes = await ufile.read()
            if not fbytes:
                continue

            f_ext = fname.lower().split('.')[-1]
            if f_ext == 'zip':
                try:
                    with zipfile.ZipFile(io.BytesIO(fbytes)) as in_zf:
                        for item in in_zf.infolist():
                            if item.is_dir() or item.filename.startswith('__MACOSX') or item.filename.split('/')[-1].startswith('.'):
                                continue
                            item_ext = item.filename.lower().split('.')[-1]
                            if item_ext in ['xml', 'docx', 'doc', 'pdf', 'rtf']:
                                raw_item_bytes = in_zf.read(item)
                                c_bytes, _, reps = process_single_file_content(raw_item_bytes, item.filename)
                                parts = item.filename.split('/')
                                parts[-1] = f"Roboto_{parts[-1]}"
                                zip_entry_name = "/".join(parts)
                                zf.writestr(zip_entry_name, c_bytes)
                                total_converted += 1
                                total_replacements += reps
                except Exception as e:
                    raise HTTPException(status_code=400, detail=f"Gagal memproses file ZIP '{fname}': {str(e)}")
            elif f_ext in ['xml', 'docx', 'doc', 'pdf', 'rtf']:
                c_bytes, _, reps = process_single_file_content(fbytes, fname)
                entry_name = get_unique_zip_entry(f"Roboto_{fname}")
                zf.writestr(entry_name, c_bytes)
                total_converted += 1
                total_replacements += reps

    if total_converted == 0:
        raise HTTPException(status_code=400, detail="Tidak ditemukan file PDF, DOCX, DOC, XML, atau RTF yang valid untuk dikonversi.")

    out_zip_buffer.seek(0)

    if len(upload_list) == 1 and (upload_list[0].filename or "").lower().endswith('.zip'):
        raw_name = upload_list[0].filename
        base_zip = os.path.splitext(raw_name)[0]
        out_zip_name = f"Roboto_{base_zip}.zip"
    else:
        out_zip_name = f"Roboto_Converted_{total_converted}_Files.zip"

    return StreamingResponse(
        out_zip_buffer,
        media_type="application/zip",
        headers={
            "Content-Disposition": f'attachment; filename="{out_zip_name}"',
            "X-Converted-Count": str(total_converted),
            "X-Replacements-Count": str(total_replacements),
            "Access-Control-Expose-Headers": "Content-Disposition, X-Converted-Count, X-Replacements-Count"
        }
    )


@app.post("/api/batch-merge")
async def batch_merge_endpoint(
    template_file: UploadFile = File(...),
    data_file: UploadFile = File(...)
):
    t_filename = template_file.filename or ""
    t_ext = t_filename.lower().split('.')[-1]
    if t_ext not in ['xml', 'pdf']:
        raise HTTPException(status_code=400, detail="File Master Template harus berformat XML (.xml) atau PDF (.pdf).")

    d_filename = data_file.filename or ""
    d_ext = d_filename.lower().split('.')[-1]
    if d_ext not in ['csv', 'xlsx', 'xls']:
        raise HTTPException(status_code=400, detail="File Data Nasabah harus berformat CSV atau Excel (.xlsx/.xls).")

    template_bytes = await template_file.read()
    data_bytes = await data_file.read()

    if not template_bytes or not data_bytes:
        raise HTTPException(status_code=400, detail="Template atau Data Nasabah tidak boleh kosong.")

    df = load_dataset(data_bytes, d_filename)
    if df.empty:
        raise HTTPException(status_code=400, detail="File Data Nasabah tidak memiliki baris data (kosong).")

    if t_ext == 'xml':
        zip_buffer, count = generate_batch_xml_zip(template_bytes, df)
        out_zip_name = f"AstraLife_XML_Batch_Result_{count}_Docs.zip"
    else:
        zip_buffer, count = generate_batch_pdf_zip(template_bytes, df)
        out_zip_name = f"AstraLife_PDF_Batch_Result_{count}_Docs.zip"

    return StreamingResponse(
        zip_buffer,
        media_type="application/zip",
        headers={
            "Content-Disposition": f'attachment; filename="{out_zip_name}"',
            "X-Generated-Count": str(count),
            "Access-Control-Expose-Headers": "Content-Disposition, X-Generated-Count"
        }
    )


@app.post("/api/preview-data")
async def preview_data_endpoint(data_file: UploadFile = File(...)):
    filename = data_file.filename or ""
    file_bytes = await data_file.read()
    df = load_dataset(file_bytes, filename)

    columns = list(df.columns)
    preview_rows = df.head(5).to_dict(orient="records")
    total_rows = len(df)

    return {
        "columns": columns,
        "rows": preview_rows,
        "total_rows": total_rows
    }


@app.get("/api/download-sample/{sample_type}")
async def download_sample(sample_type: str):
    if sample_type == "xml":
        path = os.path.join(SAMPLES_DIR, "template.xml")
        return FileResponse(path, filename="template_sample.xml", media_type="application/xml")
    elif sample_type == "docx":
        path = os.path.join(SAMPLES_DIR, "template.docx")
        return FileResponse(path, filename="template_sample.docx", media_type="application/vnd.openxmlformats-officedocument.wordprocessingml.document")
    elif sample_type == "pdf":
        path = os.path.join(SAMPLES_DIR, "template.pdf")
        return FileResponse(path, filename="template_sample.pdf", media_type="application/pdf")
    elif sample_type == "csv":
        path = os.path.join(SAMPLES_DIR, "data_nasabah.csv")
        return FileResponse(path, filename="data_nasabah_sample.csv", media_type="text/csv")
    else:
        raise HTTPException(status_code=404, detail="Tipe sample tidak ditemukan.")


if __name__ == "__main__":
    import uvicorn
    uvicorn.run("main:app", host="127.0.0.1", port=8000, reload=True)
