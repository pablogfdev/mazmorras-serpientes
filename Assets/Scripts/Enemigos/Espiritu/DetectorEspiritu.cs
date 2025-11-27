using System.Collections;
using UnityEngine;

public class DetectorEspiritu : MonoBehaviour
{

    private const float CAMBIO_ESCALA = 30f;  
    private EspirituController espiritu;
    SpriteRenderer vistaDetector;
    private Vector3 escalaBase;      
    private Vector3  escalaAtaque;  
    private bool ataqueRealizado = false;
    private float velocidadExpansion = 3f;      
    private float radioVision = 4f;

    void Start()
    {
        espiritu = GetComponentInParent<EspirituController>();
        vistaDetector = GetComponent<SpriteRenderer>();
        escalaAtaque = Vector3.one * CAMBIO_ESCALA;
        transform.localScale = Vector3.zero;
        escalaBase = Vector3.zero; 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || ataqueRealizado) return;
        espiritu.EstadoActual = EstadoEspiritu.Ataque;
        transform.localScale = escalaAtaque;
        vistaDetector.enabled = false;
        ataqueRealizado = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        float distancia = Vector2.Distance(espiritu.transform.position, other.transform.position);

        if (distancia > CAMBIO_ESCALA)
        {
            espiritu.EstadoActual = EstadoEspiritu.Ambulante;
            transform.localScale = escalaBase;
            vistaDetector.enabled = true;
            ataqueRealizado = false;
            return;
        }
        transform.localScale = escalaAtaque;  
    }

    public IEnumerator ExpandirDominio()
    {
        float escalaActual = 0f;
        float escalaMaxima = radioVision * 2f;
        transform.localScale = escalaAtaque;
        while (escalaActual < escalaMaxima)
        {
            escalaActual += velocidadExpansion * Time.deltaTime;
            if (escalaActual > escalaMaxima) escalaActual = escalaMaxima;
            transform.localScale = Vector3.one * escalaActual;
            yield return null;
        }
    }
}