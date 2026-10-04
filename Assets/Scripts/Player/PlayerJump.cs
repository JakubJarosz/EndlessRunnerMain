using System;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    private PlayerDetection detection;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 2f;
    [SerializeField] private float holdForce = 4f;
    [SerializeField] private float maxJumpTime = 0.4f;

    private float jumpTimeCounter;
    public bool isJumping { get; private set; }
    public bool isHoldingJump { get; private set; }
   
    public event Action TryToJump;

    private void Awake() {
        detection = GetComponentInChildren<PlayerDetection>();
    }

    private void Start() {
        GameInputs.Instance.IsJumpPressed += GameInputsEvent_IsJumpPressed;
    }

    private void Update() {
        if (isHoldingJump && isJumping) {
            if (jumpTimeCounter > 0) {
                jumpTimeCounter -= Time.deltaTime;
            } else {
                isJumping = false;
            }
        }
    }

    private void GameInputsEvent_IsJumpPressed(bool pressed) {
        isHoldingJump = pressed;

        if (pressed && detection.IsGrounded()) {
            isJumping = true;
            jumpTimeCounter = maxJumpTime;
            TryToJump?.Invoke();
        } 

        if (!pressed) {
            isJumping = false;
        }
    }

    // ----------------- Properties -----------------
    public float JumpForce => jumpForce;
    public float HoldForce => holdForce;    
}
