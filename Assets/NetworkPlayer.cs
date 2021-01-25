using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;

public class NetworkPlayer : MonoBehaviour
{
    public Transform head, leftHand, rightHand;

    private PhotonView photonView;

    void Start() {
        photonView = GetComponent<PhotonView>();
    }

    void Update() {
        if(photonView.IsMine) {
            rightHand.gameObject.SetActive(false);
            leftHand.gameObject.SetActive(false);
            head.gameObject.SetActive(false);
            mapPosition(head, XRNode.Head);
            mapPosition(leftHand, XRNode.LeftHand);
            mapPosition(rightHand, XRNode.RightHand);
        }
    }

    void mapPosition(Transform target, XRNode node) {
        InputDevices.GetDeviceAtXRNode(node).TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 pos);
        InputDevices.GetDeviceAtXRNode(node).TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rot);

        target.position = pos;
        target.rotation = rot;
    }
}
