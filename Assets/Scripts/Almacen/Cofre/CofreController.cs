using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CofreController : MonoBehaviour, InterfazAlmacen
{
    public InventarioController inventario { get; private set; } = new InventarioController();
    private int nivel = 1;
    private bool generado = false;
    public bool llave = false;

    List<Item> itemsFiltrados;

    private void Awake()
    {
        MazmorraController mazmorra = GetComponentInParent<MazmorraController>();
        if (mazmorra != null) nivel = mazmorra.Nivel;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!generado && other.CompareTag("Player"))
        {
            generado = true;
            CrearInventarioAleatorio();
        }
    }

    public void CrearInventarioAleatorio()
    {
        inventario.slots.Clear();
        FiltrarItemsPorNivel(nivel);


        foreach (var item in itemsFiltrados)
        {
            float probabilidad = Random.Range(0f, 100f);
            if (probabilidad <= item.probabilidadAparacición)
            {
                int cantidad = CalcularCantidad(item);
                inventario.slots.Add(new ItemStack(item, cantidad));
            }
        }
        CrearLlave();
        while (inventario.slots.Count < inventario.tamano) inventario.slots.Add(new ItemStack(null, 0));
        DividirMontones();
        DispersarItems();
    }

    private void CrearLlave() { if (llave) inventario.slots.Add(new ItemStack(ItemDatabase.Get(10), 1)); }

    private void FiltrarItemsPorNivel(int nivel)
    {
        itemsFiltrados = new List<Item>();
        foreach (var itemDB in ItemDatabase.items)
        {
            Item item = itemDB.Value;
            if (item.nivelAparicion <= nivel && item.Id != 10) itemsFiltrados.Add(item);
        }
    }

    private int CalcularCantidad(Item item)
    {
        if (!item.stackeable) return 1;
        int valorMax = Mathf.CeilToInt(item.probabilidadAparacición) / 10;
        valorMax = Mathf.Max(1, valorMax);
        valorMax = (item.nombre == "Oro") ? valorMax * 10 : (item.nombre == "Gema") ? valorMax * 5 : valorMax;
        return Random.Range(2, valorMax + 1);
    }

    private void DividirMontones()
    {
        List<ItemStack> nuevosStacks = new List<ItemStack>();

        foreach (var stack in inventario.slots)
        {
            if (stack.item == null || stack.cantidad <= 1)
            {
                nuevosStacks.Add(stack);
                continue;
            }

            int cantidad = stack.cantidad;

            if (cantidad > 15)
            {
                int a = Random.Range(1, cantidad - 1);
                int b = Random.Range(1, cantidad - a);
                int c = cantidad - a - b;

                nuevosStacks.Add(new ItemStack(stack.item, a));
                nuevosStacks.Add(new ItemStack(stack.item, b));
                nuevosStacks.Add(new ItemStack(stack.item, c));
            }
            else if (cantidad > 5)
            {
                int a = Random.Range(1, cantidad);
                int b = cantidad - a;

                nuevosStacks.Add(new ItemStack(stack.item, a));
                nuevosStacks.Add(new ItemStack(stack.item, b));
            }
            else nuevosStacks.Add(stack);

        }

        inventario.slots.Clear();
        foreach (var stack in nuevosStacks)
        {
            inventario.slots.Add(stack);
            if (inventario.slots.Count >= inventario.tamano) break;
        }

        while (inventario.slots.Count < inventario.tamano) inventario.slots.Add(new ItemStack(null, 0));
    }

    private void DispersarItems()
    {
        List<ItemStack> items = new List<ItemStack>();
        foreach (var stack in inventario.slots) if (stack != null && stack.item != null && stack.cantidad > 0) items.Add(stack);
        

        inventario.slots.Clear();
        for (int i = 0; i < inventario.tamano; i++) inventario.slots.Add(new ItemStack(null, 0));

        foreach (var stack in items)
        {
            int intentos = 0;
            int index = Random.Range(0, inventario.tamano);

            while (inventario.slots[index].item != null && intentos < inventario.tamano)
            {
                index = (index + 1) % inventario.tamano;
                intentos++;
            }

            inventario.slots[index] = stack;
        }
    }
}