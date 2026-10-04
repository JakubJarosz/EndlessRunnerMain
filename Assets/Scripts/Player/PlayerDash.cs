using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] private float dashMoveMultiplier = 2f;

    private bool isDashing;

    private void Start() {
        GameInputs.Instance.OnDashPressed += HandleDashPressed;
    }

    private void Update() {
        
    }

    private void HandleDashPressed() {
        isDashing = true;
    }
}
