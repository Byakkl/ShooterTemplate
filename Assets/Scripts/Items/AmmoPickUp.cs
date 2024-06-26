using UnityEngine;

public class AmmoPickUp : MonoBehaviour, IPickUp
{
    #region Animation Variables
    //Reference to visual group's transform
    [SerializeField]
    Transform visualTransform;
    //Move speed for float and rotation movement.
    //X is rotation speed
    //Y is vertical ping-pong speed
    [SerializeField]
    Vector2 visualMoveSpeed = new Vector2(1.0f, 1.0f);
    //Used to control if the movement is moving up or down
    bool visualMoveUp;
    //Determine the maximum movement range for the vertical float
    [SerializeField]
    float visualMoveUpThreshold = 1.0f;
    #endregion

    //The minimum amount of ammo the pickup can contain
    [SerializeField]
    int ammoMinimum = 1;

    //The maximum amount of ammo the pickup can contain
    [SerializeField]
    int ammoMaximum = 5;

    [SerializeField]
    AmmoType ammoType;

    //How much ammo the pickup contains
    int ammoValue;

    // Start is called before the first frame update
    void Start()
    {
        //Start the visual pseudo-animation in an upwards direction
        visualMoveUp = true;

        //Determine a random amount of ammo this pickup is worth
        ammoValue = Random.Range(ammoMinimum, ammoMaximum);
    }

    void Update(){
        PlayAnimation();
    }

    /// <summary>
    /// This just simulates a float and rotate animation
    /// </summary>
    void PlayAnimation(){
        //Get the current local position
        Vector3 position = visualTransform.localPosition;
        //Move the position value based on the speed, delta time and current direction of movement
        position.y += visualMoveSpeed.y * Time.deltaTime * (visualMoveUp ? 1 : -1);

        //Limit the movement to the threshold and flip the direction when it reaches it
        if(position.y >= visualMoveUpThreshold){
            position.y = visualMoveUpThreshold;
            visualMoveUp = false;
        }
        else if (position.y <= -visualMoveUpThreshold){
            position.y = -visualMoveUpThreshold;
            visualMoveUp = true;
        }

        //Assign the local position to the modified value
        visualTransform.localPosition = position;

        //Get the current local rotation
        Vector3 rotation = visualTransform.localEulerAngles;

        //Increase the rotation based on the speed and delta time
        rotation.y += visualMoveSpeed.x * Time.deltaTime;

        //Apply the local rotation to the transform
        visualTransform.localRotation = Quaternion.Euler(rotation);
    }

    public void PickUp(PlayerController a_player)
    {
        Ammo ammo = new Ammo(ammoValue);
        //Add the ammo value to the player's reserves
        a_player.AddAmmoReserve(ref ammo);

        Destroy(gameObject);
    }
}
