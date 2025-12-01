using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    public static PrefabManager prefabManager { get; private set; }

    private Dictionary<string, GameObject> prefabs = new();

    void Awake()
    {
        if (prefabManager == null) prefabManager = this;
        CargarPrefabs();
    }

    private void CargarPrefabs()
    {
        CargarPrefab("MenuNivel", "Prefabs/CanvasMenuNiveles");
        CargarPrefab("ConexionPuertas", "Prefabs/Conexion");
        CargarPrefab("Habitacion", "Prefabs/Habitacion");
        CargarPrefab("HabitacionInicial", "Prefabs/HabitacionInicial");
        CargarPrefab("Mazmorra", "Prefabs/Mazmorra");
        CargarPrefab("SalaPrincipal", "Prefabs/SalaPrincipal");
        CargarPrefab("Jugador", "Prefabs/Jugador");
        CargarPrefab("Serpiente", "Prefabs/Serpiente/Serpiente");
        CargarPrefab("Cofre", "Prefabs/Cofre");
        CargarPrefab("Llave", "Prefabs/Llave");
        CargarPrefab("Pared", "Prefabs/Pared");
        CargarPrefab("Suelo", "Prefabs/Suelo");
        CargarPrefab("Jugador", "Prefabs/Jugador");
        CargarPrefab("DetectorJugador", "Prefabs/Serpiente/DetectorJugador");
        CargarPrefab("AreaAtaque", "Prefabs/Serpiente/AreaAtaque");
        CargarPrefab("Z", "Prefabs/Serpiente/Z");
        CargarPrefab("BotonNivel", "Prefabs/BotonNivel");

        CargarPrefab("VentanaPartidasGuardadas", "Prefabs/Menus/VentanaPartidasGuardadas");
        CargarPrefab("VentanaNuevaPartida", "Prefabs/Menus/VentanaNuevaPartida");
        CargarPrefab("VentanaAjustes", "Prefabs/Menus/VentanaAjustes");
        CargarPrefab("VentanaControles", "Prefabs/Menus/VentanaControles");
        CargarPrefab("VentanaPrincipal", "Prefabs/Menus/VentanaPrincipal");
        CargarPrefab("VentanaEditarPartida", "Prefabs/Menus/VentanaEditarPartida");
        CargarPrefab("VentanaEliminarPartida", "Prefabs/Menus/VentanaEliminarPartida");
        CargarPrefab("VentanaMensaje", "Prefabs/Menus/VentanaMensaje");

        CargarPrefab("MenuGameOver", "Prefabs/Menus/MenuGameOver");
        CargarPrefab("MenuPausa", "Prefabs/Menus/MenuPausa");
        CargarPrefab("SlotInventario", "Prefabs/SlotInventario");

        CargarPrefab("BarraEfecto", "Prefabs/BarraEfecto");

        CargarPrefab("GolemPiedra", "Prefabs/GolemPiedra");
        CargarPrefab("Espiritu", "Prefabs/Espiritu");
        CargarPrefab("DetectorEspiritu", "Prefabs/DetectorEspiritu");
        CargarPrefab("ProyectilEspiritu", "Prefabs/ProyectilEspiritu"); 
    }

    private void CargarPrefab(string clave, string ruta) => prefabs[clave] = Resources.Load<GameObject>(ruta);
    
    public GameObject ObtenerPrefab(string clave) => prefabs.TryGetValue(clave, out GameObject prefab) ? prefab : null;
}