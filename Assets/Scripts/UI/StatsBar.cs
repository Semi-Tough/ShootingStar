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
    protected float TargetFillAmount;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        waitForSeconds = new WaitForSeconds(fillDelay);
    }

    public virtual void Initialize(float currentHealth, float maxHealth)
    {
        currentFillAmount = currentHealth / maxHealth;
        TargetFillAmount = currentFillAmount;
        imgFrontFill.fillAmount = currentHealth;
        imgBackFill.fillAmount = currentHealth;
    }

    public void UpdateStats(float currentHealth, float maxHealth)
    {
        if (gameObject.activeSelf == false) return;
        TargetFillAmount = currentHealth / maxHealth;
        if (fillCoroutine != null)
        {
            StopCoroutine(fillCoroutine);
        }

        if (currentFillAmount > TargetFillAmount)
        {
            imgFrontFill.fillAmount = TargetFillAmount;
            fillCoroutine = StartCoroutine(FillCoroutine(imgBackFill));
        }
        else if (currentFillAmount < TargetFillAmount)
        {
            imgBackFill.fillAmount = TargetFillAmount;
            fillCoroutine = StartCoroutine(FillCoroutine(imgFrontFill));
        }
    }

    protected virtual IEnumerator FillCoroutine(Image image)
    {
        if (delayFill) yield return waitForSeconds;

        float progress = 0;
        while (progress < 1)
        {
            progress += Time.deltaTime * fillSpeed;
            currentFillAmount = Mathf.Lerp(currentFillAmount, TargetFillAmount, progress);
            image.fillAmount = currentFillAmount;
            yield return null;
        }
    }
}