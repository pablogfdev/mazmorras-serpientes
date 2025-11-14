public class AccesoRapidoUIController : InventarioBaseUI
{
    public static AccesoRapidoUIController accesoRapidoUIController;

    protected new void Awake()
    {
        filas = 1;
        columnas = 6;
        tamano = 130f;
        espacio = -8f;
        margen = 40f;

        base.Awake();
        accesoRapidoUIController = this;
    }
}