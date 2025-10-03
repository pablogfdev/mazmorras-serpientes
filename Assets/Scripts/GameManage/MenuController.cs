using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    private GameObject ventanaPrincipal;
    private GameObject ventanaAjustes;
    private GameObject ventanaControles;
    private GameObject ventanaNuevaPartida;

    private Button botonVolverAjustes;
    private Button botonVolverControles;
    private Button botonVolverNuevaPartida;
    private Button botonSalir;
    private Button botonAjustes;
    private Button botonControles;
    private Button botonNuevaPartida;
    private Button botonCrearPartida;

    void Awake()
    {
        InstanciarMenus();
        InstanciarBotones();
        AsignarEventos();
    }

    void Start() => ActivarMenuPrincipal();

    void InstanciarMenus(){
        ventanaPrincipal = Instantiate(Resources.Load<GameObject>("Prefabs/Menus/VentanaPrincipal"));
        ventanaAjustes = Instantiate(Resources.Load<GameObject>("Prefabs/Menus/VentanaAjustes"));
        ventanaControles = Instantiate(Resources.Load<GameObject>("Prefabs/Menus/VentanaControles"));
        ventanaNuevaPartida = Instantiate(Resources.Load<GameObject>("Prefabs/Menus/VentanaNuevaPartida"));
    }

    void InstanciarBotones()
    {
        botonVolverAjustes = ventanaAjustes.transform.Find("BotonVolverAjustes").GetComponent<Button>();
        botonVolverControles = ventanaControles.transform.Find("BotonVolverControles").GetComponent<Button>();
        botonVolverNuevaPartida = ventanaNuevaPartida.transform.Find("BotonVolverNuevaPartida").GetComponent<Button>();
        botonSalir = ventanaPrincipal.transform.Find("BotonSalir").GetComponent<Button>();
        botonNuevaPartida = ventanaPrincipal.transform.Find("BotonNuevaPartida").GetComponent<Button>();
        botonAjustes = ventanaPrincipal.transform.Find("BotonAjustes").GetComponent<Button>();
        botonControles = ventanaAjustes.transform.Find("BotonControles").GetComponent<Button>();
        botonCrearPartida = ventanaNuevaPartida.transform.Find("BotonCrearPartida").GetComponent<Button>();
    }
    void AsignarEventos()
    {
        botonVolverNuevaPartida.onClick.AddListener(ActivarMenuPrincipal);
        botonVolverAjustes.onClick.AddListener(ActivarMenuPrincipal);
        botonVolverControles.onClick.AddListener(ActivarMenuAjustes);
        botonNuevaPartida.onClick.AddListener(ActivarMenuNuevaPartida);
        botonSalir.onClick.AddListener(Application.Quit);
        botonAjustes.onClick.AddListener(ActivarMenuAjustes);
        botonControles.onClick.AddListener(ActivarMenuControles);
        botonCrearPartida.onClick.AddListener(GameObject.FindWithTag("JuegoController").GetComponent<EscenasController>().CargarEscenaComerciante);
    }
    
    public void ActivarMenuPrincipal()
    {
        ventanaAjustes.SetActive(false);
        ventanaControles.SetActive(false);
        ventanaNuevaPartida.SetActive(false);
        ventanaPrincipal.SetActive(true);
    }

    void ActivarMenuAjustes()
    {
        ventanaPrincipal.SetActive(false);
        ventanaControles.SetActive(false);
        ventanaNuevaPartida.SetActive(false);
        ventanaAjustes.SetActive(true);
    }

    void ActivarMenuControles()
    {
        ventanaPrincipal.SetActive(false);
        ventanaAjustes.SetActive(false);
        ventanaNuevaPartida.SetActive(false);
        ventanaControles.SetActive(true);
    }

    void ActivarMenuNuevaPartida()
    {
        ventanaPrincipal.SetActive(false);
        ventanaAjustes.SetActive(false);
        ventanaControles.SetActive(false);
        ventanaNuevaPartida.SetActive(true);
    }
}