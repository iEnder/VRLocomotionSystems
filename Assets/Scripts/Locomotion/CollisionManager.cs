using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class CollisionManager : MonoBehaviour
{
    public SteamVR_Input_Sources handSource;
    public HandCollider handCollider;
    public GrabLocomotion grabScript;
    
    void OnCollisionEnter(Collision collision) {
        grabScript.handleChildCollisionEnter(collision);
        handCollider.hand.hand.TriggerHapticPulse(0.01f, 100, 0.5f);
    }

    void OnCollisionExit(Collision collision) {
        grabScript.handleChildCollisionExit(collision);
    }
}
