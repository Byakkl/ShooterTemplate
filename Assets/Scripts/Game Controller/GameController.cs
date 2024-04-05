using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //Reference to the scriptable object that contains the reference to this instance
    [SerializeField]
    SO_GameController gameController;
    
    //List of all active items
    List<IItem> itemInstances = new List<IItem>();

    void Awake()
    {
        //Set the scriptable object instance to this one
        gameController.instance = this;
        //Lock the cursor to the center of the screen and hide it
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update(){
        //Update active items
        foreach(IItem item in itemInstances){
            item.ItemUpdate();
        }

        //Remove inactive items
        for(int idx = itemInstances.Count - 1; idx >= 0; idx--){
            //If the item is still active, move on
            if(itemInstances[idx].isItemActive)
                continue;

            //Remove the item from the list
            RemoveItem(itemInstances[idx]);
        }
    }

    /// <summary>
    /// Begins tracking an item
    /// </summary>
    /// <param name="a_item">The item to track</param>
    public void AddItem(IItem a_item){
        //Don't add duplicates
        if(itemInstances.Contains(a_item))
            return;
        
        itemInstances.Add(a_item);
    }

    /// <summary>
    /// Removes an item from tracking
    /// </summary>
    /// <param name="a_item">The item to stop tracking</param>
    public void RemoveItem(IItem a_item){
        if(!itemInstances.Contains(a_item))
            return;

        //Remove the item from the list
        itemInstances.Remove(a_item);
        
        //Do any custom cleanup before letting go of the reference
        a_item.Cleanup();
    }
}
