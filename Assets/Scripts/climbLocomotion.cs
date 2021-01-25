using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class climbLocomotion : MonoBehaviour
{
    public SteamVR_Action_Boolean GrabAction = null;
    public SteamVR_Action_Pose PoseAction = null;

    // public GameObject leftHand, rightHand = null;

    // private HandPhysics leftHandPhysics, rightHandPhysics;

    private Rigidbody rb;

    private SteamVR_Input_Sources currentLocomotionHand;

    private bool canGrab = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // leftHandPhysics = leftHand.GetComponent<HandPhysics>();
        // rightHandPhysics = rightHand.GetComponent<HandPhysics>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // ====================================================================================================
        // TODO:
        //   Check for wall collision (Both to approve grab and to make sure I cant swing through a wall)
        //   Stop hand from moving
        //   Potentially get hand to cling to wall surface
        //   Potentially get alternate hand to ghost when to far away
        //   Center Colliders on VRCAMERA
        // ====================================================================================================

        // Collision leftHandColliding = leftHandPhysics.handCollider.colliding;
        // Collision rightHandColliding = rightHandPhysics.handCollider.colliding;

        // if(GrabAction.GetStateDown(SteamVR_Input_Sources.RightHand) && rightHandColliding != null) {
        //     if(currentLocomotionHand != SteamVR_Input_Sources.RightHand && rightHandColliding.gameObject.layer == 6) {
        //         currentLocomotionHand = SteamVR_Input_Sources.RightHand;
        //     }
        // }

        // if(GrabAction.GetStateDown(SteamVR_Input_Sources.LeftHand) && leftHandColliding != null) {
        //     if(currentLocomotionHand != SteamVR_Input_Sources.LeftHand && leftHandColliding.gameObject.layer == 6) {
        //         currentLocomotionHand = SteamVR_Input_Sources.LeftHand;
        //     }
        // }

        if(GrabAction.GetStateDown(SteamVR_Input_Sources.RightHand) && currentLocomotionHand != SteamVR_Input_Sources.RightHand) {
            currentLocomotionHand = SteamVR_Input_Sources.RightHand;
        }

        if(GrabAction.GetStateDown(SteamVR_Input_Sources.LeftHand) && currentLocomotionHand != SteamVR_Input_Sources.LeftHand) {
            currentLocomotionHand = SteamVR_Input_Sources.LeftHand;
        }

       Vector3 vel = PoseAction[currentLocomotionHand].velocity;

       if(GrabAction.GetState(currentLocomotionHand)) {
           rb.velocity = Vector3.zero;
           transform.Translate(-vel * Time.fixedDeltaTime);
       }

       if(GrabAction.GetStateUp(currentLocomotionHand)) {
           rb.AddRelativeForce(-vel, ForceMode.Impulse);
       }
    }
}
