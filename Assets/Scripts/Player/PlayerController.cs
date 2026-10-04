using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Rigidbody2D rb;

    private PlayerDetection detection;
    private PlayerJump jump;

    private enum PlayerState {
        Run,
        Jump,
        Fall
    }
    
    private PlayerState currentState;

    // ----------------- Unity Methods -----------------
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();

        detection = GetComponentInChildren<PlayerDetection>();
        jump = GetComponent<PlayerJump>();
    }

    private void Start() {
        jump.TryToJump += Jump_TryToJump;
    }

    private void Update() {
        HandeState();
        HandleJumpHold();
        switch (currentState) {
            case PlayerState.Run:
           
                break;
            case PlayerState.Jump:
             
                break;
            case PlayerState.Fall:

                break;
        }
    }

    // ----------------- Private Methods -----------------
    private void HandeState() {
        if (detection.IsGrounded()) {
            currentState = PlayerState.Run;
        } else {
            currentState = rb.linearVelocity.y > 0 ? PlayerState.Jump : PlayerState.Fall;
        }
    }

    private void HandleJumpHold() {
        if (jump.isJumping && jump.isHoldingJump) {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jump.HoldForce);
        }
    }

    // ----------------- Events -----------------

    private void Jump_TryToJump() {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jump.JumpForce);
    }

    // ----------------- Properties -----------------
    public float GetVerticalVelocity() {
        return rb.linearVelocity.y;
    }
}
