/****************************************************
    文件：EnemyProjectileAiming.cs
    作者：wzq
    邮箱：1693416984@qq.com
    日期：2022/04/12 19:32:38
    功能：敌人的瞄准子弹
*****************************************************/

using System.Collections;
using UnityEngine;

public class EnemyProjectileAiming : Projectile
{
    private GameObject target;

    private void Awake()
    {
        target = GameObject.FindWithTag("Player");
    }

    private void OnEnable()
    {
        StartCoroutine(DirectionCoroutine());
    }

    IEnumerator DirectionCoroutine()
    {
        yield return null;
        moveDirection = (target.transform.position - transform.position).normalized;
    }
}