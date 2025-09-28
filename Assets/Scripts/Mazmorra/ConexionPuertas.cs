using UnityEngine;

public class ConexionPuertas : MonoBehaviour
{
    private Transform puertaHermana;
    private GameObject jugador;
    private bool jugadorEnPuerta;

    private void Start() => puertaHermana = transform.parent.Find((gameObject.name == "PuertaA") ? "PuertaB" : "PuertaA");

    private void Update()
    {
        if (jugadorEnPuerta && Input.GetKeyDown(KeyCode.E)) jugador.transform.position = puertaHermana.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        jugadorEnPuerta = true;
        jugador = other.gameObject;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        jugadorEnPuerta = false;
        jugador = null;
    }
}