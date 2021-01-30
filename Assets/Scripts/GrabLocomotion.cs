using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class GrabLocomotion : MonoBehaviour
{
    public SteamVR_Input_Sources  grabSource;
    public SteamVR_Action_Boolean grabAction;
    public SteamVR_Action_Pose    poseAction;

    public GrabLocomotion otherHand;
    public LocomotionSystems locomotion;

    public Rigidbody rb;

    private Vector3 grabPos;
    private Quaternion grabRot;

    [HideInInspector]
    public bool isMovingHand, colliding, collidingWithGrabbable, grabbing = false;

    // ====================================================================================================
    // TODO:
    //   Stop hand from moving
    //   Potentially get hand to cling to wall surface
    //   Potentially get alternate hand to ghost when to far away
    // ====================================================================================================

    void FixedUpdate() {
        // bool grabStart = grabAction.GetStateDown(grabSource);
        Vector3 vel = poseAction[grabSource].velocity;

        if(grabAction.GetStateDown(grabSource)) {
            grabPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            grabRot = transform.rotation;
            handleGrab();
        }

         if(grabAction.GetStateUp(grabSource)) { 
            locomotion.altRotationOrigin = Vector3.zero;
            handleRelease(vel, true);
        }

        if(grabPos != null) {
            transform.position = grabPos;
            transform.rotation = new Quaternion(grabRot.x, transform.rotation.y, grabRot.z, grabRot.w);
        }

        if(grabbing && isMovingHand) {
            locomotion.altRotationOrigin = transform.position;
            handleMotion(vel);
        }
    }

    private bool checkIfGrabbable(Collision collision) {
        return collision.gameObject.layer == 6;
    }

    private void handleGrab() {
        grabbing = true;
        if(collidingWithGrabbable) {
            isMovingHand = true;
            otherHand.isMovingHand = false;
        }
    }

    private void handleMotion(Vector3 vel) {
        rb.velocity = Vector3.zero;
        rb.AddRelativeForce(-vel, ForceMode.Impulse);
    }

    public void handleRelease(Vector3 vel, bool throwPlayer) {
        grabbing = false;
        if(!colliding) {
            collidingWithGrabbable = false;
        }
        if(throwPlayer && isMovingHand) {
            rb.AddRelativeForce(-vel, ForceMode.Impulse);
        }
        isMovingHand = false;
    }

    public void handleChildCollisionEnter(Collision collision) {
        colliding = true;
        collidingWithGrabbable = checkIfGrabbable(collision);
    }

    public void handleChildCollisionExit(Collision collision) {
        colliding = false;
    }
}



// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Valve.VR;
// using Valve.VR.InteractionSystem;

// public class GrabLocomotion : MonoBehaviour
// {
//     public SteamVR_Input_Sources  grabSource;
//     public SteamVR_Action_Boolean grabAction;
//     public SteamVR_Action_Pose    poseAction;

//     public GrabLocomotion otherHand;

//     public Rigidbody rb;
//     private bool canGrab, colliding = false;
    
//     [HideInInspector]
//     public bool grabbing = false;


//     // ====================================================================================================
//     // TODO:
//     //   Stop hand from moving
//     //   Potentially get hand to cling to wall surface
//     //   Potentially get alternate hand to ghost when to far away
//     // ====================================================================================================

//     void FixedUpdate() {
//         if(canGrab) {
//             Vector3 vel = poseAction[grabSource].velocity;
//             if(grabAction.GetStateDown(grabSource)) {
//                 handleGrab();
//             }

//             if(grabAction.GetState(grabSource)) {
//                 handleMotion(vel);
//             }

//             if(grabAction.GetStateUp(grabSource)) { 
//                 handleRelease(vel, true);
//             }
//         }
//     }

//     private bool checkIfMap(Collision collision) {
//         return collision.gameObject.layer == 6;
//     }

//     private void handleGrab() {
//         if(otherHand.grabbing) {
//             otherHand.handleRelease(Vector3.zero, false);
//         }
//         if(!grabbing) grabbing = true;
//     }

//     private void handleMotion(Vector3 vel) {
//         rb.velocity = Vector3.zero;
//         rb.AddRelativeForce(-vel, ForceMode.Impulse);
//     }

//     public void handleRelease(Vector3 vel, bool throwPlayer) {
//         if(!colliding) canGrab = false;
//         if(grabbing) grabbing = false;
//         if(throwPlayer) {
//             rb.AddRelativeForce(-vel, ForceMode.Impulse);
//         }
//     }

//     public void handleChildCollisionEnter(Collision collision) {
//         if(!colliding) colliding = true;
//         canGrab = checkIfMap(collision);
//     }

//     public void handleChildCollisionExit(Collision collision) {
//         if(colliding) colliding = false;
//         if(!grabbing) canGrab = false;
//     }
// }
