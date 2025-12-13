using System;
using System.Collections.Generic;
using UnityEngine;
using PM = PrefabManager;

public class MazmorraController : MonoBehaviour
{
    private int semilla;
    private System.Random generador;
    private int nivel = 1;
    public int Nivel
    {
        get => nivel;
        set => nivel = Mathf.Max(1, value);
    }

    private int distanciaBase = 150;
    private float probabilidadRamas = 0.7f;
    private float probabilidadConexionExtra = 0.3f;

    private Dictionary<int, List<GameObject>> habitacionesPorCapa = new Dictionary<int, List<GameObject>>();
    private HashSet<Vector2Int> posicionesOcupadas = new HashSet<Vector2Int>();
    private List<(GameObject, GameObject)> conexiones = new List<(GameObject, GameObject)>();
    private Dictionary<GameObject, Vector2Int> posiciones = new Dictionary<GameObject, Vector2Int>();

    public void CrearMazmorra()
    {
        GenerarParametrosIniciales();
        LimpiarListas();
        GenerarHabitacionCentral();
        GenerarTodasLasHabitaciones();
        GenerarConexiones();
        CrearConexionesFisicas();
        AsignarLlave();
    }

    private void GenerarParametrosIniciales()
    {
        semilla = GestorPartidas.partidaActiva.semilla;
        generador = new System.Random(semilla * nivel);
    }

    private void LimpiarListas()
    {
        habitacionesPorCapa.Clear();
        posicionesOcupadas.Clear();
        conexiones.Clear();
        posiciones.Clear();
    }

    private void GenerarHabitacionCentral()
    {
        GameObject habitacionCentral = Instantiate(PM.prefabManager.ObtenerPrefab("HabitacionInicial"), Vector3.zero, Quaternion.identity, transform);
        habitacionCentral.name = $"Habitacion_0_0";
        habitacionesPorCapa[1] = new List<GameObject> { habitacionCentral };
        posicionesOcupadas.Add(new Vector2Int(0, 0));
        posiciones[habitacionCentral] = new Vector2Int(0, 0);
    }

    private void GenerarTodasLasHabitaciones()
    {
        int numeroHabitaciones = NumHabitacionesPorNivel();
        int totalGeneradas = 1;

        for (int capa = 2; capa <= MaxCapasPorNivel(); capa++)
        {
            habitacionesPorCapa[capa] = new List<GameObject>();
            List<GameObject> habitacionesPadre = habitacionesPorCapa[capa - 1];

            totalGeneradas = GenerarHabitacionesDesdePadres(habitacionesPadre, capa, numeroHabitaciones, totalGeneradas);

            if (totalGeneradas >= numeroHabitaciones) break;
        }
    }

    private int GenerarHabitacionesDesdePadres(List<GameObject> habitacionesPadre, int capa, int numeroHabitaciones, int totalGeneradas)
    {
        foreach (GameObject habitacionPadre in habitacionesPadre)
        {
            if (totalGeneradas >= numeroHabitaciones) break;

            Vector2Int coordenadasHabitacionPadre = posiciones[habitacionPadre];
            List<Vector2Int> vecinos = Vecinos(coordenadasHabitacionPadre);
            BarajarHabitacionesVecinas(vecinos);

            foreach (Vector2Int vecino in vecinos)
            {
                if (!posicionesOcupadas.Contains(vecino) && generador.NextDouble() < probabilidadRamas)
                {
                    totalGeneradas++;
                    CrearHabitacion(vecino, habitacionPadre, capa);
                }
            }
        }
        return totalGeneradas;
    }

    private void CrearHabitacion(Vector2Int coordenadas, GameObject padre, int capa)
    {
        Vector3 posicion = new Vector3(coordenadas.x * distanciaBase, coordenadas.y * distanciaBase, 0f);
        GameObject habitacion = Instantiate(PM.prefabManager.ObtenerPrefab("Habitacion"), posicion, Quaternion.identity, transform);
        habitacion.name = $"Habitacion_{coordenadas.x}_{coordenadas.y}";
        habitacion.GetComponent<HabitacionController>().Inicializar();
        habitacionesPorCapa[capa].Add(habitacion);
        posicionesOcupadas.Add(coordenadas);
        posiciones[habitacion] = coordenadas;
    }

    private void GenerarConexiones()
    {
        GenerarConexionesInternas((habitacion, vecino) => true); // Conexiones entre habitaciones
        GenerarConexionesInternas((habitacion, vecino) => generador.NextDouble() < probabilidadConexionExtra); //Conexiones extra entre habitaciones de distintas rammas
    }

    private void GenerarConexionesInternas(Func<GameObject, GameObject, bool> condicion)
    {
        foreach (var capa in habitacionesPorCapa)
        {
            foreach (GameObject habitacion in capa.Value)
            {
                foreach (Vector2Int coordenadasVecino in Vecinos(posiciones[habitacion]))
                {
                    GameObject habitacionVecina = HabitacionEnCoordenada(coordenadasVecino);
                    if (habitacionVecina != null && !ExisteConexion(habitacion, habitacionVecina) && condicion(habitacion, habitacionVecina))
                    {
                        conexiones.Add((habitacion, habitacionVecina));
                    }
                }
            }
        }
    }

    private GameObject HabitacionEnCoordenada(Vector2Int coordenada)
    {
        foreach (var capa in habitacionesPorCapa)
        {
            foreach (GameObject habitacion in capa.Value) if (posiciones[habitacion] == coordenada) return habitacion;
        }
        return null;
    }

    private bool ExisteConexion(GameObject a, GameObject b)
    {
        foreach (var con in conexiones)
        {
            if ((con.Item1 == a && con.Item2 == b) || (con.Item1 == b && con.Item2 == a)) return true;
        }
        return false;
    }

    private void CrearConexionesFisicas()
    {
        foreach (var con in conexiones)
        {
            GameObject conexion = Instantiate(PM.prefabManager.ObtenerPrefab("ConexionPuertas"), transform);
            conexion.name = $"Conexion_{con.Item1.name}_{con.Item2.name}";

            Transform puertaA = conexion.transform.Find("PuertaA");
            Transform puertaB = conexion.transform.Find("PuertaB");

            puertaA.position = con.Item1.GetComponent<PosicionPuertas>().SituarPuerta(con.Item2.transform.position);
            puertaB.position = con.Item2.GetComponent<PosicionPuertas>().SituarPuerta(con.Item1.transform.position);
        }
    }

    void AsignarLlave()
    {
        var listaHabitaciones = new List<GameObject>(posiciones.Keys);
        int indice = generador.Next(1, listaHabitaciones.Count);
        GameObject habitacionSeleccionada = listaHabitaciones[indice];
        var habitacionController = habitacionSeleccionada.GetComponent<HabitacionController>();
        habitacionController.LlaveGenerada = true;
        habitacionController.ColocarLlave();
    }

    List<Vector2Int> Vecinos(Vector2Int coord)
        => new List<Vector2Int>
        {
            new Vector2Int(coord.x + 1, coord.y),
            new Vector2Int(coord.x - 1, coord.y),
            new Vector2Int(coord.x, coord.y + 1),
            new Vector2Int(coord.x, coord.y - 1)
        };

    private void BarajarHabitacionesVecinas(List<Vector2Int> vecinos)
    {
        for (int i = vecinos.Count - 1; i > 0; i--)
        {
            int j = generador.Next(0, i + 1);
            Vector2Int variableTemporal = vecinos[i];
            vecinos[i] = vecinos[j];
            vecinos[j] = variableTemporal;
        }
    }

    int MaxCapasPorNivel() => 5 + (nivel - 1) / 5;
    int NumHabitacionesPorNivel() => 10 + (nivel - 1) / 3;

    //Debug tras pruebas
    public void DibujarConexionesDebug(Color colorLinea, float grosor)
    {
        foreach (var con in conexiones)
        {
            GameObject lineaGO = new GameObject($"Linea_{con.Item1.name}_{con.Item2.name}");
            lineaGO.transform.parent = transform;

            LineRenderer lr = lineaGO.AddComponent<LineRenderer>();
            lr.positionCount = 2;
            lr.SetPosition(0, con.Item1.transform.position);
            lr.SetPosition(1, con.Item2.transform.position);

            lr.startWidth = grosor;
            lr.endWidth = grosor;
            lr.material = new Material(Shader.Find("Sprites/Default"));
            lr.startColor = colorLinea;
            lr.endColor = colorLinea;
            lr.sortingOrder = 10;
        }
    }
}