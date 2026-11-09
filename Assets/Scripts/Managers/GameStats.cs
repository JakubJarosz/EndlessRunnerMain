using UnityEngine;

public class GameStats : MonoBehaviour
{
    public static GameStats Instance;

    public int coinCount { get; private set; }
    public int speed { get; private set; }
    public int distance { get; private set; }

    private void Awake() {
        Instance = this;
    }

    private void Update() {
        speed = (int)LevelMovement.Instance.currentSpeed;
        distance = (int)LevelMovement.Instance.totalDistance;
    }

    public void AddCoin() {
        coinCount++;
    }
}
