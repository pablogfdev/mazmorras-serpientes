using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public class SlotUI : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Image icono;
    private TextMeshProUGUI textoCantidad;
    private TextMeshProUGUI textoCantidadSombra;
    private Image imagenCuentaAtras;

    private Canvas canvas;
    private GameObject iconoDeArrastre;
    private bool enArrastre = false;
    private static List<SlotUI> SlotsEnArrastre = new();

    private AccionProlongadaController accionProlongadaController;

    private List<ItemStack> inventarioSlot;
    private int indice;

    private InventarioBaseUI inventarioOrigen;
    private InventarioBaseUI inventarioDestino;
    private JugadorController jugador;

    public ItemStack StackActual => (inventarioSlot != null && indice < inventarioSlot.Count) ? inventarioSlot[indice] : null;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        if (icono == null) icono = transform.Find("Icono").GetComponent<Image>();
        if (textoCantidad == null) textoCantidad = icono.transform.Find("Cantidad").GetComponent<TextMeshProUGUI>();
        if (textoCantidadSombra == null) textoCantidadSombra = icono.transform.Find("Cantidad_sombra").GetComponent<TextMeshProUGUI>();
        inventarioOrigen = GetComponentInParent<InventarioBaseUI>();
        jugador = GetComponentInParent<JugadorController>();
        imagenCuentaAtras = icono.transform.Find("RelojProgreso").GetComponent<Image>();
        imagenCuentaAtras.fillAmount = 0f;
        imagenCuentaAtras.enabled = false;
        accionProlongadaController = gameObject.AddComponent<AccionProlongadaController>();
    }

    public void SetInventario(List<ItemStack> inventario, int indice)
    {
        inventarioSlot = inventario;
        this.indice = indice;
        ActualizarUI();
    }

    public void ActualizarUI()
    {
        var stack = StackActual;
        if (stack == null || stack.vacio)
        {
            icono.sprite = null;
            icono.enabled = false;
            textoCantidad.text = "";
            textoCantidadSombra.text = "";
            return;
        }

        icono.sprite = IconoManager.iconoManager.ObtenerIcono(stack.item.nombre);
        icono.enabled = true;
        textoCantidad.text = stack.cantidad > 1 ? stack.cantidad.ToString() : "";
        textoCantidadSombra.text = stack.cantidad > 1 ? stack.cantidad.ToString() : "";
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ItemStack stack = StackActual;

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            if (stack == null || stack.vacio) return;
            if (ElegirInventarioDestino() == false) return;
            MoverStackInstantaneo(stack);
            ActualizarUI();
            inventarioDestino.ActualizarInventario(inventarioDestino.inventario);
            return;
        }

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (stack == null || stack.vacio || stack.item.efecto == null) return;
         
            accionProlongadaController.ActivarCorrutinaBarra(
                stack.item.tiempoDeEspera.Value,
                jugador,
                this, 
                () =>
                {
                    stack.item.efecto?.Invoke(jugador);
                    stack.cantidad--;
                    if (stack.cantidad <= 0)inventarioSlot[indice] = new ItemStack(null, 0);
                    ActualizarUI();
                    GestorPartidas.GuardarInventarios();
                }
            );
            return;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (enArrastre ||accionProlongadaController.corrutinaReloj != null) return;

        ItemStack stack = StackActual;
        if (stack == null || stack.vacio) return;
        if (!SlotsEnArrastre.Contains(this)) SlotsEnArrastre.Add(this);
        foreach (var slot in new List<SlotUI>(SlotsEnArrastre)) if (slot != this && slot.StackActual == stack) slot.ForzarCerrarIconoArraste();

        enArrastre = true;
        iconoDeArrastre = new GameObject("iconoDeArrastre");
        iconoDeArrastre.transform.SetParent(canvas.transform, false);
        iconoDeArrastre.transform.SetAsLastSibling();

        Image imagen = iconoDeArrastre.AddComponent<Image>();
        imagen.sprite = icono.sprite;
        imagen.raycastTarget = false;
        imagen.SetNativeSize();

        CanvasGroup arrastreCanvasGroup = iconoDeArrastre.AddComponent<CanvasGroup>();
        arrastreCanvasGroup.blocksRaycasts = false;
        arrastreCanvasGroup.alpha = 0.8f;

        iconoDeArrastre.transform.position = eventData.position;
    }

    public void OnDrag(PointerEventData eventData) { if (iconoDeArrastre != null) iconoDeArrastre.transform.position = eventData.position; }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(accionProlongadaController.corrutinaReloj != null) return;
        if (iconoDeArrastre != null)
        {
            Destroy(iconoDeArrastre);
            iconoDeArrastre = null;
        }

        SlotsEnArrastre.Remove(this);
        enArrastre = false;

        SlotUI destino = DetectarSlotDestino(eventData);
        if (destino != null && destino != this)
        {
            int cantidadAMover = StackActual.cantidad;
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) cantidadAMover = Mathf.CeilToInt(StackActual.cantidad / 2f);
            if (Input.GetKey(KeyCode.Z)) cantidadAMover = 1;
            if(accionProlongadaController.corrutinaReloj == null) MoverStack(destino, cantidadAMover);
        }
    }

    public void ForzarCerrarIconoArraste()
    {
        if (iconoDeArrastre != null)
        {
            Destroy(iconoDeArrastre);
            iconoDeArrastre = null;
        }
        SlotsEnArrastre.Remove(this);
    }

    private void MoverStack(SlotUI destino, int cantidadAMover)
    {
        var stackOrigen = StackActual;
        var stackDestino = destino.StackActual;

        if (stackOrigen == null || stackOrigen.vacio) return;

        if (stackDestino == null || stackDestino.vacio)
        {
            destino.inventarioSlot[destino.indice] = new ItemStack(stackOrigen.item, cantidadAMover);
            stackOrigen.cantidad -= cantidadAMover;
            if (stackOrigen.cantidad <= 0)
                inventarioSlot[indice] = new ItemStack(null, 0);
        }
        else if (!stackOrigen.item.stackeable || !stackDestino.item.stackeable || stackOrigen.item.Id != stackDestino.item.Id)
        {
            destino.inventarioSlot[destino.indice] = stackOrigen;
            inventarioSlot[indice] = stackDestino;
        }
        else
        {
            int moverFinal = Mathf.Min(cantidadAMover, stackOrigen.item.maxStack - stackDestino.cantidad);

            if (moverFinal > 0)
            {
                destino.inventarioSlot[destino.indice].cantidad += moverFinal;
                stackOrigen.cantidad -= moverFinal;
                if (stackOrigen.cantidad <= 0) inventarioSlot[indice] = new ItemStack(null, 0);
            }
        }

        ActualizarUI();
        destino.ActualizarUI();
    }

    private bool ElegirInventarioDestino()
    {
        switch (inventarioOrigen.name)
        {
            case "Panel_Almacen":
                inventarioDestino = InventarioHUDManager.inventarioHUDManager.inventarioJugadorEnAlmacenUI;
                break;

            case "Panel_Jugador":
                inventarioDestino = InventarioHUDManager.inventarioHUDManager.inventarioAlmacenUI;
                break;

            case "Panel_inventario_Jugador": break;
            case "Panel_acceso_rapido": break;
        }
        return inventarioDestino != null;
    }

    private void MoverStackInstantaneo(ItemStack stackOrigen)
    {
        while (stackOrigen.cantidad > 0)
        {
            int indiceSlotApilable = inventarioDestino.DetectarPrimerSlotApilable(stackOrigen.item);
            if (indiceSlotApilable != -1)
            {
                var destinoStack = inventarioDestino.inventario[indiceSlotApilable];
                int espacio = destinoStack.item.maxStack - destinoStack.cantidad;
                int mover = Mathf.Min(stackOrigen.cantidad, espacio);

                destinoStack.cantidad += mover;
                stackOrigen.cantidad -= mover;
                continue;
            }

            int indiceSlotVacio = inventarioDestino.DetectarPrimerSlotVacio();
            if (indiceSlotVacio != -1)
            {
                inventarioDestino.inventario[indiceSlotVacio] = new ItemStack(stackOrigen.item, stackOrigen.cantidad);
                stackOrigen.cantidad = 0;
            }
            break;
        }
    }

    private SlotUI DetectarSlotDestino(PointerEventData eventData)
    {
        var resultados = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, resultados);

        foreach (var resultado in resultados)
        {
            SlotUI slot = resultado.gameObject.GetComponent<SlotUI>();
            if (slot != null) return slot;
        }
        return null;
    }
}