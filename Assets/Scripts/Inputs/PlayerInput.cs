/****************************************************
    文件：PlayerInput.cs
    作者：wzq
    邮箱：1693416984@qq.com
    日期：2022/04/10 17:40:10
    功能：玩家输入控制
*****************************************************/

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Player Input")]
public class PlayerInput : ScriptableObject, InputActions.IGamePlayActions
{
    public event UnityAction<Vector2> StartMoveAction = delegate { };
    public event UnityAction StopMoveAction = delegate { };
    public event UnityAction StartFireAction = delegate { };
    public event UnityAction StopFireAction = delegate { };
    public event UnityAction DodgeAction = delegate { };
    public event UnityAction OverdriveAction = delegate { };

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

    public void EnableGamePlayInput()
    {
        _inputActions.GamePlay.Enable();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void DisableInput()
    {
        _inputActions.GamePlay.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            StartMoveAction.Invoke(context.ReadValue<Vector2>());
        }

        if (context.canceled)
        {
            StopMoveAction.Invoke();
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            StartFireAction.Invoke();
        }

        if (context.canceled)
        {
            StopFireAction.Invoke();
        }
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            DodgeAction.Invoke();
        }
    }

    public void OnOverdrive(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OverdriveAction.Invoke();
        }
    }

  
}