using Mono.Cecil.Cil;
using UnityEngine;

public class PlayerSlide : MonoBehaviour
{
    private BoxCollider2D playerCol;

    [SerializeField] private Vector2 slideCollider;
    [SerializeField] private Vector2 slideOffset;
    [SerializeField] private Vector2 canStandUpCheck;
    [SerializeField] private LayerMask canStandCheckLayer;

    private Vector2 standingCol;
    private Vector2 standingOffset;

    private bool isSlidingPressed;
    public bool isSliding { get; private set; }

    private void Awake() {
        playerCol = GetComponent<BoxCollider2D>();
        standingCol = playerCol.size;
        standingOffset = playerCol.offset;
    }

    private void Start() {
        GameInputs.Instance.IsSlidingPressed += HandleSlidePressed;
    }

    private void Update() {
        HandleCol();

        if (isSlidingPressed) {
            isSliding = true;
        }

        if (isSliding && !isSlidingPressed) {
            if (CanStandUpCheck()) {
                isSliding = false;
            } else {
                isSliding = true;
            }
        }
    }

    private void OnDrawGizmosSelected() {
        if (playerCol == null) return;

        Vector3 pos = transform.position;

        // Slide collider check
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(pos + (Vector3)slideOffset, new Vector2(slideCollider.x, slideCollider.y));

        // Can stand up check
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(pos + (Vector3)standingOffset + new Vector3(0f, 1f, 0f), new Vector2(canStandUpCheck.x, canStandUpCheck.y));
    }

    private bool CanStandUpCheck() {
        return !Physics2D.OverlapBox((Vector2)transform.position + standingOffset + new Vector2(0f, 1f), canStandUpCheck, 0f, canStandCheckLayer);
    }

    private void HandleCol() {
        if (isSliding) {
            playerCol.size = slideCollider;
            playerCol.offset = slideOffset;
        } else {
            playerCol.size = standingCol;
            playerCol.offset = standingOffset;
        }
    }

    private void HandleSlidePressed(bool isSliding) {
        isSlidingPressed = isSliding;
    }
}
