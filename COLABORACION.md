# Colaboración de Cartal

Repositorio: https://github.com/Crisshubb/cartal-ar-pgm611

El repositorio empieza con la base de colaboración. El proyecto completo ya existe en la entrega local.
Los cuatro paquetes integran ese trabajo mediante doce commits. Cada persona revisa y explica los archivos que sube.
Los commits documentan la integración y no reconstruyen fechas ni atribuciones anteriores.

## Antes de subir

1. Aceptar la invitación de GitHub. Un repositorio público permite leer, pero hace falta aceptar para subir ramas al mismo repositorio.
2. Instalar o disponer de Git. Usar tu propia cuenta al autenticarte y tu nombre y correo verificado en los commits.
3. Descargar tu paquete y extraerlo FUERA de la carpeta clonada. No subir el ZIP como un archivo del proyecto.
4. Clonar en una carpeta nueva. Conservar el proyecto original de trabajo.

```powershell
git clone https://github.com/Crisshubb/cartal-ar-pgm611.git
cd cartal-ar-pgm611
git switch develop
git pull --ff-only origin develop
git config user.name "TU NOMBRE COMPLETO"
git config user.email "TU CORREO VERIFICADO"
```

Cristopher usa Cristopher Iori Lazcano Gutierrez y un correo verificado de su cuenta Crisshubb.
Las otras tres personas configuran sus propios datos. Las contraseñas de las cuentas no se comparten.

## Tres commits por integrante

Crear la rama asignada desde develop. Para cada bloque, copiar el contenido de su carpeta archivos
a la raíz de la carpeta clonada, conservando rutas y archivos meta. Copiar solo el bloque actual.
Revisar el contenido y después confirmar con el mensaje indicado. No abrir Unity mientras falten los cuatro paquetes.

```powershell
git switch -c feature/TU-RAMA
# Copiar el contenido de 01-commit/archivos a la raíz del clon.
git status --short
git add --all
git diff --cached --stat
git commit -m "MENSAJE DEL PRIMER COMMIT"
# Repetir para 02-commit y 03-commit con sus respectivos mensajes.
git log --oneline -3
git push -u origin feature/TU-RAMA
```

Usar el archivo PASOS.md del paquete para los nombres exactos de rama y mensajes.
El archivo MANIFIESTO.json permite comprobar las rutas y los hashes de los archivos de cada bloque.
Si git dice que no hay cambios, detenerse y revisar qué carpeta se copió. No crear commits vacíos.

## Reparto

| Integrante | Rama | Commit 1 | Commit 2 | Commit 3 |
| --- | --- | --- | --- | --- |
| Cristopher | feature/criss | Integrar estados y comportamiento de las cartas | Integrar memoria cronometro y record | Integrar seguimiento AR y seleccion tactil |
| Alejandro | feature/alejandro | Integrar cartas y fuentes editables de pixel art | Agregar la pareja de pociones azules | Integrar ambiente sonidos y tipografia |
| Galilea | feature/galilea | Documentar el juego e incorporar los datos del equipo | Agregar el informe tecnico y manual de uso | Agregar diapositivas y guion de exposicion |
| Alex | feature/alex | Configurar Unity y los paquetes de realidad aumentada | Integrar la escena y su preparacion en el editor | Agregar pruebas y herramientas de entrega |

## Pull requests e integración

Cada integrante abre un pull request con base develop y compare su rama.
La descripción enumera sus tres commits y lo que revisó. Los paquetes contienen archivos separados.
Orden sugerido de integración: Alejandro, Cristopher, Galilea y Alex. Alex incorpora la escena y las pruebas.
Durante los primeros pull requests el repositorio todavía no contiene el proyecto completo.
Usar Create a merge commit al integrar para conservar los tres commits, sin squash.

Después de integrar los cuatro pull requests:

```powershell
git switch develop
git pull --ff-only origin develop
```

Abrir Proyecto con Unity 6000.3.11f1 y Assets/Escenas/Cartal.unity. Revisar la consola y probar
Jugar, cuatro parejas, fallo, victoria, reinicio, Acerca de y récord. El menú Cartal permite compilar.
Los scripts de Herramientas conservan las rutas de la laptop original en D:; en otra PC usar Unity Hub y el menú Cartal.
Solo tras la revisión y prueba del proyecto completo, abrir un pull request de develop hacia main.
No subir Library, cachés, instaladores ni APK en los commits de código.

## Invitaciones

Crisshubb es el propietario. Se enviaron invitaciones a Alexwuuu1 y Galileya.
La cuenta exacta del cuarto integrante se verifica con él antes de añadirla.
