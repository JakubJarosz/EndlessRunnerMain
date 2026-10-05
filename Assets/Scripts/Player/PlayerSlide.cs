using UnityEngine;

public class PlayerSlide : MonoBehaviour
{
    public bool isSlidingPressed { get; private set; }

    private void Start() {
        GameInputs.Instance.IsSlidingPressed += HandleSlidePressed;
    }

    private void HandleSlidePressed(bool isSliding) {
        isSlidingPressed = isSliding;
    }
}
