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

    public bool ContieneLlave()
    {
        foreach (var stack in slots)
        {
            if (stack != null && stack.item != null && stack.item.Id == 10 && stack.cantidad > 0) return true;
        }
        return false;
    }

    public void EliminarLlave()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            var stack = slots[i];

            if (stack != null && stack.item != null && stack.item.Id == 10)
            {
                slots[i] = new ItemStack(null, 0); 
                return;
            }
        }
    }
}