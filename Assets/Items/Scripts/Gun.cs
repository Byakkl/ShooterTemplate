
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

    public Gun(int a_startingAmmo, int a_maxAmmo, float a_fireDelay){
        currentAmmo = a_startingAmmo;
        maxAmmo = a_maxAmmo;
        fireDelay = a_fireDelay;
        fireDelayTimer = 0;

        isItemActive = true;
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

        fireDelayTimer = fireDelay;
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
        //No custom cleanup needed
        //Could do something like return currentAmmo to player reserves instead though that would be better implemented from a "discard item" on the player side
        return;
    }
    #endregion
}
