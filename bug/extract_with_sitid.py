import sys, io, os, zipfile, shutil
sys.stdout = io.TextIOWrapper(sys.stdout.buffer, encoding='utf-8')

import openpyxl

xlsx_path = r'C:\Project\Converting Font\bug\SIT LOG TANGO REPLACEMENT (1) (2).xlsx'
out_dir = r'C:\Project\Converting Font\bug\extracted_images'
os.makedirs(out_dir, exist_ok=True)

wb = openpyxl.load_workbook(xlsx_path)
ws_review = wb['Review Doc']

# Build row -> SIT ID mapping
row_to_sit = {}
for row in ws_review.iter_rows(min_row=2, values_only=False):
    sit_id = row[0].value
    if sit_id:
        row_to_sit[row[0].row] = sit_id

print("Row to SIT ID mapping:")
for r, s in sorted(row_to_sit.items()):
    print(f"  Row {r} -> {s}")

# Map images to SIT IDs
images = getattr(ws_review, '_images', [])
print(f"\nTotal images in Review Doc: {len(images)}")

# Extract images directly from zip
with zipfile.ZipFile(xlsx_path, 'r') as z:
    media_files = sorted([f for f in z.namelist() if 'xl/media/' in f])
    print(f"\nTotal media files in zip: {len(media_files)}")
    
    # Map image index to anchor row
    image_info = []
    for i, img in enumerate(images):
        anchor = img.anchor
        try:
            row = anchor._from.row + 1
        except:
            try:
                row = anchor.row + 1
            except:
                row = None
        sit_id = row_to_sit.get(row, f"Unknown(row={row})")
        image_info.append({'idx': i+1, 'row': row, 'sit_id': sit_id})
    
    # Save each extracted image with SIT ID name
    for i, (mf, info) in enumerate(zip(media_files, image_info)):
        fname = os.path.basename(mf)
        base, ext = os.path.splitext(fname)
        sit_label = str(info['sit_id']).replace('-', '_')
        new_name = f"{sit_label}_img{info['idx']}{ext}"
        out_path = os.path.join(out_dir, new_name)
        with z.open(mf) as src, open(out_path, 'wb') as dst:
            shutil.copyfileobj(src, dst)
        print(f"  Saved: {new_name}  (anchor row={info['row']}, orig={fname})")
