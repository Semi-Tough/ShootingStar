/****************************************************
    文件：TimeManager.cs
    作者：wzq
    邮箱：1693416984@qq.com
    日期：2022/04/25 10:19:11
    功能：时间管理器
*****************************************************/

using System;
using System.Collections;
using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
    [SerializeField, Range(0, 2)] private float bulletTimeScale = 0.1f;
    private float defaultFixedDeltaTime;

    protected override void Awake()
    {
        base.Awake();
        defaultFixedDeltaTime = Time.fixedDeltaTime;
    }

    public void BulletTime(float duration, float inTime = 0f, float outTime = 0f)
    {
        StartCoroutine(BulletTimeCoroutine(duration, inTime, outTime));
    }

    private IEnumerator BulletTimeCoroutine(float duration, float inTime = 0f, float outTime = 0f)
    {
        float timer = 0f;
        while (timer < 1f)
        {
            if (inTime <= 0)
            {
                Time.timeScale = bulletTimeScale;
                Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;
                break;
            }

            timer += Time.unscaledDeltaTime / inTime;
            Time.timeScale = Mathf.Lerp(1f, bulletTimeScale, timer);
            Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;
            yield return null;
        }

        yield return new WaitForSeconds(duration * bulletTimeScale);

        timer = 0f;
        while (timer < 1f)
        {
            if (inTime <= 0)
            {
                Time.timeScale = 1f;
                Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;
                break;
            }

            timer += Time.unscaledDeltaTime / outTime;
            Time.timeScale = Mathf.Lerp(bulletTimeScale, 1f, timer);
            Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;
            yield return null;
        }

        if (Math.Abs(Time.timeScale - 1f) > 0)
        {
            Time.timeScale = 1;
        }
    }
}