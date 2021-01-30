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

    public SteamVR_Action_Vector2 smoothRotationAction;

    // Continous Rotation Variables
    public bool pitchEnabled = true;
    public float rotationSpeed  = 5.0f;
    public float deadzoneRadius = 0.5f;
    public Vector3 altRotationOrigin = Vector3.zero;

    // thrusterLocomotion variables
    public float thrustSpeed = 1.0f;
    public float boostSpeed = 5.0f;

    private Transform thrustVectorLeft, thrustVectorRight;
    private Rigidbody rb;
    private SphereCollider headCollider;

    float resetRotationTimer = 0.0f;

    void Start() {
        headCollider = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();
        thrustVectorLeft = leftHand.transform.Find("ThrustVector");
        thrustVectorRight = rightHand.transform.Find("ThrustVector");
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
        bool turnRight = smoothRotationAction.GetAxis(SteamVR_Input_Sources.Any).x > deadzoneRadius;
        bool turnLeft =  smoothRotationAction.GetAxis(SteamVR_Input_Sources.Any).x < (deadzoneRadius * -1);
        bool pitchUp =   smoothRotationAction.GetAxis(SteamVR_Input_Sources.Any).y > deadzoneRadius;
        bool pitchDown = smoothRotationAction.GetAxis(SteamVR_Input_Sources.Any).y < (deadzoneRadius * -1);

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

        // if the alternate rotation origin is being set use that instead;
        Vector3 rotationOrigin = altRotationOrigin == Vector3.zero ? player.feetPositionGuess : altRotationOrigin;

        angle = angle * Time.deltaTime;
        Vector3 playerFeetOffset = player.trackingOriginTransform.position - rotationOrigin;
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

    public void switchLocomotion(bool state, Vector3 speed) {
        thrusterLocomotion = state;
        rb.velocity = Vector3.zero;
        rb.AddForce(speed, ForceMode.Impulse);
        
        if(!state) {
            thrustVectorLeft.GetComponent<LineRenderer>().enabled = false;
            thrustVectorRight.GetComponent<LineRenderer>().enabled = false;
        }

    }

    void handleThrust() {
        thrustDirection(thrustVectorLeft, thrustVectorLeft.transform.right, SteamVR_Input_Sources.LeftHand);
        thrustDirection(thrustVectorRight, thrustVectorRight.transform.right, SteamVR_Input_Sources.RightHand);
    }

    void handleBreak() {
        if(breakAction.GetStateDown(SteamVR_Input_Sources.Any)) {
            rb.velocity = rb.velocity / 10;
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

        if(thrustAction.GetState(inputSource) && thrusterLocomotion) {
            rb.AddForce(direction * thrustSpeed * Time.deltaTime, ForceMode.Impulse);
        }
    }
}
