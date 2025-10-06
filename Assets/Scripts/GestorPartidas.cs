using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class Partida
{
    public string id = Guid.NewGuid().ToString();
    public string nombre;
    public int nivel = 1;
}

[Serializable]
public class ListaPartidas { public List<Partida> partidas = new List<Partida>(); }

public static class GestorPartidas
{
    private static readonly string rutaArchivo = Path.Combine(Application.persistentDataPath, "partidas.json");
    private static ListaPartidas listaPartidas;
    public static Partida partidaActiva { get; private set; }


    public static void ObtenerPartidas()
    {
        if (!File.Exists(rutaArchivo)) { GuardarPartidasEnArchivo(); }
        string json = File.ReadAllText(rutaArchivo);
        listaPartidas = JsonUtility.FromJson<ListaPartidas>(json) ?? new ListaPartidas();
    }

    public static void GuardarPartidasEnArchivo()
    {
        if (listaPartidas == null) listaPartidas = new ListaPartidas();
        string json = JsonUtility.ToJson(listaPartidas, true);
        File.WriteAllText(rutaArchivo, json);
    }

    public static void CrearNuevaPartida()
    {
        ObtenerPartidas();
        partidaActiva = new Partida { nombre = $"Partida {listaPartidas.partidas.Count + 1}" };
        listaPartidas.partidas.Add(partidaActiva);
        GuardarPartidasEnArchivo();
    }

    public static void CargarPartida(string id)
    {
        ObtenerPartidas();
        partidaActiva = listaPartidas.partidas.Find(p => p.id == id);
    }

    public static void SubirNivel(int nivelActual)
    {
        if (nivelActual == partidaActiva.nivel)
        {
            partidaActiva.nivel++;
            GuardarPartidasEnArchivo();
        }
    }

    public static List<Partida> ListasPartidas()
    {
        ObtenerPartidas();
        return listaPartidas.partidas;
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
        partidaActiva = null;
        ObtenerPartidas();
        Partida partida = listaPartidas.partidas.Find(p => p.id == id);
        partida.nombre = nuevoNombre;
        GuardarPartidasEnArchivo();
    }
}