using UnityEngine;
using UnityEngine.Events;

public class ConexionPuertas : MonoBehaviour
{
    private Transform puertaHermana;
    private GameObject jugador;
    private bool jugadorEnPuerta;

    private void Start()
    {
        puertaHermana = transform.parent.Find((gameObject.name == "PuertaA") ? "PuertaB" : "PuertaA");
    }

    private void Update()
    {
        if (jugadorEnPuerta && Input.GetMouseButtonDown(0))
        {
            jugador.transform.position = puertaHermana.position;
            jugadorEnPuerta = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnPuerta = true;
            jugador = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorEnPuerta = false;
            jugador = null;
        }
    }
}