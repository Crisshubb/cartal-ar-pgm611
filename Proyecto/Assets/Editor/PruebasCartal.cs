// Pruebas del editor. Este archivo no se incluye en el APK ni añade scripts al juego.
using System;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;

public static class PruebasCartal
{
    private static JuegoMemoria juego;
    private static int paso;
    private static double siguiente;
    private static double limite;
    private static Carta fallidaA;
    private static Carta fallidaB;
    private static Carta tercera;
    private const string ClaveRecord = "Cartal.MejorTiempo.4Parejas";
    private static bool habiaRecord;
    private static float recordAnterior;
    private static float tiempoPausado;

    public static void Ejecutar()
    {
        PrepararCartal.Preparar();
        habiaRecord = PlayerPrefs.HasKey(ClaveRecord);
        recordAnterior = PlayerPrefs.GetFloat(ClaveRecord);
        PlayerPrefs.DeleteKey(ClaveRecord);
        EditorSceneManager.OpenScene("Assets/Escenas/Cartal.unity");
        EditorSettings.enterPlayModeOptionsEnabled = true;
        EditorSettings.enterPlayModeOptions = EnterPlayModeOptions.DisableDomainReload;
        paso = 0;
        limite = EditorApplication.timeSinceStartup + 60;
        siguiente = EditorApplication.timeSinceStartup + 1;
        EditorApplication.update += Avanzar;
        EditorApplication.EnterPlaymode();
    }

    private static void Avanzar()
    {
        try
        {
            if (EditorApplication.timeSinceStartup > limite) throw new Exception("Tiempo agotado en las pruebas");
            if (!EditorApplication.isPlaying || EditorApplication.timeSinceStartup < siguiente) return;
            switch (paso)
            {
                case 0:
                    juego = UnityEngine.Object.FindFirstObjectByType<JuegoMemoria>();
                    Exigir(juego != null && juego.menu.activeSelf && !juego.jugando, "menú inicial");
                    Exigir(juego.mejorTiempo == 0 && juego.recordMenu.text.Contains("--:--"), "primer inicio sin récord");
                    juego.menu.GetComponentsInChildren<Button>().First(b => b.name == "ACERCA DE").onClick.Invoke();
                    Exigir(juego.acercaDe.activeSelf && !juego.menu.activeSelf, "botón Acerca de");
                    Exigir(juego.acercaDe.GetComponentsInChildren<Text>().Any(t => t.text.Contains("Alex Joel Quispe Ticona")), "cuatro integrantes en Acerca de");
                    Captura("Cartal-AcercaDe.png");
                    juego.acercaDe.GetComponentInChildren<Button>().onClick.Invoke();
                    Exigir(juego.menu.activeSelf && !juego.acercaDe.activeSelf, "volver desde Acerca de");
                    Captura("Cartal-Menu.png");
                    juego.menu.GetComponentsInChildren<Button>().First(b => b.name == "JUGAR").onClick.Invoke();
                    Exigir(juego.jugando && !juego.menu.activeSelf, "botón Jugar");
                    Exigir(juego.cartas.Length == 8 && juego.cartas.GroupBy(c => c.pareja).Count() == 4 && juego.cartas.GroupBy(c => c.pareja).All(g => g.Count() == 2), "ocho cartas y cuatro parejas");
                    Exigir(juego.cartas.Count(c => c.frente.name == "carta_pocion_azul") == 2, "pareja de pociones azules");
                    Captura("Cartal-Tablero.png");
                    fallidaA = juego.cartas[0];
                    fallidaB = juego.cartas.First(c => c.pareja != fallidaA.pareja);
                    tercera = juego.cartas.First(c => c != fallidaA && c != fallidaB);
                    Tocar(fallidaA);
                    Tocar(fallidaA);
                    Exigir(!juego.resolviendo && juego.cartas.Count(c => c.revelada) == 1, "ignorar doble selección de la misma carta");
                    Tocar(fallidaB);
                    Tocar(tercera);
                    Exigir(juego.resolviendo && !tercera.revelada, "bloqueo de tercera carta durante comparación");
                    siguiente = EditorApplication.timeSinceStartup + 1.3;
                    paso++;
                    break;
                case 1:
                    Exigir(!fallidaA.revelada && !fallidaB.revelada && !juego.resolviendo && juego.parejasEncontradas == 0, "ocultar fallo tras un segundo");
                    juego.realidadAumentada.tablero.SetActive(false);
                    tiempoPausado = juego.tiempoPartida;
                    juego.Seleccionar(fallidaA);
                    Exigir(!fallidaA.revelada, "no seleccionar con tablero oculto");
                    siguiente = EditorApplication.timeSinceStartup + 0.6;
                    paso++;
                    break;
                case 2:
                    Exigir(Mathf.Abs(juego.tiempoPartida - tiempoPausado) < 0.01f, "cronómetro pausado al perder referencia");
                    juego.realidadAumentada.tablero.SetActive(true);
                    juego.SendMessage("OnApplicationPause", true);
                    siguiente = EditorApplication.timeSinceStartup + 0.6;
                    paso++;
                    break;
                case 3:
                    Exigir(Mathf.Abs(juego.tiempoPartida - tiempoPausado) < 0.01f, "cronómetro pausado con aplicación al fondo");
                    juego.SendMessage("OnApplicationPause", false);
                    siguiente = EditorApplication.timeSinceStartup + 0.6;
                    paso++;
                    break;
                case 4:
                    Exigir(juego.tiempoPartida > tiempoPausado + 0.2f, "cronómetro retoma al recuperar referencia");
                    Exigir(!PlayerPrefs.HasKey(ClaveRecord), "un intento incompleto no guarda récord");
                    juego.tiempoPartida = 12.34f;
                    foreach (var grupo in juego.cartas.GroupBy(c => c.pareja))
                    {
                        Carta[] pareja = grupo.ToArray();
                        Tocar(pareja[0]);
                        Tocar(pareja[1]);
                        Tocar(pareja[0]);
                        Exigir(pareja.All(c => c.encontrada && c.revelada), "pareja permanece descubierta");
                    }
                    Exigir(juego.parejasEncontradas == 4 && !juego.jugando, "victoria al completar cuatro parejas");
                    Exigir(Mathf.Abs(PlayerPrefs.GetFloat(ClaveRecord) - 12.34f) < 0.001f, "primera victoria guarda tiempo");
                    siguiente = EditorApplication.timeSinceStartup + 1.4;
                    paso++;
                    break;
                case 5:
                    Exigir(juego.victoria.activeSelf && Quaternion.Angle(Quaternion.identity, juego.puerta.localRotation) > 90, "abrir puerta y mostrar victoria");
                    Captura("Cartal-Victoria.png");
                    juego.victoria.GetComponentInChildren<Button>().onClick.Invoke();
                    Exigir(juego.jugando && juego.parejasEncontradas == 0 && juego.cartas.All(c => !c.encontrada && !c.revelada), "volver a jugar reinicia cartas");
                    Exigir(Quaternion.Angle(Quaternion.identity, juego.puerta.localRotation) < 0.1f, "volver a jugar cierra puerta");
                    Exigir(juego.tiempoPartida == 0 && juego.mejorTiempo > 0, "reinicio limpia cronómetro y conserva récord");
                    juego.tiempoPartida = 8.25f;
                    CompletarParejas();
                    Exigir(Mathf.Abs(PlayerPrefs.GetFloat(ClaveRecord) - 8.25f) < 0.001f && juego.resumenVictoria.text.Contains("NUEVO RECORD"), "partida más rápida mejora récord");
                    siguiente = EditorApplication.timeSinceStartup + 1.3;
                    paso++;
                    break;
                case 6:
                    juego.Jugar();
                    juego.tiempoPartida = 30f;
                    CompletarParejas();
                    Exigir(Mathf.Abs(PlayerPrefs.GetFloat(ClaveRecord) - 8.25f) < 0.001f && !juego.resumenVictoria.text.Contains("NUEVO RECORD"), "partida más lenta conserva récord");
                    siguiente = EditorApplication.timeSinceStartup + 1.3;
                    paso++;
                    break;
                case 7:
                    juego.Jugar();
                    Carta uno = juego.cartas[0];
                    Carta dos = juego.cartas.First(c => c.pareja != uno.pareja);
                    Tocar(uno);
                    Tocar(dos);
                    juego.VolverAlMenu();
                    juego.Jugar();
                    siguiente = EditorApplication.timeSinceStartup + 1.3;
                    paso++;
                    break;
                case 8:
                    Exigir(juego.jugando && juego.cartas.All(c => !c.revelada) && !juego.resolviendo, "cancelar espera al regresar al menú");
                    juego.VolverAlMenu();
                    Exigir(juego.menu.activeSelf && !juego.realidadAumentada.tablero.activeSelf, "regresar al menú");
                    Debug.Log("CARTAL_PRUEBAS_OK Unity=" + Application.unityVersion);
                    RestaurarRecord();
                    EditorApplication.update -= Avanzar;
                    EditorApplication.ExitPlaymode();
                    EditorApplication.Exit(0);
                    break;
            }
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
            Debug.LogError("CARTAL_PRUEBAS_FALLARON");
            RestaurarRecord();
            EditorApplication.update -= Avanzar;
            EditorApplication.Exit(1);
        }
    }

    private static void CompletarParejas()
    {
        foreach (var grupo in juego.cartas.GroupBy(c => c.pareja))
        {
            Carta[] pareja = grupo.ToArray();
            Tocar(pareja[0]);
            Tocar(pareja[1]);
        }
    }

    private static void RestaurarRecord()
    {
        if (habiaRecord) PlayerPrefs.SetFloat(ClaveRecord, recordAnterior);
        else PlayerPrefs.DeleteKey(ClaveRecord);
        PlayerPrefs.Save();
    }

    private static void Tocar(Carta carta)
    {
        Vector3 pantalla = juego.realidadAumentada.camara.WorldToScreenPoint(carta.transform.position);
        juego.realidadAumentada.SeleccionarEn(pantalla);
    }

    private static void Exigir(bool correcto, string caso)
    {
        if (!correcto) throw new Exception("Falló: " + caso);
        Debug.Log("CARTAL_CHECK_OK " + caso);
    }

    private static void Captura(string nombre)
    {
        Camera camara = juego.realidadAumentada.camara;
        Canvas canvas = UnityEngine.Object.FindFirstObjectByType<Canvas>();
        RenderTexture rt = new RenderTexture(1280, 900, 24);
        float aspecto = camara.aspect;
        camara.targetTexture = rt;
        camara.aspect = 1280f / 900f;
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.sortingOrder = 100;
        canvas.worldCamera = camara;
        canvas.planeDistance = 1;
        Canvas.ForceUpdateCanvases();
        camara.Render();
        RenderTexture.active = rt;
        Texture2D captura = new Texture2D(1280, 900, TextureFormat.RGB24, false);
        captura.ReadPixels(new Rect(0, 0, 1280, 900), 0, 0);
        captura.Apply();
        File.WriteAllBytes("../Entrega/" + nombre, captura.EncodeToPNG());
        RenderTexture.active = null;
        camara.targetTexture = null;
        camara.aspect = aspecto;
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        UnityEngine.Object.DestroyImmediate(rt);
        UnityEngine.Object.DestroyImmediate(captura);
    }
}
