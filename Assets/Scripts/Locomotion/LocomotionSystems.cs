using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class LocomotionSystems : MonoBehaviour
{
    public bool thrusterLocomotion, continousRotation;

    public SteamVR_Action_Boolean thrustAction, breakAction, boostAction = null;
    public GameObject leftHand, rightHand, VRCamera, playerObject = null;

    public SteamVR_Action_Boolean smoothLeftAction, smoothRightAction, pitchUpAction, pitchDownAction = null;

    // Continous Rotation Variables
    public bool pitchEnabled = true;
    public float rotationSpeed  = 5.0f;

    // thrusterLocomotion variables
    public float thrustSpeed = 1.0f;
    public float boostSpeed = 5.0f;

    private Transform thrustVectorLeft, thrustVectorRight;
    private Rigidbody rb;
    private SphereCollider headCollider;

    float resetRotationTimer = 0.0f;

    void Start() {
        if(thrusterLocomotion) thrusterStart();
    }

    void Update() {
        followHead();
        handleRotationReset();

        if(continousRotation) {
            handleRotationInput();
        }

        //  Thruster Functions
        if(thrusterLocomotion) {
            handleThrust();
            handleBoost();
            handleBreak();
        }
    }
    
    void followHead() {
        Vector3 capsuleCenter = transform.InverseTransformPoint(VRCamera.transform.position);

        headCollider.center =  new Vector3(capsuleCenter.x, capsuleCenter.y, capsuleCenter.z);
    }

    // Rotation Functions
    private void handleRotationInput() {
        bool turnLeft = smoothLeftAction.GetState(SteamVR_Input_Sources.Any);
        bool turnRight = smoothRightAction.GetState(SteamVR_Input_Sources.Any);
        bool pitchUp = pitchUpAction.GetState(SteamVR_Input_Sources.Any);
        bool pitchDown = pitchDownAction.GetState(SteamVR_Input_Sources.Any);

        if (turnLeft) {
            DoRotatePlayer(-rotationSpeed);
        }

        if (turnRight) {
            DoRotatePlayer(rotationSpeed);
        }

        if(pitchEnabled) {
            if (pitchUp) DoPitchPlayer(rotationSpeed);
            if (pitchDown) DoPitchPlayer(-rotationSpeed);
        }
    }

    private void DoRotatePlayer(float angle)
    {
        Player player = Player.instance;
        angle = angle * Time.deltaTime;
        Vector3 playerFeetOffset = player.trackingOriginTransform.position - player.feetPositionGuess;
        player.trackingOriginTransform.position -= playerFeetOffset;
        player.transform.Rotate(Vector3.up, angle);
        playerFeetOffset = Quaternion.Euler(0.0f, angle, 0.0f) * playerFeetOffset;
        player.trackingOriginTransform.position += playerFeetOffset;

    }

    private void DoPitchPlayer(float angle) {
        angle = angle * Time.deltaTime;
        Player player = Player.instance;
        player.transform.Rotate(Vector3.right, angle);
    }

    // Thruster Functions
    void thrusterStart() {
        thrustVectorLeft = leftHand.transform.Find("ThrustVector");
        thrustVectorRight = rightHand.transform.Find("ThrustVector");
        headCollider = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();
    }

    void handleRotationReset() {
        if(breakAction.GetState(SteamVR_Input_Sources.Any) && boostAction.GetState(SteamVR_Input_Sources.Any)) {
            resetRotationTimer += Time.deltaTime;
        }

        if(resetRotationTimer % 60 >= 3) {
            playerObject.transform.rotation = new Quaternion(0, playerObject.transform.rotation.y, 0, 0);
        }

        if(breakAction.GetStateUp(SteamVR_Input_Sources.Any) || boostAction.GetStateUp(SteamVR_Input_Sources.Any)) {
            resetRotationTimer = 0.0f;
        }
    }

    void handleThrust() {
        thrustDirection(thrustVectorLeft, thrustVectorLeft.transform.right, SteamVR_Input_Sources.LeftHand);
        thrustDirection(thrustVectorRight, thrustVectorRight.transform.right, SteamVR_Input_Sources.RightHand);
    }

    void handleBreak() {
        if(breakAction.GetStateDown(SteamVR_Input_Sources.Any)) {
            rb.velocity = rb.velocity / 3;
        }
    }

    void handleBoost() {
        if(boostAction.GetStateDown(SteamVR_Input_Sources.Any)) {
            rb.AddForce(VRCamera.transform.forward * boostSpeed, ForceMode.Impulse);
        }
    }

    void thrustDirection(Transform hand, Vector3 direction, SteamVR_Input_Sources inputSource) {
        if(thrustAction.GetLastStateDown(inputSource)) {
            hand.GetComponent<LineRenderer>().enabled = true;
        }
        if(thrustAction.GetLastStateUp(inputSource)) {
            hand.GetComponent<LineRenderer>().enabled = false;
        }

        if(thrustAction.GetState(inputSource)) {
            rb.AddForce(direction * thrustSpeed * Time.deltaTime, ForceMode.Impulse);
        }
    }
}
