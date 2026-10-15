using System.Collections; // Permite usar IEnumerator para esperar entre acciones.
using UnityEngine; // Permite usar componentes, colores y transformaciones.
using UnityEngine.UI; // Permite actualizar textos y botones del menú.

// Controla el menú, compara cuatro parejas y guarda el mejor tiempo en este dispositivo.
public class JuegoMemoria : MonoBehaviour
{
    private const string ClaveRecord = "Cartal.MejorTiempo.4Parejas"; // Nombre del dato guardado para este tablero de ocho cartas.
    public Carta[] cartas; // Las ocho cartas colocadas en la escena.
    public Sprite[] dibujos; // Los cuatro dibujos disponibles, en el orden de sus identificadores.
    public MostrarTableroAR realidadAumentada; // Componente que muestra y selecciona el tablero.
    public GameObject menu; // Panel de inicio con Jugar, Acerca de y Salir.
    public GameObject acercaDe; // Panel que presenta al grupo, sus roles y su institución.
    public GameObject victoria; // Panel que aparece cuando se completa la partida.
    public GameObject hud; // Panel con las instrucciones y el contador durante el juego.
    public Text contador; // Texto que muestra cuántas parejas se encontraron.
    public Text mensaje; // Texto con el estado de la partida o de la cámara.
    public Text reloj; // Muestra el tiempo de la partida actual.
    public Text recordMenu; // Muestra el mejor tiempo en el menú.
    public Text recordHud; // Muestra el mejor tiempo durante la partida.
    public Text resumenVictoria; // Muestra el tiempo final y si se consiguió un récord.
    public Transform puerta; // Bisagra de la puerta que girará al ganar.
    public AudioSource sonido; // Componente que reproduce los efectos cortos.
    public AudioClip voltear; // Efecto al descubrir una carta.
    public AudioClip acierto; // Efecto al encontrar una pareja.
    public AudioClip ganar; // Efecto al completar las cuatro parejas.
    public bool jugando; // Indica si hay una partida activa.
    public bool resolviendo; // Bloquea la selección mientras se comparan dos cartas.
    public int parejasEncontradas; // Cuenta los aciertos, de cero a cuatro.
    public float tiempoPartida; // Acumula los segundos durante los que el tablero es visible.
    public float mejorTiempo; // Conserva el menor tiempo de una partida completada.
    private bool aplicacionPausada; // Impide contar tiempo cuando Android pausa el juego.
    private Carta primera; // Guarda la primera carta seleccionada.
    private Carta segunda; // Guarda la segunda carta seleccionada.

    private void Start() // Unity llama a este método al iniciar la escena.
    {
        Application.targetFrameRate = 60; // Limita la ejecución a 60 imágenes por segundo.
        mejorTiempo = PlayerPrefs.GetFloat(ClaveRecord, 0f); // Lee el récord guardado; cero significa que todavía no hay uno.
        VolverAlMenu(); // El juego comienza mostrando el menú.
    }

    private void Update() // Unity llama a este método una vez por fotograma.
    {
        if (jugando && realidadAumentada.tablero.activeSelf && !aplicacionPausada) // Cuenta tiempo solo mientras se puede observar el tablero.
        {
            tiempoPartida += Time.unscaledDeltaTime; // Añade segundos reales, incluidos los intentos fallidos.
            reloj.text = "TIEMPO " + FormatearTiempo(tiempoPartida); // Actualiza el cronómetro visible.
        }
    }

    private void OnApplicationPause(bool pausa) // Android avisa cuando la aplicación pasa al fondo o regresa.
    {
        aplicacionPausada = pausa; // Guarda si el cronómetro debe detenerse temporalmente.
    }

    public static string FormatearTiempo(float valor) // Convierte segundos en minutos, segundos y centésimas.
    {
        int centesimas = Mathf.FloorToInt(valor * 100f); // Descarta fracciones menores que una centésima.
        int minutos = centesimas / 6000; // Cada minuto contiene seis mil centésimas.
        int segundos = centesimas / 100 % 60; // Obtiene los segundos que quedan dentro del minuto.
        int fraccion = centesimas % 100; // Obtiene las centésimas restantes.
        return minutos.ToString("00") + ":" + segundos.ToString("00") + "." + fraccion.ToString("00"); // Devuelve un formato como 01:24.35.
    }

    private void ActualizarRecord() // Mantiene el mismo récord visible en el menú y en la partida.
    {
        string texto = mejorTiempo > 0f ? FormatearTiempo(mejorTiempo) : "--:--.--"; // Distingue un tiempo guardado de un récord todavía inexistente.
        recordMenu.text = "MEJOR TIEMPO " + texto; // Actualiza el menú.
        recordHud.text = "MEJOR " + texto; // Actualiza la interfaz de juego.
    }

    public void Jugar() // Se ejecuta cuando se pulsa Jugar o Volver a jugar.
    {
        StopAllCoroutines(); // Cancela cualquier espera de la partida anterior.
        primera = null; // Elimina la selección anterior de la primera carta.
        segunda = null; // Elimina la selección anterior de la segunda carta.
        parejasEncontradas = 0; // Reinicia el número de parejas completadas.
        resolviendo = false; // Permite seleccionar cartas.
        jugando = true; // Activa la partida.
        tiempoPartida = 0f; // Reinicia el cronómetro sin borrar el récord.
        reloj.text = "TIEMPO 00:00.00"; // Muestra el tiempo inicial antes de reconocer la imagen.
        ActualizarRecord(); // Conserva visible el récord de partidas anteriores.
        puerta.localRotation = Quaternion.identity; // Cierra la puerta.
        int[] orden = { 0, 0, 1, 1, 2, 2, 3, 3 }; // Crea dos cartas de cada dibujo, incluida la poción azul.
        for (int i = orden.Length - 1; i > 0; i--) // Mezcla la lista desde el final hacia el inicio.
        {
            int otra = Random.Range(0, i + 1); // Escoge una posición entre cero e i, incluidos.
            int guardado = orden[i]; // Conserva temporalmente el valor que vamos a intercambiar.
            orden[i] = orden[otra]; // Coloca el valor aleatorio en la posición actual.
            orden[otra] = guardado; // Completa el intercambio sin perder ningún valor.
        }
        for (int i = 0; i < cartas.Length; i++) // Recorre las ocho cartas de la escena.
        {
            cartas[i].Preparar(orden[i], dibujos[orden[i]]); // Asigna el dibujo mezclado y oculta la carta.
        }
        contador.text = "PAREJAS  0 / 4"; // Reinicia el texto del contador.
        menu.SetActive(false); // Oculta el menú inicial.
        acercaDe.SetActive(false); // Cierra la información del grupo si estaba abierta.
        victoria.SetActive(false); // Oculta el panel de victoria.
        hud.SetActive(true); // Muestra las instrucciones de la partida.
        realidadAumentada.Comenzar(); // Activa la cámara AR o la vista de prueba en PC.
        Debug.Log("CARTAL_PARTIDA_INICIADA"); // Deja constancia del inicio para comprobar la aplicación.
    }

    public void Seleccionar(Carta carta) // Recibe la carta que se tocó o se pulsó con el mouse.
    {
        if (!jugando || resolviendo || !realidadAumentada.tablero.activeSelf || carta == null || carta.revelada || carta.encontrada) // Rechaza selecciones no válidas.
        {
            return; // Termina sin modificar la partida.
        }
        carta.Revelar(); // Muestra el dibujo seleccionado.
        sonido.PlayOneShot(voltear); // Reproduce un sonido de selección.
        if (primera == null) // Comprueba si todavía falta elegir la primera carta.
        {
            primera = carta; // Guarda la primera carta para compararla más tarde.
            mensaje.text = "ELIGE LA SEGUNDA CARTA"; // Indica el siguiente paso al jugador.
            return; // Espera una segunda selección.
        }
        segunda = carta; // Guarda la segunda carta.
        resolviendo = true; // Evita que se pueda descubrir una tercera carta.
        StartCoroutine(Comparar()); // Comienza la comparación de las dos cartas.
    }

    private IEnumerator Comparar() // Compara las dos cartas y espera si son diferentes.
    {
        if (primera.pareja == segunda.pareja) // Dos identificadores iguales forman una pareja.
        {
            primera.MarcarPareja(); // Deja descubierta la primera carta.
            segunda.MarcarPareja(); // Deja descubierta la segunda carta.
            parejasEncontradas++; // Añade una pareja al contador.
            contador.text = "PAREJAS  " + parejasEncontradas + " / 4"; // Actualiza el contador visible.
            mensaje.text = "SELLO RECUPERADO"; // Comunica que hubo un acierto.
            sonido.PlayOneShot(acierto); // Reproduce el efecto de pareja.
            Debug.Log("CARTAL_PAREJA " + parejasEncontradas); // Registra el acierto durante las pruebas.
        }
        else // Las cartas tienen dibujos diferentes.
        {
            mensaje.text = "RECUERDA LOS SIMBOLOS..."; // Invita a recordar las posiciones.
            yield return new WaitForSecondsRealtime(1f); // Espera un segundo para que se vean los dibujos.
            primera.Ocultar(); // Oculta la primera carta fallida.
            segunda.Ocultar(); // Oculta la segunda carta fallida.
            mensaje.text = "ENCUENTRA LAS CUATRO PAREJAS"; // Permite intentarlo de nuevo.
        }
        primera = null; // Limpia la primera selección.
        segunda = null; // Limpia la segunda selección.
        resolviendo = false; // Vuelve a permitir las selecciones.
        if (parejasEncontradas == 4) // Comprueba si el tablero está completo.
        {
            jugando = false; // Impide seleccionar cartas después de ganar.
            reloj.text = "TIEMPO " + FormatearTiempo(tiempoPartida); // Deja el cronómetro y el resultado con el mismo tiempo final.
            bool nuevoRecord = mejorTiempo <= 0f || tiempoPartida < mejorTiempo; // Comprueba si es la primera victoria o una partida más rápida.
            if (nuevoRecord) // Solo reemplaza el récord cuando mejora.
            {
                mejorTiempo = tiempoPartida; // Conserva el tiempo de esta victoria.
                PlayerPrefs.SetFloat(ClaveRecord, mejorTiempo); // Guarda el dato en el almacenamiento local de Unity.
                PlayerPrefs.Save(); // Escribe el récord para conservarlo al cerrar la aplicación.
                ActualizarRecord(); // Muestra el nuevo récord en los textos.
            }
            resumenVictoria.text = "TIEMPO " + FormatearTiempo(tiempoPartida) + (nuevoRecord ? "\nNUEVO RECORD" : "\nMEJOR " + FormatearTiempo(mejorTiempo)); // Presenta el resultado de esta partida.
            Debug.Log("CARTAL_TIEMPO_FINAL " + tiempoPartida + " MEJOR " + mejorTiempo); // Registra el resultado para verificar el cronómetro.
            StartCoroutine(AbrirPuerta()); // Abre la puerta antes de mostrar la victoria.
        }
    }

    private IEnumerator AbrirPuerta() // Anima la bisagra de la puerta.
    {
        sonido.PlayOneShot(ganar); // Reproduce el efecto de victoria.
        mensaje.text = "LA CRIPTA SE HA ABIERTO"; // Muestra el resultado de completar los sellos.
        float tiempo = 0f; // Guarda el tiempo transcurrido de la animación.
        while (tiempo < 1f) // Mantiene la animación durante un segundo.
        {
            tiempo += Time.unscaledDeltaTime; // Suma el tiempo real del último fotograma.
            puerta.localRotation = Quaternion.Euler(0f, Mathf.Lerp(0f, -100f, Mathf.Clamp01(tiempo)), 0f); // Gira la puerta progresivamente.
            yield return null; // Espera el siguiente fotograma.
        }
        victoria.SetActive(true); // Muestra Volver a jugar y Menú.
        Debug.Log("CARTAL_VICTORIA"); // Registra que la partida terminó correctamente.
    }

    public void VolverAlMenu() // Permite cancelar la partida y regresar al inicio.
    {
        StopAllCoroutines(); // Cancela comparaciones y animaciones pendientes.
        jugando = false; // Desactiva la selección de cartas.
        resolviendo = false; // Limpia el bloqueo de selección.
        primera = null; // Descarta cualquier selección incompleta.
        segunda = null; // Descarta la segunda carta, si existía.
        realidadAumentada.Detener(); // Oculta el tablero y detiene la cámara AR.
        hud.SetActive(false); // Oculta las instrucciones de partida.
        acercaDe.SetActive(false); // Cierra Acerca de al regresar o usar el botón Atrás.
        victoria.SetActive(false); // Oculta la pantalla de victoria.
        ActualizarRecord(); // Muestra el récord incluso después de cancelar una partida.
        menu.SetActive(true); // Muestra Jugar y Salir.
    }

    public void AbrirAcercaDe() // Se ejecuta al pulsar Acerca de en el menú.
    {
        menu.SetActive(false); // Oculta el menú detrás de la información del grupo.
        acercaDe.SetActive(true); // Muestra los integrantes, sus roles, la carrera y la universidad.
    }

    public void Salir() // Se ejecuta al pulsar Salir.
    {
        Application.Quit(); // Cierra la aplicación compilada; el editor ignora esta llamada.
    }
}
