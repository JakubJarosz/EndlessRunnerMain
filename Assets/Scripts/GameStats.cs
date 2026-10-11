using UnityEngine;

public class GameStats : MonoBehaviour
{
    public static GameStats Instance;

    private int coinCount;

    // Slow pick up variables


    private void Awake() {
        Instance = this;
    }

    public void AddCoin() {
        coinCount++;
    }
}
