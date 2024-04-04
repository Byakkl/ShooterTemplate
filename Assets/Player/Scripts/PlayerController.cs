using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
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

    //Reference to the current item the player is holding
    IItem currentItem;

    //Stores the ammount of reserve ammo the player has
    int ammoReserves;

    PlayerController(){
        //Set initial values
        currentItem = null;
        ammoReserves = 0;
    }

    void Update()
    {
        //Recieve user input updates
        GetPlayerInput();

        //Move the player
        ApplyMovementInput();

        //Move the camera
        ApplyCameraInput();

        //Apply reload inputs
        ApplyReloadInput();

        //Apply primary use inputs
        ApplyPrimaryUseInput();

        UpdateUI();
    }

    /// <summary>
    /// Retrieves updates from Input system
    /// </summary>
    void GetPlayerInput(){
        //Get WASD input
        inputs.movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

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
        frameMovement = frameMovement.normalized * moveSpeed * Time.deltaTime;

        //Apply the resulting movement to the transform's position
        transform.position += frameMovement;
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
    }

    /// <summary>
    /// Processes reload input
    /// </summary>
    void ApplyReloadInput(){
        //If we aren't reloading don't process it
        if(!inputs.reload)
            return;

        //Attempt to cast the current item as a gun
        Gun currentGun = currentItem as Gun;
            
        //If it isn't a gun then exit
        if(currentGun == null)
            return;

        ammoReserves += currentGun.Reload(ammoReserves);

        //Override any firing input
        inputs.usePrimary = false;
    }

    /// <summary>
    /// Process primary use inputs
    /// </summary>
    void ApplyPrimaryUseInput(){
        if(!inputs.usePrimary)
            return;

        currentItem.UsePrimary();
    }

    void UpdateUI(){
        Gun currentGun = currentItem as Gun;
        int currentAmmo = currentGun != null ? currentGun.GetCurrentAmmo() : 0;
        int maxAmmo = currentGun != null ? currentGun.GetMaxAmmo() : 0;
        ammoUI.text = $"Ammo: {currentAmmo}/{maxAmmo} ({ammoReserves})";
    }

    /// <summary>
    /// Sets the player's current item
    /// </summary>
    /// <param name="item">The item to give to the player</param>
    public void SetCurrentItem(IItem item){
        currentItem = item;
    }

    /// <summary>
    /// Adds ammo to the player's reserves
    /// </summary>
    /// <param name="ammo">The amount of ammo to add</param>
    public void AddAmmoReserve(int ammo){
        ammoReserves += ammo;
    }
}
