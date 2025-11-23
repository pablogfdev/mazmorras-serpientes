using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class AccionProlongadaController : MonoBehaviour
{
    public Coroutine corrutinaReloj;
    Image relojImagen;

    public void ActivarCorrutinaBarra(float duracion, JugadorController jugador, SlotUI slot, Action onFinalizar)
    {
        if (corrutinaReloj != null) StopCoroutine(corrutinaReloj);
        corrutinaReloj = StartCoroutine(ActivarCuentaAtras(duracion, jugador, slot, onFinalizar));
    }

    private IEnumerator ActivarCuentaAtras(float duracion, JugadorController jugador, SlotUI slot, Action onFinalizar)
    {
        Vector2 posicionInicial = jugador.rb.position;
        float tiempoRestante = duracion;

        PrepararImagenReloj(slot);
           
        while (tiempoRestante > 0)
        {
            if (!Input.GetMouseButton(1)) CancelarCorrutina();          
            if ((jugador.rb.position - posicionInicial).sqrMagnitude > 0.0001f) CancelarCorrutina();
        
            tiempoRestante -= Time.deltaTime;
            relojImagen.fillAmount = tiempoRestante / duracion;
            yield return null;
        }

        onFinalizar?.Invoke();
        relojImagen.enabled = false;
        corrutinaReloj = null;
    }

    private void CancelarCorrutina()
    {
        if (corrutinaReloj != null) StopCoroutine(corrutinaReloj);
        relojImagen.enabled = false;
        corrutinaReloj = null;
    }

    private void PrepararImagenReloj(SlotUI slot)
    {
        relojImagen = slot.transform.Find("Icono").GetComponent<Image>().transform.Find("RelojProgreso").GetComponent<Image>();        
        relojImagen.fillAmount = 1f;
        relojImagen.enabled = true;
    }
}