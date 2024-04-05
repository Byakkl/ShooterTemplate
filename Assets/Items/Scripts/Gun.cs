
using System.Collections.Generic;
using UnityEngine;

public class Gun : IItem
{
    //Current ammo in the gun
    int currentAmmo;

    //Maximum ammo allowed in the gun
    int maxAmmo;

    //Delay between shots
    float fireDelay;

    //Remaining time before another shot can be fired
    float fireDelayTimer;

    //Prefab for projectiles used by this gun
    GameObject projectilePrefab;
    //List of projectiles in the pool
    List<GameObject> projectilePool = new List<GameObject>();

    //Transform of the projectile source to be used when firing
    Transform fireOrigin;

    public Gun(int a_startingAmmo, int a_maxAmmo, float a_fireDelay, GameObject a_projectilePrefab){
        currentAmmo = a_startingAmmo;
        maxAmmo = a_maxAmmo;
        fireDelay = a_fireDelay;
        fireDelayTimer = 0;

        isItemActive = true;

        projectilePrefab = a_projectilePrefab;
        //Create an initial projectile pool
        GameObject projectile;
        for(int idx = 0; idx < 5; idx++){
            projectile = GameObject.Instantiate(projectilePrefab);
            projectile.SetActive(false);
            projectilePool.Add(projectile);
        }
    }

    /// <summary>
    /// Increases the current ammo count of the gun using the amount of 'availableAmmo' provided
    /// </summary>
    /// <param name="a_availableAmmo">The amount of ammo available to be used</param>
    /// <returns>The amount of ammo expended to reload the gun as a negative value</returns>
    public int Reload(int a_availableAmmo){
        //Ammount of ammo required to fill the gun
        int delta = maxAmmo - currentAmmo;

        //Fill the ammo counter
        currentAmmo = Mathf.Clamp(currentAmmo + a_availableAmmo, 0, maxAmmo);

        //Return the change in ammo to reduce player ammo counts
        return maxAmmo - delta - currentAmmo;

        /* 
        Example 1:
        Player has 25 ammo available.
        Gun has 1 current ammo and 6 max ammo

        availableAmmo = 25
        delta = 6 - 1 
              = 5

        currentAmmo = clamp(25 + 1, 0, 6)
                    = 6

        return = 6 - 5 - 6
               = -5

        Example 2:
        Player has 0 ammo available.
        Gun has 5 current ammo and 6 max ammo.

        availableAmmo = 0
        delta = 6 - 5
              = 1

        currentAmmo = clamp(0 + 5, 0, 6)
                    = 5

        return = 6 - 1 - 5
               = 0
        */
    }

    public int GetCurrentAmmo(){
        return currentAmmo;
    }

    public int GetMaxAmmo(){
        return maxAmmo;
    }

    /// <summary>
    /// Invoked when more projectiles are required in the pool
    /// </summary>
    private void AddToPool(){
        GameObject projectile;
        for(int idx = 0; idx < 5; idx++){
            projectile = GameObject.Instantiate(projectilePrefab);
            projectile.SetActive(false);
            projectilePool.Add(projectile);
        }
    }

    public void SetFireOrigin(Transform a_origin){
        fireOrigin = a_origin;
    }

    private void FireProjectile(GameObject a_projectile){
        //Set the initial position of the projectile
        a_projectile.transform.position = fireOrigin.position;
        //Enable the projectile
        a_projectile.SetActive(true);
        //Reset the projectile
        a_projectile.GetComponent<Projectile>().Reset();
        //Add velocity to the projectile rigidbody
        a_projectile.GetComponent<Rigidbody>().velocity = fireOrigin.forward * 50;

    }

    #region Item Implementation
    private bool _isItemActive;
    public bool isItemActive {get => _isItemActive; set => _isItemActive = value;}

    public void UsePrimary()
    {
        //*click*
        if(currentAmmo == 0)
            return;

        //Can't fire faster than the timer
        if(fireDelayTimer > 0)
            return;

        //More interesting logic could be done here depending on the gun
        currentAmmo -= 1;

        //Set the fire rate delay timer
        fireDelayTimer = fireDelay;

        //Find an inactive projectile
        for(int idx = 0; idx < projectilePool.Count; idx++){
            //Skip projectiles that are still active
            if(projectilePool[idx].activeSelf)
                continue;
            
            //Shoot the projectile
            FireProjectile(projectilePool[idx]);
            return;
        }

        //If there are no available projectiles in the pool, add some
        int newIdx = projectilePool.Count;
        AddToPool();
        //The projectiles that are recently added are unused by definition so the size of the pool before adding is now the index of the first addition
        FireProjectile(projectilePool[newIdx]);
    }

    public void UseSecondary()
    {
        //No secondary usage
        return;
    }

    public void ItemUpdate()
    {
        if(fireDelayTimer > 0)
            fireDelayTimer -= Time.deltaTime;
    }

    public void Cleanup(){
        //Could do something like return currentAmmo to player reserves instead though that would be better implemented from a "discard item" on the player side

        //Destroy all of the projectile objects in the pool
        foreach(GameObject projectile in projectilePool){
            GameObject.Destroy(projectile);
        }
        projectilePool.Clear();
        return;
    }
    #endregion
}
