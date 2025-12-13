using UnityEngine;

public class PosicionPuertas : MonoBehaviour
{
    private float ancho = 30f;
    private float altura = 20f;
    private float grosorPared = 1f;
    
    public Vector3 SituarPuerta(Vector3 destino)
    {
        Vector3 centro = transform.position;
        Vector3 dir = (destino - centro).normalized;

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            // Dirección horizontal
            return dir.x > 0
                ? new Vector3(centro.x + ancho / 2 - grosorPared, centro.y, 0)  // Derecha
                : new Vector3(centro.x - ancho / 2 + grosorPared, centro.y, 0); // Izquierda
        }
        else
        {
            // Dirección vertical
            return dir.y > 0
                ? new Vector3(centro.x, centro.y + altura / 2 - grosorPared, 0)  // Arriba
                : new Vector3(centro.x, centro.y - altura / 2 + grosorPared, 0); // Abajo
        }
    }
}