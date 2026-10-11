using UnityEngine;

public class LevelMovement : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerDash playerDash;

    [Header("Movement variables")]
    [SerializeField] private float startingSpeed;
    [SerializeField] private float speedInscrease;

    public float currentSpeed { private set; get; }
    private float speedModifier = 1f;

    private void Start() {
        currentSpeed = startingSpeed;
    }

    private void Update() {
        SetSpeedModifiers();
        SpeedIncrease();
        transform.position += currentSpeed * speedModifier * Vector3.left * Time.deltaTime;
        Debug.Log(currentSpeed);
    }

    private void SpeedIncrease() {
        currentSpeed += speedInscrease * Time.deltaTime;
    }

    private void SetSpeedModifiers() {
        if (playerController.currentState == PlayerController.PlayerState.Dash) {
            speedModifier = playerDash.GetDashMultiplier;
        } else {
            speedModifier = 1f;
        }
    }
}
