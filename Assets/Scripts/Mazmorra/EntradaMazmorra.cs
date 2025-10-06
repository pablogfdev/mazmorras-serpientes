using UnityEngine;
using PJC = PausaJuegoController;
using PM = PrefabManager;

public class EntradaMazmorra : MonoBehaviour
{
    private bool jugadorEnPuerta;

    private void Update()
    {
        if (jugadorEnPuerta && Input.GetKeyDown(KeyCode.E) && !PJC.pausaJuegoController.MenuNivelAbierto && Time.timeScale != 0)
        {
            Instantiate(PM.prefabManager.ObtenerPrefab("MenuNivel"), transform.position, Quaternion.identity);
            PJC.pausaJuegoController.ToggleMenuNiveles();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) { if (other.CompareTag("Player")) jugadorEnPuerta = true; }

    private void OnTriggerExit2D(Collider2D other) { if (other.CompareTag("Player")) jugadorEnPuerta = false; }
}