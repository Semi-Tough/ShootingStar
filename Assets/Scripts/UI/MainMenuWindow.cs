/****************************************************
    文件：MainMenuWindow.cs
    作者：wzq
    邮箱：1693416984@qq.com
    日期：2022/04/26 10:42:37
    功能：主菜单窗口
*****************************************************/
using UnityEngine;

public class MainMenuWindow : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 1;
        GameManager.GameState = GameState.Playing;
    }

    public void ClickStart()
    {
        SceneLoader.Instance.LoadGamePlayScene();
    }
}
