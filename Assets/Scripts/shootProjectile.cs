using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;


public class shootProjectile : MonoBehaviour
{
    public GameObject projectile;

    public GameObject ctrlRight, ctrlLeft;
    public SteamVR_Action_Boolean thrust;

    public float projectileSpeeed = 1.0f;

    public float XOff = 0;
    public float YOff = 0;
    public float ZOff = 0;

    public float XPOff = 0;
    public float YPOff = 0;
    public float ZPOff = 0;

    private GameObject localProjectileLeft;
    private GameObject localProjectileRight;

    private Vector3 playerVelocity = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        rightHand();
        leftHand();

        // if(localProjectileRight) {
        //     MoveTowardsTarget (playerVelocity);
        // }
    }

    void rightHand() {
        Transform form = ctrlRight.transform;
        Vector3 pos = getLocalForwardPos(form);

        if(thrust.GetStateDown(SteamVR_Input_Sources.RightHand)) {
            localProjectileRight = Instantiate(projectile, pos, form.rotation *= Quaternion.Euler(XOff, YOff, ZOff)) as GameObject;
            StartCoroutine(PhysicsLate(localProjectileRight.GetComponent<Rigidbody>(), new Vector3(0,0,1) * projectileSpeeed));
        }

        // if(thrust.GetStateUp(SteamVR_Input_Sources.RightHand)) {
        //     Destroy(localProjectileRight);
        // }

        if(localProjectileRight) {
            // localProjectileRight.transform.position = pos + new Vector3(XPOff, YPOff, ZPOff);
            // localProjectileRight.transform.rotation = form.rotation *= Quaternion.Euler(XOff, YOff, ZOff);
        }
    }
    void leftHand() {
        Transform form = ctrlLeft.transform;
        Vector3 pos = getLocalForwardPos(form);

        if(thrust.GetStateDown(SteamVR_Input_Sources.LeftHand)) {
            localProjectileLeft = Instantiate(projectile, pos, form.rotation *= Quaternion.Euler(XOff, YOff, ZOff)) as GameObject;
            StartCoroutine(PhysicsLate(localProjectileLeft.GetComponent<Rigidbody>(), new Vector3(0,0,1) * projectileSpeeed));
        }

        // if(thrust.GetStateUp(SteamVR_Input_Sources.LeftHand)) {
        //     Destroy(localProjectileLeft);
        // }

        if(localProjectileLeft) {
            // localProjectileLeft.transform.position = pos;
            // localProjectileLeft.transform.rotation = form.rotation *= Quaternion.Euler(XOff, YOff, ZOff);
        }
    }

    IEnumerator PhysicsLate (Rigidbody rb, Vector3 force){
        yield return new WaitForSeconds(0.01f);
        rb.AddRelativeForce(force, ForceMode.Impulse);
    }

    Quaternion getLocalForwardRot(Transform form) {
        return new Quaternion();
    }

    Vector3 getLocalForwardPos(Transform form) {
        Vector3 pos = form.position;
        pos += form.forward;
        pos -= form.up * 1.20f;

        return pos;
    }
    
    void MoveTowardsTarget(Vector3 target) {
        var cc = GetComponent<CharacterController>();
        var offset = target - transform.position;
        //Get the difference.
        if(offset.magnitude > .1f) {
        //If we're further away than .1 unit, move towards the target.
        //The minimum allowable tolerance varies with the speed of the object and the framerate. 
        // 2 * tolerance must be >= moveSpeed / framerate or the object will jump right over the stop.
            offset = offset.normalized * 5;
            //normalize it and account for movement speed.
            cc.Move(offset * Time.deltaTime);
            //actually move the character.
        }
    }

}
