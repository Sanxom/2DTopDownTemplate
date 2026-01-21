using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    public static PlayerInputController Instance { get; private set; }

    public static event Action<InputAction.CallbackContext> OnMoveActionPressed;
    public static event Action<InputAction.CallbackContext> OnMoveActionCanceled;
    public static event Action OnInteractActionPerformed;
    public static event Action OnOpenMenuPerformed;
    public static event Action OnCloseMenuPerformed;

    #region Hotbar Events
    public static event Action OnHotbarSlot1Performed;
    public static event Action OnHotbarSlot2Performed;
    public static event Action OnHotbarSlot3Performed;
    public static event Action OnHotbarSlot4Performed;
    public static event Action OnHotbarSlot5Performed;
    public static event Action OnHotbarSlot6Performed;
    public static event Action OnHotbarSlot7Performed;
    public static event Action OnHotbarSlot8Performed;
    public static event Action OnHotbarSlot9Performed;
    public static event Action OnHotbarSlot10Performed;
    #endregion

    private GameInput _gameInput;
    private InputAction _moveAction;
    private InputAction _openMenuAction;
    private InputAction _closeMenuAction;
    private InputAction _interactAction;

    #region Hotbar Input
    private InputAction _hotbarSlot1Action;
    private InputAction _hotbarSlot2Action;
    private InputAction _hotbarSlot3Action;
    private InputAction _hotbarSlot4Action;
    private InputAction _hotbarSlot5Action;
    private InputAction _hotbarSlot6Action;
    private InputAction _hotbarSlot7Action;
    private InputAction _hotbarSlot8Action;
    private InputAction _hotbarSlot9Action;
    private InputAction _hotbarSlot10Action;
    #endregion

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _gameInput = new();
        SetupInput();
        SetupInputSubscriptions();
    }

    private void OnEnable()
    {
        _gameInput.Enable();
        EnablePlayerDisableUI();
    }

    private void OnDestroy()
    {
        UnsubscribeAllInput();
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

    private void InteractAction_Performed(InputAction.CallbackContext context)
    {
        OnInteractActionPerformed?.Invoke();
    }

    private void OpenMenuAction_Performed(InputAction.CallbackContext context)
    {
        //EnableUIDisablePlayer(); // TODO: Uncomment this and the one in the function below to prevent player from taking in-game actions entirely if you want
        _openMenuAction.Disable();
        _closeMenuAction.Enable();
        OnOpenMenuPerformed?.Invoke();
    }

    private void CloseMenuAction_Performed(InputAction.CallbackContext context)
    {
        //EnablePlayerDisableUI();
        _closeMenuAction.Disable();
        _openMenuAction.Enable();
        OnCloseMenuPerformed?.Invoke();
    }

    private void SetupInput()
    {
        _moveAction = _gameInput.Player.Move;
        _interactAction = _gameInput.Player.Interact;
        _openMenuAction = _gameInput.Player.OpenMenu;
        _closeMenuAction = _gameInput.UI.CloseMenu;

        _hotbarSlot1Action = _gameInput.Player.HotbarSlot1;
        _hotbarSlot2Action = _gameInput.Player.HotbarSlot2;
        _hotbarSlot3Action = _gameInput.Player.HotbarSlot3;
        _hotbarSlot4Action = _gameInput.Player.HotbarSlot4;
        _hotbarSlot5Action = _gameInput.Player.HotbarSlot5;
        _hotbarSlot6Action = _gameInput.Player.HotbarSlot6;
        _hotbarSlot7Action = _gameInput.Player.HotbarSlot7;
        _hotbarSlot8Action = _gameInput.Player.HotbarSlot8;
        _hotbarSlot9Action = _gameInput.Player.HotbarSlot9;
        _hotbarSlot10Action = _gameInput.Player.HotbarSlot10;
    }

    private void HotbarSlot10Action_Performed(InputAction.CallbackContext obj)
    {
        OnHotbarSlot10Performed?.Invoke();
    }

    private void HotbarSlot9Action_Performed(InputAction.CallbackContext obj)
    {
        OnHotbarSlot9Performed?.Invoke();
    }

    private void HotbarSlot8Action_Performed(InputAction.CallbackContext obj)
    {
        OnHotbarSlot8Performed?.Invoke();
    }

    private void HotbarSlot7Action_Performed(InputAction.CallbackContext obj)
    {
        OnHotbarSlot7Performed?.Invoke();
    }

    private void HotbarSlot6Action_Performed(InputAction.CallbackContext obj)
    {
        OnHotbarSlot6Performed?.Invoke();
    }

    private void HotbarSlot5Action_Performed(InputAction.CallbackContext obj)
    {
        OnHotbarSlot5Performed?.Invoke();
    }

    private void HotbarSlot4Action_Performed(InputAction.CallbackContext obj)
    {
        OnHotbarSlot4Performed?.Invoke();
    }

    private void HotbarSlot3Action_Performed(InputAction.CallbackContext obj)
    {
        OnHotbarSlot3Performed?.Invoke();
    }

    private void HotbarSlot2Action_Performed(InputAction.CallbackContext obj)
    {
        OnHotbarSlot2Performed?.Invoke();
    }

    private void HotbarSlot1Action_Performed(InputAction.CallbackContext obj)
    {
        OnHotbarSlot1Performed?.Invoke();
    }

    private void SetupInputSubscriptions()
    {
        _moveAction.performed += MoveAction_Performed;
        _moveAction.canceled += MoveAction_Canceled;
        _interactAction.performed += InteractAction_Performed;
        _openMenuAction.performed += OpenMenuAction_Performed;
        _closeMenuAction.performed += CloseMenuAction_Performed;

        _hotbarSlot1Action.performed += HotbarSlot1Action_Performed;
        _hotbarSlot2Action.performed += HotbarSlot2Action_Performed;
        _hotbarSlot3Action.performed += HotbarSlot3Action_Performed;
        _hotbarSlot4Action.performed += HotbarSlot4Action_Performed;
        _hotbarSlot5Action.performed += HotbarSlot5Action_Performed;
        _hotbarSlot6Action.performed += HotbarSlot6Action_Performed;
        _hotbarSlot7Action.performed += HotbarSlot7Action_Performed;
        _hotbarSlot8Action.performed += HotbarSlot8Action_Performed;
        _hotbarSlot9Action.performed += HotbarSlot9Action_Performed;
        _hotbarSlot10Action.performed += HotbarSlot10Action_Performed;
    }

    private void UnsubscribeAllInput()
    {
        _moveAction.performed -= MoveAction_Performed;
        _moveAction.canceled -= MoveAction_Canceled;
        _interactAction.performed -= InteractAction_Performed;
        _openMenuAction.performed -= OpenMenuAction_Performed;
        _closeMenuAction.performed -= CloseMenuAction_Performed;

        _hotbarSlot1Action.performed -= HotbarSlot1Action_Performed;
        _hotbarSlot2Action.performed -= HotbarSlot2Action_Performed;
        _hotbarSlot3Action.performed -= HotbarSlot3Action_Performed;
        _hotbarSlot4Action.performed -= HotbarSlot4Action_Performed;
        _hotbarSlot5Action.performed -= HotbarSlot5Action_Performed;
        _hotbarSlot6Action.performed -= HotbarSlot6Action_Performed;
        _hotbarSlot7Action.performed -= HotbarSlot7Action_Performed;
        _hotbarSlot8Action.performed -= HotbarSlot8Action_Performed;
        _hotbarSlot9Action.performed -= HotbarSlot9Action_Performed;
        _hotbarSlot10Action.performed -= HotbarSlot10Action_Performed;
    }
}