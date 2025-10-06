using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using PM = PrefabManager;

public class EscenasController : MonoBehaviour
{
    public static EscenasController escenasController { get; private set; }

    private GameObject mazmorra;

    public void CargarEscenaMazmorras(int nivel) => StartCoroutine(CrearMazmorras(nivel));
    public void CargarEscenaComerciante() => StartCoroutine(CrearComerciante());
    public void CargarEscenaMenuPrincipal() => StartCoroutine(CrearMenuPrincipal());

    void Awake()
    {
        if (escenasController != null && escenasController != this)
        {
            Destroy(gameObject);
            return;
        }

        escenasController = this;
        DontDestroyOnLoad(gameObject);
    }


    IEnumerator CrearMazmorras(int nivel)
    {
        AsyncOperation procesoCarga = SceneManager.LoadSceneAsync("Mazmorras", LoadSceneMode.Single);
        while (!procesoCarga.isDone) yield return null;
        mazmorra = Instantiate(PM.prefabManager.ObtenerPrefab("Mazmorra"), Vector3.zero, Quaternion.identity);
        mazmorra.GetComponent<MazmorraController>().Nivel = nivel;
        mazmorra.GetComponent<MazmorraController>().CrearMazmorra();
    }

    IEnumerator CrearComerciante()
    {
        AsyncOperation procesoCarga = SceneManager.LoadSceneAsync("Comerciante", LoadSceneMode.Single);
        while (!procesoCarga.isDone) yield return null;
        Instantiate(PM.prefabManager.ObtenerPrefab("SalaPrincipal"), Vector3.zero, Quaternion.identity);
        Instantiate(PM.prefabManager.ObtenerPrefab("Jugador"), Vector3.zero, Quaternion.identity);
    }

    IEnumerator CrearMenuPrincipal()
    {
        AsyncOperation procesoCarga = SceneManager.LoadSceneAsync("MenuPrincipal", LoadSceneMode.Single);
        while (!procesoCarga.isDone) yield return null;
    }
}