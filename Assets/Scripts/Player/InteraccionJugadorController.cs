using UnityEngine;
using System.Collections.Generic;
using HUD = InventarioHUDManager;
using PJC = PausaJuegoController;
using PM = PrefabManager;
using EC = EscenasController;
public class InteraccionJugadorController : MonoBehaviour
{
    private List<string> tagsValidos = new() { "Puerta", "Cofre", "Taquilla", "Entrada", "Salida" };
    private InventarioJugador inventarioJugador;
    public JugadorController jugador;
    private GameObject objetoCercano;

    void Awake() => inventarioJugador = gameObject.transform.parent.GetComponent<InventarioJugador>();
    void Start() => HUD.inventarioHUDManager.MostrarAccesoRapido(inventarioJugador.inventario.slots);
    void Update() => InteraccionJugador();
    void OnTriggerEnter2D(Collider2D other) { if (tagsValidos.Contains(other.tag)) objetoCercano = other.gameObject; }
    void OnTriggerExit2D(Collider2D other) { if (tagsValidos.Contains(other.tag)) objetoCercano = null; }

    void InteraccionJugador()
    {
        if (!Input.GetKeyDown(KeyCode.E)) return;
        bool inventarioCerrado = HUD.inventarioHUDManager.panelAcceso.activeSelf;
        jugador = GetComponentInParent<JugadorController>();
        if (objetoCercano != null) { AccionarObjetoConTag(); return; }
        if (!inventarioCerrado) AccionarAccesoRapido();
        if (inventarioCerrado) AccionarInventarioJugador();
    }

    void AccionarObjetoConTag()
    {
        bool inventarioCerrado = HUD.inventarioHUDManager.panelAcceso.activeSelf;
        if (objetoCercano.tag == "Puerta") AccionarPuerta();
        if (objetoCercano.tag == "Entrada") AccionarEntradaMazmorra();
        if (objetoCercano.tag == "Salida") AccionarSalidaMazmorra();
        if ((objetoCercano.tag == "Cofre" || objetoCercano.tag == "Taquilla") && inventarioCerrado) AccionarInventarioAlmacen();
        if (!inventarioCerrado) AccionarAccesoRapido();
    }

    void AccionarPuerta()
    {
        jugador.transform.position = objetoCercano.transform.parent.Find((objetoCercano.name == "PuertaA") ? "PuertaB" : "PuertaA").position;
        return;
    }

    void AccionarEntradaMazmorra()  //Rediseñar
    {
        if (!PJC.pausaJuegoController.MenuNivelAbierto && Time.timeScale != 0f)
        {
            Instantiate(PM.prefabManager.ObtenerPrefab("MenuNivel"), transform.position, Quaternion.identity);
            PJC.pausaJuegoController.ToggleMenuNiveles();
            return;
        }
        //HUD.inventarioHUDManager.CerrarInventarios();
        return;
    }

    void AccionarSalidaMazmorra()
    {
        if (jugador.GetComponent<JugadorController>().LlaveObtenida)
        {
            GestorPartidas.SubirNivel(GameObject.FindWithTag("Mazmorra").GetComponentInChildren<MazmorraController>().Nivel);
            EC.escenasController.CargarEscenaComerciante();
        }
        else { Debug.Log("El jugador no tiene la llave."); }
        return;
    }

    void AccionarInventarioAlmacen()
    {
        var almacen = objetoCercano.GetComponent<InterfazAlmacen>();
        HUD.inventarioHUDManager.CerrarInventarios();
        HUD.inventarioHUDManager.MostrarInventarioJugadorYAlmacen(inventarioJugador.inventario.slots, almacen.inventario.slots);
        jugador.BloquearMovimiento();
        return;
    }

    void AccionarAccesoRapido()
    {
        HUD.inventarioHUDManager.MostrarAccesoRapido(inventarioJugador.inventario.slots);
        jugador.DesbloquearMovimiento();
        return;
    }

    void AccionarInventarioJugador()
    {
        HUD.inventarioHUDManager.MostrarInventarioJugador(inventarioJugador.inventario.slots);
        jugador.BloquearMovimiento();
        return;
    }
}