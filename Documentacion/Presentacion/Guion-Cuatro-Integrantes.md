# Guion de exposición para los cuatro integrantes

Duración sugerida de seis a ocho minutos. Las notas también están dentro del PowerPoint.

## Diapositiva 1

Expone: Galilea.

Buenos días. Presentamos Cartal, un juego de memoria en realidad aumentada con temática de fantasía oscura. El celular reconoce una imagen de referencia y muestra sobre ella un tablero de ocho cartas. Nuestro objetivo es encontrar cuatro parejas para abrir la cripta.

## Diapositiva 2

Expone: Alejandro.

Las cartas tienen un identificador de pareja. Al seleccionar dos, comparamos esos números. Si coinciden, las cartas permanecen visibles. Si fallamos, se ocultan después de un segundo. La poción azul usa la misma forma de la poción roja y cambia el color del líquido.

## Diapositiva 3

Expone: Galilea.

Unity organiza la escena, las imágenes, los botones y los sonidos. C# contiene las reglas. AR Foundation y ARCore reconocen la referencia y siguen la cámara. Los archivos ASE son editables en LibreSprite. El arte inicial se construyó por código píxel a píxel y el grupo puede editarlo; no afirmamos que fue dibujado a mano en LibreSprite.

## Diapositiva 4

Expone: Alex.

Abrimos Cartal en el celular y pulsamos Jugar. La primera vez aceptamos el permiso de cámara. Apuntamos a la imagen completa, sin recortarla. Tocamos dos cartas y recordamos sus posiciones. Cuando encontramos las cuatro parejas, se abre la puerta. Para el stand imprimiremos la misma referencia en papel mate y comprobaremos la iluminación.

## Diapositiva 5

Expone: Cristopher.

El cronómetro empieza cuando aparece el tablero. Si perdemos la referencia o la aplicación pasa al fondo, se pausa. Al encontrar la cuarta pareja, se compara el tiempo con el récord. Si es mejor, PlayerPrefs guarda el nuevo valor. Reiniciar la partida no borra el récord y cerrar la aplicación tampoco. El récord es local, no un ranking online.

## Diapositiva 6

Expone: Cristopher.

Carta guarda el identificador, el dibujo y el estado. JuegoMemoria mezcla las ocho cartas, guarda las dos selecciones, compara sus identificadores, controla los paneles y guarda el tiempo. MostrarTableroAR coloca el tablero en la imagen detectada y convierte cada toque en un rayo para seleccionar una carta. Los scripts incluyen comentarios en español en sus variables e instrucciones. Las herramientas del editor preparan y prueban la escena, pero no se ejecutan en el celular.

## Diapositiva 7

Expone: Alex.

Colocamos la imagen impresa de 20 por 20 centímetros en una superficie plana y con buena luz. Cada visitante pulsa Jugar y busca cuatro parejas. Mostramos un fallo, un acierto y la puerta al completar el tablero. Anotamos los nombres y tiempos en una tabla del stand. Cartal guarda el mejor tiempo en el celular. El USB sirve para instalar y cargar. La versión actual es una aplicación Android y el QR por sí solo no abre el juego en el navegador.

## Diapositiva 8

Expone: Los cuatro integrantes.

Cristopher: programación y coordinación. Alejandro: arte e interfaz. Galilea: documentación y presentación. Alex: pruebas en Android y AR. Somos estudiantes de Ingeniería de Sistemas de la Universidad Privada Franz Tamayo, UNIFRANZ, Sede La Paz, Bolivia. Fuente institucional: https://unifranz.edu.bo/sedes/la-paz/

## Diapositiva 9

Expone: Alex.

El registro de Android confirmó el reconocimiento del PNG mostrado en la laptop, pérdida y recuperación del seguimiento y cuatro parejas. La victoria registró 22.03654 segundos, mostrados como 22.03. Cerramos y reabrimos la app y el récord permaneció. Después actualizamos a 0.2.1 y también se conservó. La vista PC es una herramienta para comprobar las reglas. La captura de esta diapositiva corresponde al celular y muestra Acerca de actualizado. Evidencia: logs/android-0.2.0-victoria.log y Entrega/Cartal-Celular-AcercaDe-0.2.1.png.

## Diapositiva 10

Expone: Cristopher.

Para explicar el flujo decimos: MostrarTableroAR reconoce la imagen y recibe el toque. Un rayo encuentra una Carta y la envía a JuegoMemoria. El controlador guarda dos selecciones y compara sus identificadores. Una corrutina espera si fallamos. La cuarta pareja detiene el reloj, guarda el récord si mejora y abre la puerta. Los archivos del editor preparan y prueban la escena y no son scripts adicionales de ejecución en el celular.