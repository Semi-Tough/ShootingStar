/****************************************************
    文件：EnemyProjectile.cs
    作者：wzq
    邮箱：1693416984@qq.com
    日期：2022/04/12 17:15:59
    功能：敌人子弹类
*****************************************************/

using UnityEngine;

public class EnemyProjectile : Projectile
{
    protected override void Awake()
    {
        base.Awake();
        if (moveDirection != Vector3.left)
        {
            transform.rotation = Quaternion.FromToRotation(Vector3.left, moveDirection);
        }
    }

}