using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BarraEfectoProgresoController : MonoBehaviour
{
    public static BarraEfectoProgresoController barraEfectoProgresoController;
    private Transform contenedorBarras;

    private Dictionary<Sprite, Coroutine> corrutinas = new Dictionary<Sprite, Coroutine>();
    private Dictionary<Sprite, Image> barras = new Dictionary<Sprite, Image>();

    private void Awake()
    {
        contenedorBarras = transform.Find("ContenedorBarrasEfectos");
        barraEfectoProgresoController = this;
    }

    public void CrearBarra(Sprite icono, float duracion)
    {
        if (barras.ContainsKey(icono))
        {
            StopCoroutine(corrutinas[icono]);
            barras[icono].fillAmount = 1f;
            corrutinas[icono] = StartCoroutine(IniciarCorrutinaBarra(icono, barras[icono], duracion));
            return;
        }

        GameObject barraGO = Instantiate(PrefabManager.prefabManager.ObtenerPrefab("BarraEfecto"), contenedorBarras);
        barraGO.transform.localScale = Vector3.one;

        Image barraImagen = barraGO.transform.Find("BarraProgreso").GetComponent<Image>();
        barraImagen.fillAmount = 1f;

        Image iconoImage = barraGO.transform.Find("Icono").GetComponent<Image>();
        iconoImage.sprite = icono;

        barras[icono] = barraImagen;
        corrutinas[icono] = StartCoroutine(IniciarCorrutinaBarra(icono, barraImagen, duracion));
    }

    private IEnumerator IniciarCorrutinaBarra(Sprite icono, Image barra, float duracion)
    {
        float tiempo = duracion;

        while (tiempo > 0)
        {
            if (barra == null)
            {
                barras.Remove(icono);
                corrutinas.Remove(icono);
                yield break;
            }

            tiempo -= Time.deltaTime;
            barra.fillAmount = tiempo / duracion;
            yield return null;
        }

        Destroy(barra.gameObject.transform.parent.gameObject);
        barras.Remove(icono);
        corrutinas.Remove(icono);
    }
}