public enum TipoMoneda { Oro, Gema }

public class ItemVenta
{
    public int id;
    public TipoMoneda moneda;
    public int precio;

    public ItemVenta(int id, TipoMoneda moneda, int precio)
    {
        this.id = id;
        this.moneda = moneda;
        this.precio = precio;
    }
}