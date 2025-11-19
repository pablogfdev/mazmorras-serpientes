using System.Collections.Generic;

public static class ItemDatabase
{
    public static Dictionary<int, Item> items = new Dictionary<int, Item>();
    public static Item Get(int id) => items[id];
    public static void RegistrarItem(Item item) { if (!items.ContainsKey(item.Id)) items.Add(item.Id, item); }

    static ItemDatabase()
    {
        RegistrarItem(new Item(1, "Poción Curación", true, 20, jugador => jugador.Vida += 10));
        RegistrarItem(new Item(2, "Poción Velocidad", true, 20, jugador => jugador.VelocidadMovimiento += 5));
        RegistrarItem(new Item(3, "Oro", true, 999));
        RegistrarItem(new Item(4, "Gema", true, 100));
        RegistrarItem(new Item(5, "Vendas", true, 20));
        RegistrarItem(new Item(6, "Llave", false, 1));
    }
}