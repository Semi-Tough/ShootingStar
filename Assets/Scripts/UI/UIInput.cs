/****************************************************
    文件：UIInput.cs
    作者：wzq
    邮箱：1693416984@qq.com
    日期：2022/04/26 11:18:56
    功能：UI输入模块
*****************************************************/

using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class UIInput : Singleton<UIInput>
{
    [SerializeField] private PlayerInput playerInput;
    private InputSystemUIInputModule uiInputModule;

    protected override void Awake()
    {
        base.Awake();
        uiInputModule = GetComponent<InputSystemUIInputModule>();
        uiInputModule.enabled = false;
    }

    public void SelectUI(Selectable uiObject)
    {
        uiObject.Select();
        uiObject.OnSelect(null);
        uiInputModule.enabled = true;
    }

    public void DisableAllInput()
    {
        playerInput.DisableAllInputs();
        uiInputModule.enabled = false;
    }
    
}