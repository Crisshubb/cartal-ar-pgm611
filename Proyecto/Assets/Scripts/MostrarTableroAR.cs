using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

// Reconoce el nivel y mantiene su tablero sobre la imagen o sobre un anclaje AR.
public class MostrarTableroAR : MonoBehaviour
{
    public ARSession sesion;
    public ARTrackedImageManager imagenes;
    public ARAnchorManager anclas;
    public Camera camara;
    public GameObject tablero;
    public JuegoMemoria juego;
    public Button fijar, reubicar;
    public bool escaneando;
    public bool Fijado => ancla != null || fijadoPC;
    public bool buscandoNivel { get; private set; }
    private Transform contenedorOriginal;
    private ARAnchor ancla;
    private bool fijadoPC, creandoAncla, visibleAnterior;
    private int operacion;
    private TrackableId? imagenActual;
    private bool Android => Application.platform == RuntimePlatform.Android;

    private void Awake() => contenedorOriginal = tablero.transform.parent;

    public void BuscarNivel()
    {
        Detener();
        buscandoNivel = true;
        escaneando = true;
        if (Android) { sesion.enabled = true; imagenes.enabled = true; anclas.enabled = true; }
        juego.mensaje.text = "ENFOCA UNA DE LAS TRES IMAGENES";
        juego.reloj.text = "TIEMPO --:--.--";
        juego.recordHud.text = "CRIPTA / BIBLIOTECA / TESORO";
        ActualizarBotones();
    }

    public void Comenzar()
    {
        // Reconocer un marcador no debe reiniciar la sesión que acaba de encontrarlo.
        if (Android && buscandoNivel)
        {
            Liberar();
            buscandoNivel = false;
        }
        else Detener();
        escaneando = true;
        if (Android)
        {
            sesion.enabled = true;
            imagenes.enabled = true;
            anclas.enabled = true;
            juego.mensaje.text = "ENFOCA: " + juego.niveles[juego.nivelActual].nombre.ToUpperInvariant();
        }
        else
        {
            camara.orthographic = true;
            camara.orthographicSize = 2.85f;
            camara.transform.localPosition = new Vector3(0, 0, -8);
            camara.transform.localRotation = Quaternion.identity;
            tablero.transform.SetParent(contenedorOriginal, false);
            tablero.transform.localPosition = Vector3.zero;
            tablero.transform.localRotation = Quaternion.identity;
            tablero.transform.localScale = Vector3.one;
            tablero.SetActive(true);
            juego.mensaje.text = "VISTA PC / ENCUENTRA LAS PAREJAS";
        }
        ActualizarBotones();
    }

    private void QuitarAncla(ARAnchor valor)
    {
        if (valor == null) return;
        if (anclas != null && anclas.subsystem != null && anclas.subsystem.running && anclas.TryRemoveAnchor(valor)) return;
        Destroy(valor.gameObject);
    }

    private void Liberar()
    {
        operacion++; // Invalida una petición asíncrona que todavía esté pendiente.
        creandoAncla = false;
        tablero.transform.SetParent(contenedorOriginal, true);
        QuitarAncla(ancla);
        ancla = null;
        fijadoPC = false;
        imagenActual = null;
    }

    public void Detener()
    {
        escaneando = false;
        buscandoNivel = false;
        Liberar();
        tablero.SetActive(false);
        visibleAnterior = false;
        imagenes.enabled = false;
        anclas.enabled = false;
        sesion.enabled = false;
        ActualizarBotones();
    }

    public async void FijarTablero()
    {
        if (!escaneando || buscandoNivel || !tablero.activeSelf || Fijado || creandoAncla) return;
        if (!Android)
        {
            fijadoPC = true;
            juego.mensaje.text = "TABLERO FIJO / VISTA PC";
            ActualizarBotones();
            return;
        }
        if (ARSession.state != ARSessionState.SessionTracking) return;
        int solicitud = ++operacion;
        creandoAncla = true;
        ActualizarBotones();
        juego.mensaje.text = "FIJANDO TABLERO...";
        try
        {
            // La pose se captura antes del await, sin depender de que el marcador siga visible.
            Pose pose = new Pose(tablero.transform.position, tablero.transform.rotation);
            Vector3 escala = tablero.transform.lossyScale;
            var resultado = await anclas.TryAddAnchorAsync(pose);
            if (this == null) return;
            if (solicitud != operacion || !escaneando)
            {
                if (resultado.status.IsSuccess()) QuitarAncla(resultado.value);
                return;
            }
            if (resultado.status.IsSuccess() && resultado.value != null)
            {
                ancla = resultado.value;
                tablero.transform.SetParent(ancla.transform, false);
                tablero.transform.localPosition = Vector3.zero;
                tablero.transform.localRotation = Quaternion.identity;
                Vector3 padre = ancla.transform.lossyScale;
                tablero.transform.localScale = new Vector3(escala.x / padre.x, escala.y / padre.y, escala.z / padre.z);
                juego.mensaje.text = "TABLERO FIJO / PUEDES RETIRAR LA IMAGEN";
            }
            else juego.mensaje.text = "NO SE PUDO FIJAR / ENFOCA Y REINTENTA";
        }
        catch (Exception error)
        {
            Debug.LogWarning("No se pudo crear el anclaje: " + error.Message);
            if (this != null && solicitud == operacion && escaneando)
                juego.mensaje.text = "NO SE PUDO FIJAR / REINTENTA";
        }
        finally
        {
            if (this != null && solicitud == operacion) { creandoAncla = false; ActualizarBotones(); }
        }
    }

    public void Reubicar()
    {
        if (!escaneando || buscandoNivel) return;
        Liberar();
        if (Android) tablero.SetActive(false);
        visibleAnterior = false;
        juego.mensaje.text = Android ? "ENFOCA LA MISMA IMAGEN PARA REUBICAR" : "VISTA PC / TABLERO CENTRADO";
        ActualizarBotones();
    }

    private void ActualizarBotones()
    {
        if (fijar != null) fijar.interactable = escaneando && !buscandoNivel && tablero.activeSelf && !Fijado && !creandoAncla;
        if (reubicar != null) reubicar.interactable = escaneando && !buscandoNivel && !creandoAncla;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) juego.VolverAlMenu();
        if (!escaneando) return;
        if (Android) ActualizarImagen();
        ActualizarBotones();
        if (!tablero.activeSelf || creandoAncla) return;
        if (Input.touchCount > 0)
        {
            Touch toque = Input.GetTouch(0);
            if (toque.phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(toque.fingerId)) SeleccionarEn(toque.position);
        }
        else if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) SeleccionarEn(Input.mousePosition);
    }

    private void ActualizarImagen()
    {
        bool visible = false;
        if (ancla != null)
            visible = ancla.trackingState == TrackingState.Tracking && ARSession.state == ARSessionState.SessionTracking;
        else if (!creandoAncla)
        {
            foreach (ARTrackedImage imagen in imagenes.trackables)
            {
                if (imagen.trackingState != TrackingState.Tracking) continue;
                int nivel = Array.FindIndex(juego.niveles, n => n.marcador == imagen.referenceImage.name);
                if (nivel < 0) continue;
                if (buscandoNivel) juego.ElegirNivel(nivel);
                if (nivel != juego.nivelActual || (imagenActual.HasValue && imagenActual.Value != imagen.trackableId)) continue;
                imagenActual = imagen.trackableId;
                // El tablero no pertenece al marcador: sobrevive si AR elimina su objeto.
                tablero.transform.SetParent(contenedorOriginal, false);
                tablero.transform.SetPositionAndRotation(imagen.transform.TransformPoint(new Vector3(0, 0.005f, 0)),
                    imagen.transform.rotation * Quaternion.Euler(90, 0, 0));
                tablero.transform.localScale = Vector3.one * (imagen.size.x / 3.84f);
                visible = true;
                break;
            }
        }
        else visible = tablero.activeSelf;
        tablero.SetActive(visible);
        if (!visible && ancla == null && !creandoAncla) imagenActual = null;
        if (visible != visibleAnterior)
        {
            visibleAnterior = visible;
            juego.mensaje.text = visible ? (Fijado ? "TABLERO FIJO / CONTINUA" : "ENCUENTRA LAS PAREJAS / PUEDES FIJAR EL TABLERO")
                : (Fijado ? "MUEVE DESPACIO EL CELULAR PARA RECUPERAR EL ENTORNO" : "VUELVE A ENFOCAR LA IMAGEN");
        }
        if (ARSession.state == ARSessionState.Unsupported) juego.mensaje.text = "ESTE DISPOSITIVO NO ADMITE AR";
#if UNITY_ANDROID
        else if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.Camera))
            juego.mensaje.text = "PERMITE LA CAMARA PARA ESCANEAR";
#endif
    }

    public void SeleccionarEn(Vector2 posicion)
    {
        if (!escaneando || !tablero.activeSelf || creandoAncla) return;
        if (Physics.Raycast(camara.ScreenPointToRay(posicion), out RaycastHit impacto, 20))
            juego.Seleccionar(impacto.collider.GetComponent<Carta>());
    }
}
