using System;
using TMPro;
using UnityEngine;

public class BarraVidaObjetivoController : MonoBehaviour
{
    private EnemigoController enemigo;
    public EnemigoController Enemigo
    {
        get => enemigo;
        set
        {
            if (enemigo == value) return;
            if (enemigo != null) enemigo.eventoCambioBarraVida -= ActualizarBarraVida;
            enemigo = value;
            eventoCambioBarraEnemigo?.Invoke(enemigo);
        }
    }

    Vector3 escalaInicial = new Vector3(6f, 0.25f, 1f);
    private float intervaloInicialX = 2.5f;
    private float intervaloInicialY = 4f;

    private TextMeshProUGUI textoInformacionVida;
    private TextMeshProUGUI textoInformacionNombre;
    public event Action<EnemigoController> eventoCambioBarraEnemigo;

    private Transform barraVida;
    private Transform canvasVida;
    private Transform canvasNombre;
    private Transform fondoBarraVida;

    void OnEnable()
    {
        canvasVida.gameObject.SetActive(true);
        canvasNombre.gameObject.SetActive(true);
        fondoBarraVida.gameObject.SetActive(true);
    }

    void OnDisable()
    {
        canvasVida.gameObject.SetActive(false);
        canvasNombre.gameObject.SetActive(false);
        fondoBarraVida.gameObject.SetActive(false);
    }

    void Awake()
    {
        SuscribirEvento();
        InstanciarTextoVida();
        InstanciarBarraVida();
        InstanciarFondoBarraVida();
    }

    void ActualizarBarraEnemigo(EnemigoController enemigo)
    {
        float porcentajeVida = enemigo.Vida / enemigo.VidaMaxima;
        ActualizarTransformBarra(porcentajeVida);
        textoInformacionVida.text = enemigo.Vida.ToString();
        textoInformacionNombre.text = enemigo.name;
    }

    void ActualizarBarraVida(float vidaActual)
    {
        if (vidaActual < 0) return;
        float porcentajeVida = vidaActual / enemigo.VidaMaxima;
        ActualizarTransformBarra(porcentajeVida);
        textoInformacionVida.text = enemigo.Vida.ToString();
    }
    
    void SuscribirEvento()
    {
        eventoCambioBarraEnemigo += (enemigo) =>
        {
            if (enemigo != null) ActualizarBarraEnemigo(enemigo);
            if (enemigo != null) enemigo.eventoCambioBarraVida += ActualizarBarraVida;
        };
    }

    void InstanciarTextoVida()
    {
        canvasVida = transform.Find("CanvasVida");
        Transform textoVida = canvasVida.Find("TextoVida");

        canvasNombre = transform.Find("CanvasNombre");
        Transform textoNombre = canvasNombre.Find("TextoNombre");

        textoInformacionVida = textoVida.GetComponent<TextMeshProUGUI>();
        textoInformacionNombre = textoNombre.GetComponent<TextMeshProUGUI>();

        canvasVida.SetParent(transform.parent, false);
        canvasNombre.SetParent(transform.parent, false);

        canvasVida.localPosition = new Vector3(8f, 4.5f, 0f);
        canvasNombre.localPosition = new Vector3(4.75f, 4.5f, 0f);
    }

    void InstanciarBarraVida()
    {
        barraVida = transform;
        barraVida.localScale = escalaInicial;
        barraVida.localPosition = new Vector3(intervaloInicialX + barraVida.localScale.x / 2, intervaloInicialY, barraVida.localPosition.z);
    }

    void InstanciarFondoBarraVida()
    {
        fondoBarraVida = transform.Find("FondoBarraObjetivo");
        fondoBarraVida.parent = transform.parent;
        fondoBarraVida.transform.localScale = escalaInicial;
        fondoBarraVida.localPosition = new Vector3(intervaloInicialX + barraVida.localScale.x / 2, intervaloInicialY, barraVida.localPosition.z);
    }

    private void ActualizarTransformBarra(float porcentajeVida)
    {
        barraVida.localScale = new Vector3(porcentajeVida * escalaInicial.x, escalaInicial.y, escalaInicial.z);
        barraVida.localPosition = new Vector3(intervaloInicialX + barraVida.localScale.x / 2, intervaloInicialY, barraVida.localPosition.z);
    }
}