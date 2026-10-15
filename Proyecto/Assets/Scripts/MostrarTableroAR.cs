using UnityEngine; // Permite usar la cámara, los rayos y las transformaciones.
using UnityEngine.EventSystems; // Permite evitar que un botón del menú seleccione una carta.
using UnityEngine.XR.ARFoundation; // Proporciona la sesión AR y el reconocimiento de imágenes de Unity.
using UnityEngine.XR.ARSubsystems; // Proporciona los estados de seguimiento de la imagen.

// Coloca el tablero sobre la imagen reconocida y convierte los toques en selección de cartas.
public class MostrarTableroAR : MonoBehaviour
{
    public ARSession sesion; // Controla la sesión de realidad aumentada del celular.
    public ARTrackedImageManager imagenes; // Componente de Unity que reconoce nuestra imagen de referencia.
    public Camera camara; // Cámara que observa el tablero y genera el rayo de selección.
    public GameObject tablero; // Contenedor de todas las cartas y de la puerta.
    public JuegoMemoria juego; // Controlador de las reglas de memoria.
    public bool escaneando; // Indica si debemos buscar la imagen y aceptar entrada.
    private bool reconocido; // Recuerda si la imagen estuvo visible en el fotograma anterior.
    private Transform contenedorOriginal; // Conserva el padre original del tablero al salir de AR.

    private void Awake() // Unity lo llama antes de Start.
    {
        contenedorOriginal = tablero.transform.parent; // Guarda dónde estaba el tablero al cargar la escena.
    }

    public void Comenzar() // Prepara el tablero para una nueva partida.
    {
        escaneando = true; // Comienza a detectar la imagen y los toques.
        reconocido = false; // Reinicia el estado de detección.
        if (Application.platform == RuntimePlatform.Android) // El celular utiliza AR real.
        {
            tablero.SetActive(false); // Espera a reconocer la imagen antes de mostrar las cartas.
            sesion.enabled = true; // Inicia la sesión AR; Unity solicita permiso de cámara si hace falta.
            imagenes.enabled = true; // Activa el reconocimiento de la biblioteca de imágenes.
            juego.mensaje.text = "APUNTA A LA IMAGEN DE LA CRIPTA"; // Explica cómo encontrar el tablero.
        }
        else // La PC permite comprobar la lógica sin cámara AR.
        {
            camara.orthographic = true; // Usa una vista frontal sin perspectiva para comprobar las parejas.
            camara.orthographicSize = 2.85f; // Encuadra el tablero completo en la vista de PC.
            camara.transform.localPosition = new Vector3(0f, 0f, -8f); // Aleja la cámara del tablero incluso si el editor está configurado para Android.
            camara.transform.localRotation = Quaternion.identity; // Orienta la cámara hacia las cartas.
            tablero.transform.SetParent(contenedorOriginal, false); // Devuelve el tablero a la escena de prueba.
            tablero.transform.localPosition = Vector3.zero; // Centra el tablero delante de la cámara de PC.
            tablero.transform.localRotation = Quaternion.identity; // Orienta el tablero hacia la cámara de PC.
            tablero.transform.localScale = Vector3.one; // Utiliza el tamaño de la vista previa.
            tablero.SetActive(true); // Muestra inmediatamente las cartas en PC.
            juego.mensaje.text = "VISTA PC  -  PULSA DOS CARTAS"; // Identifica claramente el modo de prueba.
        }
    }

    public void Detener() // Desactiva el tablero al volver al menú.
    {
        escaneando = false; // Deja de procesar detecciones y toques.
        tablero.SetActive(false); // Oculta las cartas.
        tablero.transform.SetParent(contenedorOriginal, false); // Evita conservar un padre de AR que pueda desaparecer.
        imagenes.enabled = false; // Detiene la detección de imágenes.
        sesion.enabled = false; // Detiene la sesión de realidad aumentada.
    }

    private void Update() // Unity lo llama en cada fotograma.
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // El botón Atrás de Android y Escape de PC vuelven al menú.
        {
            juego.VolverAlMenu(); // Cancela la partida actual.
        }
        if (!escaneando) // En el menú no buscamos imágenes ni seleccionamos cartas.
        {
            return; // Termina este fotograma.
        }
        if (Application.platform == RuntimePlatform.Android) // Actualiza el seguimiento solamente en el celular.
        {
            ActualizarImagen(); // Busca una imagen cuyo seguimiento sea fiable.
        }
        if (!tablero.activeSelf) // Si no vemos la referencia, tampoco se pueden seleccionar cartas.
        {
            return; // Espera a recuperar el seguimiento.
        }
        if (Input.touchCount > 0) // Comprueba si hay un dedo en la pantalla.
        {
            Touch toque = Input.GetTouch(0); // Lee únicamente el primer dedo.
            if (toque.phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(toque.fingerId)) // Acepta un toque nuevo que no sea sobre la interfaz.
            {
                SeleccionarEn(toque.position); // Envía la posición del toque al selector de cartas.
            }
        }
        else if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) // Permite probar con un clic en PC.
        {
            SeleccionarEn(Input.mousePosition); // Usa la posición del mouse como punto de selección.
        }
    }

    private void ActualizarImagen() // Consulta las imágenes que el componente de Unity está siguiendo.
    {
        bool visible = false; // Parte de la suposición de que la referencia no se ve.
        foreach (ARTrackedImage imagen in imagenes.trackables) // Recorre las imágenes detectadas por Unity.
        {
            if (imagen.trackingState != TrackingState.Tracking || imagen.referenceImage.name != "Cartal") // Solo acepta nuestra referencia con seguimiento completo.
            {
                continue; // Omite una imagen que todavía no se sigue de forma fiable.
            }
            tablero.transform.SetParent(imagen.transform, false); // Vincula el tablero con la posición de la referencia.
            tablero.transform.localPosition = new Vector3(0f, 0.005f, 0f); // Separa ligeramente el tablero de la superficie real.
            tablero.transform.localRotation = Quaternion.Euler(90f, 0f, 0f); // Coloca los sprites sobre la imagen con la puerta arriba y las cartas debajo.
            tablero.transform.localScale = Vector3.one * (imagen.size.x / 3.84f); // Ajusta el ancho del tablero al ancho reconocido de la imagen.
            visible = true; // Indica que encontramos una referencia válida.
            break; // Nuestra aplicación solo necesita un tablero.
        }
        tablero.SetActive(visible); // Muestra el tablero únicamente con seguimiento fiable.
        if (visible != reconocido) // Actualiza el mensaje solo cuando cambia la detección.
        {
            reconocido = visible; // Guarda el nuevo estado de seguimiento.
            juego.mensaje.text = visible ? "ENCUENTRA LAS CUATRO PAREJAS" : "VUELVE A ENFOCAR LA IMAGEN"; // Ayuda a recuperar la referencia.
            Debug.Log(visible ? "CARTAL_IMAGEN_RECONOCIDA" : "CARTAL_IMAGEN_PERDIDA"); // Registra las detecciones para las pruebas.
        }
        if (ARSession.state == ARSessionState.Unsupported) // Comprueba si el dispositivo carece de soporte AR.
        {
            juego.mensaje.text = "ESTE DISPOSITIVO NO ADMITE AR"; // Explica por qué no puede aparecer el tablero.
        }
#if UNITY_ANDROID // Esta comprobación solo se compila para Android.
        else if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.Camera)) // Consulta si el usuario permitió utilizar la cámara.
        {
            juego.mensaje.text = "PERMITE LA CAMARA PARA ESCANEAR"; // Explica el permiso necesario sin ocultar el botón Menú.
        }
#endif // Termina la sección específica de Android.
    }

    public void SeleccionarEn(Vector2 posicion) // Convierte una posición de pantalla en una carta del mundo virtual.
    {
        Ray rayo = camara.ScreenPointToRay(posicion); // Genera un rayo desde la cámara hacia el punto tocado.
        if (Physics.Raycast(rayo, out RaycastHit impacto, 20f)) // Comprueba si el rayo alcanza un objeto con collider.
        {
            Carta carta = impacto.collider.GetComponent<Carta>(); // Busca el componente Carta en el objeto alcanzado.
            juego.Seleccionar(carta); // Entrega la carta al controlador, que valida la selección.
        }
    }
}
