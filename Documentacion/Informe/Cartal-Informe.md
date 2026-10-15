Cartal

Informe del videojuego de memoria en realidad aumentada

Ingeniería de Sistemas
Universidad Privada Franz Tamayo UNIFRANZ
Sede La Paz, Bolivia

Versión del juego 0.2.1
15 de octubre de 2026


Integrante | Responsabilidad

Cristopher Iori Lazcano Gutierrez | Programación y coordinación

Alejandro Villalpando Rojas | Arte e interfaz

Galilea Alison Llusco Asistiri | Documentación y presentación

Alex Joel Quispe Ticona | Pruebas en Android y AR

Resumen

Cartal es un juego de memoria para Android. El jugador apunta la cámara a una imagen de una cripta y selecciona ocho cartas virtuales para encontrar cuatro parejas. Al completar el tablero, una puerta se abre y aparece el resultado de la partida. El menú incluye Jugar, Acerca de y Salir.

La aplicación usa Unity 6000.3.11f1 y tres scripts propios de ejecución. El cronómetro mide el tiempo con el tablero visible y PlayerPrefs conserva el mejor resultado en el dispositivo. La validación en un Samsung Galaxy A56 registró cuatro parejas y un tiempo de 22,03 segundos, conservado después de cerrar y reabrir la app.


---


Diseño del juego

Objetivo y alcance

El objetivo es completar un juego de memoria sencillo que permita demostrar realidad aumentada y explicar su código. La cámara muestra el entorno real y Unity coloca el tablero en la posición de una imagen reconocida. Al mover el celular, cambia la perspectiva de los objetos virtuales.

La temática es fantasía oscura y el arte utiliza sprites pixel art. El alcance comprende un tablero de ocho cartas, una puerta, efectos de sonido y una interfaz de una sola escena. La partida se controla mediante toques en la pantalla.


Elemento | Comportamiento

Cuatro símbolos | Calavera, espada, poción roja y poción azul. Cada símbolo aparece dos veces.

Selección | Se elige una carta y después otra. La comparación bloquea una tercera selección.

Fallo | Las dos cartas quedan visibles durante un segundo y vuelven a ocultarse.

Acierto | La pareja permanece visible y el contador aumenta.

Victoria | La cuarta pareja detiene el tiempo y abre la puerta.

Reinicio | Vuelve a mezclar las cartas, cierra la puerta y conserva el récord.

Figura 1. Vista de pruebas en PC del tablero de ocho cartas. La vista PC permite comprobar la lógica de memoria.


---


Implementación en Unity


Script de ejecución | Responsabilidad

Carta.cs | Guarda el identificador de pareja, el dibujo y los estados revelada y encontrada. Permite mostrar, ocultar y marcar la carta.

JuegoMemoria.cs | Mezcla las cartas, valida las dos selecciones, compara sus identificadores y controla menú, tiempo, récord y victoria.

MostrarTableroAR.cs | Consulta el seguimiento de la imagen, coloca el tablero y transforma el toque en un rayo para seleccionar una carta.

Cómo se resuelve una partida

Jugar prepara la lista 0, 0, 1, 1, 2, 2, 3, 3 y la mezcla mediante intercambios aleatorios. Así conserva exactamente dos cartas de cada símbolo. El rayo de selección alcanza el collider de una carta y entrega su componente Carta al controlador. El controlador rechaza una carta ya descubierta o encontrada y guarda la primera y la segunda selección.

Comparar verifica si los identificadores son iguales. En un fallo, una corrutina espera un segundo sin congelar los demás fotogramas. En un acierto, marca las dos cartas como encontradas. La cuarta pareja inicia la animación de la puerta y muestra la victoria.

Cronómetro y almacenamiento

Update suma Time.unscaledDeltaTime mientras hay una partida activa, el tablero está visible y Android no ha pausado la aplicación. El escaneo inicial y el tiempo sin seguimiento no se cuentan. Al ganar se compara tiempoPartida con mejorTiempo. Si es el primero o es menor, PlayerPrefs.SetFloat y PlayerPrefs.Save escriben el récord local. Start lo recupera al abrir Cartal.


Tecnología o recurso | Uso en el proyecto

Unity 6000.3.11f1 y C# | Escena, componentes, botones, reglas y sonidos.

AR Foundation 6.3.5 y ARCore 6.3.5 | Seguimiento de cámara y reconocimiento de la imagen en Android.

Sprites PNG y archivos ASE | Trece fuentes editables en LibreSprite. La poción azul reutiliza la silueta de la roja.

Herramientas del editor | PrepararCartal crea la escena y compila. PruebasCartal valida la lógica. Se ejecutan en el editor.

El arte inicial se construyó por código píxel a píxel y se entrega como archivos ASE para editar en LibreSprite. La fuente Silkscreen incluye su licencia OFL y los efectos de sonido son sintéticos originales.


---


Instrucciones de uso

Preparación del celular y la referencia

Instalar Cartal.apk en un Android compatible con ARCore y con Google Play Services for AR preparado. El USB sirve para instalar la aplicación y cargar el celular. La partida se ejecuta en el teléfono. La imagen Cartal-Imagen-AR.png se puede mostrar en una pantalla o imprimir a 20 por 20 cm, sobre una superficie plana y con buena luz.

Partida

1. Abrir Cartal y pulsar Jugar. La primera vez, aceptar el permiso de cámara.

2. Enfocar la imagen completa con la cámara trasera y esperar a que aparezca el tablero.

3. Tocar dos cartas. Recordar sus posiciones si los símbolos son diferentes.

4. Encontrar las cuatro parejas y observar el tiempo final y la apertura de la puerta.

5. Pulsar Volver a jugar para reiniciar o Menú para regresar al inicio. Acerca de muestra al equipo.

Figura 2. Capturas reales en el celular del menú con récord y de Acerca de en la versión 0.2.1.

Si el tablero desaparece, volver a enfocar la referencia. Las parejas encontradas se conservan y el cronómetro permanece pausado mientras el tablero está oculto.


---


Pruebas y resultados

Las pruebas registradas se realizaron con Unity 6000.3.11f1 y en un Samsung Galaxy A56 SM-A566E con Android 16. La versión 0.2.0 incorporó ocho cartas y el récord. La 0.2.1 ajustó los textos de Acerca de y conservó la misma mecánica.


Prueba | Resultado observado

Distribución del tablero | Ocho cartas y cuatro parejas, con dos pociones azules.

Selecciones inválidas | Se rechazan la misma carta repetida y una tercera durante la comparación.

Acierto y fallo | Los aciertos permanecen visibles. Los fallos se ocultan tras un segundo.

Cronómetro en Unity | Se pausa al ocultar el tablero o pausar la aplicación y reanuda al regresar.

Récord en pruebas controladas | 12,34 s inicia el récord, 8,25 s lo mejora y 30 s no lo reemplaza.

AR en el Galaxy A56 | El PNG en la laptop se reconoce. Se pierde y recupera el seguimiento.

Partida completa en Android | El registro contiene las parejas 1, 2, 3 y 4, seguidas de CARTAL_VICTORIA.

Persistencia en Android | El menú conserva 00:22.03 después de cerrar, reabrir y actualizar a 0.2.1.

Arte y compilaciones | Trece fuentes ASE cotejadas con sus PNG. Compilaciones Windows y Android correctas.

Comprobación del tiempo activo

Entre el reconocimiento inicial y la victoria transcurrieron 27,539 segundos. El seguimiento se perdió durante 5,496 segundos. La diferencia es 22,043 segundos, coherente con el tiempo registrado de 22,03654 segundos. La pantalla muestra 00:22.03 porque descarta fracciones inferiores a una centésima.

Condiciones de la demostración

El récord pertenece a ese celular y al tablero de cuatro parejas. La vista previa para PC prueba las reglas, mientras la realidad aumentada se demuestra en Android. La referencia usada durante la prueba fue el PNG en la pantalla de la laptop. Para el stand se prevé usar la misma imagen impresa.

El 15 de octubre se inició una prueba con Wi-Fi y datos móviles desactivados y Android confirmó que no había una red activa al comienzo. El registro confirmó reconocimiento de la imagen. El usuario confirmó que Cartal funcionaba sin internet con los servicios AR ya instalados.

Al iniciar AR apareció un aviso nativo GL_INVALID_ENUM. El seguimiento y la partida completa funcionaron, pero todavía no se ha confirmado la causa del aviso. Los registros y la descripción detallada de pruebas se conservan en PRUEBAS.md y en la carpeta logs del proyecto de trabajo.


---


Organización y entrega

Colaboración en Git

Repositorio público: https://github.com/Crisshubb/cartal-ar-pgm611

El proyecto terminado se distribuye en cuatro paquetes de integración. Cada integrante revisa su parte y la incorpora mediante tres commits en su propia rama. Los archivos meta se suben junto a los recursos para conservar las referencias de Unity. La guía de colaboración asigna todos los archivos a una sola persona y fija el orden de los pull requests.


Integrante | Tres bloques de integración

Cristopher | Carta, controlador de memoria y selección con realidad aumentada.

Alejandro | Cartas originales, pareja azul y recursos de ambiente con sonidos y fuente.

Galilea | Información del grupo y guías, informe técnico y diapositivas con notas.

Alex | Configuración de Unity y AR, escena y preparación, pruebas y herramientas.

Las ramas de trabajo se integran en develop. Después de revisar el proyecto completo y comprobar la compilación, se propone integrar develop en main mediante otro pull request. Cada integrante utiliza su propia cuenta y un correo verificado. Los commits registran la integración y revisión de los archivos entregados.

Material de entrega

La entrega incluye el proyecto Unity con Assets, Packages y ProjectSettings, los tres scripts comentados, trece fuentes ASE, la imagen de referencia, el APK para Android, una compilación de vista previa para Windows, las diapositivas y este informe editable con copia PDF.

En el stand, los visitantes pueden jugar por turnos en un celular preparado y competir por tiempo. Los integrantes explican la mecánica y muestran cómo el seguimiento de la imagen mantiene el tablero sobre la referencia.

Referencias

Unity. AR Foundation 6.3. https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@6.3/manual/index.html

Unity. Seguimiento de imágenes. https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@6.3/manual/features/image-tracking.html

Google. Augmented Images en ARCore. https://developers.google.com/ar/develop/augmented-images

Google. Dispositivos compatibles con ARCore. https://developers.google.com/ar/devices

UNIFRANZ. Sede La Paz. https://unifranz.edu.bo/sedes/la-paz/