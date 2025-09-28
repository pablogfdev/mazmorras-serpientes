using UnityEngine;
using TMPro;

public class BarraVidaJugadorController : MonoBehaviour
{
    private JugadorController jugador;
    private Transform barraVida;
    private TextMeshProUGUI textoInformacionVida;
    private TextMeshProUGUI textoInformacionNombre;
    private Vector3 escalaInicial = new Vector3(6f, 0.25f, 1f);
    private float intervaloInicialX = -8.5f;
    private float intervaloInicialY = 4f;


    void Awake()
    {
        SuscribirEvento();
        IntanciarTextoVida();
        InstanciarBarraVida();
        InstanciarFondoBarraVida();
    }

    void ActualizarBarraVida(float vidaActual)
    {
        if (vidaActual < 0) return;
        float porcentajeVida = vidaActual / jugador.VidaMaxima;
        barraVida.localScale = new Vector3(porcentajeVida * escalaInicial.x, escalaInicial.y, escalaInicial.z);
        barraVida.localPosition = new Vector3(intervaloInicialX + barraVida.localScale.x / 2, intervaloInicialY, barraVida.localPosition.z);
        textoInformacionVida.text = jugador.Vida.ToString();
    }

    void SuscribirEvento()
    {
        jugador = GetComponentInParent<JugadorController>();
        jugador.eventoCambioBarraVida += ActualizarBarraVida;
    }

    void InstanciarBarraVida()
    {
        barraVida = transform;
        barraVida.localScale = escalaInicial;
        barraVida.localPosition = new Vector3(intervaloInicialX + barraVida.localScale.x / 2, intervaloInicialY, barraVida.localPosition.z);
    }

    void InstanciarFondoBarraVida()
    {
        Transform fondoBarraVida = transform.Find("FondoBarraJugador");
        fondoBarraVida.parent = transform.parent;
        fondoBarraVida.transform.localScale = escalaInicial;
        fondoBarraVida.localPosition = new Vector3(intervaloInicialX + barraVida.localScale.x / 2, intervaloInicialY, barraVida.localPosition.z);
    }


    void IntanciarTextoVida()
    {
        Transform canvasVida = transform.Find("CanvasVida");
        Transform textoVida = canvasVida.Find("TextoVida");

        Transform canvasNombre = transform.Find("CanvasNombre");
        Transform textoNombre = canvasNombre.Find("TextoNombre");

        textoInformacionVida = textoVida.GetComponent<TextMeshProUGUI>();
        textoInformacionNombre = textoNombre.GetComponent<TextMeshProUGUI>();

        canvasVida.SetParent(transform.parent, false);
        canvasNombre.SetParent(transform.parent, false);

        canvasVida.localPosition = new Vector3(-3f, 4.5f, 0f);
        canvasNombre.localPosition = new Vector3(-6.25f, 4.5f, 0f);

        textoInformacionVida.text = jugador.Vida.ToString();
        textoInformacionNombre.text = "NombreJugador";
    }
}