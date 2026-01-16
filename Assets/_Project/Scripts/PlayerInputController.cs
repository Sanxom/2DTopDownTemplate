using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    public static PlayerInputController Instance { get; private set; }

    public static event Action<InputAction.CallbackContext> OnMoveActionPressed;
    public static event Action<InputAction.CallbackContext> OnMoveActionCanceled;
    public static event Action OnOpenMenuPerformed;
    public static event Action OnCloseMenuPerformed;

    private GameInput _gameInput;
    private InputAction _moveAction;
    private InputAction _openMenuAction;
    private InputAction _closeMenuAction;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _gameInput = new();
        _moveAction = _gameInput.Player.Move;
        _openMenuAction = _gameInput.Player.OpenMenu;
        _closeMenuAction = _gameInput.UI.CloseMenu;
        _moveAction.performed += MoveAction_Performed;
        _moveAction.canceled += MoveAction_Canceled;
        _openMenuAction.performed += OpenMenuAction_Performed;
        _closeMenuAction.performed += CloseMenuAction_Performed;
    }

    private void OnEnable()
    {
        _gameInput.Enable();
        EnablePlayerDisableUI();
    }

    private void OnDestroy()
    {
        _moveAction.performed -= MoveAction_Performed;
        _moveAction.canceled -= MoveAction_Canceled;
        _openMenuAction.performed -= OpenMenuAction_Performed;
        _closeMenuAction.performed -= CloseMenuAction_Performed;
        _gameInput.Disable();
    }

    public void EnablePlayerDisableUI()
    {
        _gameInput.Player.Enable();
        _gameInput.UI.Disable();
    }

    public void EnableUIDisablePlayer()
    {
        _gameInput.UI.Enable();
        _gameInput.Player.Disable();
    }

    private void MoveAction_Performed(InputAction.CallbackContext context)
    {
        OnMoveActionPressed?.Invoke(context);
    }

    private void MoveAction_Canceled(InputAction.CallbackContext context)
    {
        OnMoveActionCanceled?.Invoke(context);
    }

    private void OpenMenuAction_Performed(InputAction.CallbackContext context)
    {
        EnableUIDisablePlayer();
        OnOpenMenuPerformed?.Invoke();
    }

    private void CloseMenuAction_Performed(InputAction.CallbackContext context)
    {
        EnablePlayerDisableUI();
        OnCloseMenuPerformed?.Invoke();
    }
}