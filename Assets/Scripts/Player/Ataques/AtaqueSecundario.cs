using UnityEngine;

public class AtaqueSecundario : MonoBehaviour
{
    private EnemigoController enemigo;
    private JugadorController jugador;

    void Awake() => jugador = gameObject.GetComponentInParent<JugadorController>();    
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemigo")) return;

        enemigo = other.GetComponent<EnemigoController>();
        enemigo.Vida -= jugador.danioBarrido * jugador.danioExtra;
    }
}