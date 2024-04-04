using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : Spawner
{
    // Update is called once per frame
    public override void Spawn()
    {
        //Run the base update
        base.Spawn();

        //Custom player things, such as resetting ammo counts, etc
        PlayerController controller = instance.GetComponent<PlayerController>();

        //Give the player their starting item
        Gun gun = new Gun(6, 6, 0.2f);
        gameController.AddItem(gun);

        controller.SetCurrentItem(gun);

        //Give the player their starting ammo reserve
        controller.AddAmmoReserve(10);
    }
}
