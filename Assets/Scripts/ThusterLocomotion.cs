using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class ThusterLocomotion : MonoBehaviour
{

    public SteamVR_Action_Boolean thrustAction, breakAction, boostAction = null;

    public GameObject leftHand, rightHand, VRCamera, Player = null;

    public float thrustSpeed = 1.0f;
    public float boostSpeed = 5.0f;

    private Transform thrustVectorLeft, thrustVectorRight;
    private Rigidbody rb;

    private SphereCollider headCollider;

    float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        thrustVectorLeft = leftHand.transform.Find("ThrustVector");
        thrustVectorRight = rightHand.transform.Find("ThrustVector");
        headCollider = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(breakAction.GetState(SteamVR_Input_Sources.Any) && boostAction.GetState(SteamVR_Input_Sources.Any)) {
            timer += Time.deltaTime;
        }

        if(timer % 60 >= 3) {
            Player.transform.rotation = new Quaternion(0, Player.transform.rotation.y, 0, 0);
        }

        if(breakAction.GetStateUp(SteamVR_Input_Sources.Any) || boostAction.GetStateUp(SteamVR_Input_Sources.Any)) {
            timer = 0.0f;
        }
        
        followHead();
        handleThrust();
        handleBoost();
        handleBreak();
    }

    void followHead() {
        Vector3 capsuleCenter = transform.InverseTransformPoint(VRCamera.transform.position);

        headCollider.center =  new Vector3(capsuleCenter.x, capsuleCenter.y, capsuleCenter.z);
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
