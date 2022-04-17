/****************************************************
    文件：ScoreManager.cs
    作者：wzq
    邮箱：1693416984@qq.com
    日期：2022/04/17 19:04:28
    功能：得分管理
*****************************************************/

using System.Collections;
using UnityEngine;

public class ScoreManager : PersistentSingleton<ScoreManager>
{
    [SerializeField] private Vector3 txtScale = new Vector3(1.2f, 1.2f, 1f);
    private Coroutine addScoreCoroutine;
    private int score;
    private int currentScore;

    private void Start()
    {
        ResetScore();
    }

    private void ResetScore()
    {
        currentScore = score = 0;
        ScoreDisplay.UpdateScore(score);
    }

    public void AddScore(int scoreValue)
    {
        currentScore += scoreValue;
        ScoreDisplay.UpdateScore(score);
        if (addScoreCoroutine != null) return;
        addScoreCoroutine = StartCoroutine(AddScoreCoroutine());
    }

    private IEnumerator AddScoreCoroutine()
    {
        ScoreDisplay.TextScale(txtScale);
        while (score < currentScore)
        {
            score += 1;
            ScoreDisplay.UpdateScore(score);
            yield return null;
        }

        ScoreDisplay.TextScale(Vector3.one);
        addScoreCoroutine = null;
    }
}