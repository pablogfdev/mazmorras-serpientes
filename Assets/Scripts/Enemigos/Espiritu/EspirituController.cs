using System.Collections;
using UnityEngine;

public enum EstadoEspiritu
{
    Ambulante,
    Ataque
}

public class EspirituController : EnemigoController
{
    private Transform jugador;
    private Transform suelo;
    private DetectorEspiritu detectorJugador;

    private Vector3 ultimaPosicion;
    private float distanciaMinima = 2f;
    private float margen = 1f;
    private float tiempoAtaque = 1.5f;

    private Coroutine corrutinaAmbulante;
    private Coroutine corrutinaAtaque;
    private Coroutine corrutinaExpansion;

    private EstadoEspiritu estadoActual;
    public EstadoEspiritu EstadoActual
    {
        get => estadoActual;
        set => CambiarEstado(estadoActual = value);
    }

    protected override void Awake()
    {
        base.Awake();
        jugador = GameObject.FindWithTag("Player").transform;
        suelo = transform.parent.Find("Suelo(Clone)");
        detectorJugador = Instantiate(PrefabManager.prefabManager.ObtenerPrefab("DetectorEspiritu"), transform).GetComponent<DetectorEspiritu>();
        EstadoActual = EstadoEspiritu.Ambulante;
    }

    public override void RecibirDanio(float danio) 
    {
        base.RecibirDanio(danio);
        Teletransportarse();
    } 

    protected override void Start()
    {
        base.Start();
        AjustarEstadisticas();
    }

    void AjustarEstadisticas()
    {
        int vidaMin = 35;
        int vidaMax = 55;
        int vidaGenerada = generador.Next(vidaMin, vidaMax + 1);

        InicializarVida(vidaGenerada);
    }
    
    private void CambiarEstado(EstadoEspiritu nuevoEstado)
    {
        CancelarCorrutinas();
        if (nuevoEstado == EstadoEspiritu.Ambulante) corrutinaAmbulante = StartCoroutine(EstadoAmbulante());
        if (nuevoEstado == EstadoEspiritu.Ataque) corrutinaAtaque = StartCoroutine(EstadoAtaque());
    }

    private IEnumerator EstadoAtaque()
    {
        while (EstadoActual == EstadoEspiritu.Ataque)
        {
            yield return new WaitForSeconds(tiempoAtaque);
            DispararProyectil();
        }
    }

    private void DispararProyectil()
    {
        if (jugador == null) return;
        Vector3 dir = (jugador.position - transform.position).normalized;
        GameObject proyectil = Instantiate(PrefabManager.prefabManager.ObtenerPrefab("ProyectilEspiritu"), transform.position, Quaternion.identity);
        proyectil.GetComponent<Rigidbody2D>().linearVelocity = dir * 6f;
    }

    private void Teletransportarse()
    {
        Vector3 punto;
        do
        {
            punto = PosicionAleatoriaHabitacion();
        } while (Vector3.Distance(punto, ultimaPosicion) < distanciaMinima);

        transform.position = punto;
        ultimaPosicion = punto;
    }

    private Vector3 PosicionAleatoriaHabitacion()
    {
        Vector3 centro = suelo.position;
        Vector3 escala = suelo.localScale;

        float x = Random.Range(centro.x - escala.x / 2f + margen, centro.x + escala.x / 2f - margen);
        float y = Random.Range(centro.y - escala.y / 2f + margen, centro.y + escala.y / 2f - margen);

        return new Vector3(x, y, 0f);
    }

    private IEnumerator EstadoAmbulante()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            detectorJugador.transform.localScale = Vector3.zero;
            Teletransportarse();
            yield return new WaitForSeconds(Random.Range(0.5f, 1f));
            corrutinaExpansion = StartCoroutine(detectorJugador.GetComponent<DetectorEspiritu>().ExpandirDominio());

            yield return corrutinaExpansion;
        }
    }

    private void CancelarCorrutinas()
    {
        if (corrutinaAmbulante != null) StopCoroutine(corrutinaAmbulante);
        if (corrutinaAtaque != null) StopCoroutine(corrutinaAtaque);
        if (corrutinaExpansion != null) StopCoroutine(corrutinaExpansion);

        corrutinaAmbulante = null;
        corrutinaAtaque = null;
        corrutinaExpansion = null;
    }

    void OnDestroy() => CancelarCorrutinas();
}