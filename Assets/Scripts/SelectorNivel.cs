using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectorNivel : MonoBehaviour
{
    private List<GameObject> botonesInstanciados = new List<GameObject>();
    public RectTransform contenidoScroll;
    private ScrollRect scrollLista;
    private PausaJuegoController pausaJuegoController;

    private GameObject menu;
    private GameObject botonCerrar;
    private GameObject botonPrefab;
    private GameObject juegoController;
    private GameObject botonAsignado;
    private GameObject zonaCierre;

    private int ultimoNivelSuperado;
    private int nivelFinalModoHistoria = 10;
    private int botonesVisibles = 60;
    private int columnas = 5;
    private int numeroNivel;
    private int primerNivelVisible;
    private int nivelMaximo;

    private float alturaFila = 140f; //tamañoCelda(120) + espacioEntreCeldas(20)

    void Awake()
    {
        menu = transform.parent.gameObject;
        contenidoScroll = menu.transform.Find("ScrollView/Ventana/Contenido").GetComponent<RectTransform>();
        botonCerrar = menu.transform.Find("BotonCerrar").gameObject;
        zonaCierre = menu.transform.Find("ZonaCierre").gameObject;
        scrollLista = menu.GetComponentInChildren<ScrollRect>();
        botonPrefab = Resources.Load<GameObject>("Prefabs/BotonNivel");
        juegoController = GameObject.FindWithTag("JuegoController");
        pausaJuegoController = juegoController.GetComponent<PausaJuegoController>();
        ultimoNivelSuperado = juegoController.GetComponent<ControlSubidaNivel>().NivelMaximo;
    }

    void Start()
    {
        // Crear botones 🔼🔽 para desplazarse de forma mas precisa
        scrollLista.scrollSensitivity = ultimoNivelSuperado / 10f;
        CalcularAlturaContenido();
        AjustarContenidoScroll();
        InstanciarBotones();
        ActualizarBotonesVisibles();
        InicializarEventosUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) CerrarMenuNiveles();
    }

    void ActualizarBotonesVisibles()
    {
        CalcularPrimerNivelVisible();

        for (int i = 0; i < botonesVisibles; i++)
        {
            botonAsignado = botonesInstanciados[i];
            numeroNivel = primerNivelVisible - i;
            if (!ActualizarEstadoBoton()) continue;
            InicializarEventosBotonAsignado();
            PosicionarBoton();
        }
    }

    void CalcularAlturaContenido()
    {
        int filasTotales = (ultimoNivelSuperado + columnas - 1) / columnas;
        float alturaContenido = filasTotales * alturaFila;
        contenidoScroll.sizeDelta = new Vector2(contenidoScroll.sizeDelta.x, alturaContenido);
    }

    void AjustarContenidoScroll()
    {
        contenidoScroll.anchorMin = new Vector2(0f, 1f);
        contenidoScroll.anchorMax = new Vector2(0f, 1f);
        contenidoScroll.pivot = new Vector2(0f, 1f);        // pivote arriba
        contenidoScroll.anchoredPosition = Vector2.zero;    // origen en (0,0)
    }

    void InstanciarBotones()
    {
        for (int i = 0; i < botonesVisibles; i++)
        {
            GameObject boton = Instantiate(botonPrefab, contenidoScroll);
            RectTransform botonRect = boton.GetComponent<RectTransform>();
            botonRect.anchorMin = new Vector2(0f, 1f);
            botonRect.anchorMax = new Vector2(0f, 1f);
            botonRect.pivot = new Vector2(0f, 1f);
            botonesInstanciados.Add(boton);
        }
    }

    void InicializarEventosUI()
    {
        scrollLista.onValueChanged.AddListener(_ => { ActualizarBotonesVisibles(); });
        botonCerrar.GetComponent<Button>().onClick.AddListener(() => CerrarMenuNiveles());
        zonaCierre.AddComponent<Button>().onClick.AddListener(() => CerrarMenuNiveles());
    }

    void CalcularPrimerNivelVisible()
    {
        float posicionVerticalScroll = scrollLista.content.anchoredPosition.y;
        int primeraFila = Mathf.FloorToInt(posicionVerticalScroll / alturaFila);
        nivelMaximo = (ultimoNivelSuperado < nivelFinalModoHistoria) ? nivelFinalModoHistoria : ultimoNivelSuperado;
        primerNivelVisible = nivelMaximo - primeraFila * columnas;
    }

    bool ActualizarEstadoBoton()
    {
        botonAsignado.SetActive(numeroNivel >= 1 && (numeroNivel <= nivelMaximo));
        botonAsignado.GetComponentInChildren<TextMeshProUGUI>().text = numeroNivel.ToString();
        botonAsignado.GetComponent<Button>().interactable = numeroNivel <= ultimoNivelSuperado;
        return botonAsignado.activeSelf;
    }

    void InicializarEventosBotonAsignado()
    {
        int nivelMazmorra = numeroNivel;
        botonAsignado.GetComponent<Button>().onClick.RemoveAllListeners();
        botonAsignado.GetComponent<Button>().onClick.AddListener(() =>
        {
            CerrarMenuNiveles();
            juegoController.GetComponent<EscenasController>().CargarEscenaMazmorras(nivelMazmorra);
        });
    }

    void PosicionarBoton()
    {
        float padding = contenidoScroll.sizeDelta.x - columnas * alturaFila;
        float x = ((nivelMaximo - numeroNivel) % columnas * alturaFila) + (padding / 2) + 10f;
        float y = -((nivelMaximo - numeroNivel) / columnas * alturaFila) - padding;
        botonAsignado.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
    }

    void CerrarMenuNiveles()
    {
        Destroy(menu);
        pausaJuegoController.ToggleMenuNiveles();
    }
}