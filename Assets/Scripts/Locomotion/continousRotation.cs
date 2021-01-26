using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class continousRotation : MonoBehaviour
{
    public float rotationSpeed  = 5.0f;

    public SteamVR_Action_Boolean smoothLeftAction = SteamVR_Input.GetBooleanAction("SmoothLeft");
    public SteamVR_Action_Boolean smoothRightAction = SteamVR_Input.GetBooleanAction("SmoothRight");
    public SteamVR_Action_Boolean pitchUpAction = SteamVR_Input.GetBooleanAction("Pitch_Up");
    public SteamVR_Action_Boolean pitchDownAction = SteamVR_Input.GetBooleanAction("Pitch_Down");

    public bool pitchEnabled = true;

    private void Update()
    {
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

    private void DoPitchPlayer(float angle) {
        angle = angle * Time.deltaTime;
        Player player = Player.instance;
        player.transform.Rotate(Vector3.right, angle);
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
}
