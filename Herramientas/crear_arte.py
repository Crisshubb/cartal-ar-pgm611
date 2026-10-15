"""Arte pixel original, exportaciones PNG y documentos .ase editables en LibreSprite."""
from pathlib import Path
from PIL import Image, ImageDraw
import random, struct, zlib, wave, math, sys

ROOT = Path(r'D:\CodexWork\Cartal')
ART = ROOT / 'Proyecto/Assets/Arte'
EDIT = ROOT / 'Arte-LibreSprite'
EDIT.mkdir(exist_ok=True)
random.seed(611)
ink = '#17151e'
dark = '#282632'
stone = '#4a4752'
gold = '#b98d55'
light = '#ebd5aa'
green = '#77a98d'

def aseprite(image, name):
    """Un frame RGBA con una capa editable según la especificación pública ASE."""
    image = image.convert('RGBA')
    layer_name = name.encode('utf-8')
    layer_data = struct.pack('<6HB3xH', 3, 0, 0, 0, 0, 0, 255, len(layer_name)) + layer_name
    cel_data = struct.pack('<HhhBHh5xHH', 0, 0, 0, 255, 2, 0, image.width, image.height) + zlib.compress(image.tobytes())
    chunks = b''.join(struct.pack('<IH', len(data)+6, kind) + data for kind, data in [(0x2004,layer_data),(0x2005,cel_data)])
    frame = struct.pack('<IHHH2xI', len(chunks)+16, 0xF1FA, 2, 100, 2) + chunks
    header = bytearray(128)
    struct.pack_into('<I6HIH', header, 0, len(frame)+128, 0xA5E0, 1, image.width, image.height, 32, 1, 0, 100)
    # Flags start at byte 14, speed at byte 18 (the fields above are overwritten explicitly).
    struct.pack_into('<IH', header, 14, 1, 100)
    header[20:28] = bytes(8)
    header[34:36] = bytes([1,1])
    (EDIT / (name+'.ase')).write_bytes(header+frame)

def save(image, name):
    image.save(ART / (name+'.png'))
    aseprite(image, name)

def icon(kind):
    im = Image.new('RGBA',(32,32))
    d = ImageDraw.Draw(im)
    if kind == 'calavera':
        d.polygon([(8,4),(22,3),(27,8),(28,18),(24,22),(23,28),(10,28),(9,22),(5,18),(5,10)],fill=ink)
        d.polygon([(9,5),(22,5),(25,9),(25,18),(21,21),(21,26),(12,26),(11,21),(7,18),(7,11)],fill='#b9aa8b')
        d.polygon([(9,7),(21,6),(24,10),(23,14),(9,14),(8,10)],fill=light)
        d.rectangle((8,14,13,19), fill=ink)
        d.rectangle((19,13,24,18), fill=ink)
        d.point((10,15),fill='#89a98b'); d.point((21,14),fill='#89a98b')
        d.polygon([(16,18),(14,22),(18,22)],fill=ink)
        for x in (12,16,20): d.line((x,24,x,26),fill=ink)
        d.line((8,11,9,8),fill='#fff2d0')
        d.line((19,7,17,10,19,12), fill='#766755')
    elif kind in ('pocion', 'pocion_azul'):
        # La poción azul conserva el frasco y solo cambia los colores del líquido.
        liquid = '#3c80cc' if kind == 'pocion_azul' else '#b44c66'
        shine = '#97d5ff' if kind == 'pocion_azul' else '#f099a0'
        d.rectangle((13,2,20,6),fill=ink); d.rectangle((14,3,19,5),fill=gold)
        d.rectangle((12,6,21,9),fill='#9da9a4')
        d.polygon([(13,9),(20,9),(20,13),(26,18),(26,27),(22,30),(10,30),(6,27),(6,18),(13,13)],fill=ink)
        d.polygon([(14,10),(19,10),(19,14),(24,19),(24,26),(21,28),(11,28),(8,26),(8,19),(14,14)],fill='#405655')
        d.polygon([(9,20),(14,19),(18,21),(23,19),(23,26),(20,27),(11,27),(9,25)],fill=liquid)
        d.rectangle((10,22,12,24),fill=shine); d.line((10,17,13,14),fill='#c9e0d1',width=2)
        d.line((16,11,16,14),fill='#9da9a4')
    elif kind == 'espada':
        d.polygon([(22,2),(29,2),(29,9),(16,23),(10,17)],fill=ink)
        d.polygon([(23,4),(27,4),(27,8),(15,21),(12,18)],fill='#b9c9c5')
        d.line((24,6,14,18),fill='#eff6df',width=2)
        d.line((26,7,16,19),fill='#6c8a8f')
        d.line((8,14,19,25),fill=ink,width=5); d.line((8,14,19,25),fill=gold,width=3)
        d.line((11,23,5,29),fill=ink,width=5); d.line((11,23,5,29),fill='#8b5650',width=3)
        d.rectangle((3,27,7,31),fill=gold); d.point((4,28),fill=light)
    return im

def card(name=None):
    im=Image.new('RGBA',(64,88),ink); d=ImageDraw.Draw(im)
    d.rectangle((2,2,61,85),fill=gold); d.rectangle((4,4,59,83),fill=dark)
    d.rectangle((7,7,56,80),outline='#786347',width=1)
    for x,y in [(5,5),(53,5),(5,77),(53,77)]:
        d.rectangle((x,y,x+5,y+5),fill=gold); d.point((x+2,y+2),fill=light)
    if name:
        im.alpha_composite(icon(name).resize((48,48),Image.Resampling.NEAREST),(8,20))
        d.line((22,73,41,73),fill=gold)
    else:
        d.polygon([(32,15),(49,44),(32,72),(15,44)],outline=gold,width=2)
        d.polygon([(32,27),(40,44),(32,60),(24,44)],fill='#443642',outline='#826947')
        d.ellipse((26,37,38,49),outline=light,width=2); d.line((32,30,32,57),fill=light)
        d.line((22,44,42,44),fill=light)
    return im

if '--solo-pocion-azul' in sys.argv:
    save(icon('pocion_azul'),'pocion_azul')
    save(card('pocion_azul'),'carta_pocion_azul')
    print('POCION_AZUL_OK: PNG y ASE creados sin modificar el arte anterior.')
    raise SystemExit(0)

for name in ('calavera','pocion','espada','pocion_azul'):
    save(icon(name),name)
    save(card(name),'carta_'+name)
save(card(),'reverso')

board=Image.new('RGBA',(384,448),ink); d=ImageDraw.Draw(board)
d.rectangle((3,3,380,444),fill=gold); d.rectangle((7,7,376,440),fill=dark)
for y in range(12,440,24):
    offset=12 if (y//24)%2 else -12
    for x in range(offset,377,42):
        shade=random.choice(['#33303d','#363441','#3c3945','#302d39'])
        d.rectangle((x,y,min(x+39,374),min(y+21,437)),fill=shade)
        d.line((x+2,y+1,min(x+37,374),y+1),fill='#514956')
        if random.random()<.4: d.line((x+23,y+4,x+18,y+11,x+20,y+16),fill=ink)
d.rectangle((14,143,369,430),fill='#211f2a',outline='#706047',width=2)
for x in (20,359):
    d.rectangle((x-3,28,x+3,126),fill=stone)
    d.rectangle((x-7,122,x+7,132),fill=gold)
    d.polygon([(x-6,65),(x-8,56),(x-3,42),(x-1,52),(x+3,40),(x+7,54),(x+5,65)],fill='#c37644')
    d.polygon([(x-3,63),(x-3,53),(x,48),(x+3,60)],fill='#f0c473')
save(board,'tablero')

door=Image.new('RGBA',(64,80)); d=ImageDraw.Draw(door)
d.polygon([(3,79),(3,25),(12,12),(24,3),(40,3),(52,12),(61,25),(61,79)],fill=stone)
d.polygon([(9,77),(9,28),(17,18),(27,9),(37,9),(47,18),(55,28),(55,77)],fill=ink)
d.polygon([(14,76),(14,29),(22,19),(28,15),(36,15),(44,22),(50,30),(50,76)],fill='#594238')
for x in (19,27,35,43): d.line((x,27,x,75),fill='#312a2b',width=2)
d.rectangle((15,37,49,40),fill='#89775b');d.rectangle((15,62,49,65),fill='#89775b')
d.rectangle((40,48,43,53),fill=gold);d.point((40,48),fill=light)
save(door,'puerta')
opening=Image.new('RGBA',(64,80));d=ImageDraw.Draw(opening)
d.polygon([(4,79),(4,25),(12,12),(24,3),(40,3),(52,12),(60,25),(60,79)],fill='#0d141c')
d.polygon([(15,79),(27,22),(35,22),(49,79)],fill='#253c3d')
d.line((30,39,30,67),fill=green,width=2)
save(opening,'salida')

# Ilustración de referencia asimétrica: ruinas, piedras, vegetación y luna.
marker=Image.new('RGBA',(256,256),'#d0bf96');d=ImageDraw.Draw(marker)
d.rectangle((5,5,250,250),outline=ink,width=4);d.rectangle((12,12,243,243),outline='#665d50',width=1)
d.rectangle((16,16,239,239),fill='#343440')
for y in range(18,239,12):
    for x in range(18+(y//12%2)*-9,239,22):
        shade=random.choice(['#53505b','#605862','#494852','#676067','#77706f'])
        d.rectangle((x,y,min(x+19,238),y+9),fill=shade)
        d.line((x+1,y,x+16,y),fill='#938a7d')
        for _ in range(3):
            px=x+random.randrange(2,17); py=y+random.randrange(2,8)
            d.line((px,py,px+2,py),fill=random.choice(['#322f3b','#a09881','#6a7260']))
d.ellipse((177,24,221,68),fill='#d7c498');d.ellipse((170,18,207,56),fill='#474450')
d.polygon([(50,223),(50,108),(72,77),(103,53),(133,56),(162,80),(177,109),(177,223)],fill=ink)
d.polygon([(61,220),(61,113),(80,87),(106,68),(130,69),(151,90),(165,113),(165,220)],fill='#88745f')
d.polygon([(72,220),(72,117),(88,96),(109,81),(127,82),(145,101),(153,117),(153,220)],fill='#242330')
for x in (82,100,118,136): d.rectangle((x,124,x+9,217),fill='#47363c'); d.line((x+2,126,x+2,212),fill='#6f5550')
for y in (143,187): d.rectangle((75,y,150,y+5),fill='#968063')
marker.alpha_composite(icon('calavera').resize((48,48),Image.Resampling.NEAREST),(90,89))
for x,y in [(27,66),(202,106),(185,182),(36,192)]:
    for j in range(7):
        d.line((x+j,y+random.randrange(-5,5),x+j-3,y+random.randrange(10,23)),fill=random.choice(['#405849','#688063','#819067']),width=2)
for j in range(35):
    x=random.randrange(20,234);y=random.randrange(221,237)
    d.rectangle((x,y,x+random.randrange(2,10),y+3),fill=random.choice(['#a79576','#514b50','#7c7161']))
save(marker,'marcador_cartal')
marker.resize((512,512),Image.Resampling.NEAREST).save(ART/'marcador_cartal.png')
marker.resize((768,768),Image.Resampling.NEAREST).save(ROOT/'Entrega/Cartal-Imagen-AR.png')

audio=ROOT/'Proyecto/Assets/Sonidos';audio.mkdir(exist_ok=True)
for name,notes in [('voltear',[330,440]),('pareja',[440,554,659]),('victoria',[330,440,554,659,880])]:
    samples=[]
    for freq in notes:
        duration=.07 if name=='voltear' else .12
        for n in range(int(22050*duration)):
            envelope=(1-n/(22050*duration))**2
            samples.append(int(4400*envelope*math.sin(2*math.pi*freq*n/22050)))
    with wave.open(str(audio/(name+'.wav')),'wb') as f:
        f.setparams((1,2,22050,len(samples),'NONE','not compressed'));f.writeframes(struct.pack('<'+'h'*len(samples),*samples))
print('Arte, PNG de referencia, sonidos y fuentes editables creados en D.')
