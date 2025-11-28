using UnityEngine;
using System.Collections;

public class GolemSpriteController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Sprite spriteGolem;   
    private string[] direcciones = { "Sur", "Norte", "Este", "Oeste" };
    public float rotacionSpeed = 180f; 
    private GolemPiedraController golemController; 
    private Coroutine corrutinaRotacion;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        golemController = GetComponent<GolemPiedraController>();
    }

    void Start()
    {
        string direccionAleatoria = direcciones[Random.Range(0, direcciones.Length)];
        spriteGolem = SpriteManager.spriteManager.ObtenerSprite($"Golem_{direccionAleatoria}");
        spriteRenderer.sprite = spriteGolem;
    }

    void Update()
    {
        if (golemController.EstadoActual == EstadoGolem.Movimiento)
        {
            if (corrutinaRotacion != null)
            {
                StopCoroutine(corrutinaRotacion);
                corrutinaRotacion = null;
            }

            spriteRenderer.sprite = SpriteManager.spriteManager.ObtenerSprite("Golem_Bola");;
            transform.Rotate(0f, 0f, rotacionSpeed * Time.deltaTime);
        }
        else if (golemController.EstadoActual == EstadoGolem.Quieto)
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
        while (golemController.EstadoActual == EstadoGolem.Quieto)
        {
            string nuevaDireccion;
            do
            {
                nuevaDireccion = direcciones[Random.Range(0, direcciones.Length)];
            } while (spriteRenderer.sprite == SpriteManager.spriteManager.ObtenerSprite($"Golem_{nuevaDireccion}"));

            spriteRenderer.sprite = SpriteManager.spriteManager.ObtenerSprite($"Golem_{nuevaDireccion}");
            float espera = Random.Range(1f, 3f);
            yield return new WaitForSeconds(espera);
        }

        spriteGolem = SpriteManager.spriteManager.ObtenerSprite($"Golem_{direcciones[Random.Range(0, direcciones.Length)]}");
        spriteRenderer.sprite = spriteGolem;
        corrutinaRotacion = null;
    }
}