/****************************************************
    文件：EnemyProjectileAiming.cs
    作者：wzq
    邮箱：1693416984@qq.com
    日期：2022/04/12 19:32:38
    功能：敌人的瞄准子弹
*****************************************************/

using UnityEngine;

public class EnemyProjectileAiming : EnemyProjectile
{
    protected override void OnEnable()
    {
        base.OnEnable();
        SetTarget(GameObject.FindWithTag("Player"));
        if (Target == null) return;
        moveDirection = (Target.transform.position - transform.position).normalized;
    }
}