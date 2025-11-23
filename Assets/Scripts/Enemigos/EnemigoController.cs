using UnityEngine;

public class EnemigoController : MonoBehaviour
{
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
        vidaBase = Random.Range(20, 61);
        vidaMaxima = vidaBase;
        vida = vidaBase;
    }
}