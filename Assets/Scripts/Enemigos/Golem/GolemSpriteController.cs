using UnityEngine;
using System.Collections;

public class GolemSpriteController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Sprite spriteGolem;   
    private string[] direcciones = { "Sur", "Norte", "Este", "Oeste" };
    public float rotacionSpeed = 180f; 
    private GolenPiedraController golenController; 
    private Coroutine corrutinaRotacion;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        golenController = GetComponent<GolenPiedraController>();
    }

    void Start()
    {
        string direccionAleatoria = direcciones[Random.Range(0, direcciones.Length)];
        spriteGolem = SpriteManager.Instancia.ObtenerSprite($"Golem_{direccionAleatoria}");
        spriteRenderer.sprite = spriteGolem;
    }

    void Update()
    {
        if (golenController.EstadoActual == EstadoGolen.Movimiento)
        {
            if (corrutinaRotacion != null)
            {
                StopCoroutine(corrutinaRotacion);
                corrutinaRotacion = null;
            }

            spriteRenderer.sprite = SpriteManager.Instancia.ObtenerSprite("Golem_Rotacion");;
            transform.Rotate(0f, 0f, rotacionSpeed * Time.deltaTime);
        }
        else if (golenController.EstadoActual == EstadoGolen.Quieto)
        {
            if (corrutinaRotacion == null)
            {
                transform.rotation = Quaternion.identity;
                corrutinaRotacion = StartCoroutine(RotarSprite());
            }
        }
    }

    private IEnumerator RotarSprite()
    {
        while (golenController.EstadoActual == EstadoGolen.Quieto)
        {
            string nuevaDireccion;
            do
            {
                nuevaDireccion = direcciones[Random.Range(0, direcciones.Length)];
            } while (spriteRenderer.sprite == SpriteManager.Instancia.ObtenerSprite($"Golem_{nuevaDireccion}"));

            spriteRenderer.sprite = SpriteManager.Instancia.ObtenerSprite($"Golem_{nuevaDireccion}");
            float espera = Random.Range(1f, 3f);
            yield return new WaitForSeconds(espera);
        }

        spriteGolem = SpriteManager.Instancia.ObtenerSprite($"Golem_{direcciones[Random.Range(0, direcciones.Length)]}");
        spriteRenderer.sprite = spriteGolem;
        corrutinaRotacion = null;
    }
}