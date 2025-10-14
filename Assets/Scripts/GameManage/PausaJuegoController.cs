using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using EC = EscenasController;
using PM = PrefabManager;

public class PausaJuegoController : MonoBehaviour
{
    public static PausaJuegoController pausaJuegoController { get; private set; }
    private List<MonoBehaviour> scriptsEnPausa = new List<MonoBehaviour>();
    public bool MenuNivelAbierto { get; private set; } = false;
    public bool juegoPausado = false;

    private Button botonSalirPausa;
    private Button botonReanudarPausa;

    private bool esMenuPrincipal;
    private bool esMenuCarga;

    private GameObject menuPausa;

    void Awake() => pausaJuegoController = this;
    
    void OnEnable() => SceneManager.sceneLoaded += IniciarEscena;

    void OnDisable() => SceneManager.sceneLoaded -= IniciarEscena;

    void Update()
    {
        if (esMenuPrincipal || esMenuCarga) return;
        if (!juegoPausado && Input.GetKeyDown(KeyCode.Space)) TogglePausa();
    }

    void IniciarEscena(Scene escena, LoadSceneMode modo)
    {
        Time.timeScale = 1f;
        esMenuPrincipal = escena.name == "MenuPrincipal";
        esMenuCarga = escena.name == "Carga";
        if (esMenuPrincipal) return; 
        StartCoroutine(BuscarJugador());
        CrearMenu();
    }

    private IEnumerator BuscarJugador()
    {
        yield return null;
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null) yield break;
        scriptsEnPausa.Clear();
        scriptsEnPausa.AddRange(player.GetComponents<MonoBehaviour>());
    }

    void CrearMenu()
    {
        if (menuPausa != null) Destroy(menuPausa);
        menuPausa = Instantiate(PM.prefabManager.ObtenerPrefab("MenuPausa"));
        botonSalirPausa = menuPausa.transform.Find("BotonSalirPartida").GetComponent<Button>();
        botonReanudarPausa = menuPausa.transform.Find("BotonReanudar").GetComponent<Button>();
        botonSalirPausa.onClick.AddListener(SalirPartida);
        botonReanudarPausa.onClick.AddListener(() => TogglePausa());
        menuPausa.SetActive(false);
    }

    private void SalirPartida()
    {
        TogglePausa();
        EC.escenasController.CargarEscenaMenuPrincipal();
    }

    public void ToggleMenuNiveles()
    {
        MenuNivelAbierto = !MenuNivelAbierto;
        foreach (var script in scriptsEnPausa) script.enabled = !MenuNivelAbierto;
    }

    public void TogglePausa()
    {
        juegoPausado = !juegoPausado;
        Time.timeScale = juegoPausado ? 0f : 1f;
        menuPausa.SetActive(juegoPausado);
    }
}