# Cartal

## Versión de tres niveles (0.3.0 en desarrollo)

La cripta conserva cuatro parejas; la biblioteca añade seis y el tesoro ocho. Los niveles se pueden completar en cualquier orden, con sellos y récords independientes. En Android cada imagen descubre su nivel y Fijar tablero permite seguir jugando sin mantener el marcador visible. Consultar [las tres imágenes y sus instrucciones](Imagenes-AR/LEEME.md).

## Descargas de la versión anterior (0.2.1, un nivel)

- [Android: descargar APK 0.2.1](https://github.com/Crisshubb/cartal-ar-pgm611/releases/download/v0.2.1/Cartal-Android-0.2.1.apk) — realidad aumentada en un celular compatible.
- [Windows: descargar ZIP 0.2.1](https://github.com/Crisshubb/cartal-ar-pgm611/releases/download/v0.2.1/Cartal-Windows-0.2.1.zip) — vista de prueba con mouse, sin AR.
- [Imagen de referencia](https://github.com/Crisshubb/cartal-ar-pgm611/releases/download/v0.2.1/Cartal-Imagen-AR.png).
- [Código fuente completo en ZIP](https://github.com/Crisshubb/cartal-ar-pgm611/releases/download/v0.2.1/Cartal-Proyecto-Unity-0.2.1.zip).

Lee [DESCARGAR-Y-JUGAR.md](DESCARGAR-Y-JUGAR.md) para instalar, abrir el proyecto y consultar los requisitos.
Esta rama contiene el proyecto completo y conserva las aportaciones del equipo.

Juego de memoria en realidad aumentada para Android con tres niveles.
Al completar el tablero se abre la puerta de la cripta y se guarda el mejor tiempo en el celular.

## Abrir el proyecto

Usar Unity 6000.3.11f1 y abrir la carpeta Proyecto, después Assets/Escenas/Cartal.unity.
En PC se dispone de una vista previa para probar la lógica. En Android se reconocen las tres imágenes de Imagenes-AR.
Las instrucciones completas están en Proyecto/LEEME.md.

## Equipo

| Integrante | Responsabilidad |
| --- | --- |
| Cristopher Iori Lazcano Gutierrez | Programación y coordinación |
| Alejandro Villalpando Rojas | Arte e interfaz |
| Galilea Alison Llusco Asistiri | Documentación y presentación |
| Alex Joel Quispe Ticona | Pruebas en Android y AR |

Ingeniería de Sistemas. Universidad Privada Franz Tamayo UNIFRANZ, Sede La Paz, Bolivia.

## Colaborar

Cada integrante integra tres bloques en su rama. Consultar COLABORACION.md y el paquete personal.
Se revisan los cuatro pull requests hacia develop antes de abrir develop hacia main.
Conservar los archivos .meta. Assets, Packages y ProjectSettings componen el proyecto Unity.
Las carpetas Library y Temp se generan en cada computadora y se excluyen de Git.

## Entrega y explicación

Documentacion contiene el informe, las diapositivas con notas y capturas de comprobación.
Proyecto/EXPLICACION-CODIGO.md explica los tres scripts de ejecución.
Arte-LibreSprite contiene las fuentes ASE editables de los tres niveles. El arte inicial se construyó por código.
Las compilaciones se distribuyen por separado del código fuente.
