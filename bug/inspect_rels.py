import zipfile
import xml.etree.ElementTree as ET

xlsx_path = r'C:\Project\Converting Font\bug\SIT LOG TANGO REPLACEMENT (1) (2).xlsx'
with zipfile.ZipFile(xlsx_path, 'r') as z:
    for name in z.namelist():
        if 'rels' in name:
            content = z.read(name).decode('utf-8', errors='replace')
            if 'image' in content or 'drawing' in content:
                print('=== RELS:', name)
                try:
                    root = ET.fromstring(content)
                    for elem in root:
                        print(f"  Id={elem.attrib.get('Id')} Type={elem.attrib.get('Type','').split('/')[-1]} Target={elem.attrib.get('Target')}")
                except Exception as e:
                    print('  Error parsing:', e)

    print("\n=== ALL DRAWING XMLs ===")
    for name in z.namelist():
        if 'drawing' in name and not name.endswith('.rels'):
            print(name)
