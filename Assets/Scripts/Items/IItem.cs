public interface IItem
{
    //Tracks if this item is active or should be cleaned up by the GameController
    public bool isItemActive {get; set;}

    public void UsePrimary();

    public void UseSecondary();

    public void Reload();

    public void ItemUpdate();

    public void Cleanup();
}
