using UnityEngine;

public class LevelMovement : MonoBehaviour
{
    public static LevelMovement Instance { private set; get; }

    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerDash playerDash;

    [Header("Movement variables")]
    [SerializeField] private float startingSpeed;
    [SerializeField] private float speedInscrease;
    [SerializeField] private float slowPickupMovementDescrease;

    public float currentSpeed { private set; get; }
    public float speedModifier { private set; get; } = 1f;
    public float totalDistance { private set; get; }

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        currentSpeed = startingSpeed;
        SlowPickup.SlowPickupCollected += SlowPickup_SlowPickupCollected;
    }

    private void SlowPickup_SlowPickupCollected() {
        currentSpeed -= slowPickupMovementDescrease;
    }

    private void Update() {
        if (GameManager.Instance.state != GameManager.GameState.Gameplay) return;
        SetSpeedModifiers();
        SpeedIncrease();
        transform.position += currentSpeed * speedModifier * Vector3.left * Time.deltaTime;
        totalDistance += currentSpeed * Time.deltaTime;
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
