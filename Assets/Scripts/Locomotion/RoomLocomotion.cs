using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLocomotion : MonoBehaviour
{
    [HideInInspector]
    public enum LocomotionType { Walking, Flying }

    public LocomotionType setOnEnter = new LocomotionType();
    public LocomotionType setOnLeave = new LocomotionType();

    private bool roomActive = false;
    private CharacterController character;
    private Rigidbody rb;

    void Start() {
        character = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
    }

    public void setLocomotion(GameObject player, LocomotionType locomotion) {
        LocomotionSystems playerLocomotion = player.GetComponent<LocomotionSystems>();

        if(locomotion == LocomotionType.Flying) {
            player.GetComponent<continousMovement>().enabled = false;
            player.GetComponent<CharacterController>().enabled = false;
            // playerLocomotion.setThrusters(true, rb.velocity);
        } else if(locomotion == LocomotionType.Walking) {
            player.GetComponent<continousMovement>().enabled = true;
            player.GetComponent<CharacterController>().enabled = true;
            // playerLocomotion.setThrusters(false, rb.velocity);
        }
    }

    void OnTriggerEnter(Collider collider) {
        Debug.Log("Collided");
        if(!roomActive) {
            setLocomotion(gameObject, setOnEnter);
        }
        roomActive = true;
    }

    void OnTriggerExit(Collider collider) {
        if(roomActive) {
            setLocomotion(gameObject, setOnLeave);
        }
        roomActive = false;
    }
}
