using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Target : MonoBehaviour, IDamageable
{
    //The mesh renderer that is updated when hit
    MeshRenderer meshRenderer;

    //Material used as default or reset to after a time when hit
    [SerializeField]
    Material defaultMat;

    //Material to change to when hit with a projectile
    [SerializeField]
    Material hitMat;

    //How many seconds until the target material resets
    [SerializeField]
    float resetTime = 2.0f;

    //Current time until the hit material is reset
    float resetTimer;

    //Current hit state
    bool isHit;

    void Start(){
        //Initialize values
        isHit = false;
        resetTimer = 0;

        meshRenderer = gameObject.GetComponent<MeshRenderer>();
    }

    void Update(){
        //If the target is in its default state there is nothing to update
        if(!isHit)
            return;

        //Decrease the timer by delta time
        resetTimer -= Time.deltaTime;

        //Once the timer expires reset the visual material and state
        if(resetTimer <= 0){
            meshRenderer.material = defaultMat;
            isHit = false;
        }
    }

    public void OnHit(){
        //Update the material for visual feedback
        meshRenderer.material = hitMat;

        //Set the reset time
        resetTimer = resetTime;

        //Flag the target as hit
        isHit = true;
    }
}
