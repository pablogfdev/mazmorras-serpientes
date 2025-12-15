using System.Collections.Generic;
using UnityEngine;
using PM = PrefabManager;

public class HabitacionController : MonoBehaviour
{
    private int semilla;
    private System.Random generador;
    private int nivel;
    private float ancho = 30;
    private float altura = 20;
    private float grosorPared = 1f;

    private int numCofres;
    private int numSerpientes;
    private int numGolems;
    private int numEspiritus;
    private MazmorraController mazmorra;

    private int numEntidadesCreadas = 0;

    public int X { get; private set; }
    public int Y { get; private set; }

    float distancia = 2f;

    public bool LlaveGenerada { get; set; } = false;

    List<GameObject> posicionesOcupadas = new List<GameObject>();

    public void Inicializar()
    {
        nivel = GetComponentInParent<MazmorraController>().Nivel;
        InicializarGenerador();
        CalcularNumeroEntidades();
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

    void CalcularNumeroEntidades()
    {
        numCofres = generador.Next(1, 4);
        numSerpientes = generador.Next(1, 5);
        numGolems = generador.Next(1, 5);
        numEspiritus = generador.Next(1, 5);
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

    private void InicializarGenerador()
    {
        semilla = GestorPartidas.partidaActiva.semilla;
        string[] partes = name.Split('_');
        int.TryParse(partes[1], out int x);
        int.TryParse(partes[2], out int y);

        X = x;
        Y = y;

        generador = new System.Random(semilla * 1000 + X * 100 + Y + nivel * 91);
    }

    void generarEntidades()
    {
        for (int i = 0; i < numCofres; i++) colocarEntidadEnPosicioValida(PM.prefabManager.ObtenerPrefab("Cofre"));
        for (int i = 0; i < Mathf.Min(Mathf.CeilToInt(numSerpientes * nivel / 10f), numSerpientes); i++) colocarEntidadEnPosicioValida(PM.prefabManager.ObtenerPrefab("Serpiente"));
        if (nivel >= 4) for (int i = 0; i < Mathf.Min(Mathf.CeilToInt(numGolems * nivel / 10f), numGolems); i++) colocarEntidadEnPosicioValida(PM.prefabManager.ObtenerPrefab("GolemPiedra"));
        if (nivel >= 7) for (int i = 0; i < Mathf.Min(Mathf.CeilToInt(numEspiritus * nivel / 10f), numEspiritus); i++) colocarEntidadEnPosicioValida(PM.prefabManager.ObtenerPrefab("Espiritu"));
    }

    void colocarEntidadEnPosicioValida(GameObject prefab)
    {
        Vector3 centro = transform.position;
        float rangoX = (ancho / 2) - grosorPared - 4f;
        float rangoY = (altura / 2) - grosorPared - 4f;

        Vector3 posicionEntidad;
        bool posicionValida;
        int intentos = 0;

        do
        {
            float posX = (float)(generador.NextDouble() * 2 * rangoX - rangoX + centro.x);
            float posY = (float)(generador.NextDouble() * 2 * rangoY - rangoY + centro.y);
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

        GameObject nuevo = Instantiate(prefab, posicionEntidad, Quaternion.identity, transform);
        int numero = numEntidadesCreadas++;
        nuevo.name = $"{prefab.tag}_{X}_{Y}_{numero}";
        posicionesOcupadas.Add(nuevo);
    }
}