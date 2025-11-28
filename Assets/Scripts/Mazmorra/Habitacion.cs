using System.Collections.Generic;
using UnityEngine;
using PM = PrefabManager;

public class HabitacionController : MonoBehaviour
{
    private int nivel;
    private float ancho;
    private float altura;
    private float grosorPared = 1f;

    private int numCofres;
    private int numSerpientes;
    private int numGolems;
    private int numEspiritus;
    private MazmorraController mazmorra;

    float distancia = 2f;

    public bool LlaveGenerada { get; set; } = false;

    List<GameObject> posicionesOcupadas = new List<GameObject>();

    void Awake()
    {
        CalcularTamañoYContenido();
        crearParedes();
        crearSuelo();
        generarEntidades();
    }

    public void ColocarLlave()
    {
        foreach (GameObject obj in posicionesOcupadas)
        {
            if (!obj.CompareTag("Cofre")) continue;
            obj.GetComponent<CofreController>().llave = true;
            return;
        }
    }

    void CalcularTamañoYContenido()
    {
        mazmorra = GetComponentInParent<MazmorraController>();
        nivel = mazmorra.Nivel;

        ancho = 15 + nivel * 2;
        altura = 12 + nivel;

        numCofres = Random.Range(1, 1 + (nivel / 3) + 1);
        numSerpientes = Random.Range(nivel / 2, nivel / 2 + 2);
        numGolems = Random.Range(nivel / 2, nivel / 2 + 2);
        numEspiritus = Random.Range(nivel / 2, nivel / 2 + 2);
    }


    void crearSuelo()
    {
        Vector3 centro = transform.position;
        GameObject suelo = Instantiate(PM.prefabManager.ObtenerPrefab("Suelo"), centro, Quaternion.identity, transform);
        suelo.transform.localScale = new Vector3(ancho - grosorPared, altura - grosorPared, 1);
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

        GameObject paredInferior = Instantiate(PM.prefabManager.ObtenerPrefab("Pared"), posicionParedInferior, Quaternion.identity, transform);
        GameObject paredSuperior = Instantiate(PM.prefabManager.ObtenerPrefab("Pared"), posicionParedSuperior, Quaternion.identity, transform);
        GameObject paredIzquierda = Instantiate(PM.prefabManager.ObtenerPrefab("Pared"), posicionParedIzquierda, Quaternion.identity, transform);
        GameObject paredDerecha = Instantiate(PM.prefabManager.ObtenerPrefab("Pared"), posicionParedDerecha, Quaternion.identity, transform);

        paredInferior.transform.localScale = escalaHorizontal;
        paredSuperior.transform.localScale = escalaHorizontal;
        paredIzquierda.transform.localScale = escalaVertical;
        paredDerecha.transform.localScale = escalaVertical;
    }

    void generarEntidades()
    {
        for (int i = 0; i < numCofres; i++) { colocarEntidadEnPosicioValida(PM.prefabManager.ObtenerPrefab("Cofre")); }
        for (int i = 0; i < numSerpientes; i++) { colocarEntidadEnPosicioValida(PM.prefabManager.ObtenerPrefab("Serpiente")); }
        for (int i = 0; i < numGolems; i++) { colocarEntidadEnPosicioValida(PM.prefabManager.ObtenerPrefab("GolemPiedra")); }
        for (int i = 0; i < numEspiritus; i++) { colocarEntidadEnPosicioValida(PM.prefabManager.ObtenerPrefab("Espiritu")); }
    }

    void colocarEntidadEnPosicioValida(GameObject prefab)
    {
        Vector3 centro = transform.position;

        //Reducimos 4f para evitar que la entidad aparezca pegado a las paredes
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

            foreach (GameObject posExistente in posicionesOcupadas)
            {
                if (Vector3.Distance(posicionEntidad, posExistente.transform.position) < distancia)
                {
                    posicionValida = false;
                    break;
                }
            }

            intentos++;
        } while (!posicionValida && intentos < 1000);

        posicionesOcupadas.Add(Instantiate(prefab, posicionEntidad, Quaternion.identity, transform));
    }
}