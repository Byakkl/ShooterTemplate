using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory<T>
{
    //List of items in the inventory
    public List<T> items = new List<T>();

    //Reference to the current item the inventory has selected
    public T currentItem;

    //Index of the item in the list that is selected
    private int currentIndex = 0;

    //Change the item directly with an index value
    public void ChangeItem(int a_index){
        if(items.Count == 0)
            return;

        if(a_index < 0 || a_index >= items.Count)
            return;

        currentIndex = a_index;
        currentItem = items[currentIndex];
    }

    public void ChangeItem(bool a_scrollPositive){
        if(items.Count == 0)
            return;

        //Update the index
        currentIndex += 1 * (a_scrollPositive ? 1 : -1);
        //Cycle for going past the number of items in inventory
        currentIndex %= items.Count;
        //Cycling for going below 0
        if(currentIndex < 0)
            currentIndex = items.Count + currentIndex;
        //Set the current item
        currentItem = items[currentIndex];
    }

    public void AddItem(ref T a_item){
        //Get the type of the input item
        Type t = a_item.GetType();
        //If it's a stackable element
        if(a_item is IStackable){
            //Fetch all of the items with the matching type
            List<T> innerItems = items.FindAll((item) => {return item.GetType() == t;});
            IStackable casted = a_item as IStackable;
            //Iterate over the items and increase their quantity, updating the number of input items
            foreach(T innerItem in innerItems){
                (innerItem as IStackable).IncreaseQuantity(ref casted);
                //If all of the input items have been stacked, exit
                if(casted.quantity == 0)
                    return;
            }
        }
        //If the item isn't stackable, or there is an overflow of a stackable item, add it to the inventory
        items.Add(a_item);
    }

    public void RemoveItem(T item){
        if(!items.Contains(item))
            return;

        items.Remove(item);
    }

    public void EmptyInventory(bool deleteItems){
        if(deleteItems)
            items.Clear();
        else
        {
            //drop the items
            items.Clear();
        }
    }
}