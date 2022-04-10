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