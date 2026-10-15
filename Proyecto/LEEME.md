# Cartal — memoria en realidad aumentada

Proyecto independiente de Snake. Cartal **0.2.1**, en **Unity 6000.3.11f1**.

## Abrir y jugar

Abre esta carpeta como proyecto de Unity y abre `Assets/Escenas/Cartal.unity`.
Presiona Play. En PC, Jugar muestra una vista previa del tablero que funciona con el mouse.
La vista PC sirve para comprobar las reglas; **la realidad aumentada funciona en Android**.

En Android, abre Cartal, pulsa Jugar y concede el permiso de cámara.
Enfoca `Cartal-Imagen-AR.png`, abierto en la pantalla de la laptop o impreso.
Encuentra las cuatro parejas de las ocho cartas tocándolas sobre la imagen de cámara.
Al ganar, la puerta gira y aparece Volver a jugar. Menú y el botón Atrás permiten volver al inicio.

El cronómetro comienza al aparecer el tablero y se pausa si se pierde la referencia o la app pasa al fondo.
Al ganar se guarda el menor tiempo en ese dispositivo mediante `PlayerPrefs`.
Reiniciar o cerrar Cartal conserva el récord; una partida incompleta no lo reemplaza.
Este récord corresponde al tablero de cuatro parejas y no requiere una cuenta ni un servidor.

El menú también incluye **Acerca de**: cuatro integrantes, roles, Ingeniería de Sistemas
y Universidad Privada Franz Tamayo (UNIFRANZ), Sede La Paz, Bolivia.
Los datos se editan en `Assets/Resources/Equipo.txt`. Después de editarlos, usa
**Cartal → Preparar escena** o vuelve a compilar: la herramienta incorpora los textos en la escena.
Los roles son una distribución provisional autorizada por el grupo y pueden ajustarse.

## Tres scripts propios del juego

| Archivo | Responsabilidad |
| --- | --- |
| `Assets/Scripts/Carta.cs` | Guarda el identificador y el dibujo; revela, oculta y marca una pareja. |
| `Assets/Scripts/JuegoMemoria.cs` | Mezcla ocho cartas, compara dos selecciones, cuenta parejas, gestiona menú, récord y victoria. |
| `Assets/Scripts/MostrarTableroAR.cs` | Coloca el tablero en la imagen reconocida y detecta toques mediante un rayo. |

Las variables y las instrucciones de estos tres scripts tienen comentarios en español.
AR Foundation y ARCore se encargan de reconocer la imagen y de seguir la cámara.
Los archivos de `Assets/Editor` preparan, compilan y prueban la escena automáticamente;
no se ejecutan en el celular y no son scripts adicionales de la mecánica.

## Imagen de referencia

Solo hay una imagen: el dibujo de una cripta. No se imprimen las ocho cartas.
La biblioteca `Assets/Configuracion/ImagenCartal.asset` contiene esa referencia.
El tamaño configurado es 20 × 20 cm. En pantalla conviene mostrarla aproximadamente a ese tamaño,
sin recortarla, y mantener el celular mirando la pantalla de frente al comenzar.
Para una impresión final, usa una hoja plana, papel mate y buena iluminación.
Si el seguimiento deja de ser fiable, se oculta el tablero y se pide volver a enfocar;
las parejas ya encontradas se conservan. No hay descarga de contenido durante la partida.

## Arte editable

`../Arte-LibreSprite/` contiene trece archivos `.ase` de una capa editables en LibreSprite.
Los dibujos se construyeron por código, píxel a píxel, y se entregan para que el grupo los revise y edite.
No se afirma que fueron dibujados a mano en LibreSprite. Los PNG de `Assets/Arte` son sus exportaciones.
La referencia tiene una exportación de mayor resolución, sin suavizar los píxeles.
La fuente Silkscreen es de Google Fonts y se incluye su licencia OFL.
Los efectos de selección, pareja y victoria son sonidos sintéticos originales.
La poción azul conserva la silueta de la roja y cambia el color del líquido.
Sus fuentes son `pocion_azul.ase` y `carta_pocion_azul.ase`.

## Presentación y stand

Las diapositivas se entregan en `../Entrega/Presentacion/` e incluyen notas para exponer.
La guía `GUIA-PRESENTACION.md` propone un reparto de la explicación y una demostración corta.
Para el stand se utilizará la referencia impresa de 20 por 20 cm.
El usuario confirmó el funcionamiento sin internet con los servicios AR previamente instalados;
la comprobación y sus registros se describen en PRUEBAS.md.

## Compilar

En el menú de Unity: **Cartal → Compilar Windows** o **Cartal → Compilar Android**.
Android necesita Android Build Support, SDK/NDK y OpenJDK de esta versión de Unity.
En esta laptop, `../Herramientas/unity.ps1 -Accion Android -Destino Android` utiliza
OpenJDK 17.0.9 y NDK 27.2.12479018 en `D:/Android/Unity6000.3.11f1`, y el SDK existente
en `D:/Android/Sdk`. En otra PC se pueden utilizar las herramientas de Unity Hub.
Se configura ARCore, ARM64, IL2CPP y OpenGL ES 3. La aplicación es `com.pgm611.cartal`.
La compilación final desactiva las herramientas de depuración del jugador. No se ha publicado en ninguna tienda.

Documentación oficial:
- https://unifranz.edu.bo/sedes/la-paz/ (nombre de la universidad y sede)
- https://developers.google.com/ar/develop/augmented-images
- https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@6.3/manual/features/image-tracking.html
