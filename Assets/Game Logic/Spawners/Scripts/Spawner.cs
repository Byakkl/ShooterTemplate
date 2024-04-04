using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    protected SO_GameController gameController;
    
    //The prefab of the object to be spawned
    [SerializeField]
    GameObject prefab;

    //Should the object start spawned
    [SerializeField]
    bool startSpawned;

    //Should the object respawn after the instance is destroyed
    [SerializeField]
    bool shouldRespawn;

    //The time it takes, in seconds, for the object to respawn if respawn is enabled
    [SerializeField]
    float respawnTime;

    //The time remaining before the object is respawned
    float respawnTimeRemaining;

    //Stores the spawned object instance for tracking when it has been destroyed
    protected GameObject instance;

    public virtual void Start(){
        //Initialize the respawn timer
        respawnTimeRemaining = respawnTime;

        //Spawn the inital object if toggled
        if(startSpawned)
            Spawn();
    }

    public virtual void Update(){
        //Exit if the item doesn't respawn
        if(!shouldRespawn)
            return;

        //Exit if the item has not been picked up yet
        if(instance != null)
            return;

        respawnTimeRemaining -= Time.deltaTime;

        if(respawnTimeRemaining <= 0)
            Spawn();
    }

    //Spawns an instance of the prefab
    public virtual void Spawn(){
        //Spawn the instance
        instance = Instantiate(prefab);
        
        //Set the initial position and rotation of the object to match the spawner
        instance.transform.position = transform.position;
        instance.transform.rotation = transform.rotation;
        
        //Reset the respawn timer
        respawnTimeRemaining = respawnTime;

        IItem itemInstance = instance.GetComponent<IItem>();
        if(itemInstance != null)
            gameController.AddItem(itemInstance);
    }
}
