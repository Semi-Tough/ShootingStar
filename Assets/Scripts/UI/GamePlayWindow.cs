/****************************************************
    文件：GamePlayWindow.cs
    作者：wzq
    邮箱：1693416984@qq.com
    日期：2022/04/25 19:43:01
    功能：游戏窗口UI
*****************************************************/

using UnityEngine;
using UnityEngine.UI;

public class GamePlayWindow : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private GameObject menusUI;
    [SerializeField] private Button btnResume;
    [SerializeField] private Button btnOptions;
    [SerializeField] private Button btnMainMenu;

    [SerializeField] private AudioData pauseSfx;
    [SerializeField] private AudioData unPauseSfx;
    private static readonly int Pressed = Animator.StringToHash("Pressed");

    private void OnEnable()
    {
        playerInput.StartPauseAction += Pause;
        playerInput.StopPauseAction += UnPause;

        ButtonPressedBehavior.ButtonFunctionDic.Add(btnResume.gameObject.name, ClickResume);
        ButtonPressedBehavior.ButtonFunctionDic.Add(btnOptions.gameObject.name, ClickOptions);
        ButtonPressedBehavior.ButtonFunctionDic.Add(btnMainMenu.gameObject.name, ClickMainMenu);
    }


    private void OnDisable()
    {
        playerInput.StartPauseAction -= Pause;
        playerInput.StopPauseAction -= UnPause;

        ButtonPressedBehavior.ButtonFunctionDic.Clear();
    }

    private void Pause()
    {
        AudioManager.Instance.PlaySfx(pauseSfx);
        menusUI.SetActive(true);
        playerInput.EnablePauseMenuInput();
        UIInput.Instance.SelectUI(btnResume);
        TimeManager.Instance.Pause();
        GameManager.GameState = GameState.Pause;
    }

    private void UnPause()
    {
        AudioManager.Instance.PlaySfx(unPauseSfx);
        UIInput.Instance.SelectUI(btnResume);
        btnResume.animator.SetTrigger(Pressed);
        TimeManager.Instance.Unpause();
        GameManager.GameState = GameState.Playing;
    }

    #region ClickEvents

    private void ClickResume()
    {
        Time.timeScale = 1;
        menusUI.SetActive(false);
        playerInput.EnableGamePlayInput();
    }

    private void ClickOptions()
    {
        //TODO 
        UIInput.Instance.SelectUI(btnOptions);
        playerInput.EnablePauseMenuInput();
    }

    private void ClickMainMenu()
    {
        menusUI.SetActive(false);
        SceneLoader.Instance.LoadMainMenuScene();
    }

    #endregion
}