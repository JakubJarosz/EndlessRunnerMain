using System;
using System.Collections;
using UnityEngine;

public class GameInputs : MonoBehaviour
{
    public static GameInputs Instance;
    private InputActions inputActions;

    // -------------- Events --------------
    public event Action OnJumpPressed;
    public event Action OnJumpReleased;
    public event Action OnDashPressed;
    public event Action<bool> IsSlidingPressed;

    // ------------- End Events --------------

    private void Awake() {
        Instance = this;

        inputActions = new InputActions();

        inputActions.Player.Jump.started += ctx => OnJumpPressed?.Invoke();
        inputActions.Player.Jump.canceled += ctx => OnJumpReleased?.Invoke();

        inputActions.Player.Dash.started += ctx => OnDashPressed?.Invoke();

        inputActions.Player.Slide.started += ctx => IsSlidingPressed?.Invoke(true);
        inputActions.Player.Slide.canceled += ctx => IsSlidingPressed?.Invoke(false);
    }

    private void Start() {
        GameManager.Instance.GameStateChanged += Instance_GameStateChanged;
    }

    private void Instance_GameStateChanged(GameManager.GameState obj) {
        if (obj == GameManager.GameState.Gameplay) {
            StartCoroutine(DelayInputs());
        } else {
            inputActions.Player.Disable();
        }
    }

    private IEnumerator DelayInputs() {
        yield return new WaitForSeconds(4f);
        inputActions.Player.Enable();
    }
}
