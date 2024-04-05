using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    float projectileLifetime = 3.0f;

    float lifeTimer;

    void Update(){
        //Decrease the projectile lifetime timer by delta time
        if(lifeTimer > 0)
            lifeTimer -= Time.deltaTime;

        //If the projectile has expired deactivate it
        if(lifeTimer <= 0)
            gameObject.SetActive(false);
    }

    /// <summary>
    /// Resets the lifetime timer of the projectile
    /// </summary>
    public void Reset(){
        lifeTimer = projectileLifetime;
    }

    public void OnTriggerEnter(Collider a_collider){
        //Attempt to get an IShootable component from the hit object
        IShootable shootable = a_collider.gameObject.GetComponent<IShootable>();

        //If it has an IShootable component then invoke its hit response
        if(shootable != null)
            shootable.OnHit();

        //Disable the projectile
        gameObject.SetActive(false);
    }
}
