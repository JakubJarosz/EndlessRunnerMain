using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private float cameraSpeed;
    private bool startCenteringPlayer;

    public static GameManager Instance;

    public event Action<GameState> GameStateChanged;
    public event Action<GameState> GameStateChangeRequest;

    private float startingCameraPos;

    public enum GameState {
        PreGame,
        Gameplay,
        Death,
        Scoreboard
    }

    public GameState state { get; private set; }

    private void Awake() {
        Instance = this;
        CompleteGameStateChange(GameState.PreGame);
    }

    private void Start() {
        startingCameraPos = cinemachineCamera.transform.position.x;
        GameStateChanged += GameManager_GameStateChanged;
    }

    private void Update() {
        if (state == GameState.PreGame) {
            Vector3 cameraPos = cinemachineCamera.transform.position;
            cameraPos.x += Time.deltaTime * cameraSpeed;
            cinemachineCamera.transform.position = new Vector3
                (cameraPos.x,
                cameraPos.y,
                cameraPos.z
                );
        }

        if (startCenteringPlayer) {
            bool isFinishedCentering = PlayerController.Instance.CenterPlayer();
            if (isFinishedCentering) {
                startCenteringPlayer = false;
            }
        }
    }

    public void RequestGameState(GameState gameState) {
        GameStateChangeRequest?.Invoke(gameState);
    }
    public void CompleteGameStateChange(GameState gameState) {
        state = gameState;
        GameStateChanged?.Invoke(state);
    }

    // --------------- PREGAME SETUP -----------------

    private void GameManager_GameStateChanged(GameState obj) {
        if (obj == GameState.PreGame || obj == GameState.Gameplay) {
            cinemachineCamera.transform.position = new Vector3
                (startingCameraPos,
                cinemachineCamera.transform.position.y,
                cinemachineCamera.transform.position.z
                );
        } else if (obj == GameState.Gameplay) {
            startCenteringPlayer = true;
        }
    }
}
