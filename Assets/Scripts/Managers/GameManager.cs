using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public event Action<GameState> StateChanged;

    public enum GameState {
        Gameplay,
        Death
    }

    public GameState state;

    private void Awake() {
        Instance = this;
    }

    public void UpdateState(GameState changeState) {
        state = changeState;
        StateChanged?.Invoke(state);
    }
}
