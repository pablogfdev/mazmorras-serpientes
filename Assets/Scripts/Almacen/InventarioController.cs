using System.Collections.Generic;
using UnityEngine;

public class InventarioController
{
    private int tamano = 25;
    public List<ItemStack> slots;

    public InventarioController()
    {
        slots = new List<ItemStack>(new ItemStack[tamano]);
        for (int i = 0; i < tamano; i++) slots[i] = new ItemStack(null, 0);
    }

    // Funcion temporal para mostrar el inventario en consola.
    // Modificar en el commit de UI inventario.
    public void MostrarInventario()
    {
        string contenido = "";
        foreach (var slot in slots) if (!slot.vacio) contenido += $"{slot.item.nombre} x{slot.cantidad}, ";
        Debug.Log($"Contenido: {contenido.TrimEnd(' ', ',')}");
    }
}