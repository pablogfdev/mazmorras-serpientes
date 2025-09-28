using UnityEngine;

public class EntradaMazmorra : MonoBehaviour
{
    PausaJuegoController pausaJuegoController;
    private GameObject menuNivel;
    private Vector3 posicionEntrada;
    private bool jugadorEnPuerta;

    void Awake()
    {
        menuNivel = Resources.Load<GameObject>("Prefabs/CanvasMenuNiveles");
        GameObject juegoController = GameObject.FindWithTag("JuegoController");
        pausaJuegoController = juegoController.GetComponent<PausaJuegoController>();
    }

    private void Update()
    {
        if (jugadorEnPuerta && Input.GetKeyDown(KeyCode.E) && !pausaJuegoController.MenuNivelAbierto)
        {
            posicionEntrada = transform.position;
            Instantiate(menuNivel, transform.position, Quaternion.identity);
            pausaJuegoController.ToggleMenuNiveles();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) => jugadorEnPuerta = other.CompareTag("Player");

    private void OnTriggerExit2D(Collider2D other) { if (other.CompareTag("Player")) jugadorEnPuerta = false; }

    public Vector3 GetPosicionEntrada() => posicionEntrada;
}