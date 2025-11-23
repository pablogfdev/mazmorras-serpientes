using System.Collections.Generic;

public class InventarioController
{
    public int tamano;
    public List<ItemStack> slots;

    public InventarioController(int t = 30)
    {
        tamano = t;
        slots = new List<ItemStack>(new ItemStack[tamano]);
        for (int i = 0; i < tamano; i++) slots[i] = new ItemStack(null, 0);
    }
}