using System.Collections.Generic;
using UnityEngine;
using PM = PrefabManager;

public class MazmorraController : MonoBehaviour
{


    private int numeroHabitaciones = 10;
    private int nivel;
    public int Nivel { get => nivel; set => nivel = value; }

    private float distanciaBase = 250f;
    private List<GameObject> habitaciones = new List<GameObject>();

    void GenerarHabitaciones()
    {
        GameObject habitacionInicial = Instantiate(PM.prefabManager.ObtenerPrefab("HabitacionInicial"), Vector3.zero, Quaternion.identity, transform);
        habitaciones.Add(habitacionInicial);

        for (int i = 1; i < numeroHabitaciones; i++)
        {
            Vector3 posicionHabitacion = new Vector3(i * (distanciaBase + nivel) * 2, 0, 0);
            GameObject habitacion = Instantiate(PM.prefabManager.ObtenerPrefab("Habitacion"), posicionHabitacion, Quaternion.identity, transform);
            habitacion.name = $"Habitacion_{i + 1}";
            habitaciones.Add(habitacion);
        }
    }

    void AsignarLlave()
    {
        GameObject habitacionSeleccionada = habitaciones[Random.Range(1, 10)];
        habitacionSeleccionada.GetComponent<HabitacionController>().LlaveGenerada = true;
        habitacionSeleccionada.GetComponent<HabitacionController>().ColocarLlave();
    }

    private void CrearConexiones()
    {
        HashSet<Vector3> posicionesConexionesUsadas = new HashSet<Vector3>();

        for (int i = 0; i < numeroHabitaciones; i++)
        {
            GameObject conexion = Instantiate(PM.prefabManager.ObtenerPrefab("ConexionPuertas"), transform);
            conexion.name = $"Conexion_{i}_{(i + 1) % numeroHabitaciones}";

            Transform puertaA = conexion.transform.Find("PuertaA");
            Transform puertaB = conexion.transform.Find("PuertaB");

            GameObject habitacionA = habitaciones[i];
            GameObject habitacionB = habitaciones[(i + 1) % numeroHabitaciones]; // conexión circular

            do
            {
                puertaA.position = habitacionA.GetComponent<PosicionPuertas>().SituarPuerta();
                puertaB.position = habitacionB.GetComponent<PosicionPuertas>().SituarPuerta();
            }
            while (posicionesConexionesUsadas.Contains(puertaA.position) || posicionesConexionesUsadas.Contains(puertaB.position));

            posicionesConexionesUsadas.Add(puertaA.position);
            posicionesConexionesUsadas.Add(puertaB.position);
        }
    }

    public void CrearMazmorra()
    {
        GenerarHabitaciones();
        CrearConexiones();
        AsignarLlave();
    }
}