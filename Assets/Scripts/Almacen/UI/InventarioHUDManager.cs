using System.Collections.Generic;
using UnityEngine;

public class InventarioHUDManager : MonoBehaviour
{
    public static InventarioHUDManager inventarioHUDManager;
    public InventarioUIController inventarioJugadorEnAlmacenUI;
    public InventarioUIController inventarioJugadorUI;
    public InventarioUIController inventarioAlmacenUI;
    public AccesoRapidoUIController accesoRapidoUI;
    public GameObject panelJugadorSolo;
    public GameObject panelComerciante;
    public GameObject panelJugador;
    public GameObject panelAlmacen;
    public GameObject panelAcceso;

    void Awake()
    {
        inventarioHUDManager = this;

        panelJugadorSolo = GameObject.Find("Panel_inventario_Jugador");
        panelComerciante = GameObject.Find("Panel_Comerciante");
        panelAcceso = GameObject.Find("Panel_acceso_rapido");
        panelJugador = GameObject.Find("Panel_Jugador");
        panelAlmacen = GameObject.Find("Panel_Almacen");

        inventarioJugadorEnAlmacenUI = panelJugador.GetComponentInChildren<InventarioUIController>();
        inventarioJugadorUI = panelJugadorSolo.GetComponentInChildren<InventarioUIController>();
        accesoRapidoUI = panelAcceso.GetComponentInChildren<AccesoRapidoUIController>();
        inventarioAlmacenUI = panelAlmacen.GetComponentInChildren<InventarioUIController>();
        CerrarInventarios();
    }

    public void MostrarInventarioJugador(List<ItemStack> inventarioJugador)
    {
        CerrarInventarios();
        panelJugadorSolo.SetActive(true);
        inventarioJugadorUI?.ActualizarInventario(inventarioJugador);
    }

    public void MostrarInventarioJugadorYAlmacen(List<ItemStack> inventarioJugador, List<ItemStack> inventarioAlmacen)
    {
        CerrarInventarios();
        panelJugador.SetActive(true);
        panelAlmacen.SetActive(true);
        inventarioJugadorEnAlmacenUI.ActualizarInventario(inventarioJugador);
        inventarioAlmacenUI.ActualizarInventario(inventarioAlmacen);
    }

    public void MostrarAccesoRapido(List<ItemStack> inventarioJugador)
    {
        CerrarInventarios();
        panelAcceso.SetActive(true);
        accesoRapidoUI.ActualizarInventario(inventarioJugador);
    }

    public void MostrarInventarioYComerciante(List<ItemStack> inventarioJugador)
    {
        CerrarInventarios();
        panelJugador.SetActive(true);
        panelComerciante.SetActive(true);
        inventarioJugadorEnAlmacenUI.ActualizarInventario(inventarioJugador);
    }

    public void CerrarInventarios()
    {
        panelJugadorSolo.SetActive(false);
        panelJugador.SetActive(false);
        panelAlmacen.SetActive(false);
        panelAcceso.SetActive(false);  
        panelComerciante.SetActive(false);  
        GestorPartidas.GuardarInventarios();
    }
}