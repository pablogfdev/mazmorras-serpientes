using System.Collections;
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
    private GameObject ventanaEditarPartida;
    private GameObject ventanaEliminarPartida;

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

    private Button botonCancelarEdicion;

    private Button botonConfirmarEdicion;

    private Button botonCancelarBorrado;

    private Button botonConfirmarBorrado;

    private GameObject botonPlantillaPartida;
    private Transform contentScrollPartidas;
    private TMP_InputField campoNombreNuevaPartida;
    private TMP_InputField campoNombreEditarPartida;

    private TMP_Dropdown dropdownDificultad;

    private GameObject ventanaMensaje;
    private TMP_Text textoMensaje;


    void Awake()
    {
        InstanciarMenus();
        InstanciarBotones();
        InstanciarElemenosVentanas();
        AsignarEventos();
        ConfigurarScrollPartidas();
    }

    void Start() => ActivarVentana(ventanaPrincipal);

    void InstanciarMenus()
    {
        ventanaPartidasGuardadas = Instantiate(PM.prefabManager.ObtenerPrefab("VentanaPartidasGuardadas"));
        ventanaEliminarPartida = Instantiate(PM.prefabManager.ObtenerPrefab("VentanaEliminarPartida"));
        ventanaEditarPartida = Instantiate(PM.prefabManager.ObtenerPrefab("VentanaEditarPartida"));
        ventanaNuevaPartida = Instantiate(PM.prefabManager.ObtenerPrefab("VentanaNuevaPartida"));
        ventanaControles = Instantiate(PM.prefabManager.ObtenerPrefab("VentanaControles"));
        ventanaPrincipal = Instantiate(PM.prefabManager.ObtenerPrefab("VentanaPrincipal"));
        ventanaAjustes = Instantiate(PM.prefabManager.ObtenerPrefab("VentanaAjustes"));
        ventanaMensaje = Instantiate(PM.prefabManager.ObtenerPrefab("VentanaMensaje"));
    }

    void InstanciarElemenosVentanas()
    {
        campoNombreNuevaPartida = ventanaNuevaPartida.transform.Find("CampoNombre").GetComponent<TMP_InputField>();
        dropdownDificultad = ventanaNuevaPartida.transform.Find("DropdownDificultad").GetComponent<TMP_Dropdown>();
        campoNombreEditarPartida = ventanaEditarPartida.transform.Find("CampoNombre").GetComponent<TMP_InputField>();
        textoMensaje = ventanaMensaje.transform.Find("Panel/TextoMensaje").GetComponent<TMP_Text>();
    }

    void InstanciarBotones()
    {
        botonVolverPartidasGuardadas = ventanaPartidasGuardadas.transform.Find("BotonVolverPartidasGuardadas").GetComponent<Button>();
        botonVolverNuevaPartida = ventanaNuevaPartida.transform.Find("BotonVolverNuevaPartida").GetComponent<Button>();
        botonPartidasGuardadas = ventanaPrincipal.transform.Find("BotonPartidasGuardadas").GetComponent<Button>();
        botonConfirmarBorrado = ventanaEliminarPartida.transform.Find("BotonConfirmar").GetComponent<Button>();
        botonVolverControles = ventanaControles.transform.Find("BotonVolverControles").GetComponent<Button>();
        botonCancelarBorrado = ventanaEliminarPartida.transform.Find("BotonCancelar").GetComponent<Button>();
        botonConfirmarEdicion = ventanaEditarPartida.transform.Find("BotonConfirmar").GetComponent<Button>();
        botonCancelarEdicion = ventanaEditarPartida.transform.Find("BotonCancelar").GetComponent<Button>();
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
        scrollLista.scrollSensitivity = Mathf.Clamp(GestorPartidas.ListasPartidas().Count, 10f, 100f);
    }

    private void ActivarVentana(GameObject ventana)
    {
        AudioSource.PlayClipAtPoint(SonidoManager.sonidoManager.ObtenerSonido("Pulsar_Boton"), Camera.main.transform.position);
        Debug.Log("Activando ventana: " + ventana.name);
        ventanaPartidasGuardadas.SetActive(false);
        ventanaEliminarPartida.SetActive(false);
        ventanaEditarPartida.SetActive(false);
        ventanaNuevaPartida.SetActive(false);
        ventanaPrincipal.SetActive(false);
        ventanaControles.SetActive(false);
        ventanaAjustes.SetActive(false);
        ventanaMensaje.SetActive(false);
        ventana.SetActive(true);
    }

    public void MostrarPartidasGuardadas()
    {
        AudioSource.PlayClipAtPoint(SonidoManager.sonidoManager.ObtenerSonido("Pulsar_Boton"), Camera.main.transform.position);
        ActivarVentana(ventanaPartidasGuardadas);
        LimpiarBotonesAnteriores();
        GenerarBotonesPartidasGuardadas();
    }

    void LimpiarBotonesAnteriores()
    {
        foreach (Transform child in contentScrollPartidas) if (child.gameObject != botonPlantillaPartida) Destroy(child.gameObject);  
    }

    void GenerarBotonesPartidasGuardadas()
    {
        foreach (Partida partida in GestorPartidas.ListasPartidas())
        {
            GameObject objetoBotonPartida = Instantiate(botonPlantillaPartida, contentScrollPartidas);
            objetoBotonPartida.SetActive(true);

            objetoBotonPartida.transform.Find("TextoNombre").GetComponent<TMP_Text>().text = partida.nombre;
            objetoBotonPartida.transform.Find("TextoInfo").GetComponent<TMP_Text>().text = $"Nivel {partida.nivel}\n{partida.dificultad}";

            Button botonPartida = objetoBotonPartida.GetComponent<Button>();
            botonPartida.onClick.RemoveAllListeners();
            botonPartida.onClick.AddListener(() => CargarPartida(partida.id));

            Button botonEditar = objetoBotonPartida.transform.Find("BotonEditar").GetComponent<Button>();
            botonEditar.onClick.RemoveAllListeners();
            botonEditar.onClick.AddListener(() => AbrirVentanaEditarPartida(partida.id));

            Button botonEliminar = objetoBotonPartida.transform.Find("BotonEliminar").GetComponent<Button>();
            botonEliminar.onClick.RemoveAllListeners();
            botonEliminar.onClick.AddListener(() => AbrirVentanaEliminarPartida(partida.id));
        }
    }

    void AbrirVentanaEditarPartida(string id)
    {
        AudioSource.PlayClipAtPoint(SonidoManager.sonidoManager.ObtenerSonido("Pulsar_Boton"), transform.position);
        Partida partida = GestorPartidas.ListasPartidas().Find(p => p.id == id);
        if (partida == null) return;
        campoNombreEditarPartida.text = partida.nombre;
        ActivarVentana(ventanaEditarPartida);

        botonConfirmarEdicion.onClick.RemoveAllListeners();
        botonConfirmarEdicion.onClick.AddListener(() =>
        {
            if (!ValidarNombre(campoNombreEditarPartida.text, id)) return;
            GestorPartidas.EditarNombrePartida(id, campoNombreEditarPartida.text);
            MostrarPartidasGuardadas();
        });

        botonCancelarEdicion.onClick.RemoveAllListeners();
        botonCancelarEdicion.onClick.AddListener(() => ActivarVentana(ventanaPartidasGuardadas));
    }

    void AbrirVentanaEliminarPartida(string id)
    {
        AudioSource.PlayClipAtPoint(SonidoManager.sonidoManager.ObtenerSonido("Pulsar_Boton"), transform.position);
        Partida partida = GestorPartidas.ListasPartidas().Find(p => p.id == id);
        if (partida == null) return;
        ActivarVentana(ventanaEliminarPartida);

        botonConfirmarBorrado.onClick.RemoveAllListeners();
        botonConfirmarBorrado.onClick.AddListener(() =>
        {   
            AudioSource.PlayClipAtPoint(SonidoManager.sonidoManager.ObtenerSonido("Pulsar_Boton"), transform.position);
            GestorPartidas.EliminarPartida(id);
            MostrarPartidasGuardadas();
        });

        botonCancelarBorrado.onClick.RemoveAllListeners();
        botonCancelarBorrado.onClick.AddListener(() => ActivarVentana(ventanaPartidasGuardadas));
    }

    private bool ValidarNombre(string nuevoNombre, string idIgnorado = null)
    {
        string mensaje = null;
        if (string.IsNullOrWhiteSpace(nuevoNombre)) mensaje = "El nombre no puede estar vacío.";
        if (nuevoNombre.Length > 20) mensaje = "El nombre no puede tener más de 20 caracteres.";
        bool existe = GestorPartidas.ListasPartidas().Exists(p => p.nombre == nuevoNombre && p.id != idIgnorado);
        if (existe) mensaje = "Ya existe una partida con ese nombre.";
        if (mensaje != null) MostrarMensaje(mensaje);
        return mensaje == null;
    }

    private void MostrarMensaje(string mensaje)
    {
        StopAllCoroutines();
        StartCoroutine(MostrarMensajeCoroutina(mensaje));
    }

    private IEnumerator MostrarMensajeCoroutina(string mensaje)
    {
        textoMensaje.text = mensaje;
        ventanaMensaje.SetActive(true);
        yield return new WaitForSeconds(2f);
        ventanaMensaje.SetActive(false);
    }

    private void CargarPartida(string id)
    {   
        AudioSource.PlayClipAtPoint(SonidoManager.sonidoManager.ObtenerSonido("Pulsar_Boton"), transform.position);
        GestorPartidas.CargarPartida(id);
        EC.escenasController.CargarEscenaComerciante();
    }

    void CrearPartida()
    {
        AudioSource.PlayClipAtPoint(SonidoManager.sonidoManager.ObtenerSonido("Pulsar_Boton"), transform.position);
        if (!ValidarNombre(campoNombreNuevaPartida.text)) return;

        DatosNuevaPartida datos = new DatosNuevaPartida
        {
            nombre = campoNombreNuevaPartida.text,
            dificultad = (Dificultad)dropdownDificultad.value
        };

        GestorPartidas.CrearNuevaPartida(datos);
        EC.escenasController.CargarEscenaComerciante();
    }
}