using System.Collections.Generic;
using UnityEngine;

public class IconoManager : MonoBehaviour
{
    public static IconoManager iconoManager { get; private set; }
    private Dictionary<string, Sprite> iconos = new();
  
    void Awake()
    {   
        if (iconoManager == null) iconoManager = this;
        CargarIconos();
    }

    private void CargarIconos()
    {
        CargarIcono("Oro", "Iconos/Oro");
        CargarIcono("Llave", "Iconos/Llave");
    }

    private void CargarIcono(string clave, string ruta) => iconos[clave] = Resources.Load<Sprite>(ruta);

    public Sprite ObtenerIcono(string clave) => iconos.TryGetValue(clave, out Sprite icono) ? icono : null;
}
