using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class JugadorController : MonoBehaviour
{
    // Contemplar manejar la vida en un codigo aparte
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
            if (vida >= vidaBase) vida = vidaBase;
            if (vida <= 0) EventoMuerte?.Invoke();
            eventoCambioBarraVida?.Invoke(vida);
        }
    }

    private float velocidadBase = 5f;
    private float multiplicadorVelocidad = 2f;
    private float velocidadExtra = 1;
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

    public float danioExtra = 1;
    public int danioEstocada = 20;
    public int danioBarrido = 5;
    private float defensa = 1;
    public bool inmune = false;

    public Rigidbody2D rb { get; private set; }

    public event Action<float> eventoCambioVelocidad;
    public event Action<float> eventoCambioBarraVida;
    public event Action EventoMuerte;

    private GameObject espadaPrimaria;
    private GameObject espadaSecundaria;
    private Coroutine corrutinaVelocidad;
    private Coroutine corrutinaDanio;
    private Coroutine corrutinaInmunidad;
    private Coroutine corrutinaDefensa;
    private Coroutine corrutinaCuracion;
    
    private JugadorMuerteController muerteController;
    [SerializeField] private Animator animCuerpo; 
    [SerializeField] private JugadorSpriteController spriteController;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        vidaMaxima = vidaBase;
        vida = vidaBase;

        animCuerpo = transform.Find("Cuerpo")?.GetComponent<Animator>();
        spriteController = GetComponent<JugadorSpriteController>();

        espadaPrimaria = gameObject.transform.Find("EspadaPrimaria").gameObject;
        espadaSecundaria = gameObject.transform.Find("EspadaSecundaria").gameObject;
        espadaPrimaria.SetActive(false);
        espadaSecundaria.SetActive(false);
        muerteController = GetComponent<JugadorMuerteController>();
        EventoMuerte += () => muerteController?.IniciarMuerte();
    }

    void Update() => ActualizarVelocidad();


    public void BloquearMovimiento() => velocidadBase = 0f;
    public void DesbloquearMovimiento() => velocidadBase = 5f;

    void ActualizarVelocidad()
    {
        float nuevaVelocidad = velocidadBase * (Input.GetKey(KeyCode.LeftShift) ? multiplicadorVelocidad : 1f) * velocidadExtra;
        VelocidadMovimiento = nuevaVelocidad;
    }


    public void RecibirDanio(float cantidad)
    {
        float danioReducido = Mathf.RoundToInt(cantidad * defensa);
        Vida -= danioReducido;
    }

    public void IniciarEstocada() => StartCoroutine(DarEstocada());
    public void IniciarBarrido() => StartCoroutine(DarBarrido());

    private IEnumerator DarEstocada()
    {
        if (espadaPrimaria.activeSelf || espadaSecundaria.activeSelf) yield break;
        spriteController.IniciarAtaque(2); 
        espadaPrimaria.SetActive(true);
        while (spriteController.EstaAtacando) yield return null;
        espadaPrimaria.SetActive(false);
    }

    private IEnumerator DarBarrido()
    {
        if (espadaPrimaria.activeSelf || espadaSecundaria.activeSelf) yield break;
        spriteController.IniciarAtaque(3);
        espadaSecundaria.SetActive(true);
        while (spriteController.EstaAtacando) yield return null;
        espadaSecundaria.SetActive(false);
    }

    public void AumentarVelocidad(float velocidadExtra, float duracion)
    {
        if (corrutinaVelocidad != null) StopCoroutine(corrutinaVelocidad);
        corrutinaVelocidad = StartCoroutine(IniciarCorrutinaVelocidad(velocidadExtra, duracion));
    }

    private IEnumerator IniciarCorrutinaVelocidad(float velocidadExtra, float duracion)
    {
        //AudioSource.PlayClipAtPoint(SonidoManager.sonidoManager.ObtenerSonido("Beber_Pocion"), Camera.main.transform.position);
        this.velocidadExtra = velocidadExtra;
        BarraEfectoProgresoController.barraEfectoProgresoController.CrearBarra(IconoManager.iconoManager.ObtenerIcono("Pocion de Velocidad"), duracion);
        yield return new WaitForSeconds(duracion);
        this.velocidadExtra = 1f;
    }

    public void AumentarDanio(float danioExtra, float duracion)
    {
        if (corrutinaDanio != null) StopCoroutine(corrutinaDanio);
        corrutinaDanio = StartCoroutine(PotenciarDanio(danioExtra, duracion));
    }

    private IEnumerator PotenciarDanio(float danioExtra, float duracion)
    {
        //AudioSource.PlayClipAtPoint(SonidoManager.sonidoManager.ObtenerSonido("Beber_Pocion"), Camera.main.transform.position);
        this.danioExtra = Mathf.RoundToInt(danioExtra);
        BarraEfectoProgresoController.barraEfectoProgresoController.CrearBarra(IconoManager.iconoManager.ObtenerIcono("Pocion de Fuerza"), duracion);
        yield return new WaitForSeconds(duracion);
        this.danioExtra = 1f;
    }

    public void RegenerarVida(int vidaExtraPorIntervalo, float duracion)
    {
        if (corrutinaCuracion != null) StopCoroutine(corrutinaCuracion);
        corrutinaCuracion = StartCoroutine(CurarVida(vidaExtraPorIntervalo, duracion));
    }

    private IEnumerator CurarVida(int vidaExtraPorIntervalo, float duracion)
    {
        //AudioSource.PlayClipAtPoint(SonidoManager.sonidoManager.ObtenerSonido("Beber_Pocion"), Camera.main.transform.position);
        float tiempoPasado = 0f;
        while (tiempoPasado < duracion)
        {
            Vida += vidaExtraPorIntervalo;
            yield return new WaitForSeconds(1f);
            tiempoPasado += 1f;
        }
    }

    public void ObtenerInmunidad(float duracion)
    {
        if (corrutinaInmunidad != null) StopCoroutine(corrutinaInmunidad);
        corrutinaInmunidad = StartCoroutine(DarInmunidad(duracion));
    }

    private IEnumerator DarInmunidad(float duracion)
    {
        //AudioSource.PlayClipAtPoint(SonidoManager.sonidoManager.ObtenerSonido("Beber_Pocion"), Camera.main.transform.position);
        inmune = true;
        BarraEfectoProgresoController.barraEfectoProgresoController.CrearBarra(IconoManager.iconoManager.ObtenerIcono("Antidoto"), duracion);
        yield return new WaitForSeconds(duracion);
        inmune = false;
    }

    public void AumentarDefensa(float defensaExtra, float duracion)
    {
        if (corrutinaDefensa != null) StopCoroutine(corrutinaDefensa);
        corrutinaDefensa = StartCoroutine(AplicarDefensa(defensaExtra, duracion));
    }

    private IEnumerator AplicarDefensa(float defensaExtra, float duracion)
    {
        AudioSource.PlayClipAtPoint(SonidoManager.sonidoManager.ObtenerSonido("Beber_Pocion"), Camera.main.transform.position);
        defensa = defensaExtra;
        BarraEfectoProgresoController.barraEfectoProgresoController.CrearBarra(IconoManager.iconoManager.ObtenerIcono("Pocion de Defensa"), duracion);
        yield return new WaitForSeconds(duracion);
        defensa = 1f;
    }
}