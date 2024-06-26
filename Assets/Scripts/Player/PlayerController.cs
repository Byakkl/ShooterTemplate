using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    //Stores a reference to the rigidbody component
    Rigidbody playerRB;

    //Stores a reference to the point to spawn projectiles from
    [SerializeField]
    Transform projectileSpawn;

    //Stores a reference to the item visual
    [SerializeField]
    Transform itemTransform;

    //Stores a reference to the camera attached to the player
    [SerializeField]
    Transform cameraTransform;

    [SerializeField]
    //Stores the player's move speed, controls how fast the player moves based on input
    float moveSpeed = 5.0f;

    //Stores the camera rotation speed, controls how fast the camera rotates based on input
    [SerializeField]
    float cameraSpeed = 50.0f;

    //Stores the user inputs for the current frame
    InputStates inputs;

    //Reference to the UI text element used to display ammunition
    [SerializeField]
    TMP_Text ammoUI;

    Inventory<IItem> itemInv = new Inventory<IItem>();

    Inventory<Ammo> ammoInv = new Inventory<Ammo>();

    PlayerController(){
        //Set initial values
        itemInv.EmptyInventory(true);
        ammoInv.EmptyInventory(true);
    }

    void Start(){
        playerRB = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        //Recieve user input updates
        GetPlayerInput();

        //Move the camera
        ApplyCameraInput();

        //Apply reload inputs
        ApplyReloadInput();

        //Apply primary use inputs
        ApplyPrimaryUseInput();

        //Update the player UI
        UpdateUI();
    }

    void FixedUpdate(){
        //Move the player
        ApplyMovementInput();
    }

    /// <summary>
    /// Retrieves updates from Input system
    /// </summary>
    void GetPlayerInput(){
        //Get WASD input
        inputs.movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        //Get Mouse input
        inputs.camera = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0);

        //Get Primary Use input state
        inputs.usePrimary = Input.GetButton("Fire1");

        //Get Reload input state
        inputs.reload = Input.GetButton("Reload");
    }

    /// <summary>
    /// Applies 'inputs.movement' vector to player transform adjusted by 'moveSpeed'
    /// </summary>
    void ApplyMovementInput(){
        //Set the result vector to zero
        Vector3 frameMovement = Vector3.zero;
        //Add relative forward/backward movement
        frameMovement += transform.forward * inputs.movement.z;
        //Add relative right/left movement
        frameMovement += transform.right * inputs.movement.x;
        //Normalize the results to ensure diagonal movement isn't faster, multiply by player move speed and adjust for delta time
        frameMovement = frameMovement.normalized * moveSpeed * Time.fixedDeltaTime;

        //Apply the resulting movement to the player's rigidbody velocity
        playerRB.velocity = frameMovement;
    }

    /// <summary>
    /// Applies 'inputs.camera' vector to camera transform adjusted by 'cameraSpeed'
    /// </summary>
    void ApplyCameraInput(){
        //Determine the Y rotation (horizontal) delta
        float yRotation = inputs.camera.x * cameraSpeed * Time.deltaTime;

        //Get the current rotation of the root player object in euler angles
        Vector3 euler = transform.eulerAngles;
        //Rotate the player object left/right. Restrict X and Z rotation to 0
        transform.rotation = Quaternion.Euler(0, euler.y + yRotation, 0);

        //Determine the X rotation (vertical) delta
        float xRotation = -inputs.camera.y * cameraSpeed * Time.deltaTime;

        //Get the current local rotation of the camera object in euler angles
        Vector3 cameraEuler = cameraTransform.localRotation.eulerAngles;
        //Adjust the X rotation by the delta
        cameraEuler.x += xRotation;
        //Adjust the rotation to be within -180 to 180 range
        cameraEuler.x = cameraEuler.x <= 180 ? cameraEuler.x : -(360 - cameraEuler.x);
        //Clamp the rotation to limit vertical camera rotation
        cameraEuler.x = Mathf.Clamp(cameraEuler.x, -60, 45);
        //Apply the local rotation to the camera. Restrict Y and Z rotation to 0
        cameraTransform.localRotation = Quaternion.Euler(cameraEuler.x, 0, 0);

        //Update the item visual to rotate vertically with the camera
        Vector3 itemEuler = itemTransform.localRotation.eulerAngles;
        //This is hardcoded because the visual cylinder is 90 degrees rotated
        itemEuler.x = cameraEuler.x + 90;
        //Apply the local transform to the item. Restrict Y and Z rotation to 0
        itemTransform.localRotation = Quaternion.Euler(itemEuler.x, 0, 0);
    }

    /// <summary>
    /// Processes reload input
    /// </summary>
    void ApplyReloadInput(){
        //If we aren't reloading don't process it
        if(!inputs.reload)
            return;

        itemInv.currentItem.Reload();

        //Override any firing input
        inputs.usePrimary = false;
    }

    /// <summary>
    /// Process primary use inputs
    /// </summary>
    void ApplyPrimaryUseInput(){
        if(!inputs.usePrimary)
            return;

        //Activate the primary usage of the current item
        itemInv.currentItem.UsePrimary();
    }

    void UpdateUI(){
        //Attempt to cast the current item as a type of Gun
        Gun currentGun = itemInv.currentItem as Gun;
        
        //If the cast is successful get the current and max ammo values; otherwise default them to 0
        int currentAmmo = currentGun != null ? currentGun.GetCurrentAmmo() : 0;
        int maxAmmo = currentGun != null ? currentGun.GetMaxAmmo() : 0;

        //Set the UI visual based on if the current item is a gun, its stats and the player ammo reserves
        ammoUI.text = $"Ammo: {currentAmmo}/{maxAmmo} ({ammoInv.currentItem?.quantity})";
    }

    /// <summary>
    /// Sets the player's current item
    /// </summary>
    /// <param name="item">The item to give to the player</param>
    public void SetCurrentItem(IItem a_item){
        itemInv.currentItem = a_item;

        //Determine if the item is a type of gun
        Gun gunItem = itemInv.currentItem as Gun;
        if(gunItem == null)
            return;

        //Set the firing point of the gun
        gunItem.SetFireOrigin(projectileSpawn);
    }

    /// <summary>
    /// Adds ammo to the player's reserves
    /// </summary>
    /// <param name="ammo">The amount of ammo to add</param>
    public void AddAmmoReserve(ref Ammo a_type){
        ammoInv.AddItem(ref a_type);

        if(ammoInv.currentItem == null || ammoInv.currentItem == default)
            ammoInv.ChangeItem(0);
    }

    private void OnTriggerEnter(Collider a_collider){
        //Attempt to fetch a component that implements IPickUp from the object that was collided with
        IPickUp pickup = a_collider.gameObject.GetComponent<IPickUp>();
        //The collision was not with a pickup
        if(pickup == null)
            return;

        pickup.PickUp(this);
    }
}
