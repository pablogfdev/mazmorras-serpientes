using System.Collections.Generic;
using UnityEngine;
using PM = PrefabManager;

public abstract class InventarioBaseUI : MonoBehaviour
{
    private List<GameObject> slotsInstanciados = new();
    protected RectTransform contenido;
    public List<ItemStack> inventario;

    protected float espacio;
    protected int columnas;
    protected float tamano;
    protected float margen;
    protected int filas;

    protected virtual void Awake() => contenido = GetComponentInChildren<RectTransform>(includeInactive: true);

    protected void CrearSlots()
    {
        if (contenido == null || slotsInstanciados.Count > 0) return;
        for (int i = 0; i < filas * columnas; i++)
        {
            GameObject slot = Instantiate(PM.prefabManager.ObtenerPrefab("SlotInventario"), contenido);
            RectTransform slotRect = slot.GetComponent<RectTransform>();

            slotRect.anchorMin = new Vector2(0, 1);
            slotRect.anchorMax = new Vector2(0, 1);
            slotRect.pivot = new Vector2(0, 1);

            int fila = i / columnas;
            int columna = i % columnas;

            float x = margen + columna * (tamano + espacio);
            float y = -margen - fila * (tamano + espacio);

            slotRect.anchoredPosition = new Vector2(x, y);
            slotRect.sizeDelta = new Vector2(tamano, tamano);

            if (i < inventario.Count) slot.AddComponent<SlotUI>().SetInventario(inventario, i); 
            slotsInstanciados.Add(slot);
        }
    }

    public void ActualizarInventario(List<ItemStack> listaInventario)
    {
        inventario = listaInventario;
        CrearSlots();

        int totalSlots = filas * columnas;
        int inicio = Mathf.Max(0, inventario.Count - totalSlots);

        for (int j = 0; j < totalSlots; j++)
        {
            int i = inicio + j;
            if (i >= inventario.Count) continue;
            SlotUI slotUI = slotsInstanciados[j].GetComponent<SlotUI>();
            if (slotUI != null) slotUI.SetInventario(inventario, i);         
        }
    }

    public int DetectarPrimerSlotApilable(Item item)
    {
        if (item == null) return -1;

        for (int i = 0; i < inventario.Count; i++)
        {
            ItemStack stack = inventario[i];
            if (stack != null && !stack.vacio && stack.item.Id == item.Id && stack.cantidad < stack.item.maxStack) return i;
        }
        return -1;
    }

    public int DetectarPrimerSlotVacio()
    {
        for (int i = 0; i < inventario.Count; i++)
        {
            ItemStack stack = inventario[i];
            if (stack == null || stack.vacio) return i;
        }
        return -1;
    }
}