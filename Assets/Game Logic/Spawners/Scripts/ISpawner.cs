using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawner
{
    [SerializeField]
    GameObject prefab {get; set;}

    void Spawn();
}
