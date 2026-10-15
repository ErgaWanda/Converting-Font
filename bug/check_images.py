import sys, io
sys.stdout = io.TextIOWrapper(sys.stdout.buffer, encoding='utf-8')

import openpyxl

wb = openpyxl.load_workbook(r'C:\Project\Converting Font\bug\SIT LOG TANGO REPLACEMENT (1) (2).xlsx')

for sheet_name in wb.sheetnames:
    ws = wb[sheet_name]
    images = getattr(ws, '_images', [])
    if images:
        print(f'\nSheet [{sheet_name}] has {len(images)} images:')
        for i, img in enumerate(images):
            anchor = img.anchor
            row = None
            col = None
            try:
                # TwoCellAnchor
                row = anchor._from.row + 1
                col = anchor._from.col + 1
            except:
                try:
                    row = anchor.row + 1
                    col = anchor.col + 1
                except:
                    pass
            img_ref = getattr(img, 'ref', 'N/A')
            print(f'  Image {i+1}: anchor_row={row}, anchor_col={col}, ref={img_ref}')
    else:
        print(f'Sheet [{sheet_name}]: no images')
