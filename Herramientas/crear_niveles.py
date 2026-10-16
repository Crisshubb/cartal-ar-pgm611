"""Amplía el pixel art de Cartal sin regenerar los recursos de la cripta."""
from pathlib import Path
from PIL import Image, ImageDraw
import struct, zlib, random, shutil

ROOT = Path(__file__).resolve().parents[1]
ART = ROOT / 'Proyecto/Assets/Arte'
EDIT = ROOT / 'Arte-LibreSprite'
MARKERS = ROOT / 'Imagenes-AR'
INK, GOLD, LIGHT = '#17151e', '#b98d55', '#ebd5aa'

def save(im, name):
    im = im.convert('RGBA')
    im.save(ART / (name + '.png'))
    label = name.encode()
    layer = struct.pack('<6HB3xH', 3, 0, 0, 0, 0, 0, 255, len(label)) + label
    cel = struct.pack('<HhhBHh5xHH', 0, 0, 0, 255, 2, 0, im.width, im.height) + zlib.compress(im.tobytes())
    chunks = b''.join(struct.pack('<IH', len(data)+6, kind)+data for kind,data in [(0x2004,layer),(0x2005,cel)])
    frame = struct.pack('<IHHH2xI',len(chunks)+16,0xF1FA,2,100,2)+chunks
    header = bytearray(128)
    struct.pack_into('<I5H', header, 0, len(frame)+128,0xA5E0,1,im.width,im.height,32)
    struct.pack_into('<IH', header,14,1,100)
    header[34:36]=bytes([1,1])
    (EDIT/(name+'.ase')).write_bytes(header+frame)

def icon(kind):
    im=Image.new('RGBA',(32,32)); d=ImageDraw.Draw(im)
    if kind=='libro':
        d.rectangle((5,4,26,28),fill=INK);d.rectangle((7,5,25,25),fill='#6d568d');d.rectangle((9,26,25,27),fill=LIGHT)
        d.line((10,5,10,24),fill=GOLD,width=2);d.polygon([(18,9),(23,15),(18,21),(13,15)],outline=LIGHT)
    elif kind=='pergamino':
        d.rectangle((7,5,25,26),fill=LIGHT);d.rectangle((5,3,23,7),fill=GOLD);d.rectangle((9,25,27,29),fill=GOLD)
        for y,w in [(11,11),(15,8),(19,10)]:d.line((11,y,11+w,y),fill='#6b504a',width=2)
    elif kind=='pluma':
        d.polygon([(5,28),(11,10),(24,2),(28,6),(22,19)],fill='#8fc6bd',outline=LIGHT)
        d.line((5,29,23,7),fill=GOLD,width=2)
        for x,y in [(14,16),(18,11),(10,21)]:d.line((x,y,x+7,y-1),fill=INK)
    elif kind=='runa':
        d.polygon([(8,3),(24,5),(28,25),(21,30),(4,26)],fill='#566d7a',outline=LIGHT)
        d.line((14,7,14,25),fill='#8ce5dc',width=2);d.line((14,8,23,14,14,18,22,23),fill='#8ce5dc',width=2)
    elif kind=='vela':
        d.rectangle((11,14,21,27),fill=LIGHT);d.rectangle((7,27,25,30),fill=GOLD)
        d.polygon([(16,1),(21,9),(17,13),(12,9)],fill='#e7a34d');d.line((16,13,16,16),fill=INK)
        d.line((19,17,19,24),fill='#ae9271',width=2)
    elif kind=='reloj':
        d.rectangle((6,2,26,5),fill=GOLD);d.rectangle((6,27,26,30),fill=GOLD)
        d.polygon([(9,6),(23,6),(21,12),(17,16),(22,22),(23,26),(9,26),(10,21),(15,16),(10,11)],fill='#9cc7c4')
        d.polygon([(11,9),(21,9),(16,15)],fill=LIGHT);d.polygon([(11,25),(21,25),(16,20)],fill=GOLD)
    elif kind=='llave':
        d.ellipse((3,3,17,17),fill=GOLD);d.ellipse((7,7,13,13),fill=INK)
        d.line((14,14,27,27),fill=GOLD,width=4);d.line((20,22,18,26),fill=GOLD,width=3);d.line((25,26,22,30),fill=GOLD,width=3)
    elif kind=='moneda':
        d.ellipse((4,3,28,29),fill=GOLD,outline=LIGHT,width=2);d.ellipse((8,7,24,25),outline='#795738',width=2)
        d.line((16,10,12,16,19,16,16,23),fill=LIGHT,width=2)
    elif kind=='gema':
        d.polygon([(9,3),(23,3),(29,12),(16,30),(3,12)],fill='#4fa3bf',outline=LIGHT)
        d.line((3,12,29,12),fill=LIGHT);d.line((9,3,11,12,16,30,21,12,23,3),fill='#a1ebe4')
    elif kind=='corona':
        d.polygon([(4,8),(11,14),(16,3),(22,14),(29,8),(25,26),(8,26)],fill=GOLD,outline=LIGHT)
        d.rectangle((9,23,24,28),fill='#8b563a');d.rectangle((14,17,18,21),fill='#cd6c82')
    elif kind=='caliz':
        d.polygon([(7,4),(25,4),(23,15),(18,20),(18,26),(25,28),(25,30),(7,30),(7,28),(14,26),(14,20),(9,15)],fill=GOLD,outline=LIGHT)
        d.rectangle((14,8,18,13),fill='#709cb5')
    elif kind=='anillo':
        d.ellipse((6,11,26,30),outline=GOLD,width=4);d.polygon([(10,7),(16,1),(23,7),(17,16)],fill='#a376bc',outline=LIGHT)
    elif kind=='escudo':
        d.polygon([(4,5),(16,2),(28,5),(26,21),(16,30),(6,21)],fill='#526d83',outline=GOLD,width=2)
        d.line((16,7,16,24),fill=LIGHT,width=2);d.line((9,14,23,14),fill=LIGHT,width=2)
    else:
        d.line((7,3,16,13,25,3),fill=GOLD,width=2);d.polygon([(16,10),(26,20),(16,30),(6,20)],fill='#83ac87',outline=LIGHT)
        d.ellipse((13,17,19,23),fill=GOLD)
    return im

def card(kind):
    im=Image.new('RGBA',(64,88),INK);d=ImageDraw.Draw(im)
    d.rectangle((2,2,61,85),fill=GOLD);d.rectangle((4,4,59,83),fill='#282632');d.rectangle((7,7,56,80),outline='#786347')
    for x,y in [(5,5),(53,5),(5,77),(53,77)]:d.rectangle((x,y,x+5,y+5),fill=GOLD);d.point((x+2,y+2),fill=LIGHT)
    im.alpha_composite(icon(kind).resize((48,48),Image.Resampling.NEAREST),(8,20));d.line((22,73,41,73),fill=GOLD)
    return im

def illustration(theme):
    rng=random.Random(611 if theme=='biblioteca' else 912)
    im=Image.new('RGBA',(256,256),LIGHT);d=ImageDraw.Draw(im)
    d.rectangle((5,5,250,250),fill=INK);d.rectangle((12,12,243,243),fill='#393241' if theme=='biblioteca' else '#304548')
    for y in range(18,239,17):
        for x in range(18,238,23):
            d.rectangle((x,y,x+18,y+12),fill=rng.choice(['#66536a','#807566','#4a5960','#998169']))
            d.line((x+2,y+3,x+rng.randint(5,15),y+3),fill=LIGHT)
    if theme=='biblioteca':
        for y in (35,91,153):
            d.rectangle((25,y+40,182,y+46),fill=GOLD)
            x=29
            while x<177:
                w=rng.randint(7,13);h=rng.randint(19,38)
                d.rectangle((x,y+39-h,x+w,y+39),fill=rng.choice(['#a76d68','#6d9c92','#ad9761','#7970a4']),outline=INK)
                d.line((x+2,y+43-h,x+w-2,y+43-h),fill=LIGHT);x+=w+3
        d.rectangle((39,207,217,222),fill='#725044');d.rectangle((52,222,62,238),fill=GOLD)
        im.alpha_composite(icon('libro').resize((68,68),Image.Resampling.NEAREST),(104,158))
        im.alpha_composite(icon('vela').resize((55,55),Image.Resampling.NEAREST),(185,54))
    else:
        d.polygon([(25,198),(66,165),(102,184),(171,157),(234,195),(230,237),(25,237)],fill='#957143')
        for _ in range(80):
            x,y=rng.randrange(28,230),rng.randrange(194,235);d.ellipse((x,y,x+5,y+3),fill=rng.choice([GOLD,LIGHT,'#caa355']))
        d.rectangle((60,118,193,202),fill=INK);d.rectangle((65,123,188,197),fill='#86533e')
        for y in (133,152,174):d.line((67,y,185,y),fill='#4e3837',width=3)
        d.rectangle((75,121,86,197),fill=GOLD);d.rectangle((167,121,178,197),fill=GOLD)
        d.rectangle((64,153,189,162),fill=GOLD);d.rectangle((117,148,137,173),fill=LIGHT);d.rectangle((124,154,129,166),fill=INK)
        im.alpha_composite(icon('gema').resize((55,55),Image.Resampling.NEAREST),(26,43))
        im.alpha_composite(icon('corona').resize((64,64),Image.Resampling.NEAREST),(162,37))
    return im

def main():
    MARKERS.mkdir(exist_ok=True)
    for kind in ('libro','pergamino','pluma','runa','vela','reloj','llave','moneda','gema','corona','caliz','anillo','escudo','amuleto'):
        save(card(kind),'carta_'+kind)
    for theme in ('biblioteca','tesoro'):
        marker=illustration(theme).resize((512,512),Image.Resampling.NEAREST)
        save(marker,'marcador_'+theme)
        marker.save(MARKERS/('Cartal-'+theme+'.png'))
        board=Image.open(ART/'tablero.png').convert('RGBA');d=ImageDraw.Draw(board)
        d.rectangle((14,95,369,430),fill='#252332' if theme=='biblioteca' else '#203034',outline=GOLD,width=2)
        save(board,'tablero_'+theme)
    book=Image.new('RGBA',(64,80));book.alpha_composite(icon('libro').resize((64,64),Image.Resampling.NEAREST),(0,8));save(book,'premio_biblioteca')
    chest=Image.new('RGBA',(64,80));d=ImageDraw.Draw(chest)
    d.rectangle((3,20,61,65),fill=INK);d.rectangle((6,23,58,62),fill='#86533e')
    for y in (30,43,56):d.line((7,y,57,y),fill='#46303a',width=2)
    for x in (12,46):d.rectangle((x,23,x+5,62),fill=GOLD)
    d.rectangle((5,39,59,44),fill=GOLD);d.rectangle((28,37,36,49),fill=LIGHT);save(chest,'premio_tesoro')
    shutil.copyfile(ART/'marcador_cartal.png', MARKERS/'Cartal-cripta.png')
    print('14 cartas, dos tableros, dos premios y tres imágenes AR preparados.')

if __name__ == '__main__': main()
