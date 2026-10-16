// Herramienta del editor: crea la escena y compila; no forma parte del código ejecutado en el celular.
using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.SpatialTracking;
using Unity.XR.CoreUtils;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.XR.ARSubsystems;
using UnityEditor.XR.ARCore;
using UnityEditor.XR.Management;
using UnityEditor.XR.Management.Metadata;
using UnityEngine.XR.Management;

public static class PrepararCartal
{
    private const string Escena = "Assets/Escenas/Cartal.unity";
    private static readonly Color Fondo = new Color32(21, 20, 29, 255);
    private static readonly Color Oro = new Color32(203, 167, 107, 255);
    private static Font fuente;

    [MenuItem("Cartal/Preparar escena")]
    public static void Preparar()
    {
        Directory.CreateDirectory("Assets/Escenas");
        Directory.CreateDirectory("Assets/Configuracion");
        foreach (string ruta in Directory.GetFiles("Assets/Arte", "*.png"))
        {
            TextureImporter importador = (TextureImporter)AssetImporter.GetAtPath(ruta.Replace('\\', '/'));
            importador.textureType = TextureImporterType.Sprite;
            importador.spriteImportMode = SpriteImportMode.Single;
            importador.spritePixelsPerUnit = 100;
            importador.filterMode = FilterMode.Point;
            importador.mipmapEnabled = false;
            importador.textureCompression = TextureImporterCompression.Uncompressed;
            importador.isReadable = ruta.Contains("marcador");
            importador.SaveAndReimport();
        }
        fuente = AssetDatabase.LoadAssetAtPath<Font>("Assets/Fuentes/Silkscreen.ttf");
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        GameObject control = new GameObject("Reglas del juego");
        JuegoMemoria juego = control.AddComponent<JuegoMemoria>();
        MostrarTableroAR ar = control.AddComponent<MostrarTableroAR>();
        juego.realidadAumentada = ar;
        ar.juego = juego;
        juego.dibujos = new[] { Sprite("carta_calavera"), Sprite("carta_pocion"), Sprite("carta_espada"), Sprite("carta_pocion_azul") };
        juego.niveles = new[] {
            new NivelCartal { nombre = "La cripta", marcador = "Cartal", resultado = "CRIPTA ABIERTA", dibujos = juego.dibujos, fondo = Sprite("tablero"), premio = Sprite("puerta") },
            new NivelCartal { nombre = "La biblioteca", marcador = "Biblioteca", resultado = "SELLO DEL CONOCIMIENTO", dibujos = new[] { Sprite("carta_libro"), Sprite("carta_pergamino"), Sprite("carta_pluma"), Sprite("carta_runa"), Sprite("carta_vela"), Sprite("carta_reloj") }, fondo = Sprite("tablero_biblioteca"), premio = Sprite("premio_biblioteca") },
            new NivelCartal { nombre = "El tesoro", marcador = "Tesoro", resultado = "TESORO DESCUBIERTO", dibujos = new[] { Sprite("carta_llave"), Sprite("carta_moneda"), Sprite("carta_gema"), Sprite("carta_corona"), Sprite("carta_caliz"), Sprite("carta_anillo"), Sprite("carta_escudo"), Sprite("carta_amuleto") }, fondo = Sprite("tablero_tesoro"), premio = Sprite("premio_tesoro") }
        };
        juego.sonido = control.AddComponent<AudioSource>();
        juego.sonido.playOnAwake = false;
        juego.sonido.volume = 0.5f;
        juego.voltear = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sonidos/voltear.wav");
        juego.acierto = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sonidos/pareja.wav");
        juego.ganar = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Sonidos/victoria.wav");

        GameObject sessionObject = new GameObject("AR Session");
        ar.sesion = sessionObject.AddComponent<ARSession>();
        ar.sesion.attemptUpdate = true;
        sessionObject.AddComponent<ARInputManager>();
        ar.sesion.enabled = false;
        GameObject originObject = new GameObject("XR Origin");
        XROrigin origin = originObject.AddComponent<XROrigin>();
        origin.CameraYOffset = 0;
        GameObject offset = new GameObject("Camera Offset");
        offset.transform.SetParent(originObject.transform, false);
        origin.CameraFloorOffsetObject = offset;
        GameObject cameraObject = new GameObject("Main Camera");
        cameraObject.tag = "MainCamera";
        cameraObject.transform.SetParent(offset.transform, false);
        Camera camera = cameraObject.AddComponent<Camera>();
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = Fondo;
        camera.nearClipPlane = 0.01f;
        camera.farClipPlane = 30f;
        cameraObject.AddComponent<AudioListener>();
        origin.Camera = camera;
        ar.camara = camera;
        ARCameraManager manager = cameraObject.AddComponent<ARCameraManager>();
        manager.autoFocusRequested = true;
        ARCameraBackground background = cameraObject.AddComponent<ARCameraBackground>();
        // El componente de Unity aplica la posición del visor/celular a la cámara.
        TrackedPoseDriver pose = cameraObject.AddComponent<TrackedPoseDriver>();
        pose.SetPoseSource(TrackedPoseDriver.DeviceType.GenericXRDevice, TrackedPoseDriver.TrackedPose.Center);
        pose.UseRelativeTransform = false;
        bool android = EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android;
        manager.enabled = false;
        background.enabled = false;
        pose.enabled = false;
        if (!android)
        {
            camera.orthographic = true;
            camera.orthographicSize = 2.85f;
            cameraObject.transform.localPosition = new Vector3(0, 0, -8);
        }

        XRReferenceImageLibrary library = AssetDatabase.LoadAssetAtPath<XRReferenceImageLibrary>("Assets/Configuracion/ImagenCartal.asset");
        if (library == null)
        {
            library = ScriptableObject.CreateInstance<XRReferenceImageLibrary>();
            AssetDatabase.CreateAsset(library, "Assets/Configuracion/ImagenCartal.asset");
            library.Add();
        }
        Texture2D marcador = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Arte/marcador_cartal.png");
        library.SetName(0, "Cartal");
        library.SetTexture(0, marcador, true);
        library.SetSpecifySize(0, true);
        library.SetSize(0, new Vector2(0.20f, 0.20f));
        EditorUtility.SetDirty(library);
        while (library.count < 3) library.Add();
        for (int i = 1; i < 3; i++)
        {
            library.SetName(i, juego.niveles[i].marcador);
            library.SetTexture(i, AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Arte/marcador_" + (i == 1 ? "biblioteca" : "tesoro") + ".png"), true);
            library.SetSpecifySize(i, true);
            library.SetSize(i, new Vector2(0.20f, 0.20f));
        }
        ar.anclas = originObject.AddComponent<ARAnchorManager>();
        ar.anclas.enabled = false;
        ar.imagenes = originObject.AddComponent<ARTrackedImageManager>();
        ar.imagenes.referenceLibrary = library;
        ar.imagenes.enabled = false;

        GameObject tablero = new GameObject("Tablero de ocho cartas");
        ar.tablero = tablero;
        juego.fondoTablero = Dibujar("Piedras del tablero", Sprite("tablero"), tablero.transform, Vector3.zero, -10);
        Dibujar("Interior de la salida", Sprite("salida"), tablero.transform, new Vector3(0, 1.40f, -0.02f), -2).transform.localScale = Vector3.one * 1.35f;
        GameObject bisagra = new GameObject("Bisagra de la puerta");
        bisagra.transform.SetParent(tablero.transform, false);
        bisagra.transform.localPosition = new Vector3(-0.43f, 1.40f, -0.06f);
        juego.premio = Dibujar("Puerta", Sprite("puerta"), bisagra.transform, new Vector3(0.43f, 0, 0), -1);
        juego.premio.transform.localScale = Vector3.one * 1.35f;
        juego.puerta = bisagra.transform;
        juego.cartas = new Carta[8];
        for (int i = 0; i < 8; i++)
        {
            Vector3 posicion = new Vector3((i % 4 - 1.5f) * 0.90f, i < 4 ? 0.12f : -1.20f, -0.12f);
            SpriteRenderer renderer = Dibujar("Carta " + (i + 1), Sprite("reverso"), tablero.transform, posicion, 2);
            renderer.transform.localScale = Vector3.one * 1.10f;
            Carta carta = renderer.gameObject.AddComponent<Carta>();
            carta.dibujo = renderer;
            carta.reverso = Sprite("reverso");
            BoxCollider collider = renderer.gameObject.AddComponent<BoxCollider>();
            collider.size = new Vector3(0.64f, 0.88f, 0.08f);
            juego.cartas[i] = carta;
        }

        GameObject ui = new GameObject("Interfaz", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        Canvas canvas = ui.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;
        CanvasScaler scaler = ui.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1080, 1920);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 1;
        GameObject events = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));

        juego.hud = Panel("Indicaciones", ui.transform, new Vector2(0, 0), Vector2.one, Color.clear);
        Panel("Fondo superior", juego.hud.transform, new Vector2(0, 0.84f), Vector2.one, new Color(0.08f, 0.07f, 0.10f, 0.95f));
        juego.tituloNivel = Texto("Titulo pequeño", "CARTAL", juego.hud.transform, new Vector2(0.10f, 0.943f), new Vector2(0.9f, 0.987f), 38, Oro);
        juego.contador = Texto("Contador", "PAREJAS  0 / 4", juego.hud.transform, new Vector2(0.04f, 0.905f), new Vector2(0.47f, 0.94f), 27, Color.white);
        juego.reloj = Texto("Cronometro", "TIEMPO 00:00.00", juego.hud.transform, new Vector2(0.50f, 0.905f), new Vector2(0.96f, 0.94f), 27, Color.white);
        juego.recordHud = Texto("Record HUD", "MEJOR --:--.--", juego.hud.transform, new Vector2(0.05f, 0.875f), new Vector2(0.95f, 0.902f), 24, Oro);
        juego.mensaje = Texto("Instruccion", "APUNTA A LA IMAGEN DE LA CRIPTA", juego.hud.transform, new Vector2(0.04f, 0.843f), new Vector2(0.96f, 0.874f), 23, Oro);
        ar.fijar = Boton("FIJAR TABLERO", juego.hud.transform, new Vector2(0.04f, 0.10f), new Vector2(0.48f, 0.16f), ar.FijarTablero);
        ar.reubicar = Boton("REUBICAR", juego.hud.transform, new Vector2(0.52f, 0.10f), new Vector2(0.96f, 0.16f), ar.Reubicar);
        Boton("MENU", juego.hud.transform, new Vector2(0.32f, 0.025f), new Vector2(0.68f, 0.085f), juego.VolverAlMenu);

        juego.menu = Panel("Menu inicial", ui.transform, Vector2.zero, Vector2.one, Fondo);
        Texto("Subtitulo superior", "MEMORIA  /  REALIDAD AUMENTADA", juego.menu.transform, new Vector2(0.07f, 0.88f), new Vector2(0.93f, 0.94f), 22, Oro);
        Texto("Nombre", "CARTAL", juego.menu.transform, new Vector2(0.06f, 0.77f), new Vector2(0.94f, 0.89f), 112, new Color32(236, 218, 185, 255));
        Texto("Lema", "TRES LUGARES. TRES SELLOS.\nDESCUBRE LOS SECRETOS DE CARTAL.", juego.menu.transform, new Vector2(0.08f, 0.69f), new Vector2(0.92f, 0.77f), 29, Oro);
        GameObject ilustracion = Panel("Ilustracion", juego.menu.transform, new Vector2(0.23f, 0.40f), new Vector2(0.77f, 0.66f), Color.white);
        Image dibujo = ilustracion.GetComponent<Image>();
        dibujo.sprite = Sprite("marcador_cartal");
        dibujo.preserveAspect = true;
        juego.progreso = Texto("Progreso", "SELLOS RECUPERADOS 0 / 3", juego.menu.transform, new Vector2(0.04f, 0.35f), new Vector2(0.96f, 0.395f), 27, Oro);
        juego.recordMenu = Texto("Record menu", "", juego.menu.transform, new Vector2(0.04f, 0.325f), new Vector2(0.96f, 0.35f), 18, Color.white);
        Boton("JUGAR", juego.menu.transform, new Vector2(0.20f, 0.25f), new Vector2(0.8f, 0.32f), juego.Explorar);
        Boton("ACERCA DE", juego.menu.transform, new Vector2(0.20f, 0.16f), new Vector2(0.8f, 0.23f), juego.AbrirAcercaDe);
        Boton("SALIR", juego.menu.transform, new Vector2(0.20f, 0.07f), new Vector2(0.8f, 0.14f), juego.Salir);
        Texto("Ayuda", "EXPLORA / FIJA EL TABLERO / COMPLETA LOS SELLOS", juego.menu.transform, new Vector2(0.04f, 0.008f), new Vector2(0.96f, 0.058f), 23, new Color32(150, 143, 155, 255));

        juego.modos = Panel("Modo de juego", ui.transform, Vector2.zero, Vector2.one, Fondo);
        Texto("Titulo modos", "COMO QUIERES JUGAR", juego.modos.transform, new Vector2(0.05f, 0.78f), new Vector2(0.95f, 0.9f), 48, Oro);
        juego.estadoAR = Texto("Estado AR", "ELIGE COMO QUIERES JUGAR", juego.modos.transform, new Vector2(0.07f, 0.59f), new Vector2(0.93f, 0.74f), 28, Color.white);
        juego.botonAR = Boton("EXPLORAR CON CAMARA", juego.modos.transform, new Vector2(0.1f, 0.45f), new Vector2(0.9f, 0.54f), ar.ComprobarCompatibilidad);
        Boton("JUGAR SIN CAMARA", juego.modos.transform, new Vector2(0.1f, 0.31f), new Vector2(0.9f, 0.40f), juego.SinCamara);
        Texto("Ayuda modos", "EL MODO SIN CAMARA INCLUYE LOS TRES NIVELES.\nLA REALIDAD AUMENTADA REQUIERE UN CELULAR COMPATIBLE.", juego.modos.transform, new Vector2(0.07f, 0.15f), new Vector2(0.93f, 0.27f), 24, Oro);
        Boton("VOLVER", juego.modos.transform, new Vector2(0.2f, 0.04f), new Vector2(0.8f, 0.11f), juego.VolverAlMenu);
        juego.modos.SetActive(false);

        juego.selector = Panel("Elegir nivel", ui.transform, Vector2.zero, Vector2.one, Fondo);
        Texto("Titulo niveles", "LOS TRES SELLOS", juego.selector.transform, new Vector2(0.05f, 0.88f), new Vector2(0.95f, 0.97f), 52, Oro);
        Texto("Ayuda niveles", "ELIGE UN LUGAR PARA JUGAR", juego.selector.transform, new Vector2(0.05f, 0.82f), new Vector2(0.95f, 0.88f), 25, Color.white);
        juego.resumenNiveles = new Text[3];
        UnityEngine.Events.UnityAction[] acciones = { juego.Cripta, juego.Biblioteca, juego.Tesoro };
        for (int i = 0; i < 3; i++)
        {
            float y = 0.59f - i * 0.22f;
            GameObject tarjeta = Panel("Nivel " + (i + 1), juego.selector.transform, new Vector2(0.07f, y), new Vector2(0.93f, y + 0.19f), new Color32(40, 38, 50, 255));
            Image imagen = Panel("Imagen del nivel", tarjeta.transform, new Vector2(0.025f, 0.08f), new Vector2(0.28f, 0.92f), Color.white).GetComponent<Image>();
            imagen.sprite = Sprite("marcador_" + (i == 0 ? "cartal" : i == 1 ? "biblioteca" : "tesoro"));
            imagen.preserveAspect = true;
            Boton(juego.niveles[i].nombre.ToUpperInvariant(), tarjeta.transform, new Vector2(0.31f, 0.40f), new Vector2(0.96f, 0.89f), acciones[i]);
            juego.resumenNiveles[i] = Texto("Estado", "", tarjeta.transform, new Vector2(0.29f, 0.06f), new Vector2(0.98f, 0.35f), 20, Oro);
        }
        Boton("VOLVER", juego.selector.transform, new Vector2(0.2f, 0.045f), new Vector2(0.8f, 0.11f), juego.VolverAlMenu);
        juego.selector.SetActive(false);

        juego.acercaDe = Panel("Acerca de", ui.transform, Vector2.zero, Vector2.one, Fondo);
        Texto("Titulo acerca", "ACERCA DE CARTAL", juego.acercaDe.transform, new Vector2(0.05f, 0.89f), new Vector2(0.95f, 0.96f), 50, Oro);
        TextAsset equipo = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Resources/Equipo.txt");
        if (equipo == null) throw new Exception("Faltan los datos de Assets/Resources/Equipo.txt");
        Text integrantes = Texto("Equipo", equipo.text, juego.acercaDe.transform, new Vector2(0.10f, 0.20f), new Vector2(0.90f, 0.86f), 27, Color.white);
        integrantes.alignment = TextAnchor.UpperLeft;
        Boton("VOLVER", juego.acercaDe.transform, new Vector2(0.20f, 0.075f), new Vector2(0.8f, 0.145f), juego.VolverAlMenu);
        juego.acercaDe.SetActive(false);

        juego.victoria = Panel("Victoria", ui.transform, new Vector2(0.03f, 0.18f), new Vector2(0.97f, 0.53f), new Color(0.08f, 0.07f, 0.10f, 0.98f));
        juego.tituloVictoria = Texto("Resultado", "CRIPTA ABIERTA", juego.victoria.transform, new Vector2(0.04f, 0.75f), new Vector2(0.96f, 0.98f), 32, Oro);
        juego.resumenVictoria = Texto("Tiempo final", "TIEMPO 00:00.00", juego.victoria.transform, new Vector2(0.04f, 0.49f), new Vector2(0.96f, 0.75f), 27, Color.white);
        Boton("VOLVER A JUGAR", juego.victoria.transform, new Vector2(0.07f, 0.28f), new Vector2(0.93f, 0.46f), juego.Jugar);
        Boton("BUSCAR OTRO SELLO", juego.victoria.transform, new Vector2(0.07f, 0.05f), new Vector2(0.93f, 0.23f), juego.Explorar);
        juego.victoria.SetActive(false);
        juego.hud.SetActive(false);
        tablero.SetActive(false);

        PlayerSettings.companyName = "PGM611";
        PlayerSettings.productName = "Cartal";
        PlayerSettings.bundleVersion = "0.3.0";
        PlayerSettings.Android.bundleVersionCode = 4;
        PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.Android, "com.pgm611.cartal");
        PlayerSettings.defaultScreenWidth = 1280;
        PlayerSettings.defaultScreenHeight = 900;
        PlayerSettings.fullScreenMode = FullScreenMode.Windowed;
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.Portrait;
        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel25;
        PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto;
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;
        PlayerSettings.SetScriptingBackend(NamedBuildTarget.Android, ScriptingImplementation.IL2CPP);
        PlayerSettings.SetManagedStrippingLevel(NamedBuildTarget.Android, ManagedStrippingLevel.Low);
        PlayerSettings.stripEngineCode = true;
        PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.Android, false);
        PlayerSettings.SetGraphicsAPIs(BuildTarget.Android, new[] { UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3 });
        SerializedObject settings = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/ProjectSettings.asset")[0]);
        settings.FindProperty("activeInputHandler").intValue = 0;
        settings.ApplyModifiedPropertiesWithoutUndo();
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), Escena);
        EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(Escena, true) };
        ConfigurarXR();
        AssetDatabase.SaveAssets();
        Debug.Log("CARTAL_ESCENA_PREPARADA Unity=" + Application.unityVersion);
    }

    private static void ConfigurarXR()
    {
        ARCoreSettings proveedor = ARCoreSettings.currentSettings;
        if (proveedor == null)
        {
            proveedor = ScriptableObject.CreateInstance<ARCoreSettings>();
            AssetDatabase.CreateAsset(proveedor, "Assets/Configuracion/ARCore.asset");
            ARCoreSettings.currentSettings = proveedor;
        }
        proveedor.requirement = ARCoreSettings.Requirement.Optional;
        proveedor.depth = ARCoreSettings.Requirement.Optional;
        EditorUtility.SetDirty(proveedor);
        XRGeneralSettingsPerBuildTarget perTarget = AssetDatabase.LoadAssetAtPath<XRGeneralSettingsPerBuildTarget>("Assets/Configuracion/XR.asset");
        if (perTarget == null)
        {
            perTarget = ScriptableObject.CreateInstance<XRGeneralSettingsPerBuildTarget>();
            AssetDatabase.CreateAsset(perTarget, "Assets/Configuracion/XR.asset");
            XRGeneralSettings general = ScriptableObject.CreateInstance<XRGeneralSettings>();
            general.name = "Android XR";
            XRManagerSettings manager = ScriptableObject.CreateInstance<XRManagerSettings>();
            manager.name = "Android XR Manager";
            manager.automaticLoading = true;
            manager.automaticRunning = true;
            general.Manager = manager;
            general.InitManagerOnStart = true;
            AssetDatabase.AddObjectToAsset(general, perTarget);
            AssetDatabase.AddObjectToAsset(manager, perTarget);
            perTarget.SetSettingsForBuildTarget(BuildTargetGroup.Android, general);
        }
        EditorBuildSettings.AddConfigObject(XRGeneralSettings.k_SettingsKey, perTarget, true);
        XRGeneralSettings android = perTarget.SettingsForBuildTarget(BuildTargetGroup.Android);
        if (!XRPackageMetadataStore.AssignLoader(android.Manager, "UnityEngine.XR.ARCore.ARCoreLoader", BuildTargetGroup.Android))
        {
            throw new Exception("No se pudo configurar ARCore");
        }
        EditorUtility.SetDirty(perTarget);
        EditorUtility.SetDirty(android);
        EditorUtility.SetDirty(android.Manager);
    }

    [MenuItem("Cartal/Compilar Windows")]
    public static void Windows()
    {
        Preparar();
        Compilar(BuildTarget.StandaloneWindows64, "../Entrega/Windows/Cartal.exe");
    }

    [MenuItem("Cartal/Compilar Android")]
    public static void Android()
    {
#if UNITY_ANDROID
        // El lanzador puede indicar herramientas en D: para evitar rutas demasiado largas.
        string herramientas = Environment.GetEnvironmentVariable("CARTAL_ANDROID_TOOLS");
        if (!string.IsNullOrEmpty(herramientas))
        {
            // Estas rutas solo configuran la compilación; no se utilizan durante el juego.
            UnityEditor.Android.AndroidExternalToolsSettings.jdkRootPath = Path.Combine(herramientas, "OpenJDK");
            UnityEditor.Android.AndroidExternalToolsSettings.ndkRootPath = Path.Combine(herramientas, "android-ndk-r27c");
            UnityEditor.Android.AndroidExternalToolsSettings.sdkRootPath = Environment.GetEnvironmentVariable("ANDROID_HOME");
        }
#endif
        EditorUserBuildSettings.buildAppBundle = false;
        EditorUserBuildSettings.development = false;
        EditorUserBuildSettings.allowDebugging = false;
        Preparar();
        Compilar(BuildTarget.Android, "../Entrega/Cartal.apk");
    }

    private static void Compilar(BuildTarget target, string path)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        BuildReport report = BuildPipeline.BuildPlayer(new BuildPlayerOptions { scenes = new[] { Escena }, locationPathName = path, target = target, options = target == BuildTarget.Android ? BuildOptions.CompressWithLz4HC : BuildOptions.None });
        if (report.summary.result != BuildResult.Succeeded)
        {
            throw new Exception("Falló la compilación: " + report.summary.result);
        }
        Debug.Log("CARTAL_BUILD_OK " + target + " Unity=" + Application.unityVersion + " bytes=" + report.summary.totalSize);
    }

    private static Sprite Sprite(string nombre) => AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Arte/" + nombre + ".png");

    private static SpriteRenderer Dibujar(string nombre, Sprite sprite, Transform padre, Vector3 posicion, int orden)
    {
        GameObject objeto = new GameObject(nombre);
        objeto.transform.SetParent(padre, false);
        objeto.transform.localPosition = posicion;
        SpriteRenderer renderer = objeto.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        renderer.sortingOrder = orden;
        return renderer;
    }

    private static GameObject Panel(string nombre, Transform padre, Vector2 minimo, Vector2 maximo, Color color)
    {
        GameObject objeto = new GameObject(nombre, typeof(RectTransform), typeof(Image));
        objeto.transform.SetParent(padre, false);
        Ajustar(objeto.GetComponent<RectTransform>(), minimo, maximo);
        objeto.GetComponent<Image>().color = color;
        objeto.GetComponent<Image>().raycastTarget = color.a > 0;
        return objeto;
    }

    private static void Ajustar(RectTransform rect, Vector2 minimo, Vector2 maximo)
    {
        rect.anchorMin = minimo;
        rect.anchorMax = maximo;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }

    private static Text Texto(string nombre, string texto, Transform padre, Vector2 minimo, Vector2 maximo, int tamaño, Color color)
    {
        GameObject objeto = new GameObject(nombre, typeof(RectTransform), typeof(Text));
        objeto.transform.SetParent(padre, false);
        Ajustar(objeto.GetComponent<RectTransform>(), minimo, maximo);
        Text label = objeto.GetComponent<Text>();
        label.text = texto;
        label.font = fuente;
        label.fontSize = tamaño;
        label.alignment = TextAnchor.MiddleCenter;
        label.color = color;
        label.raycastTarget = false;
        label.horizontalOverflow = HorizontalWrapMode.Wrap;
        label.verticalOverflow = VerticalWrapMode.Overflow;
        return label;
    }

    private static Button Boton(string titulo, Transform padre, Vector2 minimo, Vector2 maximo, UnityEngine.Events.UnityAction accion)
    {
        GameObject objeto = Panel(titulo, padre, minimo, maximo, new Color32(59, 49, 48, 255));
        Outline borde = objeto.AddComponent<Outline>();
        borde.effectColor = Oro;
        borde.effectDistance = new Vector2(2, -2);
        Button button = objeto.AddComponent<Button>();
        ColorBlock colors = button.colors;
        colors.highlightedColor = new Color32(183, 150, 114, 255);
        colors.pressedColor = new Color32(104, 137, 115, 255);
        button.colors = colors;
        UnityEditor.Events.UnityEventTools.AddPersistentListener(button.onClick, accion);
        Texto("Texto", titulo, objeto.transform, new Vector2(0.025f, 0.05f), new Vector2(0.975f, 0.95f), 28, new Color32(236, 218, 185, 255));
        return button;
    }
}
