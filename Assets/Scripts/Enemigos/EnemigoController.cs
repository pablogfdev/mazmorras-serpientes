using UnityEngine;

public class EnemigoController : MonoBehaviour
{
    protected System.Random generador;
    private int vidaBase;
    private float vidaMaxima;
    public float VidaMaxima { get => vidaMaxima; }
    private float vida;
    public float Vida
    {
        get => vida;
        set
        {
            vida = value;
            eventoCambioBarraVida?.Invoke(vida);
            if (vida <= 0) Destroy(gameObject);
        }
    }

    public event System.Action<float> eventoCambioBarraVida;

    protected virtual void Awake()
    {
        vidaBase = 50;
        vidaMaxima = vidaBase;
        vida = vidaBase;
    }

    protected virtual void Start() => IniciarGenerador();

    protected void InicializarVida(float vidaInicial)
    {
        vidaMaxima = vidaInicial;
        vida = vidaInicial;
        eventoCambioBarraVida?.Invoke(vida);
    }

    public virtual void RecibirDanio(float danio) => Vida -= danio;
    
    private void IniciarGenerador()
    {
        MazmorraController mazmorra = GetComponentInParent<MazmorraController>();

        string[] partes = name.Split('_');
        int x = 0, y = 0, numero = 0;

        int.TryParse(partes[1], out x);
        int.TryParse(partes[2], out y);
        int.TryParse(partes[3], out numero);

        generador = new System.Random(GestorPartidas.partidaActiva.semilla + x * 91 + y * 93 + numero * 97 + mazmorra.Nivel * 101);
    }
}