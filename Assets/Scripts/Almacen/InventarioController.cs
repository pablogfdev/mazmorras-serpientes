using System.Collections.Generic;
using UnityEngine;

public class InventarioController
{
    private int tamano = 30;
    public List<ItemStack> slots;

    public InventarioController()
    {
        slots = new List<ItemStack>(new ItemStack[tamano]);
        for (int i = 0; i < tamano; i++) slots[i] = new ItemStack(null, 0);
    }
}