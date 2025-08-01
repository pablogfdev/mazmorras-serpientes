using UnityEngine;

[RequireComponent(typeof(JugadorController))]
public class MovimientoJugador : MonoBehaviour
{
    private JugadorController jugador;
    private Vector2 movimiento;
    private float velocidadActual;

    void Awake()
    {
        jugador = GetComponent<JugadorController>();
        velocidadActual = jugador.VelocidadMovimiento;
        jugador.eventoCambioVelocidad += ActualizarVelocidad;
    }

    void ActualizarVelocidad(float nuevaVelocidad) { velocidadActual = nuevaVelocidad; }

    void Update()
    {
        movimiento.x = Input.GetAxisRaw("Horizontal");
        movimiento.y = Input.GetAxisRaw("Vertical");
        movimiento.Normalize();
    }

    void FixedUpdate() { jugador.rb.MovePosition(jugador.rb.position + movimiento * velocidadActual * Time.fixedDeltaTime); }
    void OnDestroy() { jugador.eventoCambioVelocidad -= ActualizarVelocidad; }
}