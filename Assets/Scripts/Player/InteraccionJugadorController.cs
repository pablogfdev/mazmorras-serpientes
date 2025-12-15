using UnityEngine;
using System.Collections.Generic;
using HUD = InventarioHUDManager;
using PJC = PausaJuegoController;
using PM = PrefabManager;
using EC = EscenasController;
public class InteraccionJugadorController : MonoBehaviour
{
    private List<string> tagsValidos = new() { "Puerta", "Cofre", "Taquilla", "Entrada", "Salida", "Comerciante" };
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
        if (objetoCercano.tag == "Comerciante") AccionarComerciante();
        if ((objetoCercano.tag == "Cofre" || objetoCercano.tag == "Taquilla") && inventarioCerrado) AccionarInventarioAlmacen();
        if (objetoCercano.tag == "Cofre") objetoCercano.GetComponent<CofreScriptController>().AbrirCofre();
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
            AudioSource.PlayClipAtPoint(SonidoManager.sonidoManager.ObtenerSonido("Abrir_Puerta"), transform.position);
            Instantiate(PM.prefabManager.ObtenerPrefab("MenuNivel"), transform.position, Quaternion.identity);
            PJC.pausaJuegoController.ToggleMenuNiveles();
        }
        return;
    }

    void AccionarSalidaMazmorra()
    { 
        if (jugador.GetComponent<InventarioJugador>().inventario.ContieneLlave())
        {
            jugador.GetComponent<InventarioJugador>().inventario.EliminarLlave();
            GestorPartidas.SubirNivel(GameObject.FindWithTag("Mazmorra").GetComponentInChildren<MazmorraController>().Nivel);
            EC.escenasController.CargarEscenaComerciante();
        }
        else AudioSource.PlayClipAtPoint(SonidoManager.sonidoManager.ObtenerSonido("Error"), Camera.main.transform.position);
        return;
    }

    void AccionarInventarioAlmacen()
    {
        AudioSource.PlayClipAtPoint(SonidoManager.sonidoManager.ObtenerSonido("Abrir_Cofre"), transform.position);
        var almacen = objetoCercano.GetComponent<InterfazAlmacen>();
        HUD.inventarioHUDManager.CerrarInventarios();
        HUD.inventarioHUDManager.MostrarInventarioJugadorYAlmacen(inventarioJugador.inventario.slots, almacen.inventario.slots);
        jugador.BloquearMovimiento();
        return;
    }

    void AccionarComerciante()
    {
        HUD.inventarioHUDManager.CerrarInventarios();
        HUD.inventarioHUDManager.MostrarInventarioYComerciante(inventarioJugador.inventario.slots);
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