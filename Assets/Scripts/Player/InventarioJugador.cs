using UnityEngine;

public class InventarioJugador : MonoBehaviour, InterfazAlmacen
{
    public InventarioController inventario { get; private set; }
    void Start() => inventario = GestorPartidas.inventarioJugador;
}