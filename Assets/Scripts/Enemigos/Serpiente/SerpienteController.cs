using System.Collections;
using UnityEngine;
using PM = PrefabManager;

public enum EstadoSerpiente 
{
    Pasivo,
    Alerta,
    Ataque
}

public class SerpienteController : EnemigoController
{
    private int nivel;

    float velocidadMovimientoBase;
    private float velocidadMovimiento;
    private float velocidadAtaque;
    private float distanciaDeteccionPared = 5f;

    private Vector3 direccionAleatoria;
    private float tiempoEnMovimiento;
    private float tiempoDespierto;

    private GameObject zInstanciada;    // Codigo temporal: representa la serpiente dormida

    private bool venenoso;

    private Coroutine corrutinaDescanso;
    private Coroutine corrutinaMovimiento;
    private Coroutine corrutinaDormir;
    private Coroutine corrutinaAlerta;
    private Coroutine corrutinaAtaque;
    private Coroutine corrutinaVeneno;

    private Transform jugador;
    public Transform Jugador { set => jugador = value; }
    private JugadorController jugadorController;

    public event System.Action eventoAtaque;
    public event System.Action eventoAlerta;
    public event System.Action eventoPasivo;

    private EstadoSerpiente estadoActual;
    public EstadoSerpiente EstadoSerpiente
    {
        get => estadoActual;
        set
        {
            estadoActual = value;
            if (!gameObject.activeInHierarchy) return;
            if (estadoActual == EstadoSerpiente.Ataque) eventoAtaque?.Invoke();
            if (estadoActual == EstadoSerpiente.Alerta) eventoAlerta?.Invoke();
            if (estadoActual == EstadoSerpiente.Pasivo) eventoPasivo?.Invoke();
        }
    }

    protected override void Awake()
    {
        base.Awake();
        AsignarJugadorController();
        AjustarEstadisticasPorNivel();
        InstanciarHijosSerpiente();
        AsignarTiempoEnMovimiento();
        AsignarTiempoDespierto();
        AplicarColorVenenoso();
    }

    void Start()
    {
        StartCoroutine(Mover());

        eventoAtaque += () =>
        {
            CancelarCorrutinas();
            corrutinaAtaque = StartCoroutine(Atacar());
        };
        eventoAlerta += () =>
        {
            CancelarCorrutinas();
            corrutinaAlerta = StartCoroutine(SeguirJugador());

        };
        eventoPasivo += () =>
        {
            CancelarCorrutinas();
            corrutinaMovimiento = StartCoroutine(Mover());
        };
    }

    IEnumerator Mover()
    {
        while (estadoActual == EstadoSerpiente.Pasivo)
        {
            if (velocidadMovimiento == 0f)
            {
                yield return null;
                continue;
            }

            if (Time.time >= tiempoEnMovimiento)
            {
                corrutinaDescanso = StartCoroutine(Descansar());
                yield return null;
                continue;
            }

            if (Time.time >= tiempoDespierto)
            {
                corrutinaDormir = StartCoroutine(Dormir());
                yield return null;
                continue;
            }

            // Detección frontal
            bool hayParedFrente = Physics2D.Raycast(transform.position, direccionAleatoria, distanciaDeteccionPared, LayerMask.GetMask("Pared"));
            Debug.DrawRay(transform.position, direccionAleatoria * distanciaDeteccionPared, Color.red);

            // Detección lateral derecha (perpendicular positiva)
            Vector3 direccionDerecha = new Vector3(direccionAleatoria.y, -direccionAleatoria.x, 0f);
            bool hayParedDerecha = Physics2D.Raycast(transform.position, direccionDerecha, distanciaDeteccionPared, LayerMask.GetMask("Pared"));
            Debug.DrawRay(transform.position, direccionDerecha * distanciaDeteccionPared, Color.red);

            // Detección lateral izquierda (perpendicular negativa)
            Vector3 direccionIzquierda = new Vector3(-direccionAleatoria.y, direccionAleatoria.x, 0f);
            bool hayParedIzquierda = Physics2D.Raycast(transform.position, direccionIzquierda, distanciaDeteccionPared, LayerMask.GetMask("Pared"));
            Debug.DrawRay(transform.position, direccionIzquierda * distanciaDeteccionPared, Color.red);

            if (hayParedFrente || hayParedDerecha || hayParedIzquierda) CambiarDireccionAleatoria();
            else transform.position += direccionAleatoria * velocidadMovimiento * Time.deltaTime;
            yield return null;
        }
    }

    private void IniciarCorrutinaVeneno(int cantidadIntervalo, float duracion)
    {
        if(corrutinaVeneno != null) StopCoroutine(corrutinaVeneno);
        corrutinaVeneno = StartCoroutine(EnvenenarJugador(cantidadIntervalo, duracion));
    }

    private IEnumerator EnvenenarJugador(int cantidadPorSegundo, float duracion)
    {
        float tiempoPasado = 0;
        while (tiempoPasado < duracion)
        {
            jugadorController.RecibirDanio(cantidadPorSegundo);
            yield return new WaitForSeconds(1f);
            tiempoPasado += 1f;
        }
    }

    IEnumerator SeguirJugador()
    {
        while (estadoActual == EstadoSerpiente.Alerta)
        {
            if (jugador == null)
            {
                yield return null;
                continue;
            }
            velocidadMovimiento = velocidadMovimientoBase;
            Vector3 direccion = (jugador.position - transform.position).normalized;
            transform.position += direccion * velocidadMovimiento * Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator Atacar()
    {
        if(venenoso && !jugadorController.inmune) IniciarCorrutinaVeneno(10, 3);
        while (true)
        {
            yield return new WaitForSeconds(velocidadAtaque * 0.25f);
            jugadorController.RecibirDanio(10);
            yield return new WaitForSeconds(velocidadAtaque * 0.75f);
        }
    }

    IEnumerator Descansar()
    {
        velocidadMovimiento = 0f;
        yield return new WaitForSeconds(Random.Range(3f, 6f));
        CambiarDireccionAleatoria();
        velocidadMovimiento = velocidadMovimientoBase;
        AsignarTiempoEnMovimiento();
    }

    IEnumerator Dormir()
    {
        velocidadMovimiento = 0f;
        instanciarZ();
        yield return new WaitForSeconds(Random.Range(8f, 15f));
        velocidadMovimiento = velocidadMovimientoBase;
        DestruirZ();
        AsignarTiempoDespierto();
    }

    void CambiarDireccionAleatoria()
    {
        float anguloActual = Mathf.Atan2(direccionAleatoria.y, direccionAleatoria.x) * Mathf.Rad2Deg;
        float anguloBase = anguloActual + 180f;
        float nuevoAngulo = Random.Range(anguloBase - 90f, anguloBase + 90f);
        float radianes = nuevoAngulo * Mathf.Deg2Rad;
        direccionAleatoria = new Vector3(Mathf.Cos(radianes), Mathf.Sin(radianes), 0f).normalized;
    }

    void CancelarCorrutinas()
    {
        if (corrutinaDescanso != null)
        {
            StopCoroutine(corrutinaDescanso);
            corrutinaDescanso = null;
        }

        if (corrutinaDormir != null)
        {
            StopCoroutine(corrutinaDormir);
            corrutinaDormir = null;
            if (zInstanciada != null) DestruirZ();  // Codigo temporal: destruye el objeto Z si la serpiente se despierta antes de que termine la corutina de dormir

        }
        if (corrutinaAtaque != null)
        {
            StopCoroutine(corrutinaAtaque);
            corrutinaAtaque = null;
        }

        if (corrutinaMovimiento != null)
        {
            StopCoroutine(corrutinaMovimiento);
            corrutinaMovimiento = null;
        }

        if (corrutinaAlerta != null)
        {
            StopCoroutine(corrutinaAlerta);
            corrutinaAlerta = null;
        }
    }

    void AsignarJugadorController()
    {
        jugadorController = GameObject.FindGameObjectWithTag("Player").GetComponent<JugadorController>();
    }

    void AjustarEstadisticasPorNivel()
    {
        MazmorraController mazmorra = GetComponentInParent<MazmorraController>();
        nivel = mazmorra.Nivel;
        velocidadMovimientoBase = 2f + nivel * 0.1f;
        velocidadAtaque = 2f - nivel * 0.1f;
        venenoso = nivel >= 5 && Random.value < (nivel - 4) * 0.05f;
        velocidadMovimiento = velocidadMovimientoBase;
    }

    void InstanciarHijosSerpiente()
    {
        Instantiate(PM.prefabManager.ObtenerPrefab("DetectorJugador"), transform.position, Quaternion.identity, transform);
        Instantiate(PM.prefabManager.ObtenerPrefab("AreaAtaque"), transform.position, Quaternion.identity, transform);
    }

    void AsignarTiempoDespierto() => tiempoDespierto = Time.time + Random.Range(40f, 60f);

    void AsignarTiempoEnMovimiento() => tiempoEnMovimiento = Time.time + Random.Range(3f, 6f);

    void AplicarColorVenenoso() => GetComponent<SpriteRenderer>().color = venenoso ? new Color(0.7f, 0.4f, 0.85f) : Color.white;

    void instanciarZ()  //Metodo temporal: muestra en la UI que la serpiente está dormida
    {
        Vector3 posicionZ = transform.position + Vector3.left * 0.5f;
        posicionZ.z = -1f;
        zInstanciada = Instantiate(PM.prefabManager.ObtenerPrefab("Z"), posicionZ, Quaternion.identity, transform);
    }

    void DestruirZ()    //Metodo temporal: destruye el objeto Z instanciado que representa la serpiente dormida
    {
        Destroy(zInstanciada);
        zInstanciada = null;
    }

    void OnDestroy()
    {
        CancelarCorrutinas();
        eventoAtaque = null;
        eventoAlerta = null;
        eventoPasivo = null;
    }
}