using System;
using System.Collections.Generic;

[Serializable] public class ItemGuardado
{
    public int id;
    public int cantidad;
    public int slotIndex;

    public ItemGuardado(int id, int cantidad, int slotIndex)
    {
        this.id = id;
        this.cantidad = cantidad;
        this.slotIndex = slotIndex;
    }
}

[Serializable] public class Partida
{
    public string id = Guid.NewGuid().ToString();
    public string nombre;
    public int nivel = 1;
    public Dificultad dificultad = Dificultad.Normal; // valor por defecto

    public List<ItemGuardado> inventarioJugador = new List<ItemGuardado>();
    public List<ItemGuardado> inventarioTaquilla = new List<ItemGuardado>();
}

[Serializable] public class ListaPartidas { public List<Partida> partidas = new List<Partida>(); }