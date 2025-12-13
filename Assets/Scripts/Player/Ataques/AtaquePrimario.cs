using UnityEngine;

public class AtaquePrimario : MonoBehaviour
{
    private EnemigoController enemigo;
    private bool ataqueEjecutado = false;
    private BarraVidaObjetivoController barraVida;
    private JugadorController jugador;

    private void OnEnable() => ataqueEjecutado = false;


    void Awake()
    {
        jugador = gameObject.GetComponentInParent<JugadorController>();
        barraVida = GameObject.FindWithTag("Player").transform.Find("BarraVidaObjetivo").GetComponent<BarraVidaObjetivoController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (ataqueEjecutado) return;
        if (!other.CompareTag("Enemigo")) return;
        enemigo = barraVida.Enemigo;
        if (enemigo != null) enemigo.RecibirDanio(jugador.danioEstocada * jugador.danioExtra);
        AudioSource.PlayClipAtPoint(SonidoManager.sonidoManager.ObtenerSonido("Lanza_Clavada"), transform.position);
        ataqueEjecutado = true;
    }
}