using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour, ISpawner
{
    //The prefab of the item to be spawned
    [SerializeField]
    public GameObject prefab { get => prefab; set => prefab = value; }

    //Should the item start spawned
    [SerializeField]
    bool startSpawned = true;

    //Should the item respawn after being picked up
    [SerializeField]
    bool shouldRespawn = true;

    //Stores the spawned object instance for tracking when it has been picked up
    GameObject instance;

    // Start is called before the first frame update
    void Start()
    {
        if(startSpawned)
            Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn(){
        instance = Instantiate(prefab);
    }
}
