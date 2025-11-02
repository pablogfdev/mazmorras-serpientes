using System;

public class Item
{
    private int id;  
    public int Id { get => id;  private set { id = value; } }
    public string nombre;
    public string descripcion;
    public bool stackeable = true;
    public int maxStack = 20;
    public Action<JugadorController> efecto;

    public Item(int id, string nombre, bool stackeable,Action<JugadorController> efecto = null)
    {
        this.id = id;
        this.nombre = nombre;
        this.efecto = efecto;
        this.stackeable = stackeable;
    }

    public void Usar(JugadorController jugador) => efecto?.Invoke(jugador);    
}