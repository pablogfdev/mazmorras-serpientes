using UnityEngine;
using EC = EscenasController;
using PM = PrefabManager;

public class SalidaMazmorra : MonoBehaviour
{
    private GameObject jugador;
    private bool jugadorEnPuerta;

    void Awake() => jugador = Instantiate(PM.prefabManager.ObtenerPrefab("Jugador"));
    
    private void Update()
    {
        if (jugadorEnPuerta && Input.GetKeyDown(KeyCode.E))
        {
            if (jugador.GetComponent<JugadorController>().LlaveObtenida)
            {
                GestorPartidas.SubirNivel(GameObject.FindWithTag("Mazmorra").GetComponentInChildren<MazmorraController>().Nivel);
                EC.escenasController.CargarEscenaComerciante();
            }
            else { Debug.Log("El jugador no tiene la llave."); }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) { if (other.CompareTag("Player")) jugadorEnPuerta = true; }

    private void OnTriggerExit2D(Collider2D other) { if (other.CompareTag("Player")) jugadorEnPuerta = false; }
}