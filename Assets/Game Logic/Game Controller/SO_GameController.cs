using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Logic/Game Controller", fileName = "so_GameController")]
public class SO_GameController : ScriptableObject
{
    //Instance of the GameController
    public GameController instance;

    /// <summary>
    /// Shortcut method
    /// </summary>
    /// <param name="a_item">Item to be added</param>
    public void AddItem(IItem a_item){
        instance.AddItem(a_item);
    }

    /// <summary>
    /// Shortcut method
    /// </summary>
    /// <param name="a_item">Item to be removed</param>
    public void RemoveItem(IItem a_item){
        instance.RemoveItem(a_item);
    }
}
