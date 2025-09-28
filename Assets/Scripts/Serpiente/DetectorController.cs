using UnityEngine;

public class DetectorController : MonoBehaviour
{

    private SerpienteController controladorSerpiente;
    private int nivel;
    private float escalaDetector;
    private float cambioEscala = 16f;
    private bool esDetectorJugador;

    void Start()
    {
        controladorSerpiente = GetComponentInParent<SerpienteController>();
        MazmorraController mazmorra = GetComponentInParent<MazmorraController>();
        nivel = mazmorra.Nivel;
        esDetectorJugador = name.Contains("DetectorJugador");
        escalaDetector = esDetectorJugador ? 5f + nivel * 0.1f : 1f + nivel * 0.1f;

        Vector3 escalaPadre = transform.parent.lossyScale;
        transform.localScale = new Vector3(escalaDetector / escalaPadre.x, escalaDetector / escalaPadre.y, 1f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (esDetectorJugador)
        {
            controladorSerpiente.Jugador = other.transform;
            controladorSerpiente.EstadoSerpiente = EstadoSerpiente.Alerta;
            transform.localScale *= cambioEscala;
        }
        else controladorSerpiente.EstadoSerpiente = EstadoSerpiente.Ataque;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (esDetectorJugador)
        {
            controladorSerpiente.Jugador = null;
            controladorSerpiente.EstadoSerpiente = EstadoSerpiente.Pasivo;
            transform.localScale /= cambioEscala;
        }
        else controladorSerpiente.EstadoSerpiente = EstadoSerpiente.Alerta;
    }
}