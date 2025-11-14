using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CofreController : MonoBehaviour, InterfazAlmacen
{
    public InventarioController inventario { get; private set; } = new InventarioController();

    private bool generado = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!generado && other.CompareTag("Player"))
        {
            generado = true;
            CrearInventarioAleatorio();
        }
    }

    // Codigo temporal para crear un inventario aleatorio en el cofre
    // En futuros commits, mejorar el sistema de generacion de cofres
    public void CrearInventarioAleatorio()
    {
        inventario.slots.Clear();

        foreach (KeyValuePair<int, Item> par in ItemDatabase.items)
        {
            Item item = par.Value;
            int cantidad = Random.Range(1, item.maxStack + 1);
            inventario.slots.Add(new ItemStack(item, cantidad));
        }

        for (int i = 1; i <= 24; i++) inventario.slots.Add(new ItemStack(null, 0));       
    }
}