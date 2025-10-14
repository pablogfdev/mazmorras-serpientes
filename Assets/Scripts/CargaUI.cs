using UnityEngine;
using UnityEngine.UI;

public class CargaUI : MonoBehaviour
{
    public Slider barra;
    public static CargaUI cargaUI;

    [System.Obsolete]
    void Awake()
    {
        cargaUI = this;
        barra = FindObjectOfType<Slider>(includeInactive: true);
        barra.value = 0f;
    }

    public void ActualizarProgreso(float progreso) => barra.value = progreso;    
}
