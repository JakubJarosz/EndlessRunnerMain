using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public event Action<GameState> GameStateChanged;

    private float transitionTime;

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

    //private IEnumerator Transition() {

    //}
}
