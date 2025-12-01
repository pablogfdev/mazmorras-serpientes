using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class JugadorMuerteController : MonoBehaviour
{
    private Rigidbody2D rb;

    void Awake() => rb = GetComponent<Rigidbody2D>();
    
    public void IniciarMuerte()
    {
        StartCoroutine(SecuenciaMuerte());
        ProcesarMuerteSegunDificultad();
    }
   
    private IEnumerator SecuenciaMuerte()
    {
        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
        foreach (var s in scripts) if (s != this) s.enabled = false;;
        foreach (var col in GetComponentsInChildren<Collider2D>()) col.enabled = false;
        transform.Find("Cuerpo").rotation = Quaternion.Euler(0f, 0f, 90f);
        yield return new WaitForSeconds(3f);
        MostrarGameOverUI();
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