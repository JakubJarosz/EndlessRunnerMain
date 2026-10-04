using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] private float dashMovementMultiplier = 2f;
    [SerializeField] private float dashDuration = 0.5f;

    private float dashDurationTimer;

    public bool isDashing { get; private set; }

    private void Start() {
        GameInputs.Instance.OnDashPressed += HandleDashPressed;
    }

    private void Update() {
        if (isDashing) {
            dashDurationTimer -= Time.deltaTime;

            if (dashDurationTimer <= 0f) {
                isDashing = false;
            }
        }
    }

    private void HandleDashPressed() {
        if (isDashing) return;

        isDashing = true;
        dashDurationTimer = dashDuration;
    }

    // ----------------- Properties -----------------
    public float GetDashMultiplier => dashMovementMultiplier;
}
