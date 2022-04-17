/****************************************************
    文件：WaveUI.cs
    作者：wzq
    邮箱：1693416984@qq.com
    日期：2022/04/16 19:24:37
    功能：波数UI
*****************************************************/

using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{
    private Text txtWave;


    private void Awake()
    {
        txtWave = GetComponentInChildren<Text>();
    }

    private void OnEnable()
    {
        txtWave.text = "-WAVE " + EnemyManager.Instance.WaveNumber + "-";
    }
}