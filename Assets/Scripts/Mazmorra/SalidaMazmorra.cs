using UnityEngine;

public class SalidaMazmorra : MonoBehaviour
{
    private GameObject jugador;
    private GameObject jugardorPrefabs;
    private bool jugadorEnPuerta;
    private GameObject juegoController;


    void Awake()
    {
        juegoController = GameObject.FindWithTag("JuegoController");
        jugardorPrefabs = Resources.Load<GameObject>("Prefabs/Jugador");
        jugador = Instantiate(jugardorPrefabs);
    }

    private void Update()
    {
        if (jugadorEnPuerta && Input.GetKeyDown(KeyCode.E))
        {
            //Con este codigo se puede subir de nivel sin haber terminado la mazmorra
            //Arreglar en proximos commits cuando la estructura del proyecto este mas avanzada
            //Se debe al condicional, se sube de nivel pero si no tiene la llave no se avanza
            juegoController.GetComponent<ControlSubidaNivel>().SubirNivel(GameObject.FindWithTag("Mazmorra").GetComponentInChildren<MazmorraController>().Nivel);
            if (jugador.GetComponent<JugadorController>().LlaveObtenida) juegoController.GetComponent<EscenasController>().CargarEscenaComerciante();    
            else { Debug.Log("El jugador no tiene la llave."); }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) { if (other.CompareTag("Player")) jugadorEnPuerta = true; }

    private void OnTriggerExit2D(Collider2D other) { if (other.CompareTag("Player")) jugadorEnPuerta = false; }
}