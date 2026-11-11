using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public event Action<GameState> GameStateChanged;

    public enum GameState {
        PreGame,
        Gameplay,
        Death,
        Scoreboard
    }

    public GameState state { get; private set; }

    private void Awake() {
        Instance = this;
        UpdateGameState(GameState.PreGame);
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.P)) {
            UpdateGameState(GameState.Scoreboard);
        }
    }

    public void UpdateGameState(GameState gameState) {
        state = gameState;
        GameStateChanged?.Invoke(state);
    }
}
