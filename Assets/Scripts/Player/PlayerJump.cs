using System;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    private PlayerDetection detection;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 4f;
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private float bufferJumpTime = 0.2f;

    public bool jumpHeld { get; private set; }

    public event Action TryToJump;

    private float coyoteTimeCounter;
    private float bufferJumpCounter;

    private void Awake() {
        detection = GetComponentInChildren<PlayerDetection>();
    }

    private void Start() {
        GameInputs.Instance.OnJumpPressed += HandleJumpPressed;
        GameInputs.Instance.OnJumpReleased += HandleJumpReleased;
    }

    private void Update() {
        // coyote time
        if (detection.IsGrounded()) {
            coyoteTimeCounter = coyoteTime;
        } else {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // buffer jump time
        bufferJumpCounter -= Time.deltaTime;

        // Trying to jump
         if (bufferJumpCounter > 0 && coyoteTimeCounter > 0) {
            TryToJump?.Invoke();

            bufferJumpCounter = 0f; // Reset buffer jump counter after jumping
            coyoteTimeCounter = 0f; // Reset coyote time counter after jumping
        }
     }

    private void HandleJumpPressed() {
        jumpHeld = true;  
        bufferJumpCounter = bufferJumpTime;
    }

    private void HandleJumpReleased() {
        jumpHeld = false;
    }   

    // ----------------- Properties -----------------
    public float JumpForce => jumpForce;
}
