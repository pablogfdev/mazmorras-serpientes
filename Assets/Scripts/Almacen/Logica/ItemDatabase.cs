using System.Collections.Generic;

public static class ItemDatabase
{
    public static Dictionary<int, Item> items = new Dictionary<int, Item>();
    public static Item Get(int id) => items[id];
    public static void RegistrarItem(Item item) { if (!items.ContainsKey(item.Id)) items.Add(item.Id, item); }

    static ItemDatabase()
    {
        RegistrarItem(new Item(1,  "Vendas", true, 20, 1.5f, jugador => jugador.Vida += 15));
        RegistrarItem(new Item(2,  "Botiquin", true, 20, 2.5f, jugador => jugador.Vida += 80));
        RegistrarItem(new Item(3,  "Pocion de Vida", true, 20, 2f, jugador => jugador.RegenerarVida(5, 10)));
        RegistrarItem(new Item(4,  "Antidoto", true, 20, 2f, jugador => jugador.ObtenerInmunidad(10)));
        RegistrarItem(new Item(5,  "Pocion de Velocidad", true, 20, 2f, jugador => jugador.AumentarVelocidad(2, 10)));
        RegistrarItem(new Item(6,  "Pocion de Fuerza", true, 20, 2f, jugador => jugador.AumentarDanio(2, 30)));
        RegistrarItem(new Item(7,  "Pocion de Defensa", true, 20, 2f, jugador => jugador.AumentarDefensa(0.5f, 5)));
        RegistrarItem(new Item(8,  "Oro", true, 999));
        RegistrarItem(new Item(9,  "Gema", true, 999));
        RegistrarItem(new Item(10, "Llave", false, 1));
    }
}