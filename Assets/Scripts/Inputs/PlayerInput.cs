using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Player Input")]
public class PlayerInput : ScriptableObject, InputActions.IGamePlayActions
{
    public event UnityAction<Vector2> StartMove = delegate { };
    public event UnityAction StopMove = delegate { };

    private InputActions _inputActions;

    private void OnEnable()
    {
        _inputActions = new InputActions();
        _inputActions.GamePlay.SetCallbacks(this);
    }

    private void OnDisable()
    {
        DisableInput();
    }

    private void DisableInput()
    {
        _inputActions.GamePlay.Disable();
    }

    public void EnableGamePlayInput()
    {
        _inputActions.GamePlay.Enable();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            StartMove.Invoke(context.ReadValue<Vector2>());
        }

        if (context.phase == InputActionPhase.Canceled)
        {
            StopMove.Invoke();
        }
    }
}
