using UnityEngine;

public class LlaveController : MonoBehaviour
{
    //SCRIPT TEMPORAL HASTA QUE SE DEFINA LA MECANICA DEL INVENTARIO

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        other.GetComponent<JugadorController>().LlaveObtenida = true;
        Debug.Log("Llave recogida por: " + other.name);
        Debug.Log("Llave obtenida: " + other.GetComponent<JugadorController>().LlaveObtenida);

        Destroy(gameObject);
    }
}
