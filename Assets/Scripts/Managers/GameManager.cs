using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public event Action<GameState> GameStateChanged;

    public enum GameState {
        PreGame,
        Gameplay,
        Death
    }

    public GameState state { get; private set; }

    private void Awake() {
        Instance = this;
        UpdateGameState(GameState.PreGame);
    }

    public void UpdateGameState(GameState gameState) {
        state = gameState;
        GameStateChanged?.Invoke(state);
    }
}
