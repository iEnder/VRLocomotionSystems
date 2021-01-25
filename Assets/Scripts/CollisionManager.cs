using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class CollisionManager : MonoBehaviour
{
    private climbLocomotion climbScript;
    public SteamVR_Input_Sources handSource;

    public HandCollider handCollider;

    void Start() {
        climbScript = transform.parent.GetComponent<climbLocomotion>();
    }

    void OnCollisionEnter(Collision collision) {
        climbScript.handleChildCollisionEnter(collision, handSource);
        handCollider.hand.hand.TriggerHapticPulse(0.01f, 100, 0.5f);
    }

    void OnCollisionExit(Collision collision) {
        climbScript.handleChildCollisionExit(collision, handSource);
    }
}
