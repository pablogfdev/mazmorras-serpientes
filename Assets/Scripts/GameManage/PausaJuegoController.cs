using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;

public class PausaJuegoController : MonoBehaviour
{
    public List<MonoBehaviour> scriptsEnPausa = new List<MonoBehaviour>();
    public bool MenuNivelAbierto { get; private set; } = false;
    public bool JuegoPausado { get; private set; } = false;

    void OnEnable() => SceneManager.sceneLoaded += IniciarEscena;

    void OnDisable() => SceneManager.sceneLoaded -= IniciarEscena;

    void IniciarEscena(Scene escena, LoadSceneMode modo)
    {
        if (escena.name != "Comerciante") return;
        scriptsEnPausa.Clear();
        scriptsEnPausa.AddRange(GameObject.FindWithTag("Player").GetComponents<MonoBehaviour>());
    }

    public void ToggleMenuNiveles()
    {
        MenuNivelAbierto = !MenuNivelAbierto;
        foreach (var script in scriptsEnPausa)
            script.enabled = !MenuNivelAbierto;
    }

    //Metodos ResumeGame() y PauseGame() no implementados
    //Implementar junto a la creación del menú de pausa
    //Ademas, simplificar en un solo metodo: TogglePausaJuego()
    public void ResumeGame()
    {
        if (!JuegoPausado) return;
        JuegoPausado = false;
        Time.timeScale = 1f;
    }

    public void PauseGame()
    {
        if (JuegoPausado) return;
        JuegoPausado = true;
        Time.timeScale = 0f;
    }
}