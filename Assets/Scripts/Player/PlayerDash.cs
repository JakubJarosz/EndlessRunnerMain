using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] private float dashMovementMultiplier = 2f;
    [SerializeField] private float dashDuration = 0.5f;
    [SerializeField] private float cooldownDuration = 0.2f;

    private float dashDurationTimer;
    private float cooldownTimer;

    public bool isDashing { get; private set; }
    private bool cooldownActive;

    private void Start() {
        GameInputs.Instance.OnDashPressed += HandleDashPressed;
    }

    private void Update() {
        if (isDashing) {
            dashDurationTimer -= Time.deltaTime;

            if (dashDurationTimer <= 0f) {
                isDashing = false;
                cooldownActive = true;
            }
        }

        if (cooldownActive) {
            cooldownTimer += Time.deltaTime;

            if (cooldownTimer >= cooldownDuration) {
                cooldownTimer = 0f;
                cooldownActive = false;
            }
        }
    }

    private void HandleDashPressed() {
        if (isDashing || PlayerController.Instance.currentState == PlayerController.PlayerState.Slide || cooldownActive) return;

        isDashing = true;
        dashDurationTimer = dashDuration;
    }

    // ----------------- Properties -----------------
    public float GetDashMultiplier => dashMovementMultiplier;
}
