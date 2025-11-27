using UnityEngine;

public class ProyectilEspiritu : MonoBehaviour
{
    private int daño = 10;  //Sacar daño del enemigo en el commit de balance

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            JugadorController jugador = other.GetComponent<JugadorController>();
            if (jugador != null) jugador.RecibirDanio(daño);
            Destroy(gameObject);
        }

        if (other.CompareTag("Pared")) Destroy(gameObject);
    }
}