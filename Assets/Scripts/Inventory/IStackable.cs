public interface IStackable
{
    public int quantity { get; set; }

    public int maxStack { get; set; }

    public int IncreaseQuantity(int a_quantity);

    public int IncreaseQuantity(ref IStackable a_other);

    public void DecreaseQuantity(int a_quantity);
}