using System.Collections.Generic;
using UnityEngine;

public class SonidoManager : MonoBehaviour
{
    private AudioSource audioMusica;
    public static SonidoManager sonidoManager { get; private set; }
    private Dictionary<string, AudioClip> sonidos = new();
    
    void Awake()
    {
        if (sonidoManager == null) sonidoManager = this;
        audioMusica = gameObject.AddComponent<AudioSource>();
        audioMusica.loop = true;
        audioMusica.playOnAwake = false;
        CargarSonidos();
    }

    private void CargarSonidos()
    {
        CargarSonido("Abrir_Puerta", "Sonidos/Efectos/Abrir_Puerta");
        CargarSonido("Pulsar_Boton", "Sonidos/Efectos/Pulsar_Boton");

        CargarSonido("Ataque_Lanza", "Sonidos/Efectos/Ataque_Lanza");
        CargarSonido("Ataque_Latigo", "Sonidos/Efectos/Ataque_Latigo");
        CargarSonido("Beber_Pocion", "Sonidos/Efectos/Beber_Pocion");
        CargarSonido("Lanza_Clavada", "Sonidos/Efectos/Lanza_Clavada");

        CargarSonido("Sonido_Muerte", "Sonidos/Efectos/Sonido_Muerte");
        CargarSonido("Abrir_Cofre", "Sonidos/Efectos/Abrir_Cofre");
        CargarSonido("Vendas", "Sonidos/Efectos/Vendas");

        CargarSonido("Error", "Sonidos/Efectos/Error");
        CargarSonido("Compra", "Sonidos/Efectos/Compra");

        CargarSonido("Musica_Menu", "Sonidos/Musica/Musica_Menu");
        CargarSonido("Musica_Mazmorra", "Sonidos/Musica/Musica_Mazmorra");
        CargarSonido("Musica_Comerciante", "Sonidos/Musica/Musica_Comerciante");
    }

    public void ReproducirMusica(string nombre, float volumen = 1f)
    {
        AudioClip clip = ObtenerSonido(nombre);
        audioMusica.clip = clip;
        audioMusica.volume = volumen;
        audioMusica.Play();
    }

    public void PararMusica() => audioMusica.Stop();

    private void CargarSonido(string clave, string ruta) => sonidos[clave] = Resources.Load<AudioClip>(ruta);

    public AudioClip ObtenerSonido(string clave) => sonidos.TryGetValue(clave, out AudioClip s) ? s : null;
}