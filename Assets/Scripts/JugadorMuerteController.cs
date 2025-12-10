using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class JugadorMuerteController : MonoBehaviour
{
    private bool yaMuerto = false;
    private GameObject cuerpo;

    void Awake() => cuerpo = transform.Find("Cuerpo").gameObject;
    
    public void IniciarMuerte()
    {
        if (yaMuerto) return; 
        yaMuerto = true;
        StartCoroutine(SecuenciaMuerte());
        ProcesarMuerteSegunDificultad();
    }
   
    private IEnumerator SecuenciaMuerte()
    {   
        SonidoManager.sonidoManager.PararMusica();
        //AudioSource.PlayClipAtPoint(SonidoManager.sonidoManager.ObtenerSonido("Sonido_Muerte"), Camera.main.transform.position);
        cuerpo.GetComponent<Animator>().enabled = false;
        cuerpo.GetComponent<SpriteRenderer>().sprite = SpriteManager.spriteManager.ObtenerSprite("Jugador_Muerto");
        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
        foreach (var s in scripts) if (s != this) s.enabled = false;
        foreach (var col in GetComponentsInChildren<Collider2D>()) col.enabled = false;
        yield return new WaitForSeconds(3f);
        MostrarGameOverUI();

        var jugador = GameObject.FindGameObjectWithTag("Player");
        var cam = jugador.GetComponentInChildren<Camera>()?.transform;
        cam.SetParent(null);
        Destroy(GameObject.FindGameObjectWithTag("Mazmorra"));
    }

    private void MostrarGameOverUI()
    {
        GameObject menuGameOver = Instantiate(PrefabManager.prefabManager.ObtenerPrefab("MenuGameOver"));
        Button botonMenu = menuGameOver.transform.Find("BotonMenuPrincipal")?.GetComponent<Button>();
        Button botonSalir = menuGameOver.transform.Find("BotonSalirPartida")?.GetComponent<Button>();

        if (botonMenu != null) botonMenu.onClick.AddListener(EscenasController.escenasController.CargarEscenaComerciante);
        if (botonSalir != null) botonSalir.onClick.AddListener(EscenasController.escenasController.CargarEscenaMenuPrincipal);
    }

    private void ProcesarMuerteSegunDificultad()
    {
        GestorPartidas.inventarioJugador.slots.ForEach(s => s.Limpiar());

        switch (GestorPartidas.partidaActiva.dificultad)
        {
            case Dificultad.Normal:
                break;

            case Dificultad.Dificil:
                GestorPartidas.inventarioTaquilla.slots.ForEach(s => s.Limpiar());
                break;

            case Dificultad.Experto:
                GestorPartidas.partidaActiva.nivel = 1;
                GestorPartidas.inventarioTaquilla.slots.ForEach(s => s.Limpiar());
                break;
        }

        GestorPartidas.GuardarInventarios();
    }
}