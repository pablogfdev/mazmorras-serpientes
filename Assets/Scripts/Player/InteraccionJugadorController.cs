using UnityEngine;
using System.Collections.Generic;

public class InteraccionJugadorController : MonoBehaviour
{
    private List<string> tagsValidos = new() { "Puerta", "Cofre", "Taquilla", "Entrada", "Salida" };
    private GameObject objetoCercano;
    private InventarioJugador inventarioJugador;

    void Awake() => inventarioJugador = gameObject.transform.parent.GetComponent<InventarioJugador>();

    void OnTriggerEnter2D(Collider2D other) { if (tagsValidos.Contains(other.tag)) objetoCercano = other.gameObject; }

    void OnTriggerExit2D(Collider2D other) { if (tagsValidos.Contains(other.tag)) objetoCercano = null; }

    void Update() { if (Input.GetKeyDown(KeyCode.E)) (objetoCercano?.GetComponent<InterfazAlmacen>()?.inventario ?? inventarioJugador.inventario).MostrarInventario(); }
}