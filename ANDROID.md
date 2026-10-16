# Cartal para Android

El APK incluye los tres niveles y dos formas de jugar:

- **Jugar sin cámara**: selecciona el nivel y toca las cartas en pantalla. No requiere ARCore ni permiso de cámara.
- **Explorar con cámara**: comprueba la disponibilidad de ARCore, solicita sus servicios cuando corresponde y permite descubrir los niveles con sus imágenes. Fijar tablero crea un anclaje espacial durante la partida.

## Compatibilidad prevista

Android 7.1 (API 25) o superior, procesador ARM de 32 o 64 bits y OpenGL ES 3.0. Estos requisitos permiten instalar en más equipos, pero no garantizan el rendimiento de cada modelo.

El Huawei Y7 2019 no aparece en la lista oficial de ARCore. Usar **Jugar sin cámara** en ese modelo. No se ha realizado una prueba física en ese teléfono.

Para realidad aumentada, comprobar el modelo exacto en [la lista de dispositivos compatibles de Google](https://developers.google.com/ar/devices), además de tener los Servicios de Google Play para RA y permiso de cámara. Un Android más reciente no garantiza compatibilidad AR. El APK no se instala en iPhone.

## Ajustes de esta compilación

- 30 FPS en móviles y 60 en PC.
- IL2CPP, ARMv7 y ARM64 en un mismo APK.
- Compilación sin Development Build ni Script Debugging.
- Compresión LZ4HC de los datos y eliminación de código de motor no utilizado.
- Eliminación conservadora de código administrado (Low).
- ARCore opcional para permitir el modo normal en dispositivos sin AR.
- Cámara AR y seguimiento desactivados durante el modo normal.

Los ajustes buscan reducir consumo y tamaño; no se han medido todavía FPS, temperatura ni batería en teléfonos físicos.

## APK verificado

Compilación completada el 16 de octubre de 2026. Firma v2 verificada, ARMv7 y ARM64 presentes, ARCore opcional, versión 0.3.0 (4), mínimo API 25 y destino API 36. Tamaño: 46.506.325 bytes. No es una compilación de depuración. El mínimo efectivo de la actividad de Unity es Android 7.1.

Firmado con la clave local de desarrollo para instalación directa, no preparado para publicación en Google Play. Si otra instalación tiene una firma diferente, Android no permitirá actualizarla directamente; conserva tus datos antes de decidir reemplazarla.
