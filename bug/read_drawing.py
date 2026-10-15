import sys, io, zipfile, re
sys.stdout = io.TextIOWrapper(sys.stdout.buffer, encoding='utf-8')

xlsx_path = r'C:\Project\Converting Font\bug\SIT LOG TANGO REPLACEMENT (1) (2).xlsx'

with zipfile.ZipFile(xlsx_path, 'r') as z:
    # Baca drawing1.xml lengkap
    drawing_xml = z.read('xl/drawings/drawing1.xml').decode('utf-8', errors='replace')
    
    # Cari semua anchor - coba berbagai namespace/tag
    print("=== RAW DRAWING XML (first 5000 chars) ===")
    print(drawing_xml[:5000])
    print("\n=== LAST 2000 chars ===")
    print(drawing_xml[-2000:])
    print(f"\nTotal length: {len(drawing_xml)}")
    
    # Hitung berapa gambar ada di drawing
    rId_count = len(re.findall(r'r:id="rId\d+"', drawing_xml))
    blip_count = len(re.findall(r'<[^>]*:blip[^>]*/>', drawing_xml))
    print(f"\nrId references: {rId_count}")
    print(f"blip tags: {blip_count}")
    
    # Sheet rels - cari drawing reference
    for srel in ['xl/worksheets/_rels/sheet3.xml.rels', 'xl/worksheets/_rels/sheet4.xml.rels']:
        try:
            content = z.read(srel).decode('utf-8', errors='replace')
            print(f"\n--- {srel} ---")
            print(content)
        except:
            pass
