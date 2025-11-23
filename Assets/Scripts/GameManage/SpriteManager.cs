using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    public static SpriteManager Instancia { get; private set; }
    private Dictionary<string, Sprite> sprites = new();

    void Awake()
    {
        if (Instancia == null) Instancia = this;
        CargarSprites();
    }

    private void CargarSprites()
    {
        //GOLEM
        CargarSprite("Golem_Norte", "Sprites/Golem/Golem_Norte");
        CargarSprite("Golem_Sur", "Sprites/Golem/Golem_Sur");
        CargarSprite("Golem_Este", "Sprites/Golem/Golem_Este");
        CargarSprite("Golem_Oeste", "Sprites/Golem/Golem_Oeste");
        CargarSprite("Golem_Rotacion", "Sprites/Golem/Golem_Rotacion");
    }

    private void CargarSprite(string clave, string ruta) => sprites[clave] = Resources.Load<Sprite>(ruta);
    public Sprite ObtenerSprite(string clave) => sprites.TryGetValue(clave, out Sprite s) ? s : null;
}
