/****************************************************
    文件：Controller.cs
    作者：wzq
    邮箱：1693416984@qq.com
    日期：2022/04/12 17:53:33
    功能：控制器基类
*****************************************************/

using System.Collections;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [Header("--------VFX--------")]
    [SerializeField] protected GameObject deathVFX;

    [Header("--------Health--------")]
    [SerializeField] protected float maxHealth;

    private float health;

    protected virtual void OnEnable()
    {
        health = maxHealth;
    }

    protected virtual void TakeDamage(float value)
    {
        health -= value;
        if (health < 0)
        {
            Die();
        }
    }

    protected virtual void RestoreHealth(float value)
    {
        health += value;
        if (health >= maxHealth)
        {
            health = maxHealth;
        }

        //health = Mathf.Clamp(health += value, 0, maxHealth);
    }

    protected virtual void Die()
    {
        health = 0f;
        PoolManager.Release(deathVFX, transform.position);
        gameObject.SetActive(false);
    }

    protected IEnumerator HealthRegenerateCoroutine(WaitForSeconds waitForHealth, float percent)
    {
        while (health < maxHealth)
        {
            yield return waitForHealth;
            RestoreHealth(maxHealth * percent);
        }
    }

    protected IEnumerator DamageOverTimeCoroutine(WaitForSeconds waitForDamage, float percent)
    {
        while (health > 0)
        {
            yield return waitForDamage;
            TakeDamage(maxHealth * percent);
        }
        // ReSharper disable once IteratorNeverReturns
    }
}