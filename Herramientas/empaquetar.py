"""Empaqueta fuentes y arte editable, sin incluir cachés ni archivos temporales de Unity."""
from pathlib import Path
from zipfile import ZipFile, ZIP_DEFLATED
import struct, zlib
from PIL import Image

root = Path(r'D:\CodexWork\Cartal')
for path in (root/'Arte-LibreSprite').glob('*.ase'):
    data = path.read_bytes()
    size, magic, frames, width, height, depth = struct.unpack_from('<I5H', data)
    assert size == len(data) and magic == 0xA5E0 and frames == 1 and depth == 32, path
    frame_size, frame_magic, count = struct.unpack_from('<IHH', data, 128)
    assert frame_magic == 0xF1FA and frame_size == len(data)-128 and count == 2, path
    offset = 144
    recovered = None
    for _ in range(count):
        chunk_size, kind = struct.unpack_from('<IH', data, offset)
        content = data[offset+6:offset+chunk_size]
        if kind == 0x2005:
            cel_width, cel_height = struct.unpack_from('<HH', content, 16)
            pixels = zlib.decompress(content[20:])
            assert len(pixels) == width*height*4 and (cel_width,cel_height)==(width,height), path
            recovered = Image.frombytes('RGBA', (width,height), pixels)
        offset += chunk_size
    expected = Image.open(root/'Proyecto/Assets/Arte'/f'{path.stem}.png').convert('RGBA')
    if expected.size != recovered.size:
        expected = expected.resize(recovered.size, Image.Resampling.NEAREST)
    assert expected.tobytes() == recovered.tobytes(), path
print('ASE_OK: capas editables y píxeles verificados en todas las fuentes de arte.')

target = root/'Entrega/Cartal-Proyecto.zip'
with ZipFile(target,'w',ZIP_DEFLATED) as archive:
    for directory in ['Proyecto/Assets', 'Proyecto/Packages', 'Proyecto/ProjectSettings', 'Arte-LibreSprite', 'Herramientas']:
        for path in (root/directory).rglob('*'):
            if path.is_file(): archive.write(path,path.relative_to(root))
    for path in (root/'Proyecto').glob('*.md'):
        archive.write(path,path.relative_to(root))
    archive.write(root/'Proyecto/.gitignore','Proyecto/.gitignore')
    archive.write(root/'Entrega/Cartal-Imagen-AR.png','Cartal-Imagen-AR.png')
print(f'PROYECTO_ZIP_OK: {target} ({target.stat().st_size} bytes)')
