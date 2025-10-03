using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PausaJuegoController : MonoBehaviour
{
    public List<MonoBehaviour> scriptsEnPausa = new List<MonoBehaviour>();
    public bool MenuNivelAbierto { get; private set; } = false;
    public bool JuegoPausado { get; private set; } = false;

    private Button botonSalirPausa;
    private Button botonReanudarPausa;

    private bool esMenuPrincipal;

    private GameObject menuPausa;

    private EscenasController escenasController;

    void OnEnable() => SceneManager.sceneLoaded += IniciarEscena;

    void OnDisable() => SceneManager.sceneLoaded -= IniciarEscena;

    void Update()
    {
        if (esMenuPrincipal) return;
        if (!JuegoPausado && Input.GetKeyDown(KeyCode.Space)) TogglePausa();
    }

    void IniciarEscena(Scene escena, LoadSceneMode modo)
    {
        Time.timeScale = 1f;
        escenasController = GameObject.FindWithTag("JuegoController").GetComponent<EscenasController>();
        esMenuPrincipal = escena.name == "MenuPrincipal";
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
        menuPausa = Instantiate(Resources.Load<GameObject>("Prefabs/Menus/MenuPausa"));
        botonSalirPausa = menuPausa.transform.Find("BotonSalirPartida").GetComponent<Button>();
        botonReanudarPausa = menuPausa.transform.Find("BotonReanudar").GetComponent<Button>();
        botonSalirPausa.onClick.AddListener(() =>
        {
            TogglePausa();
            escenasController.CargarEscenaMenuPrincipal();
        });
        botonReanudarPausa.onClick.AddListener(() => TogglePausa());
        menuPausa.SetActive(false);
    }

    public void ToggleMenuNiveles()
    {
        MenuNivelAbierto = !MenuNivelAbierto;
        foreach (var script in scriptsEnPausa) script.enabled = !MenuNivelAbierto;
    }

    public void TogglePausa()
    {
        JuegoPausado = !JuegoPausado;
        Time.timeScale = JuegoPausado ? 0f : 1f;
        menuPausa.SetActive(JuegoPausado);
    }
}