/****************************************************
    文件：StatsBar.cs
    作者：wzq
    邮箱：1693416984@qq.com
    日期：2022/04/13 19:09:58
    功能：状态栏
*****************************************************/

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StatsBar : MonoBehaviour
{
    [SerializeField] private Image imgBackFill;
    [SerializeField] private Image imgFrontFill;
    [SerializeField] private float fillSpeed = 0.1f;
    [SerializeField] private float fillDelay = 0.5f;
    [SerializeField] private bool delayFill = true;

    private WaitForSeconds waitForSeconds;
    private Coroutine fillCoroutine;
    private Canvas canvas;
    private float currentFillAmount;
    private float targetFillAmount;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        waitForSeconds = new WaitForSeconds(fillDelay);
    }


    public void Initialize(float currentValue, float maxValue)
    {
        currentFillAmount = currentValue / maxValue;
        targetFillAmount = currentFillAmount;
        imgFrontFill.fillAmount = currentValue;
        imgBackFill.fillAmount = currentValue;
    }

    public void UpdateStats(float currentValue, float maxValue)
    {
        if (gameObject.activeSelf == false) return;
        targetFillAmount = currentValue / maxValue;
        if (fillCoroutine != null)
        {
            StopCoroutine(fillCoroutine);
        }

        if (currentFillAmount > targetFillAmount)
        {
            imgFrontFill.fillAmount = targetFillAmount;
            fillCoroutine = StartCoroutine(FillCoroutine(imgBackFill));
        }

        if (currentFillAmount < targetFillAmount)
        {
            imgBackFill.fillAmount = targetFillAmount;
            fillCoroutine = StartCoroutine(FillCoroutine(imgFrontFill));
        }
    }

    IEnumerator FillCoroutine(Image image)
    {
        if (delayFill) yield return waitForSeconds;

        float progress = 0;
        while (progress < 1)
        {
            progress += Time.deltaTime * fillSpeed;
            currentFillAmount = Mathf.Lerp(currentFillAmount, targetFillAmount, progress);
            image.fillAmount = currentFillAmount;
            yield return null;
        }
    }
}