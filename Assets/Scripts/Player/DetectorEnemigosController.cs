using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorEnemigosController : MonoBehaviour
{
    private List<EnemigoController> enemigos = new();
    private BarraVidaObjetivoController objetivoController;
    EnemigoController enemigoMasCercano;

    void Awake()
    {
        objetivoController = transform.parent.Find("BarraVidaObjetivo").GetComponent<BarraVidaObjetivoController>();
    }

    void Start()
    {
        StartCoroutine(ActualizarEnemigoCercano());
    }

    IEnumerator ActualizarEnemigoCercano()
    {
        while (true)
        {
            ObtenerEnemigoMasCercano();
            yield return new WaitForSeconds(0.1f);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemigo")) return;
        enemigos.Add(other.GetComponent<EnemigoController>());
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Enemigo")) return;
        enemigos.Remove(other.GetComponent<EnemigoController>());
    }

    void ObtenerEnemigoMasCercano()
    {
        if (enemigos.Count <= 0)
        {
            objetivoController.gameObject.SetActive(false);
            return;
        }

        enemigoMasCercano = enemigos[0];
        float distanciaMinima = Vector2.Distance(transform.position, enemigoMasCercano.transform.position);

        foreach (var enemigo in enemigos)
        {
            float distancia = Vector2.Distance(transform.position, enemigo.transform.position);
            if (distancia < distanciaMinima)
            {
                distanciaMinima = distancia;
                enemigoMasCercano = enemigo;
            }
        }

        objetivoController.gameObject.SetActive(true);
        objetivoController.Enemigo = enemigoMasCercano;
    }
}