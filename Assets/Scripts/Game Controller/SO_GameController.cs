using UnityEngine;

[CreateAssetMenu(menuName = "Game Logic/Game Controller", fileName = "so_GameController")]
public class SO_GameController : ScriptableObject
{
    //Instance of the GameController
    private GameController instance;

    /// <summary>
    /// Sets the instance of the GameController
    /// </summary>
    /// <param name="a_instance"></param>
    public void SetGameControllerInstance(GameController a_instance) => instance = a_instance;

    /// <summary>
    /// Shortcut method
    /// </summary>
    /// <param name="a_item">Item to be added</param>
    public void AddItem(IItem a_item) => instance.AddItem(a_item);

    /// <summary>
    /// Shortcut method
    /// </summary>
    /// <param name="a_item">Item to be removed</param>
    public void RemoveItem(IItem a_item) => instance.RemoveItem(a_item);
}
