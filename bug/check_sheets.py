import zipfile
import re

xlsx_path = r'C:\Project\Converting Font\bug\SIT LOG TANGO REPLACEMENT (1) (2).xlsx'
with zipfile.ZipFile(xlsx_path, 'r') as z:
    for name in ['xl/worksheets/sheet1.xml', 'xl/worksheets/sheet2.xml', 'xl/worksheets/sheet3.xml', 'xl/worksheets/sheet4.xml']:
        xml = z.read(name).decode('utf-8', errors='replace')
        print(f"=== {name} ===")
        # check for drawing, legacyDrawing, table, picture, etc.
        matches = re.findall(r'<[^>]*drawing[^>]*>', xml, re.IGNORECASE)
        print("  drawing tags:", matches)
        matches2 = re.findall(r'<[^>]*object[^>]*>', xml, re.IGNORECASE)
        print("  object tags:", matches2)
        matches3 = re.findall(r'<[^>]*picture[^>]*>', xml, re.IGNORECASE)
        print("  picture tags:", matches3)
        matches4 = re.findall(r'<[^>]*media[^>]*>', xml, re.IGNORECASE)
        print("  media tags:", matches4)
