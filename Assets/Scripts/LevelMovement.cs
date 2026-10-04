using UnityEngine;

public class LevelMovement : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerDash playerDash;

    [SerializeField] private float startingSpeed;
    private float speedModifier = 1f;

    private void Update() {
        SetSpeedModifiers();
        transform.position += startingSpeed * speedModifier * Vector3.left * Time.deltaTime;
    }

    private void SetSpeedModifiers() {
        if (playerController.currentState == PlayerController.PlayerState.Dash) {
            speedModifier = playerDash.GetDashMultiplier;
        } else {
            speedModifier = 1f;
        }
    }
}
