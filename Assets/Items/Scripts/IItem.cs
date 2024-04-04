using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem
{
    public abstract void UsePrimary();

    public abstract void UseSecondary();

    public abstract void ItemUpdate();
}
