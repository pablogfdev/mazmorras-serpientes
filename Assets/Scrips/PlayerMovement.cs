using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerMovement : MonoBehaviour
{
    private PlayerController jugador;
    private Vector2 movimiento;
    private float velocidadActual;

    void Awake()
    {
        jugador = GetComponent<PlayerController>();
        velocidadActual = jugador.VelocidadMovimiento;
        jugador.EventoCambioVelocidad += ActualizarVelocidad;
    }

    void ActualizarVelocidad(float nuevaVelocidad) { velocidadActual = nuevaVelocidad; }

    void Update()
    {
        movimiento.x = Input.GetAxisRaw("Horizontal");
        movimiento.y = Input.GetAxisRaw("Vertical");
        movimiento.Normalize();     //Evita que el movimiento sea más rápido en diagonal
    }

    void FixedUpdate() { jugador.rb.MovePosition(jugador.rb.position + movimiento * velocidadActual * Time.fixedDeltaTime); }
    void OnDestroy() { jugador.EventoCambioVelocidad -= ActualizarVelocidad; }
}