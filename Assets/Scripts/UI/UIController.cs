using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;

    private void Start() {
        // Applying custom divider texture to the divider element in the UI
        VisualElement startButton = uiDocument.rootVisualElement.Q<VisualElement>("StartBtn");
        VisualElement optionsButton = uiDocument.rootVisualElement.Q<VisualElement>("OptionsBtn");

        startButton.AddManipulator(new Clickable(() => {
            //GameManager.Instance.UpdateGameState(GameManager.GameState.Gameplay);
            Debug.Log("start button clicked");
        }));

        optionsButton.AddManipulator(new Clickable(() => {
            //GameManager.Instance.UpdateGameState(GameManager.GameState.Gameplay);
            Debug.Log("options button clicked");
        }));
    }
}
