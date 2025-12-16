using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class ComercianteController : MonoBehaviour
{
    private List<ItemVenta> itemsEnVenta = new();
    private InventarioController inventarioJugador;
    private int moneda;
    private int cantidadMonedas;


    void Awake()
    {
        inventarioJugador = GameObject.FindGameObjectWithTag("Player").GetComponent<InventarioJugador>().inventario;
        CrearCatalogo();
        InicializarBotones();
    }

    void CrearCatalogo()
    {
        itemsEnVenta = new List<ItemVenta>
        {
            new ItemVenta(1, TipoMoneda.Oro, 30),
            new ItemVenta(2, TipoMoneda.Oro, 100),
            new ItemVenta(3, TipoMoneda.Gema, 100),
            new ItemVenta(4, TipoMoneda.Gema, 25),
            new ItemVenta(5, TipoMoneda.Gema, 50),
            new ItemVenta(6, TipoMoneda.Gema, 50),
            new ItemVenta(7, TipoMoneda.Gema, 50),
        };
    }

    void InicializarBotones()
    {
        foreach (var producto in itemsEnVenta)
        {
            Button boton = transform.Find(ItemDatabase.Get(producto.id).nombre).GetComponentInChildren<Button>();

            TextMeshProUGUI textoBoton = boton.GetComponentInChildren<TextMeshProUGUI>();
            textoBoton.text = $"{(producto.moneda == TipoMoneda.Oro ? "Oro" : "Gemas")} x {producto.precio}";

            boton.onClick.RemoveAllListeners();
            boton.onClick.AddListener(() => ComprarObjeto(producto));
        }
    }

    void ComprarObjeto(ItemVenta producto)
    {
        Item item = ItemDatabase.Get(producto.id);
        moneda = producto.moneda == TipoMoneda.Oro ? 8 : 9;
        ContarMonedasJugador(moneda);

        if (!TieneMonedasSuficientes(producto.precio)) return;
        if (!AgregarItemAlInventario(item)) return;

        RestarMoneda(producto);
        AudioSource.PlayClipAtPoint(SonidoManager.sonidoManager.ObtenerSonido("Compra"), Camera.main.transform.position);
        GestorPartidas.GuardarInventarios();
        InventarioHUDManager.inventarioHUDManager.inventarioJugadorEnAlmacenUI.ActualizarInventario(inventarioJugador.slots);
    }

    bool TieneMonedasSuficientes(int precio)
    {
        if (precio > cantidadMonedas) AudioSource.PlayClipAtPoint(SonidoManager.sonidoManager.ObtenerSonido("Error"), Camera.main.transform.position);
        return cantidadMonedas >= precio;
    }

    void RestarMoneda(ItemVenta producto)
    {
        int precioPendiente = producto.precio;

        for (int i = 0; i < inventarioJugador.slots.Count && precioPendiente > 0; i++)
        {
            ItemStack stackActual = inventarioJugador.slots[i];

            if (stackActual.vacio || stackActual.item.Id != moneda)continue;
            int cantidadARestar = Mathf.Min(stackActual.cantidad, precioPendiente);

            stackActual.cantidad -= cantidadARestar;
            precioPendiente -= cantidadARestar;

            if (stackActual.cantidad <= 0) inventarioJugador.slots[i] = new ItemStack(null, 0);
        }
    }

    void ContarMonedasJugador(int itemId)
    {
        cantidadMonedas = 0;
        foreach (var stack in inventarioJugador.slots) if (!stack.vacio && stack.item.Id == itemId)  cantidadMonedas += stack.cantidad;
    }

    bool AgregarItemAlInventario(Item item)
    {
        if (item.stackeable)
        {
            foreach (var stack in inventarioJugador.slots)
            {
                if (!stack.vacio && stack.item.Id == item.Id && stack.cantidad < item.maxStack)
                {
                    stack.cantidad++;
                    return true;
                }
            }
        }

        for (int i = 0; i < inventarioJugador.slots.Count; i++)
        {
            if (inventarioJugador.slots[i].vacio)
            {
                inventarioJugador.slots[i] = new ItemStack(item, 1);
                return true;
            }
        }

        AudioSource.PlayClipAtPoint(SonidoManager.sonidoManager.ObtenerSonido("Error"), Camera.main.transform.position);
        return false;
    }
}