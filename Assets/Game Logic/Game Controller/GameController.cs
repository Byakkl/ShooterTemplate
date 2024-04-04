using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //Spawn point for player prefab
    [SerializeField]
    Transform spawnPoint;
    
    void Start()
    {
        //Lock the cursor to the center of the screen and hide it
        Cursor.lockState = CursorLockMode.Locked;
    }
}
