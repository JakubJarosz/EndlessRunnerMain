using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static GameManager;

public class UIController : MonoBehaviour {

    private UIDocument uiDocument;
    private GameStats gameStats;

    [SerializeField] private Material fadeShader;
    [SerializeField] private VisualTreeAsset ScoreElemPref;

    private VisualElement StartUI;
    private VisualElement InGameUI;
    private VisualElement DeathUI;
    private VisualElement ScoreboardUI;
    private VisualElement TransitionUI;

    private List<GameStats.SaveScore> inMemoryScoreList = new();
    private Dictionary<VisualElement, Vector2> oldPositions = new();
    private Dictionary<VisualElement, Vector2> newPositions = new();

    private void Awake() {
        uiDocument = GetComponent<UIDocument>();
    }

    private void Start() {
        StartUI = uiDocument.rootVisualElement.Q<VisualElement>("StartUI");
        InGameUI = uiDocument.rootVisualElement.Q<VisualElement>("InGameUI");
        DeathUI = uiDocument.rootVisualElement.Q<VisualElement>("DeathUI");
        ScoreboardUI = uiDocument.rootVisualElement.Q<VisualElement>("ScoreboardUI");
        TransitionUI = uiDocument.rootVisualElement.Q<VisualElement>("TransitionUI");

        GameManager.Instance.GameStateChangeRequest += Instance_GameStateChangeRequest;
        GameManager.Instance.GameStateChanged += Instance_GameStateChanged;

        // Initial Start, makes the visualElelemts clickable, fetch gameStats
        StartUI.style.display = DisplayStyle.Flex;
        gameStats = GameStats.Instance;
        CreateStartButtons();
        PopulateScoreboard();
    }


    private void Update() {
        FetchGameStats();
    }

    private void Instance_GameStateChangeRequest(GameManager.GameState obj) {
        if (obj == GameManager.GameState.PreGame) {
            StartCoroutine(Transition(GameManager.GameState.PreGame));
        } else if (obj == GameManager.GameState.Gameplay) {
             StartCoroutine(Transition(GameManager.GameState.Gameplay));
        }  else if (obj == GameManager.GameState.Scoreboard) {
            StartCoroutine(Transition(GameManager.GameState.Scoreboard));
        }
    }

    private void Instance_GameStateChanged(GameState obj) {
        if (obj == GameManager.GameState.Death) {
            ChangeUI(GameManager.GameState.Death);
        }
    }

    private void ChangeUI(GameManager.GameState state) {
        StartUI.style.display = DisplayStyle.None;
        InGameUI.style.display = DisplayStyle.None;
        DeathUI.style.display = DisplayStyle.None;
        ScoreboardUI.style.display = DisplayStyle.None;

        switch (state) {
            case GameManager.GameState.PreGame:
                StartUI.style.display = DisplayStyle.Flex;
                break;

            case GameManager.GameState.Gameplay:
                InGameUI.style.display = DisplayStyle.Flex;
                break;

            case GameManager.GameState.Death:
                DeathUI.style.display = DisplayStyle.Flex;
                StartCoroutine(HandleDeathUI());
                break;

            case GameManager.GameState.Scoreboard:
                ScoreboardUI.style.display = DisplayStyle.Flex;
                break;
        }
    }

    private void CreateStartButtons() {
        VisualElement startButton = StartUI.Q<VisualElement>("StartBtn");
        VisualElement optionsButton = StartUI.Q<VisualElement>("OptionsBtn");

        startButton.AddManipulator(new Clickable(() => {
            GameManager.Instance.RequestGameState(GameState.Gameplay);
        }));

        optionsButton.AddManipulator(new Clickable(() => {
            Debug.Log("options button clicked");
        }));
    }

    private void FetchGameStats() {
        Label distance = InGameUI.Q<Label>("DistanceText");
        Label coins = InGameUI.Q<Label>("CoinText");
        Label speed = InGameUI.Q<Label>("SpeedText");

        distance.text = gameStats.distanceCount.ToString() + " m";
        coins.text = gameStats.coinCount.ToString();
        speed.text = gameStats.speed.ToString() + " m/s";
    }

    private IEnumerator HandleDeathUI() {
        VisualElement element = DeathUI.Q<VisualElement>("Background");
        float duration = 3f;
        float time = 0f;

        while (time < duration) {
            time += Time.deltaTime;

            float t = Mathf.Clamp01(time / duration);

            element.style.opacity = Mathf.Lerp(0f, 1f, t);

            yield return null;
        }
        element.style.opacity = 1f;

        yield return new WaitForSeconds(0.3f);

        GameManager.Instance.RequestGameState(GameState.Scoreboard);
    }


    // SCOREBOARD LOGIC
    private void PopulateScoreboard() {
        // Adding clickable events to the buttons in the scoreboard UI
        VisualElement retryButton = ScoreboardUI.Q<VisualElement>("RetryButton");
        VisualElement exitButton = ScoreboardUI.Q<VisualElement>("ExitButton");

        retryButton.AddManipulator(new Clickable(() => {
            GameManager.Instance.RequestGameState(GameState.PreGame);
        }));

        exitButton.AddManipulator(new Clickable(() => {
            Debug.Log("App close");
            Application.Quit();
        }));

        VisualElement parent = ScoreboardUI.Q<VisualElement>("ScoresBox");
        SaveSystem.SaveData save = SaveSystem.Instance.LoadGame();


        // Handling The population of scoreboard
        for (int i = 0; i < save.scoreboardList.Count; i++) {
            // Instantiate element
            VisualElement scoreElem = ScoreElemPref.Instantiate();

            // Create Memory Copy List
            inMemoryScoreList.Add(new GameStats.SaveScore {
                coins = save.scoreboardList[i].coins,
                distance = save.scoreboardList[i].distance,
                score = save.scoreboardList[i].score
            });
            // Add to parent
            parent.Add(scoreElem);
            // Get reference to label elements of instantiated element
            Label place = scoreElem.Q<Label>("PlaceText");
            Label coin = scoreElem.Q<Label>("CoinText");
            Label distance = scoreElem.Q<Label>("MetersText");
            Label score = scoreElem.Q<Label>("ScoreText");
            // Update values of labels
            place.text = (i + 1).ToString();
            coin.text = save.scoreboardList[i].coins.ToString();
            distance.text = save.scoreboardList[i].distance.ToString() + " m";
            score.text = save.scoreboardList[i].score.ToString();
        }
    }

    // ----------------------- SCORBOARD LOGIC -------------------------------
    private IEnumerator GetOldLayoutPos() {
        VisualElement parent = ScoreboardUI.Q<VisualElement>("ScoresBox");

        if (parent.childCount == 0)
            yield break;

        yield return new WaitUntil(() =>
            parent.worldBound.width > 0 &&
            parent.worldBound.height > 0 &&
            parent.childCount > 0 &&
            parent.Children().All(el => el.worldBound.height > 0)
        );

        oldPositions.Clear();

        foreach (VisualElement el in parent.Children()) {
            oldPositions[el] = el.worldBound.position;
        }
    }

    private void UpdateScoreLayout() {
        int insertIndex = SaveSystem.Instance.InsertIndex;
        if (insertIndex == -2) return;

        VisualElement parent = ScoreboardUI.Q<VisualElement>("ScoresBox");
        VisualElement scoreElem = ScoreElemPref.Instantiate();

        if (insertIndex == -1)
            parent.Add(scoreElem);
        else
            parent.Insert(insertIndex, scoreElem);

        if (inMemoryScoreList.Count == 10)
            parent.RemoveAt(parent.childCount - 1);
    }

    private IEnumerator GetNewLayoutPos() {
        VisualElement parent = ScoreboardUI.Q<VisualElement>("ScoresBox");

        yield return new WaitUntil(() =>
            parent.childCount > 0 &&
            parent.Children().All(el => el.worldBound.height > 0)
        );

        newPositions.Clear();

        foreach (VisualElement el in parent.Children()) {
            newPositions[el] = el.worldBound.position;
        }
    }

    private IEnumerator PrepareScoreboardAnimation() {
        yield return StartCoroutine(GetOldLayoutPos());

        UpdateScoreLayout();

        yield return StartCoroutine(GetNewLayoutPos());
    }

    private IEnumerator XYZ() {
        // Check to make sure that positions are set
        yield return new WaitUntil(() =>
        oldPositions.Count >= 0 &&
        newPositions.Count > 0
        );


    }

    // ------------------------------ TRANSITION -----------------------------

    private void SetFade(float value) {
        var material = TransitionUI.resolvedStyle.unityMaterial;

        material.SetFloat("_FadeAmount", value);

        TransitionUI.style.unityMaterial = material;
    }

    private IEnumerator Transition(GameManager.GameState gameState) {
        yield return StartCoroutine(FadeIn());

        // Set selected Ui element to Flex and the rest to None
        ChangeUI(gameState);
        
        // Set the oldPositions and newPostions so the data is ready when FadeOut finishes
        if (gameState == GameState.Scoreboard) {
            StartCoroutine(PrepareScoreboardAnimation());
        } 

        yield return new WaitForSeconds(0.5f);

        GameManager.Instance.CompleteGameStateChange(gameState);

        yield return StartCoroutine(FadeOut());

        // Start Scorboard Animations
        if (gameState == GameState.Scoreboard) {
            StartCoroutine(XYZ());
        }
    }

    private IEnumerator FadeIn() {
        float duration = 1f;
        float time = 0f;

        while (time < 1f) {
            time += Time.deltaTime / duration;

            float value = Mathf.Lerp(0f, 1f, time);

            SetFade(value);

            yield return null;
        }
    }

    private IEnumerator FadeOut() {
        float duration = 1f;
        float time = 0f;

        while (time < 1f) {
            time += Time.deltaTime / duration;

            float value = Mathf.Lerp(1f, 0f, time);

            SetFade(value);

            yield return null;
        }
    }
}
