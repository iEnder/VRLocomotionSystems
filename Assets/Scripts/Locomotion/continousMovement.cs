using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class continousMovement : MonoBehaviour
{
    public float speed = 1;
    public float gravity = -9.81f;
    public float jumpHeight;
    private float fallingSpeed;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    public float extraHeight = 0.2f;
    public LayerMask groundLayer;

    public SteamVR_Action_Vector2 moveAction;
    public SteamVR_Action_Boolean jumpAction;
    public GameObject VRCamera;

    [SerializeField]
    private GameObject[] forwardProviders;
    public enum directionDevice{Head, LeftHand, RightHand};
    public directionDevice directionProvider;

    private GameObject getProvider (directionDevice name) {
        return forwardProviders[(int)name];
    }

    private CharacterController character;

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();
    }

    void FixedUpdate()
    {
        groundedPlayer = CheckIfGrounded() || character.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0f)
        {
            playerVelocity.y = 0f;
        }
        continousMove();
        // useGravity();
        handleJump();

    }

    void handleJump() {
        if (jumpAction.GetStateDown(SteamVR_Input_Sources.Any) && groundedPlayer) {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -1.0f * gravity);
        }
    }

    void useGravity() {
        playerVelocity.y += gravity * Time.fixedDeltaTime;
        character.Move(playerVelocity * Time.fixedDeltaTime);
    }

    void continousMove() {
        CapsuleFollowHeadset();

        Quaternion forwardYaw = Quaternion.Euler(0, getProvider(directionProvider).transform.eulerAngles.y, 0);
        Vector3 direction = forwardYaw * new Vector3(moveAction.axis.x, 0, moveAction.axis.y);

        character.Move(direction * Time.fixedDeltaTime * speed);

    }

    void CapsuleFollowHeadset() {
        Player player = Player.instance;
        character.height = player.eyeHeight + extraHeight;

        Vector3 capsuleCenter = transform.InverseTransformPoint(VRCamera.transform.position);

        character.center = new Vector3(capsuleCenter.x, character.height / 2 + character.skinWidth, capsuleCenter.z);
    }

    bool CheckIfGrounded() {
        Vector3 rayStart = transform.TransformPoint(character.center);
        float rayLength = character.center.y + 0.01f;

        bool hasHit = Physics.Raycast(rayStart, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer);
        return hasHit;
    }
}
