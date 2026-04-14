using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof (CharacterController))]
public class NetworkFPSPlayer : NetworkBehaviour
{
    [Header("Player Components")]
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private Camera playerCamera;

    [Header("Player Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float lookSensitivity = 2f;
    [SerializeField] private float maxPitch = 80f;
    [SerializeField] private float jumpHeight = 5f;

    private PlayerInput pi;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private CharacterController cc;

    private float pitch;

    public override void OnNetworkSpawn()
    {
        cc = GetComponent<CharacterController>();
        pi = GetComponent<PlayerInput>();

        if (!IsOwner)
        {
            if(playerCamera) playerCamera.enabled = false;
            if(pi) pi.enabled = false;
            enabled = false;
            return;
        }

        moveAction = pi.actions["Move"];
        lookAction = pi.actions["Look"];
        jumpAction = pi.actions["Jump"];
        moveAction.Enable();
        lookAction.Enable();
        jumpAction.Enable();

        if (playerCamera) playerCamera.enabled = true;
    }

    private void Update()
    {
        Vector2 m = moveAction.ReadValue<Vector2>();
        Vector3 move = transform.right * m.x + transform.forward * m.y;
        cc.Move(move * moveSpeed * Time.deltaTime);

        Vector2 look = lookAction.ReadValue<Vector2>();
        transform.Rotate(0f, look.x, 0f);

        pitch -= look.y;
        pitch = Mathf.Clamp(pitch, -maxPitch, maxPitch);
        cameraPivot.localEulerAngles = new Vector3(pitch, 0f, 0f);

        Vector2 j = jumpAction.ReadValue<Vector2>();
        j = transform.position * jumpHeight * j;
    }
}
