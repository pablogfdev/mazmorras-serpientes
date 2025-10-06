using TMPro;
using UnityEngine;
using UnityEngine.UI;
using EC = EscenasController;
using PM = PrefabManager;


public class MenuController : MonoBehaviour
{
    private GameObject ventanaPartidasGuardadas;
    private GameObject ventanaNuevaPartida;
    private GameObject ventanaControles;
    private GameObject ventanaPrincipal;
    private GameObject ventanaAjustes;

    private Button botonVolverPartidasGuardadas;
    private Button botonVolverNuevaPartida;
    private Button botonPartidasGuardadas;
    private Button botonVolverControles;
    private Button botonVolverAjustes;
    private Button botonNuevaPartida;
    private Button botonCrearPartida;
    private Button botonControles;
    private Button botonAjustes;
    private Button botonSalir;

    private GameObject botonPlantillaPartida;
    private Transform contentScrollPartidas;
    

    void Awake()
    {
        InstanciarMenus();
        InstanciarBotones();
        AsignarEventos();
        ConfigurarScrollPartidas();
    }

    void Start() => ActivarVentana(ventanaPrincipal);

    void InstanciarMenus()
    {
        ventanaPartidasGuardadas = Instantiate(PM.prefabManager.ObtenerPrefab("VentanaPartidasGuardadas"));
        ventanaNuevaPartida = Instantiate(PM.prefabManager.ObtenerPrefab("VentanaNuevaPartida"));
        ventanaControles = Instantiate(PM.prefabManager.ObtenerPrefab("VentanaControles"));
        ventanaPrincipal = Instantiate(PM.prefabManager.ObtenerPrefab("VentanaPrincipal"));
        ventanaAjustes = Instantiate(PM.prefabManager.ObtenerPrefab("VentanaAjustes"));
    }

    void InstanciarBotones()
    {
        botonVolverPartidasGuardadas = ventanaPartidasGuardadas.transform.Find("BotonVolverPartidasGuardadas").GetComponent<Button>();
        botonVolverNuevaPartida = ventanaNuevaPartida.transform.Find("BotonVolverNuevaPartida").GetComponent<Button>();
        botonPartidasGuardadas = ventanaPrincipal.transform.Find("BotonPartidasGuardadas").GetComponent<Button>();
        botonVolverControles = ventanaControles.transform.Find("BotonVolverControles").GetComponent<Button>();
        botonCrearPartida = ventanaNuevaPartida.transform.Find("BotonCrearPartida").GetComponent<Button>();
        botonVolverAjustes = ventanaAjustes.transform.Find("BotonVolverAjustes").GetComponent<Button>();
        botonNuevaPartida = ventanaPrincipal.transform.Find("BotonNuevaPartida").GetComponent<Button>();
        botonControles = ventanaAjustes.transform.Find("BotonControles").GetComponent<Button>();
        botonAjustes = ventanaPrincipal.transform.Find("BotonAjustes").GetComponent<Button>();
        botonSalir = ventanaPrincipal.transform.Find("BotonSalir").GetComponent<Button>();
    }
    void AsignarEventos()
    {
        botonVolverPartidasGuardadas.onClick.AddListener(() => ActivarVentana(ventanaPrincipal));
        botonVolverNuevaPartida.onClick.AddListener(() => ActivarVentana(ventanaPrincipal));
        botonNuevaPartida.onClick.AddListener(() => ActivarVentana(ventanaNuevaPartida));
        botonVolverAjustes.onClick.AddListener(() => ActivarVentana(ventanaPrincipal));
        botonVolverControles.onClick.AddListener(() => ActivarVentana(ventanaAjustes));
        botonControles.onClick.AddListener(() => ActivarVentana(ventanaControles));
        botonAjustes.onClick.AddListener(() => ActivarVentana(ventanaAjustes));
        botonPartidasGuardadas.onClick.AddListener(MostrarPartidasGuardadas);
        botonCrearPartida.onClick.AddListener(CrearPartida);
        botonSalir.onClick.AddListener(Application.Quit);
    }

    void ConfigurarScrollPartidas()
    {
        contentScrollPartidas = ventanaPartidasGuardadas.transform.Find("ScrollPartidas/Viewport/Content");
        botonPlantillaPartida = contentScrollPartidas.Find("BotonPlantillaPartida").gameObject;
        botonPlantillaPartida.SetActive(false);
        ScrollRect scrollLista = ventanaPartidasGuardadas.transform.Find("ScrollPartidas").GetComponent<ScrollRect>();
        scrollLista.scrollSensitivity = Mathf.Clamp(GestorPartidas.ListasPartidas().Count , 10f, 100f);
    }

    private void ActivarVentana(GameObject ventana)
    {
        ventanaPartidasGuardadas.SetActive(false);
        ventanaNuevaPartida.SetActive(false);
        ventanaPrincipal.SetActive(false);
        ventanaControles.SetActive(false);
        ventanaAjustes.SetActive(false);
        ventana.SetActive(true);
    }

    public void MostrarPartidasGuardadas()
    {
        ActivarVentana(ventanaPartidasGuardadas);
        LimpiarBotonesAnteriores();
        GenerarBotonesPartidasGuardadas();
    }

    void LimpiarBotonesAnteriores()
    {
        foreach (Transform child in contentScrollPartidas)
        {
            if (child.gameObject != botonPlantillaPartida) Destroy(child.gameObject);
        }
    }

    void GenerarBotonesPartidasGuardadas()
    {
        foreach (Partida partida in GestorPartidas.ListasPartidas())
        {
            GameObject objetoBotonPartida = Instantiate(botonPlantillaPartida, contentScrollPartidas);
            objetoBotonPartida.SetActive(true);

            objetoBotonPartida.GetComponentInChildren<TMP_Text>().text = $"{partida.nombre} (Nivel {partida.nivel})";

            Button botonPartida = objetoBotonPartida.GetComponent<Button>();
            botonPartida.onClick.RemoveAllListeners();
            botonPartida.onClick.AddListener(() => CargarPartida(partida.id));
        }
    }

    private void CargarPartida(string id)
    {
        GestorPartidas.CargarPartida(id);
        EC.escenasController.CargarEscenaComerciante();
    }

    void CrearPartida()
    {
        GestorPartidas.CrearNuevaPartida();
        EC.escenasController.CargarEscenaComerciante();
    }
}