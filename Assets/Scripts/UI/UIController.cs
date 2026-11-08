using System;
using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;

    private void Start() {
        GameManager.Instance.GameStateChanged += Instance_GameStateChanged;
        VisualElement startButton = uiDocument.rootVisualElement.Q<VisualElement>("StartBtn");
        VisualElement optionsButton = uiDocument.rootVisualElement.Q<VisualElement>("OptionsBtn");

        startButton.AddManipulator(new Clickable(() => {
            GameManager.Instance.UpdateGameState(GameManager.GameState.Gameplay);
        }));

        optionsButton.AddManipulator(new Clickable(() => {
            Debug.Log("options button clicked");
        }));
    }

    private void Instance_GameStateChanged(GameManager.GameState state) {
        if (state == GameManager.GameState.PreGame) {
            uiDocument.rootVisualElement.style.display = DisplayStyle.Flex;
        } else  {
            uiDocument.rootVisualElement.style.display = DisplayStyle.None;
        }
    }
}
