public class DatosNuevaPartida
{
    public string nombre;
    public Dificultad dificultad;
    public int? semilla;
}

public enum Dificultad
{
    Normal = 0,
    Dificil = 1,
    Experto = 2
}