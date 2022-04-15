/****************************************************
    文件：StatsBarHUD.cs
    作者：wzq
    邮箱：1693416984@qq.com
    日期：2022/04/14 11:42:11
    功能：屏幕状态栏
*****************************************************/

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StatsBarHUD : StatsBar
{
    [SerializeField] private Text txtPercent;

    private void SetTxtPercent()
    {
        txtPercent.text = Mathf.RoundToInt(TargetFillAmount * 100) + "%";
    }

    public override void Initialize(float currentValue, float maxHealth)
    {
        base.Initialize(currentValue, maxHealth);
        SetTxtPercent();
    }

    protected override IEnumerator FillCoroutine(Image image)
    {
        SetTxtPercent();
        return base.FillCoroutine(image);
    }
}