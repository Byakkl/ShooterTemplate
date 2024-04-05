using UnityEngine;

public struct InputStates
{
    //Stores input axis as a vector for moving the player
    public Vector3 movement;

    //Stores the input delta for moving the camera
    public Vector3 camera;

    //Stores the input state of the 'Fire1' input
    public bool usePrimary;

    //Stores the input state of the 'Reload' input
    public bool reload;
}
