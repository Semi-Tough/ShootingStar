/****************************************************
    文件：ScoreDisplay.cs
    作者：wzq
    邮箱：1693416984@qq.com
    日期：2022/04/17 19:00:46
    功能：得分显示
*****************************************************/

using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    private static Text _txtScore;

    private void Awake()
    {
        _txtScore = GetComponent<Text>();
    }

    public static void UpdateScore(int score)
    {
        _txtScore.text = score.ToString();
    }

    public static void TextScale(Vector3 targetScale)
    {
        _txtScore.rectTransform.localScale = targetScale;
    }
}