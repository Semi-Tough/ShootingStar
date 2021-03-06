/****************************************************
    文件：SceneLoader.cs
    作者：wzq
    邮箱：1693416984@qq.com
    日期：2022/04/17 17:24:15
    功能：场景加载器
*****************************************************/

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : PersistentSingleton<SceneLoader>
{
    private const string GamePlay = "GamePlay";
    private const string MainMenu = "MainMenu";
    [SerializeField] private Image imgTransition;
    [SerializeField] private float fadeTime = 1.5f;
    private Color color;


    private IEnumerator LoadingCoroutine(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        imgTransition.gameObject.SetActive(true);
        while (color.a < 1)
        {
            color.a = Mathf.Clamp01(color.a + Time.unscaledDeltaTime / fadeTime);
            imgTransition.color = color;
            yield return null;
        }

        yield return new WaitUntil(() => asyncLoad.progress >= 0.9f);
        asyncLoad.allowSceneActivation = true;

        while (color.a > 0)
        {
            color.a = Mathf.Clamp01(color.a - Time.unscaledDeltaTime / fadeTime);
            imgTransition.color = color;
            yield return null;
        }

        imgTransition.gameObject.SetActive(false);
    }

    public void LoadGamePlayScene()
    {
        StopAllCoroutines();
        StartCoroutine(LoadingCoroutine(GamePlay));
    }

    public void LoadMainMenuScene()
    {
        StopAllCoroutines();
        StartCoroutine(LoadingCoroutine(MainMenu));
    }
}