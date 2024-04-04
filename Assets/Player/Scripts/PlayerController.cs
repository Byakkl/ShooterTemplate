using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Stores a reference to the camera attached to the player
    [SerializeField]
    Transform cameraTransform;

    //Stores the input axis as a vector for moving the player each frame
    Vector3 input_movement;

    [SerializeField]
    //Stores the player's move speed, controls how fast the player moves based on input
    float moveSpeed = 5.0f;

    //Stores the input delta of the mouse as a vector for moving the camera each frame
    Vector3 input_camera;

    //Stores the camera rotation speed, controls how fast the camera rotates based on input
    [SerializeField]
    float cameraSpeed = 50.0f;

    void Update()
    {
        //Recieve user input updates
        GetPlayerInput();

        ApplyMovementInput();

        ApplyCameraInput();
    }

    /// <summary>
    /// Retrieves updates from Input system
    /// </summary>
    void GetPlayerInput(){
        //Get WASD input
        input_movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        //Get Mouse input
        input_camera = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0);
    }

    /// <summary>
    /// Applies 'input_movement' vector to player transform adjusted by 'moveSpeed'
    /// </summary>
    void ApplyMovementInput(){
        //Set the result vector to zero
        Vector3 frameMovement = Vector3.zero;
        //Add relative forward/backward movement
        frameMovement += transform.forward * input_movement.z;
        //Add relative right/left movement
        frameMovement += transform.right * input_movement.x;
        //Normalize the results to ensure diagonal movement isn't faster, multiply by player move speed and adjust for delta time
        frameMovement = frameMovement.normalized * moveSpeed * Time.deltaTime;

        //Apply the resulting movement to the transform's position
        transform.position += frameMovement;
    }

    /// <summary>
    /// Applies 'input_camera' vector to camera transform adjusted by 'cameraSpeed'
    /// </summary>
    void ApplyCameraInput(){
        //Determine the Y rotation (horizontal) delta
        float yRotation = input_camera.x * cameraSpeed * Time.deltaTime;

        //Get the current rotation of the root player object in euler angles
        Vector3 euler = transform.eulerAngles;
        //Rotate the player object left/right. Restrict X and Z rotation to 0
        transform.rotation = Quaternion.Euler(0, euler.y + yRotation, 0);

        //Determine the X rotation (vertical) delta
        float xRotation = -input_camera.y * cameraSpeed * Time.deltaTime;

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
}
