using UnityEngine;

public class AtaqueSecundario : MonoBehaviour
{
    private EnemigoController enemigo;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemigo")) return;

        enemigo = other.GetComponent<EnemigoController>();
        enemigo.Vida -= 5;
    }
}