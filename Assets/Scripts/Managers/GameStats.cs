using UnityEngine;

public class GameStats : MonoBehaviour
{
    public static GameStats Instance;

    private int coinCount;

    // Slow pick up variables


    private void Awake() {
        Instance = this;
    }

    private void Update() {
        Debug.Log(coinCount);
    }

    public void AddCoin() {
        coinCount++;
    }
}
