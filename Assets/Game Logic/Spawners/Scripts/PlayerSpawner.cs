using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : Spawner
{
    // Update is called once per frame
    public override void Update()
    {
        //Run the base update
        base.Update();

        //Custom player things, such as resetting ammo counts, etc
    }
}
