using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private const string IS_WALKING_ANIM_NAME = "isWalking";
    private const string INPUT_X_ANIM_NAME = "inputX";
    private const string INPUT_Y_ANIM_NAME = "inputY";
    private const string LAST_INPUT_X_ANIM_NAME = "lastInputX";
    private const string LAST_INPUT_Y_ANIM_NAME = "lastInputY";

    [SerializeField] private float _moveSpeed;

    private Rigidbody2D _rb;
    private Animator _anim;

    private Vector2 _moveInput;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        PlayerInputController.OnMoveActionPressed += MoveAction_Performed;
        PlayerInputController.OnMoveActionCanceled += MoveAction_Canceled;
    }

    private void Update()
    {
        _rb.linearVelocity = _moveInput * _moveSpeed;
    }

    private void OnDisable()
    {
        PlayerInputController.OnMoveActionPressed -= MoveAction_Performed;
        PlayerInputController.OnMoveActionCanceled -= MoveAction_Canceled;
    }

    private void MoveAction_Performed(InputAction.CallbackContext context)
    {
        _anim.SetBool(IS_WALKING_ANIM_NAME, true);
        _moveInput = context.ReadValue<Vector2>();
        _anim.SetFloat(INPUT_X_ANIM_NAME, _moveInput.x);
        _anim.SetFloat(INPUT_Y_ANIM_NAME, _moveInput.y);
    }

    private void MoveAction_Canceled(InputAction.CallbackContext context)
    {
        _anim.SetBool(IS_WALKING_ANIM_NAME, false);
        _anim.SetFloat(LAST_INPUT_X_ANIM_NAME, _moveInput.x);
        _anim.SetFloat(LAST_INPUT_Y_ANIM_NAME, _moveInput.y);
        _moveInput = Vector2.zero;
        _anim.SetFloat(INPUT_X_ANIM_NAME, _moveInput.x);
        _anim.SetFloat(INPUT_Y_ANIM_NAME, _moveInput.y);
    }
}