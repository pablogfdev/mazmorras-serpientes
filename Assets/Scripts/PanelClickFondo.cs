using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using HUD = InventarioHUDManager;

public class ClickFondoManager : MonoBehaviour
{
    private JugadorController jugador; 

    void Awake() => jugador = GetComponentInParent<JugadorController>();

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && ClickSobreFondo()) jugador.IniciarEstocada(); 
        if (Input.GetMouseButtonDown(1) && ClickSobreFondo()) jugador.IniciarBarrido();  
    }


    //ClickSobreFondo() --> Este metodo no es eficiente, consume mucho, cambiar logica si da tiempo
    private bool ClickSobreFondo()
    {
        if(!HUD.inventarioHUDManager.panelAcceso.activeSelf) return false;
        if(PausaJuegoController.pausaJuegoController.MenuNivelAbierto) return false;    //TESTEAR: POSIBLE ERROR
        if(PausaJuegoController.pausaJuegoController.juegoPausado) return false;
        
        PointerEventData eventData = new PointerEventData(EventSystem.current) { position = Input.mousePosition };

        List<RaycastResult> resultados = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, resultados);

        foreach (var r in resultados)
        {
            if (r.gameObject.GetComponent<SlotUI>() != null || r.gameObject.GetComponent<InventarioBaseUI>() != null) return false;   
        }

        return true;
    }
}
