# Validación de Cartal 0.2.1

Editor utilizado: **Unity 6000.3.11f1 (3000ef702840)**.

## Actualización de ocho cartas: verificado en Unity y PC

- Compilación Windows con la versión exacta de Unity.
- Menú Jugar, Acerca de y Salir; Acerca de incluye a los cuatro integrantes.
- Ocho cartas mezcladas: dos calaveras, dos espadas, dos pociones rojas y dos azules.
- Selección mediante el rayo y los colliders reales de la escena.
- Se rechazan una selección repetida de la misma carta y una tercera durante una comparación.
- Los fallos se ocultan después de un segundo; las parejas permanecen descubiertas.
- El tablero oculto no acepta selecciones.
- Cuatro parejas abren la puerta y muestran la victoria.
- El reloj se pausa al perder la referencia y al pasar la app al fondo.
- El reloj reanuda al recuperar el tablero y termina al ganar.
- Una partida incompleta no guarda récord.
- Primer récord de prueba: 12.34 segundos. Un tiempo de 8.25 lo mejora; uno de 30 no lo reemplaza.
- Reiniciar conserva el mejor tiempo y limpia cartas, puerta y cronómetro.
- Volver al menú cancela las corrutinas pendientes, sin afectar la siguiente partida.
- Las pruebas preservan el récord previo del editor y restauran su almacenamiento al finalizar.

Registro: ../logs/unity-Pruebas-Windows.log, con CARTAL_PRUEBAS_OK Unity=6000.3.11f1.
Capturas PC: Cartal-Menu.png, Cartal-AcercaDe.png, Cartal-Tablero.png y Cartal-Victoria.png.
Los tiempos de la captura de victoria PC son datos controlados de prueba, no un récord humano.
La vista PC usa la lógica del juego, pero no constituye una prueba de realidad aumentada.
El editor generó un aviso interno de Unity Search durante su inicio; las pruebas terminaron correctamente.

## Arte e imagen

- Referencia evaluada con arcoreimg: **95/100**; la imagen se conserva en esta actualización.
- Trece fuentes .ase: cabecera, capa editable, tamaño y píxeles RGBA cotejados con los PNG.
- La nueva poción azul conserva la silueta de la roja y modifica el líquido.

## Android

Herramientas instaladas en D:. Android Build Support corresponde a Unity 6000.3.11f1.
OpenJDK 17.0.9 y NDK 27.2.12479018 están en D:/Android/Unity6000.3.11f1.
El SDK está en D:/Android/Sdk. Se compila ARM64 con IL2CPP y OpenGL ES 3.
La aplicación es com.pgm611.cartal, versión 0.2.0, código 2. La compilación Android final terminó correctamente con Unity 6000.3.11f1. El APK mide 26 830 319 bytes y su firma APK v2 fue verificada con apksigner.

La versión anterior **0.1.0, de seis cartas**, sí fue instalada y probada por USB en un
Samsung Galaxy A56 (SM-A566E), Android 16. Se comprobó el reconocimiento del PNG en la laptop,
la pérdida y recuperación de seguimiento, tres parejas, victoria, reinicio, Atrás y Salir.
Evidencia histórica: android-celular-primera-partida.log, android-celular-final.log,
android-salir.log y Cartal-AR-Victoria.png. Esa captura corresponde al tablero anterior.
En esa versión hubo un aviso nativo GL_INVALID_ENUM al iniciar AR; la cámara y el juego funcionaron.
Su causa no se ha confirmado.

## Versión 0.2.0: verificada en el celular

- Instalación por USB correcta en el Samsung Galaxy A56; versión 0.2.0 y código 2 confirmados.
- Menú y Acerca de legibles: cuatro nombres, roles propuestos, Ingeniería de Sistemas y UNIFRANZ.
- Reconocimiento de la imagen en la laptop: CARTAL_IMAGEN_RECONOCIDA.
- Pérdida y recuperación de la referencia sin reiniciar los aciertos.
- Cuatro parejas completadas: CARTAL_PAREJA 1, 2, 3 y 4, incluida la pareja de pociones azules.
- Victoria: CARTAL_VICTORIA y tiempo final 22.03654 segundos, mostrado como 00:22.03.
- Inicio de otra partida después de ganar: CARTAL_PARTIDA_INICIADA.
- Cierre completo de la aplicación y reapertura: el menú conserva MEJOR TIEMPO 00:22.03.

El lapso entre reconocer la imagen y terminar fue 27.539 segundos. El seguimiento se perdió
durante 5.496 segundos. El tiempo activo esperado es aproximadamente 22.043 segundos,
coherente con el cronómetro registrado de 22.03654: el tiempo sin tablero no se contó.

Registros: android-0.2.0-victoria.log y android-0.2.0-reabierto.log.
Capturas: Cartal-Celular-Menu-0.2.0.png, Cartal-Celular-AcercaDe-0.2.0.png
y Cartal-Celular-Record-0.2.0.png. La captura Cartal-Celular-Partida-0.2.0.png
muestra la espera inicial, con tiempo cero antes de reconocer la referencia.
El aviso nativo GL_INVALID_ENUM también apareció al iniciar AR en esta actualización;
el seguimiento y la partida completa funcionaron, pero su causa sigue sin confirmarse.

## Actualización de textos 0.2.1

Se retiraron del panel Acerca de el sufijo / ROLES PROPUESTOS, la materia y el semestre.
Se conservaron los cuatro integrantes, los roles, la carrera y la universidad.
Las versiones Windows y Android se recompilaron con Unity 6000.3.11f1.
La versión 0.2.1, código 3, se instaló por USB y se comprobó visualmente en el celular.
El menú conservó el récord 00:22.03 después de la actualización.
Evidencia: Cartal-Celular-AcercaDe-0.2.1.png y Cartal-Celular-Menu-0.2.1.png.
La mecánica permanece igual que en las pruebas de la versión 0.2.0 descritas arriba.

## Comprobación sin internet el 15 de octubre

Se desactivaron Wi-Fi y datos móviles y Android confirmó Active default network: none al inicio.
Se abrió Cartal 0.2.1 desde un proceso nuevo. El registro confirmó reconocimiento de la imagen.
El usuario confirmó que funciona sin internet. La prueba parte de servicios AR ya instalados.
No se registró una victoria en ese proceso durante la comprobación.
Después se restauraron los estados previos: Wi-Fi activado y datos móviles desactivados.
Evidencia: android-offline-estado-previo.json, android-offline-partida.log y android-offline-resultado.json.

## Referencia para el stand

Se utilizará Cartal-Imagen-AR.png impreso a 20 por 20 cm. La validación de seguimiento
documentada se realizó mostrando el mismo PNG en la pantalla de la laptop.
