using UnityEngine;

public class ControlSubidaNivel : MonoBehaviour
{
    public int NivelMaximo { get; set; } = 9222;
    
    public void SubirNivel(int nivelActual) => NivelMaximo = (nivelActual == NivelMaximo) ? NivelMaximo + 1 : NivelMaximo;
}