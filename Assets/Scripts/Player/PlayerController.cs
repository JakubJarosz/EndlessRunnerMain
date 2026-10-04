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
        jump.TryToJump += HandleTryToJump;
    }
    private void Update() {
        HandeState();
        HandleGravity();
  
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
        if (detection.IsGrounded()) {
            currentState = PlayerState.Run;
        } else {
            currentState = rb.linearVelocity.y > 0 ? PlayerState.Jump : PlayerState.Fall;
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
    public float GetVerticalVelocity() {
        return rb.linearVelocity.y;
    }
}
