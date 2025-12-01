public class ItemStack
{
    public Item item;
    public int cantidad;
    public bool vacio => item == null || cantidad <= 0;

    public ItemStack(Item item, int cantidad)
    {
        this.item = item;
        this.cantidad = cantidad;
    }

    public void Limpiar()
    {
        item = null;
        cantidad = 0;
    }
}