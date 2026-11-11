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
    [SerializeField] private VisualTreeAsset DeathUI;

    [SerializeField] private VisualTreeAsset ScoreElemPref;

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
            SwitchUI(DeathUI);
            isGamePlaying = false;
        } else if (state == GameManager.GameState.Scoreboard) {
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
        // Adding clickable events to the buttons in the scoreboard UI
        VisualElement retryButton = uiDocuemnt.rootVisualElement.Q<VisualElement>("RetryButton");
        VisualElement exitButton = uiDocuemnt.rootVisualElement.Q<VisualElement>("ExitButton");

        retryButton.AddManipulator(new Clickable(() => {
            GameManager.Instance.UpdateGameState(GameManager.GameState.PreGame);
        }));

        exitButton.AddManipulator(new Clickable(() => {
            Debug.Log("App close");
            Application.Quit();
        }));


        VisualElement parent = uiDocuemnt.rootVisualElement.Q<VisualElement>("ScoresBox");
        SaveSystem.SaveData save = SaveSystem.Instance.LoadGame();


        // Handling The population of scoreboard
        for (int i = 0; i < save.scoreboardList.Count; i++) {
            // Instantiate element
            VisualElement scoreElem = ScoreElemPref.Instantiate();
            // Add to parent
            parent.Add(scoreElem);
            // Get reference to label elements of instantiated element
            Label place = scoreElem.Q<Label>("PlaceText");
            Label coin = scoreElem.Q<Label>("CoinText");
            Label distance = scoreElem.Q<Label>("MetersText");
            Label score = scoreElem.Q<Label>("ScoreText");
            // Update values of labels
            place.text = (i+1).ToString();
            coin.text = save.scoreboardList[i].coins.ToString();
            distance.text = save.scoreboardList[i].distance.ToString() + " m";
            score.text = save.scoreboardList[i].score.ToString();
        }
    }

    private void HandleInGameUI() {
        Label distance = uiDocuemnt.rootVisualElement.Q<Label>("DistanceText");
        Label coins = uiDocuemnt.rootVisualElement.Q<Label>("CoinText");
        Label speed = uiDocuemnt.rootVisualElement.Q<Label>("SpeedText");

        distance.text = gameStats.distanceCount.ToString() + " m";
        coins.text = gameStats.coinCount.ToString();
        speed.text = gameStats.speed.ToString() + " m/s";
    }
}
