using System.Collections.Generic;
using System.Diagnostics;

public static class ItemDatabase
{
    public static Dictionary<int, Item> items = new Dictionary<int, Item>();
    public static Item Get(int id) => items[id];
    public static void RegistrarItem(Item item) { if (!items.ContainsKey(item.Id)) items.Add(item.Id, item); }

    static ItemDatabase()
    {
        RegistrarItem(new Item(1, "Poción Curación", true, jugador => jugador.Vida += 10));
        RegistrarItem(new Item(2, "Poción Velocidad", true, jugador => jugador.VelocidadMovimiento += 5));
        RegistrarItem(new Item(3, "Oro", true));
        RegistrarItem(new Item(4, "Gema", true));
        RegistrarItem(new Item(5, "Vendas", true));
    }
}