# Guía para explicar Cartal en clase

## Presentación corta

“Cartal es un juego de memoria en realidad aumentada. Al reconocer una imagen de una cripta,
Unity coloca un tablero virtual sobre ella. El jugador toca dos cartas y compara sus dibujos.
Si coinciden, quedan visibles; si son diferentes, se ocultan tras un segundo.
Al completar cuatro parejas, la puerta se abre y guardamos el mejor tiempo en el celular.
La mecánica usa tres scripts propios.”

## Orden para presentar el código

1. **Carta:** cada carta guarda un número de pareja, el frente y el reverso. Dos cartas con el mismo
   número forman una pareja. `Revelar` cambia el sprite y `Ocultar` lo restaura.
2. **JuegoMemoria.Jugar:** crea la lista `0, 0, 1, 1, 2, 2, 3, 3`, la mezcla mediante intercambios y prepara
   las ocho cartas. Así nunca falta una pareja ni sobra un símbolo. El símbolo 3 es la poción azul.
3. **MostrarTableroAR:** Unity reconoce nuestra imagen. El script consulta ese resultado y coloca
   el tablero como hijo de la imagen para que comparta su posición y orientación.
4. **Selección:** desde el punto tocado se lanza un rayo. Si alcanza el collider de una carta,
   se entrega esa carta a `JuegoMemoria.Seleccionar`.
5. **Comparación:** se guarda la primera carta y después la segunda. Se bloquea una tercera
   selección mientras se resuelve el intento. Se comparan los números de pareja.
6. **Fallo:** una corrutina espera un segundo sin congelar toda la aplicación y oculta ambas cartas.
7. **Acierto:** se marca la pareja y se incrementa el contador. Con cuatro parejas, otra corrutina
   gira la bisagra de la puerta durante un segundo y muestra el panel de victoria.
8. **Menú y reinicio:** se cancelan las corrutinas pendientes, se limpian las selecciones y se
   reinicia el estado de todas las cartas; no hace falta cambiar de escena.
9. **Cronómetro:** `Update` suma `Time.unscaledDeltaTime` solo si la partida sigue activa,
   el tablero está visible y la aplicación no está pausada. El tiempo de escaneo no cuenta.
10. **Récord:** al completar cuatro parejas comparamos el tiempo con el anterior. Solo se guarda
    si es el primero o es menor. `PlayerPrefs.SetFloat` registra el valor y `Save` lo escribe.
    `Start` lo recupera la próxima vez. `FormatearTiempo` muestra minutos, segundos y centésimas.
11. **Acerca de:** un botón activa el panel y oculta el menú; Volver realiza la operación inversa.
    Los nombres y los roles se incorporan desde `Assets/Resources/Equipo.txt` al preparar la escena.

## Variables que deben conocer

| Variable | Significado |
| --- | --- |
| `pareja` | Identificador del símbolo, de 0 a 3. |
| `revelada` | La carta muestra el frente. |
| `encontrada` | Su pareja ya fue acertada y no admite otra selección. |
| `primera`, `segunda` | Referencias a las dos cartas del intento. |
| `resolviendo` | Bloqueo que impide revelar una tercera carta. |
| `parejasEncontradas` | Cantidad de parejas completadas, de 0 a 4. |
| `jugando` | La partida sigue activa y admite selección. |
| `escaneando` | Se consulta la detección de la referencia y la entrada. |
| `tiempoPartida` | Segundos acumulados con el tablero visible durante la partida. |
| `mejorTiempo` | Menor tiempo guardado de una partida completa. |
| `ClaveRecord` | Nombre del dato local para el récord del tablero de cuatro parejas. |
| `aplicacionPausada` | Evita que el cronómetro avance cuando la app está en segundo plano. |
| `nuevoRecord` | Resultado de comparar el tiempo actual con el mejor anterior. |

## Conceptos de Unity

- **Sprite:** imagen 2D de una carta, de la pared o de la puerta.
- **SpriteRenderer:** componente que dibuja un sprite en la escena.
- **Collider:** volumen invisible que puede alcanzar el rayo de selección.
- **Raycast:** consulta para saber qué objeto está debajo del toque.
- **Corrutina:** secuencia que puede esperar y continuar después, sin detener los demás fotogramas.
- **Transform:** posición, rotación y tamaño de un objeto.
- **ARTrackedImageManager:** componente de Unity que reconoce la imagen; no programamos un algoritmo de visión.
- **Canvas:** interfaz del menú, los botones y los mensajes.
- **PlayerPrefs:** almacenamiento local sencillo de un número; no es una base de datos online.

## Preguntas habituales

**¿Por qué esto es AR?** La cámara muestra el entorno real y coloca las cartas virtuales en la posición
de una imagen física, actualizando la perspectiva al mover el celular.

**¿Por qué puede usarse un PNG en la laptop?** La cámara observa una imagen visible en una superficie;
la pantalla permite probar la misma referencia antes de imprimirla. Los reflejos, el brillo y el ángulo
pueden afectar al reconocimiento, por eso también se prueba la versión impresa para la presentación.

**¿Necesita internet para jugar?** El dibujo de referencia, los sprites y las reglas se incluyen en la app.
El reconocimiento de la imagen se realiza en el dispositivo. La preparación de Unity y los servicios AR
pueden requerir descarga previa; no se confunde esto con el funcionamiento offline de la partida.

**¿Son tres scripts en total?** Son tres scripts propios de ejecución. Además hay componentes de Unity,
paquetes AR y herramientas del editor para preparar y validar el proyecto.
