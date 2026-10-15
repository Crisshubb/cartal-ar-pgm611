# Descargar y jugar Cartal 0.2.1

## Android: juego con realidad aumentada

1. Descargar Cartal-Android-0.2.1.apk desde la publicación de GitHub.
2. Instalarlo en un celular Android ARM64 compatible con ARCore. Android puede pedir autorización para instalar desde el navegador o el administrador de archivos.
3. Tener Google Play Services for AR instalado y actualizado. La preparación inicial puede requerir internet.
4. Abrir Cartal, permitir la cámara y pulsar Jugar.
5. Mostrar Cartal-Imagen-AR.png en otra pantalla o imprimirlo a 20 x 20 cm, sin recortarlo. Usar una superficie plana y buena luz.
6. Enfocar la referencia y tocar las ocho cartas para encontrar cuatro parejas. Al completarlas se abre la puerta y se guarda el mejor tiempo del dispositivo.

Una vez preparada la aplicación y sus servicios AR, la partida funciona sin internet. Compatibilidad oficial: https://developers.google.com/ar/devices

## Windows: vista de prueba sin realidad aumentada

1. Descargar Cartal-Windows-0.2.1.zip.
2. Extraer el ZIP completo en una carpeta. Conservar Cartal.exe, Cartal_Data, UnityPlayer.dll y las otras carpetas juntas.
3. Ejecutar Cartal.exe, pulsar Jugar y seleccionar las cartas con el mouse.

Esta versión muestra el tablero en pantalla; no utiliza la cámara ni ofrece realidad aumentada en PC.

## Código fuente completo

La rama release/cartal-funcional contiene todos los aportes ya integrados, con sus commits y autores originales.
Clonar y abrir la carpeta Proyecto con Unity 6000.3.11f1:

    git clone --branch release/cartal-funcional --single-branch https://github.com/Crisshubb/cartal-ar-pgm611.git

Abrir Assets/Escenas/Cartal.unity y pulsar Play. También se puede descargar Cartal-Proyecto-Unity-0.2.1.zip sin usar Git.
La carpeta Library se genera al abrir Unity y no se incluye en la descarga.

## Alcance y comprobación

Ocho cartas, cuatro parejas, menú, Acerca de, cronómetro, récord local y apertura de la puerta.
Las compilaciones 0.2.1 se generaron previamente con Unity 6000.3.11f1. Android se comprobó en Samsung Galaxy A56; Windows ofrece la vista de prueba.
La validación de seguimiento utilizó la imagen PNG mostrada en una laptop. El usuario confirmó que funciona sin internet con los servicios AR preparados.
El registro de Android contiene un aviso nativo GL_INVALID_ENUM al iniciar AR; el seguimiento y la partida funcionaron y su causa no se ha confirmado.
Más detalles en Proyecto/PRUEBAS.md.

No se incluye una versión para iPhone ni para navegador. El APK y el ZIP de Windows se distribuyen en Releases, separados del historial del código.
