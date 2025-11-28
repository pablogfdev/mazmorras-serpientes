using UnityEngine;

public class CofreScriptController : MonoBehaviour
{
    private SpriteRenderer sprite;
    private bool abierto = false;
    public bool tieneContenido = true; 

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        ActualizarSprite();
    }

    public void AbrirCofre()
    {
        if (!abierto)
        {
            abierto = true;
            ActualizarSprite();
        }
    }

    private void ActualizarSprite()
    {
        if (!abierto) sprite.sprite = SpriteManager.spriteManager.ObtenerSprite("Cofre_Cerrado");
        else sprite.sprite = SpriteManager.spriteManager.ObtenerSprite("Cofre_Abierto");
    }
}
