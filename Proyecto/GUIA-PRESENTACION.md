# Guía de exposición de Cartal

Carrera: Ingeniería de Sistemas.
Universidad Privada Franz Tamayo (UNIFRANZ), Sede La Paz, Bolivia.
Fuente del nombre institucional y sede: https://unifranz.edu.bo/sedes/la-paz/.

## Reparto propuesto del trabajo

| Integrante | Rol propuesto | Explicación que puede preparar |
| --- | --- | --- |
| Cristopher Iori Lazcano Gutierrez | Programación y coordinación | Comparación de cartas, cronómetro y PlayerPrefs. |
| Alejandro Villalpando Rojas | Arte e interfaz | Sprites, fuentes ASE editables en LibreSprite y paneles. |
| Galilea Alison Llusco Asistiri | Documentación y presentación | Objetivo, instrucciones y tecnologías usadas. |
| Alex Joel Quispe Ticona | Pruebas en Android y AR | Reconocimiento de imagen, toques y demostración. |

Son responsabilidades propuestas para repartir el trabajo desde ahora; no certifican aportes anteriores.
Se pueden ajustar en `Assets/Resources/Equipo.txt` y recompilar la app.

## Texto breve para decir

“Nuestro proyecto se llama Cartal. Es un juego de memoria en realidad aumentada para Android.
Al apuntar la cámara a esta imagen, aparecen ocho cartas virtuales. Tocamos dos cartas:
si sus identificadores coinciden, permanecen descubiertas; si no, se ocultan después de un segundo.
Al encontrar cuatro parejas, se abre la puerta de la cripta.

El cronómetro cuenta mientras el tablero está visible y se pausa si perdemos la imagen.
Al ganar, guardamos el menor tiempo con PlayerPrefs, por lo que el récord permanece al cerrar la app.
El menú permite jugar, ver la información del grupo o salir.

Usamos Unity 6000.3.11f1, C#, AR Foundation y ARCore. La lógica principal tiene tres scripts:
Carta guarda el estado; JuegoMemoria compara parejas y controla el tiempo;
MostrarTableroAR coloca el tablero y detecta los toques.
El arte inicial se generó por código y entregamos sus fuentes ASE para editarlas en LibreSprite.”

## Demostración de dos minutos

1. Abrir Cartal y enseñar Jugar, Acerca de y Salir.
2. Enfocar la referencia completa en la laptop o la impresión de 20 × 20 cm.
3. Mostrar una pareja fallida y cómo las dos cartas vuelven a ocultarse.
4. Encontrar un acierto y explicar los identificadores iguales.
5. Completar cuatro parejas, enseñar la puerta y el tiempo final.
6. Volver al menú y señalar que el mejor tiempo se conserva.
7. Abrir Acerca de y presentar nombres, roles, carrera y universidad.

## Preparación del stand

Llevar el celular cargado, la app instalada, la referencia, un cargador y las diapositivas.
Usar papel mate y una superficie plana; evitar reflejos y mantener buena iluminación.
El USB se usa para instalar y cargar; la partida se ejecuta en el celular.
Antes del evento comprobar una partida con la imagen impresa y otra sin internet,
después de instalar los servicios AR. No presentar estas comprobaciones como realizadas si aún no lo están.
