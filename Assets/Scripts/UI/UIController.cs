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

    private VisualElement scoreParent;
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

        scoreParent = ScoreboardUI.Q<VisualElement>("ScoresBox");
        // Initial Start, makes the visualElelemts clickable, fetch gameStats
        StartUI.style.display = DisplayStyle.Flex;
        gameStats = GameStats.Instance;
        CreateStartButtons();
        InitialPopulateScoreboard();
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


    // ----------------------- SCORBOARD LOGIC -------------------------------
    private void InitialPopulateScoreboard() {
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

        SaveSystem.SaveData save = SaveSystem.Instance.LoadGame();

        // Handling The population of scoreboard
        for (int i = 0; i < save.scoreboardList.Count; i++) {
            // Instantiate element
            VisualElement scoreElem = ScoreElemPref.Instantiate();

            // Add to scoreParent
            scoreParent.Add(scoreElem);       
        }
        PopulateScoreboardElement(save);
    }

    private void PopulateScoreboardElement(SaveSystem.SaveData save) {
        for (int i = 0; i < save.scoreboardList.Count; i++) {
            VisualElement scoreElem = scoreParent.ElementAt(i);

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

    private IEnumerator GetOldLayoutPos() {

        if (scoreParent.childCount == 0)
            yield break;

        yield return new WaitUntil(() =>
            scoreParent.worldBound.width > 0 &&
            scoreParent.worldBound.height > 0 &&
            scoreParent.childCount > 0 &&
            scoreParent.Children().All(el => el.worldBound.height > 0)
        );

        oldPositions.Clear();

        foreach (VisualElement el in scoreParent.Children()) {
            oldPositions[el] = el.worldBound.position;
        }
    }

    private void UpdateScoreLayout() {
        int insertIndex = SaveSystem.Instance.InsertIndex;
        if (insertIndex == -2) return;

        VisualElement scoreElem = ScoreElemPref.Instantiate();

        if (insertIndex == -1)
            scoreParent.Add(scoreElem);
        else
            scoreParent.Insert(insertIndex, scoreElem);

        //if (inMemoryScoreList.Count == 10)
        //    scoreParent.RemoveAt(scoreParent.childCount - 1);
    }

    private IEnumerator GetNewLayoutPos() {

        yield return new WaitUntil(() =>
            scoreParent.childCount > 0 &&
            scoreParent.Children().All(el => el.worldBound.height > 0)
        );

        newPositions.Clear();

        foreach (VisualElement el in scoreParent.Children()) {
            newPositions[el] = el.worldBound.position;
        }
    }

    private IEnumerator PrepareScoreboardAnimation() {
        SaveSystem.SaveData save = SaveSystem.Instance.LoadGame();

        yield return StartCoroutine(GetOldLayoutPos());

        UpdateScoreLayout();

        yield return StartCoroutine(GetNewLayoutPos());
        // Populating every element with data;
        PopulateScoreboardElement(save);

        yield return StartCoroutine(SetOffsetPositions());
    }

    private IEnumerator SetOffsetPositions() {
        // Check to make sure that positions are set
        yield return new WaitUntil(() =>
        newPositions.Count > 0
        );

        int insertIndex = SaveSystem.Instance.InsertIndex;

        if (insertIndex == -2) {
            yield break;

        } if (insertIndex == -1) {
            float startOffset = -700f;
            scoreParent.ElementAt(scoreParent.childCount - 1).style.translate = new Translate(0, startOffset);
        } else {
            for (int i = 0; i < scoreParent.childCount; i++) {
                VisualElement element = scoreParent.ElementAt(i);

                // Element above inserted so skip
                if (i < insertIndex)
                    continue;

                if (oldPositions.TryGetValue(element, out Vector2 oldPos)) {
                    // This are elements that are undearneath the inserted element, they will translate to the origianl position
                    // but for now they need to start from the one index above posiion
                    float startOffset = oldPos.y - newPositions[element].y;
                    element.style.translate = new Translate(0, startOffset);
                } else {
                    // Thats the inserted element that will start from some Y position and go to 0
                    float startOffset = -700f;
                    element.style.translate = new Translate(0, startOffset);
                }
            }
        }
    }

    private void HandleScoreAnimation() {
        int insertIndex = SaveSystem.Instance.InsertIndex;

        foreach (VisualElement el in scoreParent.Children()) {
            int index = scoreParent.IndexOf(el);

            float startOffset;

            if (insertIndex == -1) {
                startOffset = index == scoreParent.childCount - 1 ? -700f : 0f;
            } else if (index < insertIndex) {
                startOffset = 0f;
            } else if (oldPositions.TryGetValue(el, out Vector2 oldPos)) {
                startOffset = oldPos.y - newPositions[el].y;
            } else {
                startOffset = -700f;
            }

            if (index == 10) {
                // Its 11th element that should dissapear
                StartCoroutine(RemoveLastElem(el, 2f, startOffset, 100f));
            } else {
                StartCoroutine(AnimateElement(el, 2f, startOffset, 0f));
            }
        }
    }

    private IEnumerator AnimateElement(VisualElement element, float duration, float startY, float endY) {
        float elapsed = 0f;

        while(elapsed < duration) {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / duration);
            t = Mathf.SmoothStep(0f, 1f, t);
            float y = Mathf.Lerp(startY, endY, t);

            element.style.translate = new Translate(0, y);

            yield return null;
        }
        element.style.translate = new Translate(0, endY);
    }

    private IEnumerator RemoveLastElem(VisualElement element, float duration, float startY, float endY) {
        yield return StartCoroutine(AnimateElement(element, duration, startY, endY));
        scoreParent.RemoveAt(scoreParent.childCount - 1);
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
            HandleScoreAnimation();
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
