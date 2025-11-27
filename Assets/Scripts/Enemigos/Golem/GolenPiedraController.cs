using System.Collections;
using UnityEngine;

public enum EstadoGolem
{
    Quieto,
    Movimiento
}

public class GolemPiedraController : EnemigoController
{
    private float velocidadBase = 2f;
    private float distanciaDeteccionPared = 1f;
    private float tiempoQuietoMin = 3f;
    private float tiempoQuietoMax = 6f;
    private float tiempoMovimientoMin = 2f;
    private float tiempoMovimientoMax = 5f;
    private bool ataque = false;
    private int danio = 10;

    private EstadoGolem estadoActual;
    public EstadoGolem EstadoActual
    {
        get => estadoActual;
        set
        {
            estadoActual = value;
            CancelarCorrutinas();
            if (estadoActual == EstadoGolem.Movimiento) corrutinaMovimiento = StartCoroutine(Mover());
            else if (estadoActual == EstadoGolem.Quieto) corrutinaQuieto = StartCoroutine(Esperar());
        }
    }

    private Coroutine corrutinaMovimiento;
    private Coroutine corrutinaQuieto;
    private Vector3 direccionAleatoria;
    private Transform jugador;

    protected override void Awake()
    {
        base.Awake();
        jugador = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (jugador != null)
        CambiarDireccionAleatoria();
        EstadoActual = (Random.value < 0.5f) ? EstadoGolem.Movimiento : EstadoGolem.Quieto;
    }

    IEnumerator Esperar()
    {
        float tiempo = Random.Range(tiempoQuietoMin, tiempoQuietoMax);
        yield return new WaitForSeconds(tiempo);
        EstadoActual = EstadoGolem.Movimiento;
    }

    IEnumerator Mover()     //Hacer el revote reflejado si hay tiempo
    {
        float tiempoMovimiento = Random.Range(tiempoMovimientoMin, tiempoMovimientoMax);
        float tiempoPasado = 0f;

        while (tiempoPasado < tiempoMovimiento)
        {
            bool hayParedFrente = Physics2D.Raycast(transform.position, direccionAleatoria, distanciaDeteccionPared, LayerMask.GetMask("Pared"));
            Debug.DrawRay(transform.position, direccionAleatoria * distanciaDeteccionPared, Color.red);

            Vector3 dirreccionDerecha = new Vector3(direccionAleatoria.y, -direccionAleatoria.x, 0f);
            bool hayParedDerecha = Physics2D.Raycast(transform.position, dirreccionDerecha, distanciaDeteccionPared, LayerMask.GetMask("Pared"));
            Debug.DrawRay(transform.position, dirreccionDerecha * distanciaDeteccionPared, Color.blue);

            Vector3 dirreccionIzquierda = new Vector3(-direccionAleatoria.y, direccionAleatoria.x, 0f);
            bool hayParedIzquierda = Physics2D.Raycast(transform.position, dirreccionIzquierda, distanciaDeteccionPared, LayerMask.GetMask("Pared"));
            Debug.DrawRay(transform.position, dirreccionIzquierda * distanciaDeteccionPared, Color.green);

            if (hayParedFrente || hayParedDerecha || hayParedIzquierda) CambiarDireccionAleatoria();
            else transform.position += direccionAleatoria * velocidadBase * Time.deltaTime;

            tiempoPasado += Time.deltaTime;
            yield return null;
        }

        EstadoActual = EstadoGolem.Quieto;
    }

    void CambiarDireccionAleatoria()
    {
        float angulo = Random.Range(0f, 360f);
        float radio = angulo * Mathf.Deg2Rad;
        direccionAleatoria = new Vector3(Mathf.Cos(radio), Mathf.Sin(radio), 0f).normalized;
    }
    
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && estadoActual == EstadoGolem.Movimiento && !ataque)
        {
            jugador = other.gameObject.transform;
            StartCoroutine(DaniarJugador());
        }
    }

    private IEnumerator DaniarJugador()
    {
        ataque = true;
        jugador.GetComponent<JugadorController>().RecibirDanio(danio);
        yield return new WaitForSeconds(1f);
        ataque = false;
    }

    void CancelarCorrutinas()
    {
        if (corrutinaMovimiento != null) StopCoroutine(corrutinaMovimiento);
        if (corrutinaQuieto != null) StopCoroutine(corrutinaQuieto);
        corrutinaMovimiento = null;
        corrutinaQuieto = null;
    }
    void OnDestroy() => CancelarCorrutinas();
}