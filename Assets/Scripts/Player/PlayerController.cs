using System;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public static PlayerController Instance;

    private Rigidbody2D rb;

    private PlayerDetection detection;
    private PlayerJump jump;
    public PlayerDash dash;
    private PlayerSlide slide;

    // ----------------- Events -----------------
    public event Action PerformDashVisual;
    public event Action<bool> PerformSlideVisual;

    [SerializeField] private float centerDelay;
    private float centerTimer;
    private float lastPosition;
    

    public enum PlayerState {
        Run,
        Jump,
        Fall,
        Dash,
        Slide
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
        slide = GetComponent<PlayerSlide>();
    }

    private void Start() {
        jump.TryToJump += HandleTryToJump;
    }
    private void Update() {
        HandeState();
        HandleGravity();
        HandlePlayerPosition();
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
        } else if (detection.IsGrounded()) {
            currentState = slide.isSliding ? PlayerState.Slide : PlayerState.Run;
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
            rb.linearVelocity = Vector2.zero;
        }

        // Handle Slide start
        if (currentState == PlayerState.Slide) {
            PerformSlideVisual?.Invoke(true);
        }

        // Handle Slide end
        if (previousState == PlayerState.Slide && currentState != PlayerState.Slide) {
            PerformSlideVisual?.Invoke(false);
        }
    }

    private void HandleTryToJump() {
        if (detection.IsGrounded() && currentState != PlayerState.Slide) {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jump.JumpForce);
        }
    }

    private void HandleGravity() {
        if (currentState == PlayerState.Dash) return;

        if (rb.linearVelocity.y > 0) {
            if (jump.jumpHeld)
                rb.gravityScale = 2f;       // full jump
            else
                rb.gravityScale = 6f;       // cut jump 
        } else {
            rb.gravityScale = 3f;         // falling
        }
    }

    private void HandlePlayerPosition() {
        float currentPosition = transform.position.x;
        float delta = currentPosition - lastPosition;
        bool isBeingPushed = delta < -0.01f;
        bool isBeingCentered = delta > 0.01f;
        bool isNotMoving = Mathf.Abs(delta) <= 0.01f;

        if (currentPosition < 0) {
            if (isBeingPushed) {
                centerTimer = 0f;
            } else if (isBeingCentered) {
                centerTimer = 0f;
                CenterPlayer();
            } else if (isNotMoving) {
                centerTimer += Time.deltaTime;

                if (centerTimer >= centerDelay) {
                    CenterPlayer();
                }
            }
        }

        lastPosition = currentPosition;
    }

    private void CenterPlayer() {
        Vector2 pos = transform.position;
        // 2f is the speed of the centering
        pos.x = Mathf.MoveTowards(pos.x, 0f, 2f * Time.deltaTime);
        transform.position = pos;
    }

    // ----------------- Events -----------------


    // ----------------- Properties -----------------
    public float GetVerticalVelocity => rb.linearVelocity.y;

}
