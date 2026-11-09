using System;
using UnityEngine;
using UnityEngine.UIElements;


public class UIController : MonoBehaviour
{
    private UIDocument uiDocuemnt;
    private GameStats gameStats;

    [SerializeField] private VisualTreeAsset MainMenuUI;
    [SerializeField] private VisualTreeAsset InGameUI;
    [SerializeField] private VisualTreeAsset ScoreboardUI;

    private bool isGamePlaying;

    private void Awake() {
        uiDocuemnt = GetComponent<UIDocument>();
    }

    private void Start() {
        gameStats = GameStats.Instance;
        GameManager.Instance.GameStateChanged += Instance_GameStateChanged;
        SwitchUI(MainMenuUI);
        HandleMainMenuUI();
    }

    private void Update() {
       if (isGamePlaying) {
            HandleInGameUI();
        }
    }

    private void SwitchUI(VisualTreeAsset newUI) {
        uiDocuemnt.rootVisualElement.Clear();
        var UI = newUI.Instantiate();
        uiDocuemnt.rootVisualElement.Add(UI);
    }

    private void Instance_GameStateChanged(GameManager.GameState state) {
        if (state == GameManager.GameState.PreGame) {
            SwitchUI(MainMenuUI);
            HandleMainMenuUI();
            isGamePlaying = false;
        } else if (state == GameManager.GameState.Gameplay) {
            SwitchUI(InGameUI);
            isGamePlaying = true;
        } else if (state == GameManager.GameState.Death) {
            SwitchUI(ScoreboardUI);
            HandleScoreboardUI();
            isGamePlaying = false;
        }
    }

    private void HandleMainMenuUI() {
        VisualElement startButton = uiDocuemnt.rootVisualElement.Q<VisualElement>("StartBtn");
        VisualElement optionsButton = uiDocuemnt.rootVisualElement.Q<VisualElement>("OptionsBtn");

        startButton.AddManipulator(new Clickable(() => {
            GameManager.Instance.UpdateGameState(GameManager.GameState.Gameplay);
        }));

        optionsButton.AddManipulator(new Clickable(() => {
            Debug.Log("options button clicked");
        }));
    }

    private void HandleScoreboardUI() {
        VisualElement retryButton = uiDocuemnt.rootVisualElement.Q<VisualElement>("RetryButton");
        VisualElement exitButton = uiDocuemnt.rootVisualElement.Q<VisualElement>("ExitButton");

        retryButton.AddManipulator(new Clickable(() => {
            GameManager.Instance.UpdateGameState(GameManager.GameState.PreGame);
        }));

        exitButton.AddManipulator(new Clickable(() => {
            Debug.Log("App close");
            Application.Quit();
        }));
    }

    private void HandleInGameUI() {
        Label distance = uiDocuemnt.rootVisualElement.Q<Label>("DistanceText");
        Label coins = uiDocuemnt.rootVisualElement.Q<Label>("CoinText");
        Label speed = uiDocuemnt.rootVisualElement.Q<Label>("SpeedText");

        distance.text = gameStats.distance.ToString() + " m";
        coins.text = gameStats.coinCount.ToString();
        speed.text = gameStats.speed.ToString() + " m/s";
    }
}
