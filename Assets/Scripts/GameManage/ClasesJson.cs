using System;
using System.Collections.Generic;

[Serializable] public class ItemGuardado
{
    public int id;
    public int cantidad;

    public ItemGuardado(int id, int cantidad)
    {
        this.id = id;
        this.cantidad = cantidad;
    }
}

[Serializable] public class Partida
{
    public string id = Guid.NewGuid().ToString();
    public string nombre;
    public int nivel = 1;

    public List<ItemGuardado> inventarioJugador = new List<ItemGuardado>();
    public List<ItemGuardado> inventarioTaquilla = new List<ItemGuardado>();
}

[Serializable] public class ListaPartidas { public List<Partida> partidas = new List<Partida>(); }