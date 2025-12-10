using UnityEngine;

public class AtaqueSecundario : MonoBehaviour
{
    private EnemigoController enemigo;
    private JugadorController jugador;

    private float ultimoLanzaClavadaTime = -10f;
    public float cooldownLanzaClavada = 0.15f; // Ajusta al gusto

    void Awake() => jugador = gameObject.GetComponentInParent<JugadorController>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemigo")) return;
        ReproducirLanzaClavada();
        enemigo = other.GetComponent<EnemigoController>();
        enemigo.Vida -= jugador.danioBarrido * jugador.danioExtra;
    }

    public void ReproducirLanzaClavada()
    {
        if (Time.time - ultimoLanzaClavadaTime < cooldownLanzaClavada) return;
        ultimoLanzaClavadaTime = Time.time;
        AudioSource.PlayClipAtPoint(SonidoManager.sonidoManager.ObtenerSonido("Lanza_Clavada"), transform.position);
    }
}