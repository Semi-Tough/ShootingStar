/****************************************************
    文件：PlayerProjectileTracking.cs
    作者：wzq
    邮箱：1693416984@qq.com
    日期：2022/04/19 14:41:21
    功能：玩家追踪子弹
*****************************************************/

using UnityEngine;

public class PlayerProjectileTracking : PlayerProjectile
{
    [SerializeField] private ProjectileGuidingSystem projectileGuidingSystem;

    protected override void OnEnable()
    {
        base.OnEnable();
        SetTarget(EnemyManager.Instance.RandomEnemy);
        if (Target == null) return;
        StartCoroutine(projectileGuidingSystem.GuidingCoroutine(Target));
    }
}