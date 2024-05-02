using UnityEngine;

public class PlayerSpawner : Spawner
{
    //Store the prefab for the projectile to be used by the player's starting gun
    [SerializeField]
    GameObject projectilePrefab;

    // Update is called once per frame
    public override void Spawn()
    {
        //Run the base update
        base.Spawn();

        //Custom player things, such as resetting ammo counts, etc
        PlayerController controller = instance.GetComponent<PlayerController>();

        //Create the player's starting starting item
        Gun gun = new Gun(6, 6, 0.2f, projectilePrefab);
        //Add the item to tracking
        gameController.AddItem(gun);

        //Set the player's current item to the created gun
        controller.SetCurrentItem(gun);

        Ammo startingAmmo = new Ammo(2);
        //Give the player their starting ammo reserve
        controller.AddAmmoReserve(ref startingAmmo);
    }
}
