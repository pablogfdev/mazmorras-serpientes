using UnityEngine;

public class AtaquePrimario : MonoBehaviour
{
    private EnemigoController enemigo;
    private bool ataqueEjecutado = false;
    private BarraVidaObjetivoController barraVida;

    private void OnEnable() => ataqueEjecutado = false;


    void Awake()
    {
        barraVida = GameObject.FindWithTag("Player")
            .transform.Find("BarraVidaObjetivo")
            .GetComponent<BarraVidaObjetivoController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (ataqueEjecutado) return;
        if (!other.CompareTag("Enemigo")) return;
        enemigo = barraVida.Enemigo;
        if (enemigo != null) enemigo.Vida -= 20;
        ataqueEjecutado = true;
    }
}