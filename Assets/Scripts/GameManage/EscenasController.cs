using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using PM = PrefabManager;

public class EscenasController : MonoBehaviour
{
    public static EscenasController escenasController { get; private set; }

    private static string escenaDestino;
    private static System.Action alFinalizar;

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

    public void CargarEscenaMazmorras(int nivel)
    {
        alFinalizar = () =>
        {
            InstanciarElementosEscenaMazmorras(nivel);
        };
        escenaDestino = "Mazmorras";
        StartCoroutine(CargarEscenaConProgreso());
    }



    public void CargarEscenaComerciante()
    {
        alFinalizar = () => { InstanciarElementosEscenaComerciante(); };
        escenaDestino = "Comerciante";
        StartCoroutine(CargarEscenaConProgreso());
    }
    
    

    public void CargarEscenaMenuPrincipal()
    {
        alFinalizar = null;
        escenaDestino = "MenuPrincipal";
        SceneManager.LoadScene(escenaDestino);
    }

    private IEnumerator CargarEscenaConProgreso()
    {
        yield return SceneManager.LoadSceneAsync("Carga");
        yield return null;

        if (CargaUI.cargaUI != null) CargaUI.cargaUI.ActualizarProgreso(0f);

        AsyncOperation proceso = SceneManager.LoadSceneAsync(escenaDestino);
        proceso.allowSceneActivation = false;

        float progresoVisual = 0f;
        float progresoReal = 0f;

        while (proceso.progress < 0.9f)
        {
            progresoReal = Mathf.Clamp01(proceso.progress / 0.9f);
            progresoVisual = Mathf.MoveTowards(progresoVisual, progresoReal, Time.deltaTime * 0.5f);
            if (CargaUI.cargaUI != null) CargaUI.cargaUI.ActualizarProgreso(progresoVisual);
            yield return null;
        }

        float timer = 0f;
        while (timer < 1f)
        {
            progresoVisual = Mathf.MoveTowards(progresoVisual, 1f, Time.deltaTime);
            if (CargaUI.cargaUI != null) CargaUI.cargaUI.ActualizarProgreso(progresoVisual);
            timer += Time.deltaTime;
            yield return null;
        }

        proceso.allowSceneActivation = true;
        yield return null;
        alFinalizar?.Invoke();
    }
    
    void InstanciarElementosEscenaMazmorras(int nivel)
    {
        GameObject mazmorra = Instantiate(PM.prefabManager.ObtenerPrefab("Mazmorra"), Vector3.zero, Quaternion.identity);
        MazmorraController mazmorraController = mazmorra.GetComponent<MazmorraController>();
        mazmorraController.Nivel = nivel;
        mazmorraController.CrearMazmorra();
    }

    void InstanciarElementosEscenaComerciante()
    {
        Instantiate(PM.prefabManager.ObtenerPrefab("SalaPrincipal"), Vector3.zero, Quaternion.identity);
        Instantiate(PM.prefabManager.ObtenerPrefab("Jugador"), Vector3.zero, Quaternion.identity);
    }
}