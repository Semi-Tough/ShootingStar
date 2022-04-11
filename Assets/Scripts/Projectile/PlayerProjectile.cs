/****************************************************
    文件：PlayerProjectile.cs
    作者：wzq
    邮箱：1693416984@qq.com
    日期：2022/04/11 12:05:08
    功能：玩家子弹基类
*****************************************************/

using UnityEngine;

public class PlayerProjectile : Projectile
{
    [SerializeField] private TrailRenderer trailRenderer;

    private void OnDisable()
    {
        trailRenderer.Clear();
    }
}