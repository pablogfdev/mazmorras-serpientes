using UnityEngine;
using System.Collections;

public class SerpienteSpriteController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private SerpienteController serpienteController;
    private Transform jugador;

    private float intervaloFrameAlerta = 0.25f;
    private float tiempoUltimoFrameAlerta = 0f;

    private Coroutine corrutinaRotacion;
    private Coroutine corrutinaCaminar;

    private string ultimaDireccion = "Sur";
    private int frameActual = 1;

    private readonly string[] direcciones = { "Sur", "Norte", "Este", "Oeste" };

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        serpienteController = GetComponent<SerpienteController>();
        jugador = GameObject.FindWithTag("Player").transform;
    }

    void Start() => CambiarSprite("Sur", 1);

    void Update()
    {
        switch (serpienteController.EstadoSerpiente)
        {
            case EstadoSerpiente.Pasivo:
                AnimarEstadoPasivo();
                break;

            case EstadoSerpiente.Alerta:
                AnimarEstadoAlerta();
                break;

            case EstadoSerpiente.Ataque:
                AnimarEstadoAtaque();
                break;
        }
    }


    private void AnimarEstadoPasivo()
    {
        bool moviendose = serpienteController.VelocidadMovimiento > 0.01f;

        if (moviendose)
        {
            if (corrutinaCaminar == null)
            {
                DetenerCorrutinas();
                corrutinaCaminar = StartCoroutine(IniciarAnimacion());
            }
        }
        else
        {
            if (corrutinaRotacion == null)
            {
                DetenerCorrutinas();
                corrutinaRotacion = StartCoroutine(RotarSprite());
            }
        }
    }

    private void AnimarEstadoAlerta()
    {
        DetenerCorrutinas(); 

        if (Time.time - tiempoUltimoFrameAlerta >= intervaloFrameAlerta)
        {
            ActualizarDireccionHaciaJugador();

            if (serpienteController.VelocidadMovimiento > 0.01f)
            {
                frameActual = (frameActual == 1) ? 2 : 1;
                CambiarSprite(ultimaDireccion, frameActual);
            }
            else CambiarSprite(ultimaDireccion, 1);
            tiempoUltimoFrameAlerta = Time.time;
        }
    }

    private void AnimarEstadoAtaque()
    {
        DetenerCorrutinas();
        ActualizarDireccionHaciaJugador();
        CambiarSprite(ultimaDireccion, 1);
    }

    private void ActualizarDireccionSegunMovimiento()
    {
        Vector3 velocidad = serpienteController.DireccionAleatoria;
        if (velocidad == Vector3.zero) return;
        if (Mathf.Abs(velocidad.x) > Mathf.Abs(velocidad.y)) ultimaDireccion = (velocidad.x > 0) ? "Este" : "Oeste";
        else ultimaDireccion = (velocidad.y > 0) ? "Norte" : "Sur";
    }

    private void ActualizarDireccionHaciaJugador()
    {
        Vector2 direccion = jugador.position - transform.position;
        if (Mathf.Abs(direccion.x) > Mathf.Abs(direccion.y)) ultimaDireccion = (direccion.x > 0) ? "Este" : "Oeste";
        else ultimaDireccion = (direccion.y > 0) ? "Norte" : "Sur";
        CambiarSprite(ultimaDireccion, 1);
    }

    private void CambiarSprite(string direccion, int frame) => spriteRenderer.sprite = SpriteManager.spriteManager.ObtenerSprite($"Serpiente_{direccion}_{frame}");

    private void DetenerCorrutinas()
    {
        if (corrutinaCaminar != null) StopCoroutine(corrutinaCaminar);
        if (corrutinaRotacion != null) StopCoroutine(corrutinaRotacion);
        corrutinaCaminar = null;
        corrutinaRotacion = null;
    }

    private IEnumerator IniciarAnimacion()
    {
        while (serpienteController.EstadoSerpiente == EstadoSerpiente.Pasivo && serpienteController.VelocidadMovimiento > 0.01f)
        {
            ActualizarDireccionSegunMovimiento();
            frameActual = (frameActual == 1) ? 2 : 1;
            CambiarSprite(ultimaDireccion, frameActual);
            yield return new WaitForSeconds(0.25f);
        }
        corrutinaCaminar = null;
    }

    private IEnumerator RotarSprite()
    {
        while (serpienteController.EstadoSerpiente == EstadoSerpiente.Pasivo && serpienteController.VelocidadMovimiento <= 0.01f)
        {
            string nuevaDireccion;
            do nuevaDireccion = direcciones[Random.Range(0, direcciones.Length)];
            while (nuevaDireccion == ultimaDireccion);
            ultimaDireccion = nuevaDireccion;
            CambiarSprite(ultimaDireccion, 1);
            yield return new WaitForSeconds(Random.Range(1f, 3f));
        }
        corrutinaRotacion = null;
    }
}