using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscenasController : MonoBehaviour
{
    private GameObject mazmorraPrefab;
    private GameObject mazmorra;
    //private GameObject comerciantePrefab;

    void Awake()
    {
        if (GameObject.FindGameObjectsWithTag("JuegoController").Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        mazmorraPrefab = Resources.Load<GameObject>("Prefabs/Mazmorra");
        //comerciantePrefab = Resources.Load<GameObject>("Prefabs/SalaPrincipal");
    }

    public void CargarEscenaMazmorras(int nivel) => StartCoroutine(CrearMazmorras(nivel)); 
    public void CargarEscenaComerciante() => StartCoroutine(CrearComerciante()); 

    IEnumerator CrearMazmorras(int nivel)
    {
        AsyncOperation procesoCarga = SceneManager.LoadSceneAsync("Mazmorras", LoadSceneMode.Single);
        while (!procesoCarga.isDone) yield return null;
        mazmorra = Instantiate(mazmorraPrefab, Vector3.zero, Quaternion.identity);
        mazmorra.GetComponent<MazmorraController>().Nivel = nivel;
        mazmorra.GetComponent<MazmorraController>().CrearMazmorra();
    }

    IEnumerator CrearComerciante()
    {
        AsyncOperation procesoCarga = SceneManager.LoadSceneAsync("Comerciante", LoadSceneMode.Single);
        while (!procesoCarga.isDone) yield return null;
        //Instantiate(comerciantePrefab, Vector3.zero, Quaternion.identity);
        //Eliminar el prefabs de la escena del comerciante cuando se cree la escena del menu
    }
}