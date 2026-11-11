using UnityEngine;
using static GameManager;

public class GameStats : MonoBehaviour
{
    public static GameStats Instance;

    public int coinCount { get; private set; }
    public int speed { get; private set; }
    public int distanceCount { get; private set; }

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        GameManager.Instance.GameStateChanged += Instance_GameStateChanged;
    }

    private void Instance_GameStateChanged(GameManager.GameState obj) {
        if (obj == GameManager.GameState.PreGame) {
            coinCount = 0;
        } else if (obj == GameManager.GameState.Death) {

        }
    }

    private void Update() {
        speed = (int)LevelMovement.Instance.currentSpeed;
        distanceCount = (int)LevelMovement.Instance.totalDistance;
    }

    public void AddCoin() {
        coinCount++;
    }

    // ------------ Save System ------------
    [System.Serializable]
    public class SaveScore {
        public int coins;
        public int distance;
        public int score;
    }

    public SaveScore Save() {
        return new SaveScore {
            coins = coinCount,
            distance = distanceCount,
            score = coinCount == 0 ? distanceCount : coinCount * distanceCount,
        };
    }
}
