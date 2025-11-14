using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PM = PrefabManager;
using IM = IconoManager;
using TMPro;

public abstract class InventarioBaseUI : MonoBehaviour
{
    protected int filas;
    protected int columnas;
    protected float tamano;
    protected float espacio;
    protected float margen;

    protected RectTransform contenido;
    protected GameObject prefabSlot;
    protected List<GameObject> slotsInstanciados = new();

    protected virtual void Awake() => contenido = GetComponentInChildren<RectTransform>(includeInactive: true);

    protected void CrearSlots()
    {
        if (contenido == null) return;
        prefabSlot = PM.prefabManager.ObtenerPrefab("SlotInventario");
        foreach (RectTransform hijo in contenido) Destroy(hijo.gameObject);
        slotsInstanciados.Clear();

        for (int i = 0; i < filas * columnas; i++)
        {
            GameObject slot = Instantiate(prefabSlot, contenido);
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

            slotsInstanciados.Add(slot);
        }
    }

    public void ActualizarInventario(List<ItemStack> inventario)
    {
        CrearSlots();

        int totalSlots = filas * columnas;
        int inicio = Mathf.Max(0, inventario.Count - totalSlots);

        for (int j = 0; j < totalSlots; j++)
        {
            int i = inicio + j;
            GameObject slotGO = slotsInstanciados[j];
            Image icono = slotGO.transform.Find("Icono").GetComponent<Image>();
            TextMeshProUGUI textoCantidad = icono.transform.Find("Cantidad").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI textoCantidad_sombra = icono.transform.Find("Cantidad_sombra").GetComponent<TextMeshProUGUI>();

            if (i >= inventario.Count || inventario[i].vacio) continue;

            ItemStack stack = inventario[i];
            Sprite sprite = IM.iconoManager.ObtenerIcono(stack.item.nombre);

            /*if (icono != null)
            {*/
                icono.sprite = sprite;
                icono.enabled = sprite != null;
                icono.color = Color.white;
            //}

            string cantidad = stack.cantidad > 1 ? stack.cantidad.ToString() : "";
            textoCantidad.text = cantidad;
            textoCantidad_sombra.text = cantidad;
        }
    }
}