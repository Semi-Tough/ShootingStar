/****************************************************
    文件：PoolManager.cs
    作者：wzq
    邮箱：1693416984@qq.com
    日期：2022/04/11 13:04:05
    功能：对象池管理器
*****************************************************/

using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] private Pool[] playerProjectilePools;

    private void Start()
    {
        Initialized(playerProjectilePools);
    }

    private void Initialized(Pool[] pools)
    {
        foreach (Pool pool in pools)
        {
            Transform poolParent = new GameObject("Pool:" + pool.Prefab.name).transform;
            pool.Initialize(poolParent);
        }
    }
}