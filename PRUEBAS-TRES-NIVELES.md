# Comprobación de Cartal 0.3.0

Fecha: 16 de octubre de 2026. Unity 6000.3.11f1, Windows.

- 49 comprobaciones aprobadas mediante `PruebasCartal.Ejecutar` en Play Mode.
- Regresión de la cripta: selección, rechazo de tercera carta, parejas, animación, récord y reinicio.
- Selector de PC, ocho parejas del tesoro, seis de la biblioteca y acceso en cualquier orden.
- Desactivación de cartas sobrantes al cambiar a un tablero menor.
- Tres sellos guardados y tiempos independientes; las pruebas restauran los datos previos del editor.
- Pausa del cronómetro al ocultar el tablero o poner la aplicación en segundo plano.
- Fijar y reubicar en vista PC conservan la selección; regresar al menú libera el estado de fijación.
- Tres referencias en la biblioteca AR; evaluación de características: biblioteca 95/100 y tesoro 100/100.
- Compilación Windows completada; inicio del ejecutable sin excepciones.
- ZIP completo verificado mediante CRC, con EXE, UnityPlayer.dll, datos y dependencias.
- 69 GUID de Assets sin duplicados. Las 20 fuentes ASE nuevas coinciden píxel por píxel con sus PNG.

## Pendiente en Android

Se generó el APK 0.3.0 con soporte ARMv7 y ARM64, firma v2 verificada y ARCore opcional. Se añadieron pruebas del selector sin cámara y de la desactivación de ARCameraManager. Falta una prueba en teléfono: la validación en PC no verifica la estabilidad física de un anclaje AR. Consultar ANDROID.md.

En un celular compatible, comprobar:

1. Detectar por separado cada imagen de 20 × 20 cm y abrir el nivel correcto.
2. Fijar el tablero, esperar confirmación y retirar la imagen; comprobar estabilidad al mover el celular.
3. Perder y recuperar seguimiento del entorno; el tiempo debe pausarse mientras el tablero está oculto.
4. Reubicar con el mismo marcador sin perder cartas, parejas ni tiempo acumulado.
5. Regresar al menú mientras se está creando un anclaje; no debe reaparecer el tablero anterior.
6. Cambiar entre los tres niveles, completar los sellos y reiniciar la aplicación para verificar persistencia.
7. Comprobar que las 16 cartas se toquen cómodamente y que el mensaje de permiso de cámara sea correcto.

Los tiempos de las capturas son valores controlados por las pruebas, no marcas obtenidas por una persona.
