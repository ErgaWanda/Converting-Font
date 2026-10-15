import sys, io, os
sys.stdout = io.TextIOWrapper(sys.stdout.buffer, encoding='utf-8')
sys.path.insert(0, os.path.dirname(os.path.dirname(os.path.abspath(__file__))))
import re

from main import fix_wording_and_formatting

print("=== HASIL TEST fix_wording_and_formatting ===\n")
tests = [
    ("Koma double (,,)",         "Hormat Kami,,",                         "Hormat Kami,"),
    ("Rupiah positif",           "Rupiah 13000000",                        "Rp 13.000.000,00"),
    ("Rupiah kecil",             "Rupiah 617754",                          "Rp 617.754,00"),
    ("Rupiah negatif",           "Rupiah -12857415",                       "-Rp 12.857.415,00"),
    ("Nomor telepon",            "1-500-AVA (282)",                        "1-500-282"),
    ("Wording billing",          "yang perlu Anda bayarkan",               "yang akan didebet"),
    ("Polis Lama → Baru",        "Pembayar Premi Polis Lama",             "Pembayar Premi Polis Baru"),
    ("Terima kasih + email",     "hello@astralife.co.id. Terima kasih.",  "hello@astralife.co.id.\nTerima kasih atas perhatian"),
    ("Terima kasih standalone",  "Terima kasih.",                         "Terima kasih atas perhatian"),
    ("Titik ganda",              "terlampir..untuk info",                  "terlampir.untuk info"),
    ("Spasi sebelum koma",       "Hormat Kami ,",                         "Hormat Kami,"),
    ("Kurung tutup Peserta",     "Nama Peserta (Tertanggung",              "Nama Peserta (Tertanggung)"),
    ("Format tanggal Indonesia", "Jakarta, 02/08/2025",                    "Jakarta, 02 Agustus 2025"),
    ("Email hyperlink HTML",     "<p>Hubungi hello@astralife.co.id</p>",   '<a href="mailto:hello@astralife.co.id"'),
]

all_pass = True
for name, inp, expected_substr in tests:
    result = fix_wording_and_formatting(inp)
    ok = expected_substr in result
    if not ok:
        all_pass = False
    icon = "✅" if ok else "❌"
    print(f"{icon} [{name}]")
    if not ok:
        print(f"   IN : {repr(inp)}")
        print(f"   EXP: {repr(expected_substr)}")
        print(f"   OUT: {repr(result)}")

print()
print("✅ SEMUA TEST BERHASIL!" if all_pass else "❌ ADA TEST YANG GAGAL!")
