using System.Collections.Generic;
using UnityEngine;

public class Habitacion : MonoBehaviour
{
    private int nivel;
    private float ancho;
    private float altura;
    private float grosorPared = 1f;

    private int numCofres;
    private int numSerpientes;

    private GameObject paredPrefab;
    private GameObject sueloPrefab;
    private GameObject cofrePrefab;
    private GameObject serpientePrefab;
    private Mazmorra piso;

    List<Vector3> posicionesOcupadas = new List<Vector3>();

    [System.Obsolete]
    void Awake()
    {
        asignacionPrefabs();
        CalcularTamañoYContenido();
    }

    void Start()
    {
        crearParedes();
        crearSuelo();
        generarEntidades();
    }

    [System.Obsolete]
    void CalcularTamañoYContenido()
    {
        piso = FindObjectOfType<Mazmorra>();
        nivel = piso.Nivel;

        ancho = 15 + nivel * 2 + Random.Range(-1, 2);
        altura = 12 + nivel + Random.Range(-1, 2);

        numCofres = (nivel < 3) ? Random.Range(0, 2) :
                    (nivel < 7) ? Random.Range(1, 3) :
                                Random.Range(2, 4);

        numSerpientes = (nivel < 2) ? Random.Range(0, 2) :
                        (nivel < 5) ? Random.Range(1, 4) :
                        (nivel < 7) ? Random.Range(3, 5) :
                        (nivel < 9) ? Random.Range(5, 7) :
                                    Random.Range(6, 9);
    }

    void asignacionPrefabs()
    {
        paredPrefab = Resources.Load<GameObject>("Prefabs/Pared");
        sueloPrefab = Resources.Load<GameObject>("Prefabs/Suelo");
        cofrePrefab = Resources.Load<GameObject>("Prefabs/Cofre");
        serpientePrefab = Resources.Load<GameObject>("Prefabs/Serpiente");
    }

    void crearSuelo()
    {
        Vector3 centro = transform.position;
        Vector3 escala = new Vector3(ancho - grosorPared, altura - grosorPared, 1);
        GameObject suelo = Instantiate(sueloPrefab, centro, Quaternion.identity, transform);
        suelo.transform.localScale = escala;
    }

    void crearParedes()
    {
        Vector3 centro = transform.position;

        Vector3 escalaHorizontal = new Vector3(ancho, grosorPared, 1);
        Vector3 escalaVertical = new Vector3(grosorPared, altura, 1);

        Vector3 posicionParedInferior = centro + new Vector3(0, -altura / 2, 0);
        Vector3 posicionParedSuperior = centro + new Vector3(0, altura / 2, 0);
        Vector3 posicionParedIzquierda = centro + new Vector3(-ancho / 2, 0, 0);
        Vector3 posicionParedDerecha = centro + new Vector3(ancho / 2, 0, 0);

        GameObject paredInferior = Instantiate(paredPrefab, posicionParedInferior, Quaternion.identity, transform);
        GameObject paredSuperior = Instantiate(paredPrefab, posicionParedSuperior, Quaternion.identity, transform);
        GameObject paredIzquierda = Instantiate(paredPrefab, posicionParedIzquierda, Quaternion.identity, transform);
        GameObject paredDerecha = Instantiate(paredPrefab, posicionParedDerecha, Quaternion.identity, transform);

        paredInferior.transform.localScale = escalaHorizontal;
        paredSuperior.transform.localScale = escalaHorizontal;
        paredIzquierda.transform.localScale = escalaVertical;
        paredDerecha.transform.localScale = escalaVertical;
    }

    void generarEntidades()
    {
        for (int i = 0; i < numCofres; i++) { colocarEntidadEnPosicioValida(cofrePrefab); }
        for (int i = 0; i < numSerpientes; i++) { colocarEntidadEnPosicioValida(serpientePrefab); }
    }

    void colocarEntidadEnPosicioValida(GameObject prefab)
    {
        Vector3 centro = transform.position;
        float distancia = (prefab == cofrePrefab) ? 4f : 2f;

        // Reducimos 4f para evitar que la entidad aparezca pegado a las paredes
        float rangoX = (ancho / 2) - grosorPared - 4f;
        float rangoY = (altura / 2) - grosorPared - 4f;

        Vector3 posicionEntidad;
        bool posicionValida;
        int intentos = 0;

        do
        {
            float posX = Random.Range(centro.x - rangoX, centro.x + rangoX);
            float posY = Random.Range(centro.y - rangoY, centro.y + rangoY);
            posicionEntidad = new Vector3(posX, posY, 0);

            posicionValida = true;

            foreach (Vector3 posExistente in posicionesOcupadas)
            {
                if (Vector3.Distance(posicionEntidad, posExistente) < distancia)
                {
                    posicionValida = false;
                    break;
                }
            }

            intentos++;
        } while (!posicionValida && intentos < 100);

        Instantiate(prefab, posicionEntidad, Quaternion.identity, transform);
        posicionesOcupadas.Add(posicionEntidad);
    }

    public Vector3 SituarPuerta()
    {
        Vector3 centro = transform.position;
        int pared;

        pared = Random.Range(0, 4);

        return pared switch
        {
            0 => new Vector3(centro.x, centro.y - altura / 2 + grosorPared, 0),  // Abajo
            1 => new Vector3(centro.x, centro.y + altura / 2 - grosorPared, 0),  // Arriba
            2 => new Vector3(centro.x - ancho / 2 + grosorPared, centro.y, 0),   // Izquierda
            _ => new Vector3(centro.x + ancho / 2 - grosorPared, centro.y, 0)    // Derecha
        };
    }
}