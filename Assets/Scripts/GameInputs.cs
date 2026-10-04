using System;
using UnityEngine;

public class GameInputs : MonoBehaviour
{
    public static GameInputs Instance;
    private InputActions inputActions;

    // -------------- Events --------------
    public event Action OnJumpPressed;
    public event Action OnJumpReleased;
    public event Action OnDashPressed;

    // ------------- End Events --------------

    private void Awake() {
        Instance = this;

        inputActions = new InputActions();

        inputActions.Player.Enable();

        inputActions.Player.Jump.started += ctx => OnJumpPressed?.Invoke();
        inputActions.Player.Jump.canceled += ctx => OnJumpReleased?.Invoke();

        inputActions.Player.Dash.started += ctx => OnDashPressed?.Invoke();
    }
}
