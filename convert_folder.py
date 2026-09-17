import os
import sys
import argparse
import time
from pathlib import Path


from main import convert_xml_arial_to_roboto, convert_docx_arial_to_roboto


def process_folder(input_dir: str, output_dir: str = None, recursive: bool = True):
    input_path = Path(input_dir).resolve()
    if not input_path.exists() or not input_path.is_dir():
        print(f"[ERROR] Folder input '{input_dir}' tidak ditemukan atau bukan direktori.")
        sys.exit(1)

    if output_dir:
        output_path = Path(output_dir).resolve()
    else:
        output_path = input_path.parent / f"{input_path.name}_ROBOTO"

    output_path.mkdir(parents=True, exist_ok=True)

    print("=" * 70)
    print("   ASTRA LIFE - BATCH FONT CONVERTER (Arial -> Roboto)")
    print("=" * 70)
    print(f"Folder Sumber : {input_path}")
    print(f"Folder Output : {output_path}")
    print(f"Pencarian     : {'Rekursif (termasuk sub-folder)' if recursive else 'Hanya folder utama'}")
    print("-" * 70)


    files_to_process = []
    pattern = "**/*" if recursive else "*"
    for item in input_path.glob(pattern):
        if item.is_file() and item.suffix.lower() in ['.xml', '.docx']:

            if not item.name.startswith("~$"):
                files_to_process.append(item)

    total_files = len(files_to_process)
    if total_files == 0:
        print("[INFO] Tidak ditemukan file .xml atau .docx di dalam folder tersebut.")
        return

    print(f"[FOUND] Menemukan {total_files} file yang akan dikonversi...\n")

    start_time = time.time()
    success_count = 0
    fail_count = 0
    total_replacements = 0

    for idx, file_path in enumerate(files_to_process, 1):
        rel_path = file_path.relative_to(input_path)

        dest_filename = f"Roboto_{file_path.name}"
        dest_file_path = output_path / rel_path.parent / dest_filename
        dest_file_path.parent.mkdir(parents=True, exist_ok=True)

        try:
            with open(file_path, "rb") as f:
                content = f.read()

            ext = file_path.suffix.lower()
            if ext == ".xml":
                converted_bytes, replacements = convert_xml_arial_to_roboto(content)
            elif ext == ".docx":
                converted_bytes, replacements = convert_docx_arial_to_roboto(content)
            else:
                continue

            with open(dest_file_path, "wb") as f:
                f.write(converted_bytes)

            success_count += 1
            total_replacements += replacements
            print(f"[{idx}/{total_files}] OK: {rel_path} -> {dest_filename} ({replacements} font diubah)")

        except Exception as e:
            fail_count += 1
            print(f"[{idx}/{total_files}] FAILED: {rel_path} - Error: {e}")

    elapsed = time.time() - start_time
    print("-" * 70)
    print("RINGKASAN KONVERSI SELESAI:")
    print(f"  - Total File Diproses : {total_files}")
    print(f"  - Berhasil Dikonversi : {success_count} file")
    print(f"  - Gagal               : {fail_count} file")
    print(f"  - Total Penggantian   : {total_replacements} deklarasi font Arial -> Roboto")
    print(f"  - Waktu Eksekusi      : {elapsed:.2f} detik")
    print(f"  - Lokasi Hasil        : {output_path}")
    print("=" * 70)


def main():
    parser = argparse.ArgumentParser(
        description="Konversi font Arial ke Roboto untuk seluruh file XML & DOCX dalam 1 folder."
    )
    parser.add_argument("folder", help="Path ke folder yang berisi dokumen (.xml / .docx)")
    parser.add_argument("-o", "--output", help="Path ke folder output (opsional, default: <folder>_ROBOTO)", default=None)
    parser.add_argument("--no-recursive", action="store_true", help="Jangan cari subfolder (hanya folder tingkat pertama)")

    args = parser.parse_args()
    process_folder(args.folder, args.output, recursive=not args.no_recursive)


if __name__ == "__main__":
    main()
