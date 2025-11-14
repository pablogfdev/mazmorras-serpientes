public class InventarioUIController : InventarioBaseUI
{
    public static InventarioUIController inventarioUIController;

    protected new void Awake()
    {
        filas = 5;
        columnas = 6;
        tamano = 130f;
        espacio = -10f;
        margen = 45f;

        base.Awake();
        inventarioUIController = this;
    }
}