using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class GestorPartidas
{
    private static readonly string rutaArchivo = Path.Combine(Application.persistentDataPath, "partidas.json");
    private static ListaPartidas listaPartidas;
    public static Partida partidaActiva { get; private set; }
    public static InventarioController inventarioJugador;
    public static InventarioController inventarioTaquilla;

    public static void CrearNuevaPartida()
    {
        ObtenerPartidas();
        partidaActiva = new Partida{ nombre = $"Partida {listaPartidas.partidas.Count + 1}" };
        listaPartidas.partidas.Add(partidaActiva);
        GuardarPartidasEnArchivo();
        InstanciarInventarios();
    }

    public static void CargarPartida(string id)
    {
        ObtenerPartidas();
        partidaActiva = listaPartidas.partidas.Find(p => p.id == id);
        InstanciarInventarios();
        CargarInventario(partidaActiva.inventarioJugador, inventarioJugador);
        CargarInventario(partidaActiva.inventarioTaquilla, inventarioTaquilla);
    }

    public static void SubirNivel(int nivelActual)
    {
        if (partidaActiva == null) return;
        if (nivelActual == partidaActiva.nivel)
        {
            partidaActiva.nivel++;
            GuardarInventario(inventarioJugador, partidaActiva.inventarioJugador);
            GuardarPartidasEnArchivo();
        }
    }

    public static List<Partida> ListasPartidas()
    {
        ObtenerPartidas();
        return listaPartidas.partidas;
    }

    private static void InstanciarInventarios()
    {
        inventarioJugador = new InventarioController();
        inventarioTaquilla = new InventarioController();
    }

    private static void ObtenerPartidas()
    {
        if (!File.Exists(rutaArchivo)) GuardarPartidasEnArchivo();
        string json = File.ReadAllText(rutaArchivo);
        listaPartidas = JsonUtility.FromJson<ListaPartidas>(json) ?? new ListaPartidas();
    }

    private static void GuardarInventario(InventarioController inventario, List<ItemGuardado> inventarioGuardado)
    {
        inventarioGuardado.Clear();
        foreach (var slot in inventario.slots)
        {
            if (slot == null || slot.vacio || slot.item == null) continue;
            inventarioGuardado.Add(new ItemGuardado(slot.item.Id, slot.cantidad));
        }
    }
    
    private static void CargarInventario(List<ItemGuardado> origen, InventarioController inventario)
    {
        for (int i = 0; i < origen.Count && i < inventario.slots.Count; i++)
        {
            var item = ItemDatabase.Get(origen[i].id);
            inventario.slots[i] = new ItemStack(item, origen[i].cantidad);
        }
    }

    private static void GuardarPartidasEnArchivo()
    {
        if (listaPartidas == null) listaPartidas = new ListaPartidas();
        string json = JsonUtility.ToJson(listaPartidas, true);
        File.WriteAllText(rutaArchivo, json);
    }

    public static void EliminarPartida(string id)
    {
        ObtenerPartidas();
        Partida partida = listaPartidas.partidas.Find(p => p.id == id);
        listaPartidas.partidas.Remove(partida);
        if (partidaActiva == partida) partidaActiva = null;
        GuardarPartidasEnArchivo();
    }

    public static void EditarNombrePartida(string id, string nuevoNombre)
    {
        ObtenerPartidas();
        Partida partida = listaPartidas.partidas.Find(p => p.id == id);
        partida.nombre = nuevoNombre;
        GuardarPartidasEnArchivo();
    }
}