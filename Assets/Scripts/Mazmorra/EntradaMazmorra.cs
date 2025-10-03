using UnityEngine;

public class EntradaMazmorra : MonoBehaviour
{
    PausaJuegoController pausaJuegoController;
    private GameObject menuNivel;
    private bool jugadorEnPuerta;

    void Awake()
    {
        menuNivel = Resources.Load<GameObject>("Prefabs/CanvasMenuNiveles");
        GameObject juegoController = GameObject.FindWithTag("JuegoController");
        pausaJuegoController = juegoController.GetComponent<PausaJuegoController>();
    }

    private void Update()
    {
        if (jugadorEnPuerta && Input.GetKeyDown(KeyCode.E) && !pausaJuegoController.MenuNivelAbierto && !pausaJuegoController.JuegoPausado)
        {
            Instantiate(menuNivel, transform.position, Quaternion.identity);
            pausaJuegoController.ToggleMenuNiveles();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) { if (other.CompareTag("Player")) jugadorEnPuerta = true; }

    private void OnTriggerExit2D(Collider2D other) { if (other.CompareTag("Player")) jugadorEnPuerta = false; }
}