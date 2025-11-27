using UnityEngine;
using System.Collections;

public class EspirituSpriteController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private EspirituController espirituController; 
    private Coroutine corrutinaRotacion;

    private Transform jugador;    
    private string ultimaDireccion = "";

    private readonly string[] direcciones = { "Sur", "Norte", "Este", "Oeste" };

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        espirituController = GetComponent<EspirituController>();
        jugador = GameObject.FindWithTag("Player").transform;
    }

    void Start() => CambiarSprite(direcciones[Random.Range(0, direcciones.Length)]);
    
    void Update()
    {
        if (espirituController.EstadoActual == EstadoEspiritu.Ataque)
        {
            if (corrutinaRotacion != null) StopCoroutine(corrutinaRotacion); corrutinaRotacion = null;
            ActualizarDireccionHaciaJugador();
        }
        else if (espirituController.EstadoActual == EstadoEspiritu.Ambulante)
        {
            if (corrutinaRotacion == null) corrutinaRotacion = StartCoroutine(RotarSprite());
        }
    }

    private IEnumerator RotarSprite()
    {
        while (espirituController.EstadoActual == EstadoEspiritu.Ambulante)
        {
            string nuevaDireccion;
            do nuevaDireccion = direcciones[Random.Range(0, direcciones.Length)];
            while (nuevaDireccion == ultimaDireccion);
            CambiarSprite(nuevaDireccion);
            yield return new WaitForSeconds(Random.Range(1f, 3f));
        }
        corrutinaRotacion = null;
    }

    private void ActualizarDireccionHaciaJugador()
    {
        Vector2 dir = jugador.position - transform.position;
        string nuevaDir;

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y)) nuevaDir = (dir.x > 0) ? "Este" : "Oeste";
        else nuevaDir = (dir.y > 0) ? "Norte" : "Sur";

        if (nuevaDir != ultimaDireccion) CambiarSprite(nuevaDir);
    }

    private void CambiarSprite(string direccion)
    {
        ultimaDireccion = direccion;
        spriteRenderer.sprite = SpriteManager.Instancia.ObtenerSprite($"Espiritu_{direccion}");
    }
}
