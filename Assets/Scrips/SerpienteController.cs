using System.Collections;
using UnityEngine;

public enum EstadoSerpiente
{
    Pasivo,
    Alerta,
    Ataque
}

public class SerpienteController : MonoBehaviour
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
    private GameObject zPrefab;         // Codigo temporal: prefab de la serpiente dormida
    private GameObject detectorJugadorPrefab;
    private GameObject areaAtaquePrefab;
    private bool venenoso;

    private Coroutine corrutinaDescanso;
    private Coroutine corrutinaMovimiento;
    private Coroutine corrutinaDormir;
    private Coroutine corrutinaAlerta;
    private Coroutine corrutinaAtaque;

    private Transform jugador;
    public Transform Jugador { set => jugador = value; }

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
            if (estadoActual == EstadoSerpiente.Ataque) eventoAtaque?.Invoke();
            if (estadoActual == EstadoSerpiente.Alerta) eventoAlerta?.Invoke();
            if (estadoActual == EstadoSerpiente.Pasivo) eventoPasivo?.Invoke();
        }
    }

    void Awake()
    {
        asignarPrefabs();
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
        while (true)
        {
            Debug.Log("Serpiente atacando al jugador");
            yield return new WaitForSeconds(velocidadAtaque);
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

    void asignarPrefabs()
    {
        detectorJugadorPrefab = Resources.Load<GameObject>("Prefabs/Serpiente/DetectorJugador");
        areaAtaquePrefab = Resources.Load<GameObject>("Prefabs/Serpiente/AreaAtaque");
        zPrefab = Resources.Load<GameObject>("Prefabs/Serpiente/Z");      //Codigo temporal: Z representa la serpiente dormida
    }

    void AjustarEstadisticasPorNivel()
    {
        Mazmorra mazmorra = GetComponentInParent<Mazmorra>();
        nivel = mazmorra.Nivel;
        velocidadMovimientoBase = 2f + nivel * 0.1f;
        velocidadAtaque = 2f - nivel * 0.1f;
        venenoso = nivel >= 5 && Random.value < (nivel - 4) * 0.05f;
        velocidadMovimiento = velocidadMovimientoBase;
    }

    void InstanciarHijosSerpiente()
    {
        Instantiate(detectorJugadorPrefab, transform.position, Quaternion.identity, transform);
        Instantiate(areaAtaquePrefab, transform.position, Quaternion.identity, transform);
    }

    void AsignarTiempoDespierto()
    {
        tiempoDespierto = Time.time + Random.Range(40f, 60f);
    }

    void AsignarTiempoEnMovimiento()
    {
        tiempoEnMovimiento = Time.time + Random.Range(3f, 6f);
    }

    void AplicarColorVenenoso()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (venenoso) sr.color = new Color(0.7f, 0.4f, 0.85f); // Morado  
    }
    void instanciarZ()  //Metodo temporal: muestra en la UI que la serpiente está dormida
    {
        Vector3 posicionZ = transform.position + Vector3.left * 0.5f;
        posicionZ.z = -1f;
        zInstanciada = Instantiate(zPrefab, posicionZ, Quaternion.identity, transform);
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
