/****************************************************
    文件：PlayerProjectile.cs
    作者：wzq
    邮箱：1693416984@qq.com
    日期：2022/04/11 12:05:08
    功能：玩家子弹类
*****************************************************/

using UnityEngine;

public class PlayerProjectile : Projectile
{
    private TrailRenderer trailRenderer;

    private void Awake()
    {
        trailRenderer = GetComponentInChildren<TrailRenderer>();
    }

    private void OnDisable()
    {
        trailRenderer.Clear();
    }
}