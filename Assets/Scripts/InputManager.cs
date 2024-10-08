using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static PlayerInput PlayerInput;

    private InputAction _mousePositionAction;
    private InputAction _mouseAction;
    public static Vector2 MousePosition;

    public static bool WasLeftMouseButtonPressed;
    public static bool WasRightMouseButtonReleased;
    public static bool IsLeftMousePressed;

    private void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();

        _mousePositionAction = PlayerInput.actions["MousePosition"]; // Corrected
        _mouseAction = PlayerInput.actions["Mouse"];
    }

    private void Update() // Fixed method name casing
    {
        MousePosition = _mousePositionAction.ReadValue<Vector2>();
        WasLeftMouseButtonPressed = _mouseAction.WasPressedThisFrame();
        WasRightMouseButtonReleased = _mouseAction.WasReleasedThisFrame();
        IsLeftMousePressed = _mouseAction.IsPressed();
    }
}