using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    public static SpriteManager spriteManager { get; private set; }
    private Dictionary<string, Sprite> sprites = new();

    void Awake()
    {
        if (spriteManager == null) spriteManager = this;
        CargarSprites();
    }

    private void CargarSprites()
    {
        //GOLEM
        CargarSprite("Golem_Norte", "Sprites/Golem/Golem_Norte");
        CargarSprite("Golem_Sur", "Sprites/Golem/Golem_Sur");
        CargarSprite("Golem_Este", "Sprites/Golem/Golem_Este");
        CargarSprite("Golem_Oeste", "Sprites/Golem/Golem_Oeste");
        CargarSprite("Golem_Bola", "Sprites/Golem/Golem_Bola");

        //ESPIRITU
        CargarSprite("Espiritu_Norte", "Sprites/Espiritu/Espiritu_Norte");
        CargarSprite("Espiritu_Sur", "Sprites/Espiritu/Espiritu_Sur");
        CargarSprite("Espiritu_Este", "Sprites/Espiritu/Espiritu_Este");
        CargarSprite("Espiritu_Oeste", "Sprites/Espiritu/Espiritu_Oeste");

        //COFRE
        CargarSprite("Cofre_Abierto", "Sprites/Cofre/Cofre_Abierto");
        CargarSprite("Cofre_Cerrado", "Sprites/Cofre/Cofre_Cerrado");
    }

    private void CargarSprite(string clave, string ruta) => sprites[clave] = Resources.Load<Sprite>(ruta);
    public Sprite ObtenerSprite(string clave) => sprites.TryGetValue(clave, out Sprite s) ? s : null;
}
