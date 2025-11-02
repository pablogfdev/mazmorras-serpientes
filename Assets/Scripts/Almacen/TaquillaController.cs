using UnityEngine;

public class TaquillaController : MonoBehaviour, InterfazAlmacen
{
    public InventarioController inventario { get; private set; }
    void Start() => inventario = GestorPartidas.inventarioTaquilla;
}