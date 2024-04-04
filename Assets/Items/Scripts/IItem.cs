using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem
{
    //Tracks if this item is active or should be cleaned up by the GameController
    public bool isItemActive {get; set;}

    public void UsePrimary();

    public void UseSecondary();

    public void ItemUpdate();

    public void Cleanup();
}
