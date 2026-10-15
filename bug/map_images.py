import sys, io, zipfile, os
sys.stdout = io.TextIOWrapper(sys.stdout.buffer, encoding='utf-8')
import openpyxl

xlsx_path = r'C:\Project\Converting Font\bug\SIT LOG TANGO REPLACEMENT (1) (2).xlsx'

# Baca mapping gambar dari drawing rels di dalam xlsx (untuk semua sheet)
with zipfile.ZipFile(xlsx_path, 'r') as z:
    all_files = z.namelist()
    # cari semua drawing rels
    drawing_rels = [f for f in all_files if 'drawing' in f.lower() and 'rels' in f.lower()]
    drawings = [f for f in all_files if 'drawing' in f.lower() and f.endswith('.xml') and 'rels' not in f]
    sheet_rels = [f for f in all_files if 'sheet' in f.lower() and 'rels' in f.lower()]
    
    print("Drawing rels files:")
    for f in drawing_rels:
        print(f"  {f}")
    print("\nDrawing xml files:")
    for f in drawings:
        print(f"  {f}")
    print("\nSheet rels files:")
    for f in sheet_rels:
        print(f"  {f}")
    
    # Baca setiap drawing rels untuk cari mapping image ke posisi
    print("\n=== DRAWING RELS CONTENT ===")
    for drel in drawing_rels:
        print(f"\n--- {drel} ---")
        content = z.read(drel).decode('utf-8', errors='replace')
        print(content[:3000])
    
    # Baca drawing xml untuk cari anchor (row/col) dan image ref
    print("\n=== DRAWING XML SNIPPETS (anchor info) ===")
    for d in drawings:
        print(f"\n--- {d} ---")
        content = z.read(d).decode('utf-8', errors='replace')
        # Tampilkan snippet yang berisi from/to anchor info
        import re
        anchors = re.findall(r'<xdr:(?:twoCellAnchor|oneCellAnchor|absoluteAnchor).*?</xdr:(?:twoCellAnchor|oneCellAnchor|absoluteAnchor)>', content, re.DOTALL)
        for i, a in enumerate(anchors[:5]):
            print(f"  Anchor {i+1}: {a[:400]}")
        print(f"  Total anchors: {len(anchors)}")
