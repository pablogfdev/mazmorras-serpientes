using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using PM = PrefabManager;

public class EscenasController : MonoBehaviour
{
    public static EscenasController escenasController { get; private set; }
    public static string escenaDestino;
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

    void Start() => SonidoManager.sonidoManager.ReproducirMusica("Musica_Menu", 0.15f);

    public void CargarEscenaMazmorras(int nivel)
    {   
        SonidoManager.sonidoManager.PararMusica();
        alFinalizar = () =>
        {
            InstanciarElementosEscenaMazmorras(nivel);
            SonidoManager.sonidoManager.ReproducirMusica("Musica_Mazmorra", 0.1f);
        };
        escenaDestino = "Mazmorras";
        StartCoroutine(CargarEscenaConProgreso());
    }

    public void CargarEscenaComerciante()
    {   
        SonidoManager.sonidoManager.PararMusica();
        alFinalizar = () => { 
            InstanciarElementosEscenaComerciante();
            SonidoManager.sonidoManager.ReproducirMusica("Musica_Comerciante", 0.1f);
        };
        escenaDestino = "Comerciante";
        StartCoroutine(CargarEscenaConProgreso());
    }
    

    public void CargarEscenaMenuPrincipal()
    {
        SonidoManager.sonidoManager.PararMusica();
        alFinalizar = null;
        escenaDestino = "MenuPrincipal";
        SceneManager.LoadScene(escenaDestino);
        SonidoManager.sonidoManager.ReproducirMusica("Musica_Menu", 0.15f);

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
        //mazmorraController.DibujarConexionesDebug(Color.black, 2f);
    }

    void InstanciarElementosEscenaComerciante()
    {
        Instantiate(PM.prefabManager.ObtenerPrefab("SalaPrincipal"), Vector3.zero, Quaternion.identity);
        Instantiate(PM.prefabManager.ObtenerPrefab("Jugador"), Vector3.zero, Quaternion.identity);
    }
}