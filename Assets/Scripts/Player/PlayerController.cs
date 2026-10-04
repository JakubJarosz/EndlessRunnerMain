using System;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public static PlayerController Instance;

    private Rigidbody2D rb;

    private PlayerDetection detection;
    private PlayerJump jump;
    public PlayerDash dash;

    // ----------------- Events -----------------
    public event Action PerformDashVisual;

    public enum PlayerState {
        Run,
        Jump,
        Fall,
        Dash
    }
    
    public PlayerState currentState { get; private set; }
    private PlayerState previousState;

    // ----------------- Unity Methods -----------------
    private void Awake() {
        Instance = this;

        rb = GetComponent<Rigidbody2D>();

        detection = GetComponentInChildren<PlayerDetection>();
        jump = GetComponent<PlayerJump>();
        dash = GetComponent<PlayerDash>();
    }

    private void Start() {
        jump.TryToJump += HandleTryToJump;
    }
    private void Update() {
        HandeState();
        HandleGravity();
        Debug.Log(currentState);
        //switch (currentState) {
        //    case PlayerState.Run:

        //        break;
        //    case PlayerState.Jump:

        //        break;
        //    case PlayerState.Fall:

        //        break;
        //}
    }

    // ----------------- Private Methods -----------------
    private void HandeState() {
        previousState = currentState;

        if (dash.isDashing) {
            currentState = PlayerState.Dash;
            return;
        }
        if (detection.IsGrounded()) {
            currentState = PlayerState.Run;
        } else {
            currentState = rb.linearVelocity.y > 0 ? PlayerState.Jump : PlayerState.Fall;
        }

        HandleStateChange();
    }

    private void HandleStateChange() {
        if (currentState == previousState) return;

        // Handle Dash state
        if (currentState == PlayerState.Dash) {
            PerformDashVisual?.Invoke();
            rb.gravityScale = 0f;
        } 
    }

    private void HandleTryToJump() {
        if (detection.IsGrounded()) {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jump.JumpForce);
        }
    }

    private void HandleGravity() {
        if (rb.linearVelocity.y > 0) {
            if (jump.jumpHeld)
                rb.gravityScale = 2f;       // full jump
            else
                rb.gravityScale = 6f;       // cut jump 
        } else {
            rb.gravityScale = 3f;         // falling
        }
    }

    // ----------------- Events -----------------


    // ----------------- Properties -----------------
    public float GetVerticalVelocity => rb.linearVelocity.y;

}
