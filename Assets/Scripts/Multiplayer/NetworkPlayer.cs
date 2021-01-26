using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;
using Valve.VR.InteractionSystem;


public class NetworkPlayer : MonoBehaviour
{
    public Transform head, leftHand, rightHand;

    // public GameObject player;
    private PhotonView photonView;


    private Transform headRig, leftHandRig, rightHandRig;


    void Start() {
        photonView = GetComponent<PhotonView>();
        Player player = FindObjectOfType<Player>();
        headRig = player.transform.Find("SteamVRObjects/VRCamera");
        leftHandRig = player.transform.Find("SteamVRObjects/LeftHand");
        rightHandRig = player.transform.Find("SteamVRObjects/RightHand");
    }

    void Update() {
        if(photonView.IsMine) {
            rightHand.gameObject.SetActive(false);
            leftHand.gameObject.SetActive(false);
            head.gameObject.SetActive(false);


            mapPosition(head, headRig);
            mapPosition(leftHand, leftHandRig);
            mapPosition(rightHand, rightHandRig);
        }
    }

    void mapPosition(Transform target, Transform rigTransform) {
        target.position = rigTransform.position;
        target.rotation = rigTransform.rotation;
    }
}
