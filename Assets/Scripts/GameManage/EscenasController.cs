using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscenasController : MonoBehaviour
{
    private GameObject mazmorraPrefab;
    private GameObject comerciantePrefab;
    private GameObject jugadorPrefab;
    private GameObject mazmorra;

    public void CargarEscenaMazmorras(int nivel) => StartCoroutine(CrearMazmorras(nivel));
    public void CargarEscenaComerciante() => StartCoroutine(CrearComerciante());
    public void CargarEscenaMenuPrincipal() => StartCoroutine(CrearMenuPrincipal());

    void Awake()
    {
        if (GameObject.FindGameObjectsWithTag("JuegoController").Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        mazmorraPrefab = Resources.Load<GameObject>("Prefabs/Mazmorra");
        comerciantePrefab = Resources.Load<GameObject>("Prefabs/SalaPrincipal");
        jugadorPrefab = Resources.Load<GameObject>("Prefabs/Jugador");
    }

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
        Instantiate(comerciantePrefab, Vector3.zero, Quaternion.identity);
        Instantiate(jugadorPrefab, Vector3.zero, Quaternion.identity);
    }


    IEnumerator CrearMenuPrincipal()
    {
        AsyncOperation procesoCarga = SceneManager.LoadSceneAsync("MenuPrincipal", LoadSceneMode.Single);
        while (!procesoCarga.isDone) yield return null;
    }
}