using System;
using UnityEngine;

public class GameInputs : MonoBehaviour
{
    public static GameInputs Instance;
    private InputActions inputActions;

    // -------------- Events --------------
    public event Action<bool> IsJumpPressed;

    // ------------- End Events --------------

    private void Awake() {
        Instance = this;

        inputActions = new InputActions();

        inputActions.Player.Enable();

        inputActions.Player.Jump.started += ctx => IsJumpPressed?.Invoke(true);
        inputActions.Player.Jump.canceled += ctx => IsJumpPressed?.Invoke(false);
    }
}
