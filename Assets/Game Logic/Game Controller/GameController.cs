using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    SO_GameController gameController;
    
    //List of all active items
    List<IItem> itemInstances = new List<IItem>();

    void Awake()
    {
        gameController.instance = this;
        //Lock the cursor to the center of the screen and hide it
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update(){
        //Update active items
        foreach(IItem item in itemInstances){
            item.ItemUpdate();
        }
    }

    public void AddItem(IItem a_item){
        //Don't add duplicates
        if(itemInstances.Contains(a_item))
            return;
        
        itemInstances.Add(a_item);
    }

    public void RemoveItem(IItem a_item){
        if(!itemInstances.Contains(a_item))
            return;

        itemInstances.Remove(a_item);
    }
}
