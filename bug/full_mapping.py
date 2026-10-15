import sys, io, zipfile, re, os
sys.stdout = io.TextIOWrapper(sys.stdout.buffer, encoding='utf-8')

xlsx_path = r'C:\Project\Converting Font\bug\SIT LOG TANGO REPLACEMENT (1) (2).xlsx'

with zipfile.ZipFile(xlsx_path, 'r') as z:
    rels_xml = z.read('xl/drawings/_rels/drawing1.xml.rels').decode('utf-8', errors='replace')
    rid_to_file = {}
    for m in re.finditer(r'Id="(rId\d+)"[^>]*Target="([^"]+)"', rels_xml):
        rid_to_file[m.group(1)] = m.group(2).split('/')[-1]

    drawing_xml = z.read('xl/drawings/drawing1.xml').decode('utf-8', errors='replace')
    anchors = re.findall(r'<twoCellAnchor[^>]*>(.*?)</twoCellAnchor>', drawing_xml, re.DOTALL)

    anchor_map = []
    for anchor in anchors:
        from_row_m = re.search(r'<from>.*?<row>(\d+)</row>', anchor, re.DOTALL)
        rid_m = re.search(r'r:embed="(rId\d+)"', anchor)
        if from_row_m and rid_m:
            row_0 = int(from_row_m.group(1))
            rid = rid_m.group(1)
            img_file = rid_to_file.get(rid, 'UNKNOWN')
            anchor_map.append((row_0, rid, img_file))

import openpyxl
wb = openpyxl.load_workbook(xlsx_path)
ws = wb['Review Doc']

# Build row_0indexed -> data
row_to_data = {}
for r_idx, row in enumerate(ws.iter_rows(min_row=2, values_only=True)):
    if row[0]:
        row_to_data[r_idx] = {  # r_idx=0 means spreadsheet row 2
            'sit_id': row[0],
            'template': row[2] or '',
            'issue': (row[4] or '')[:120],
            'expected': (row[5] or '')[:80],
            'category': row[9] or '',
            'severity': row[10] or ''
        }

print("=== MAPPING: Image -> SIT ID ===")
print(f"{'row_0':>5} | {'rId':>6} | {'ImageFile':>14} | {'SIT ID':>8} | Template | Issue snippet")
print("-"*110)
for row_0, rid, img_file in sorted(anchor_map, key=lambda x: x[0]):
    data = row_to_data.get(row_0, {})
    sit = data.get('sit_id', f'?row={row_0}')
    tmpl = data.get('template', '')[:25]
    issue = data.get('issue', '')[:50]
    cat = data.get('category', '')
    sev = data.get('severity', '')
    print(f"  {row_0:3d}  | {rid:>6} | {img_file:>14} | {sit:8} | {tmpl:25} | [{cat}/{sev}] {issue}")

# Images not in XLS
xls_images = {img for _, _, img in anchor_map}
folder_images = set(os.listdir(r'C:\Project\Converting Font\bug\image'))
not_in_xls = sorted(folder_images - xls_images)
in_xls = sorted(xls_images & folder_images)
print()
print(f"=== SUMMARY ===")
print(f"Folder image total   : {len(folder_images)}")
print(f"Di XLS (via drawing) : {len(xls_images)} -> {sorted(xls_images)}")
print(f"Tidak di XLS         : {len(not_in_xls)} -> {not_in_xls}")
