using System;

public class Item
{
    private int id;  
    public int Id { get => id;  private set { id = value; } }
    public string nombre;
    public string descripcion;
    public bool stackeable = true;
    public int maxStack;
    public float? tiempoDeEspera;
    public Action<JugadorController> efecto;
    public float probabilidadAparacición = 1f; 
    public int nivelAparicion = 1; 

    public Item(
        int id,
        string nombre,
        bool stackeable,
        int maxStack,
        float? tiempoDeEspera = null,
        Action<JugadorController> efecto = null,
        float probabilidadAparacición = 1f,
        int nivelMinDrop = 1)
    {
        Id = id;
        this.nombre = nombre;
        this.stackeable = stackeable;
        this.maxStack = stackeable ? maxStack : 1;
        this.tiempoDeEspera = tiempoDeEspera;
        this.efecto = efecto;
        this.probabilidadAparacición = probabilidadAparacición;
        this.nivelAparicion = nivelMinDrop;
    }

    public void Usar(JugadorController jugador) => efecto?.Invoke(jugador);    
}