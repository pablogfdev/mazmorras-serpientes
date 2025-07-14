using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    private float velocidadBase = 5f;
    private float multiplicadorVelocidad = 2f;
    private float velocidadMovimiento;
    public float VelocidadMovimiento
    {
        get => velocidadMovimiento;
        set
        {
            if (Mathf.Approximately(velocidadMovimiento, value)) return;
            velocidadMovimiento = value;
            EventoCambioVelocidad?.Invoke(velocidadMovimiento);
        }
    }

    public Rigidbody2D rb { get; private set; }

    public event Action<float> EventoCambioVelocidad;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (GetComponent<PlayerMovement>() == null) gameObject.AddComponent<PlayerMovement>();
    }

    void Update()
    {
        ActualizarVelocidad();
    }

    void ActualizarVelocidad()
    {
        float nuevaVelocidad = velocidadBase * (Input.GetKey(KeyCode.LeftShift) ? multiplicadorVelocidad : 1f);
        VelocidadMovimiento = nuevaVelocidad;
    }
}