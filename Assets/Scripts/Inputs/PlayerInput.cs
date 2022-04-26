/****************************************************
    文件：PlayerInput.cs
    作者：wzq
    邮箱：1693416984@qq.com
    日期：2022/04/10 17:40:10
    功能：玩家输入控制
*****************************************************/

using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Player Input")]
public class PlayerInput : ScriptableObject, InputActions.IGamePlayActions, InputActions.IPauseMenuActions
{
    public event UnityAction<Vector2> StartMoveAction = delegate { };
    public event UnityAction StopMoveAction = delegate { };
    public event UnityAction StartFireAction = delegate { };
    public event UnityAction StopFireAction = delegate { };
    public event UnityAction DodgeAction = delegate { };
    public event UnityAction OverdriveAction = delegate { };
    public event UnityAction StartPauseAction = delegate { };
    public event UnityAction StopPauseAction = delegate { };

    private InputActions inputActions;

    private void OnEnable()
    {
        inputActions = new InputActions();
        inputActions.GamePlay.SetCallbacks(this);
        inputActions.PauseMenu.SetCallbacks(this);
    }

    private void OnDisable()
    {
        DisableAllInputs();
    }

    private void SwitchToDynamicUpdateMode()
    {
        InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
    }

    private void SwitchToFixedUpdateMode()
    {
        InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;
    }

    private void SwitchActionMap([NotNull] InputActionMap actionMap, bool isUIInput)
    {
        if (actionMap == null) throw new ArgumentNullException(nameof(actionMap));
        inputActions.Disable();
        actionMap.Enable();

        if (isUIInput)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void DisableAllInputs()
    {
        inputActions.Disable();
    }


    #region GamePlay

    public void EnableGamePlayInput()
    {
        SwitchActionMap(inputActions.GamePlay, false);
        SwitchToFixedUpdateMode();
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

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            StartPauseAction.Invoke();
        }
    }

    #endregion


    #region PauseMenu

    public void EnablePauseMenuInput()
    {
        SwitchActionMap(inputActions.PauseMenu, true);
        SwitchToDynamicUpdateMode();
    }

    public void OnUnpause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            StopPauseAction.Invoke();
        }
    }

    #endregion
}