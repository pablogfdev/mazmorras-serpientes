using UnityEngine;
using System;
using System.Collections;

public class JugadorController : MonoBehaviour
{
    public bool LlaveObtenida { get; set; } = false;
    private int vidaBase = 100;
    private float vidaMaxima;
    public float VidaMaxima { get => vidaMaxima; }
    private float vida;
    public float Vida
    {
        get => vida;
        set
        {
            vida = value;
            if (vida <= 0) EventoMuerte?.Invoke();
            eventoCambioBarraVida?.Invoke(vida);
        }
    }

    private float velocidadBase = 5f;
    private float multiplicadorVelocidad = 2f;
    private float velocidadMovimiento;
    public float VelocidadMovimiento
    {
        get => velocidadMovimiento;
        set
        {
            if (Mathf.Approximately(velocidadMovimiento, value)) return;
            velocidadMovimiento = value;
            eventoCambioVelocidad?.Invoke(velocidadMovimiento);
        }
    }

    public Rigidbody2D rb { get; private set; }

    public event Action<float> eventoCambioVelocidad;
    public event Action<float> eventoCambioBarraVida;
    public event Action EventoMuerte;

    private GameObject espadaPrimaria;
    private GameObject espadaSecundaria;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        vidaMaxima = vidaBase;
        vida = vidaBase;

        espadaPrimaria = gameObject.transform.Find("EspadaPrimaria").gameObject;
        espadaSecundaria = gameObject.transform.Find("EspadaSecundaria").gameObject;
        espadaPrimaria.SetActive(false);
        espadaSecundaria.SetActive(false);

        EventoMuerte += () =>
        {
            Transform camara = transform.Find("CamaraJugador");
            camara.parent = null;       // Codigo temporal: Desacoplar la camara del jugador para que siga funcionando tras su muerte
            Destroy(gameObject);
        };
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !espadaPrimaria.activeSelf && !espadaSecundaria.activeSelf) StartCoroutine(DarEstocada());
        if (Input.GetMouseButtonDown(1) && !espadaPrimaria.activeSelf && !espadaSecundaria.activeSelf) StartCoroutine(DarBarrido());
        ActualizarVelocidad();
    }

    void ActualizarVelocidad()
    {
        float nuevaVelocidad = velocidadBase * (Input.GetKey(KeyCode.LeftShift) ? multiplicadorVelocidad : 1f);
        VelocidadMovimiento = nuevaVelocidad;
    }

    IEnumerator DarEstocada()
    {
        espadaPrimaria.SetActive(true);
        yield return new WaitForSeconds(0.33f);
        espadaPrimaria.SetActive(false);
    }

    IEnumerator DarBarrido()
    {
        espadaSecundaria.SetActive(true);
        yield return new WaitForSeconds(0.33f);
        espadaSecundaria.SetActive(false);
    }
}