using UnityEngine;

public class CamaraJugador : MonoBehaviour
{
    private Transform jugador; 

    void Awake()
    {    
        jugador = GameObject.FindWithTag("Player").transform;
    }


    void LateUpdate()
    {
        transform.position = jugador.position + new Vector3(0, 0, -10); 
    }
}
