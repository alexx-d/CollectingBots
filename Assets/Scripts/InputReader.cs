using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, PlayerInput.IPlayerActions
{
    private PlayerInput _controls;

    public Vector2 MovementInput { get; private set; }
    public Vector2 MousePosition { get; private set; }
    public float ZoomInput { get; private set; }

    public event Action LeftClickPressed;

    private void Awake()
    {
        _controls = new PlayerInput();

        _controls.Player.SetCallbacks(this);
    }

    private void OnEnable() => _controls.Player.Enable();
    private void OnDisable() => _controls.Player.Disable();

    public void OnMovement(InputAction.CallbackContext context)
    {
        MovementInput = context.ReadValue<Vector2>();
    }

    public void OnMousePosition(InputAction.CallbackContext context)
    {
        MousePosition = context.ReadValue<Vector2>();
    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        ZoomInput = context.ReadValue<Vector2>().y;
    }

    public void OnLeftClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            LeftClickPressed?.Invoke();
        }
    }
}