using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class climbLocomotion : MonoBehaviour
{
    public SteamVR_Action_Boolean GrabAction = null;
    public SteamVR_Action_Pose PoseAction = null;
    private Rigidbody rb;
    private SteamVR_Input_Sources currentLocomotionHand;
    private bool lHandCanGrab, rHandCanGrab = false;

    private bool lGrabbing = false;
    private bool rGrabbing = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // ====================================================================================================
        // TODO:
        //   Stop hand from moving
        //   Potentially get hand to cling to wall surface
        //   Potentially get alternate hand to ghost when to far away
        // ====================================================================================================

        if (rHandCanGrab) {
            if(GrabAction.GetStateDown(SteamVR_Input_Sources.RightHand) && currentLocomotionHand != SteamVR_Input_Sources.RightHand) {
                currentLocomotionHand = SteamVR_Input_Sources.RightHand;
            }
        }

        if (lHandCanGrab) {
            if(GrabAction.GetStateDown(SteamVR_Input_Sources.LeftHand) && currentLocomotionHand != SteamVR_Input_Sources.LeftHand) {
                currentLocomotionHand = SteamVR_Input_Sources.LeftHand;
            }
        }

        if (lHandCanGrab || rHandCanGrab) {
            Vector3 vel = PoseAction[currentLocomotionHand].velocity;

            if(GrabAction.GetState(currentLocomotionHand)) {
                setGrabbing(currentLocomotionHand, true);
                rb.velocity = Vector3.zero;
                transform.Translate(-vel * Time.fixedDeltaTime);
            }

            if(GrabAction.GetStateUp(currentLocomotionHand)) {
                setGrabbing(currentLocomotionHand, false);
                rb.AddRelativeForce(-vel, ForceMode.Impulse);
            }

            // if(GrabAction.GetStateUp(SteamVR_Input_Sources.Any)) {
            //     setGrabbing(SteamVR_Input_Sources.LeftHand, false);
            //     setGrabbing(SteamVR_Input_Sources.RightHand, false);
            //     rb.AddRelativeForce(-vel, ForceMode.Impulse);
            // }
        }
    }

    private bool checkIfMap(Collision collision) {
        return collision.gameObject.layer == 6;
    }

    private void setGrabbing(SteamVR_Input_Sources hand, bool state) {
        if (hand == SteamVR_Input_Sources.LeftHand) lGrabbing = state;
        if (hand == SteamVR_Input_Sources.RightHand) rGrabbing = state;
    }

    public void handleChildCollisionEnter(Collision collision, SteamVR_Input_Sources hand) {
        if (hand == SteamVR_Input_Sources.LeftHand) {
            lHandCanGrab = checkIfMap(collision);
        }
        if (hand == SteamVR_Input_Sources.RightHand) {
            rHandCanGrab = checkIfMap(collision);
        }
    }

    public void handleChildCollisionExit(Collision collision, SteamVR_Input_Sources hand) {
        if(hand == SteamVR_Input_Sources.RightHand) {
            if(rHandCanGrab && !rGrabbing) rHandCanGrab = false;
        }

        if(hand == SteamVR_Input_Sources.LeftHand) {
            if(lHandCanGrab && !lGrabbing) lHandCanGrab = false;
        }
    }
}
