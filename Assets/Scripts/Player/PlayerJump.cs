using System;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    private PlayerDetection detection;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 4f;

    public bool jumpHeld { get; private set; }

    public event Action TryToJump;

    private void Awake() {
        detection = GetComponentInChildren<PlayerDetection>();
    }

    private void Start() {
        GameInputs.Instance.OnJumpPressed += HandleJumpPressed;
        GameInputs.Instance.OnJumpReleased += HandleJumpReleased;
    }

    private void Update() {
    }

    private void HandleJumpPressed() {
        TryToJump?.Invoke();
        jumpHeld = true;
    }

    private void HandleJumpReleased() {
        jumpHeld = false;
    }   

    // ----------------- Properties -----------------
    public float JumpForce => jumpForce;
}
