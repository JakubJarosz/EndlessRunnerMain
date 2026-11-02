using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;

    private void Start() {
        // Applying custom divider texture to the divider element in the UI
        Texture2D dividerTexture = UIGradient.CreateDividerTexture();
        VisualElement divider = uiDocument.rootVisualElement.Q<VisualElement>("Divider");

        divider.style.backgroundImage = new StyleBackground(dividerTexture);
    }
}
