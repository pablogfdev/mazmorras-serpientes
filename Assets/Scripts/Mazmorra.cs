using System.Collections.Generic;
using UnityEngine;

public class Mazmorra : MonoBehaviour
{
    private GameObject habitacionPrefab;
    private GameObject conexionPrefab;
    private int numeroHabitaciones = 10;
    private float distanciaHabitaciones = 100f;

    private int nivel = 0;
    public int Nivel { get => nivel; }

    public List<GameObject> habitaciones = new List<GameObject>();

    private void Awake()
    {
        
        conexionPrefab = Resources.Load<GameObject>("Prefabs/Conexion");
        habitacionPrefab = Resources.Load<GameObject>("Prefabs/Habitacion");
    }
    private void Start()
    {
        GenerarHabitaciones();
        CrearConexiones();
    }

    void GenerarHabitaciones()
    {
        for (int i = 0; i < numeroHabitaciones; i++)
        {
            nivel++;
            Vector3 posicionHabitacion = new Vector3(i * distanciaHabitaciones, 0, 0);
            GameObject habitacion = Instantiate(habitacionPrefab, posicionHabitacion, Quaternion.identity, transform);
            habitacion.name = $"Habitacion_{i + 1}";
            habitaciones.Add(habitacion);
            
        }
    }

    private void CrearConexiones()
    {
        HashSet<Vector3> posicionesConexionesUsadas = new HashSet<Vector3>();

        for (int i = 0; i < numeroHabitaciones; i++)
        {        
            GameObject conexion = Instantiate(conexionPrefab, transform);
            conexion.name = $"Conexion_{i}_{(i + 1) % numeroHabitaciones}";

            Transform puertaA = conexion.transform.Find("PuertaA");
            Transform puertaB = conexion.transform.Find("PuertaB");

            GameObject habitacionA = habitaciones[i];
            GameObject habitacionB = habitaciones[(i + 1) % numeroHabitaciones]; // conexión circular

            do
            {
                puertaA.position = habitacionA.GetComponent<Habitacion>().SituarPuerta();
                puertaB.position = habitacionB.GetComponent<Habitacion>().SituarPuerta();
            }
            while (posicionesConexionesUsadas.Contains(puertaA.position) || posicionesConexionesUsadas.Contains(puertaB.position));

            posicionesConexionesUsadas.Add(puertaA.position);
            posicionesConexionesUsadas.Add(puertaB.position);
        }
    }
}