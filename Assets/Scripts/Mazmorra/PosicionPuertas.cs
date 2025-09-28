using UnityEngine;

public class PosicionPuertas : MonoBehaviour
{
    private float ancho;
    private float altura;
    private float grosorPared = 1f;

    void Awake()
    {
        Transform suelo = transform.Find("Suelo(Clone)");
        ancho  = suelo.localScale.x + 1;
        altura = suelo.localScale.y + 1;
        
    }

    public Vector3 SituarPuerta()
    {
        Vector3 centro = transform.position;

        return Random.Range(0, 4) switch
        {
            0 => new Vector3(centro.x, centro.y - altura / 2 + grosorPared, 0),  // Abajo
            1 => new Vector3(centro.x, centro.y + altura / 2 - grosorPared, 0),  // Arriba
            2 => new Vector3(centro.x - ancho / 2 + grosorPared, centro.y, 0),   // Izquierda
            _ => new Vector3(centro.x + ancho / 2 - grosorPared, centro.y, 0)    // Derecha
        };
    }
}