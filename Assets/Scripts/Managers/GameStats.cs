using UnityEngine;

public class GameStats : MonoBehaviour
{
    public static GameStats Instance;

    private int coinCount;

    private void Awake() {
        Instance = this;
    }

    public void AddCoin() {
        coinCount++;
    }
}
