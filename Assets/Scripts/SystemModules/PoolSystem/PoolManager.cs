/****************************************************
    文件：PoolManager.cs
    作者：wzq
    邮箱：1693416984@qq.com
    日期：2022/04/11 13:04:05
    功能：对象池管理器
*****************************************************/

using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] private Pool[] playerProjectilePools;
    [SerializeField] private Pool[] enemyProjectilePools;
    [SerializeField] private Pool[] hitVFXPools;
    [SerializeField] private Pool[] deathVFXPools;
    private static Dictionary<GameObject, Pool> _dictionary;

    private void Awake()
    {
        _dictionary = new Dictionary<GameObject, Pool>();
    }

    private void Start()
    {
        Initialized(playerProjectilePools);
        Initialized(enemyProjectilePools);
        Initialized(hitVFXPools);
        Initialized(deathVFXPools);
    }

    private void Initialized(Pool[] pools)
    {
        foreach (Pool pool in pools)
        {
#if UNITY_EDITOR
            if (_dictionary.ContainsKey(pool.Prefab))
            {
                Debug.LogError("Same Prefab in Multiple pools! Prefab:" + pool.Prefab.name);
            }
#endif
            _dictionary.Add(pool.Prefab, pool);

            Transform poolParent = new GameObject("Pool:" + pool.Prefab.name).transform;
            pool.Initialize(poolParent);
        }
    }

#if UNITY_EDITOR

    private void CheckPoolSize(Pool[] pools)
    {
        foreach (var pool in pools)
        {
            if (pool.RunTimeSize > pool.Size)
            {
                Debug.LogWarning($"Pool:{pool.Prefab.name} " +
                                 $" has a runtime size {pool.RunTimeSize.ToString()}" +
                                 $" bigger than its initial size {pool.Size.ToString()}");
            }
        }
    }

    private void OnDestroy()
    {
        CheckPoolSize(playerProjectilePools);
        CheckPoolSize(enemyProjectilePools);
        CheckPoolSize(hitVFXPools);
        CheckPoolSize(deathVFXPools);
    }
#endif

    #region Release 重载

    #region XML注释

    /// <summary>
    /// <para>Return a specified<paramref name="prefab"/>gameObject in pool</para>
    /// <para>根据传入的<paramref name="prefab"/>参数,返回对象池中预备好的游戏对象</para>
    /// </summary>
    /// <param name="prefab"></param>
    /// <para>Specified gameObject in the pool</para>
    /// <para>对象池中预备好的游戏对象</para>
    /// <returns></returns>

    #endregion

    public static GameObject Release(GameObject prefab)
    {
#if UNITY_EDITOR
        if (!_dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager could not find prefab:" + prefab.name);
            return null;
        }
#endif
        return _dictionary[prefab].PreparedObject();
    }

    #region XML注释

    /// <summary>
    /// <para>Return a specified<paramref name="prefab"/>gameObject in pool</para>
    /// <para>根据传入的<paramref name="prefab"/>参数,返回对象池中预备好的游戏对象</para>
    /// </summary>
    /// <param name="prefab"></param>
    /// <para>Specified gameObject in the pool</para>
    /// <para>对象池中预备好的游戏对象</para>
    /// <param name="position"></param>
    /// <para>specified release position</para>
    /// <para>指定释放位置</para>
    /// <returns></returns>

    #endregion

    public static GameObject Release(GameObject prefab, Vector3 position)
    {
#if UNITY_EDITOR
        if (!_dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager could not find prefab:" + prefab.name);
            return null;
        }
#endif
        return _dictionary[prefab].PreparedObject(position);
    }

    #region XMl注释

    /// <summary>
    /// <para>Return a specified<paramref name="prefab"/>gameObject in pool</para>
    /// <para>根据传入的<paramref name="prefab"/>参数,返回对象池中预备好的游戏对象</para>
    /// </summary>
    /// <param name="prefab"></param>
    /// <para>Specified gameObject in the pool</para>
    /// <para>对象池中预备好的游戏对象</para>
    /// <param name="position"></param>
    /// <para>specified release position</para>
    /// <para>指定释放位置</para>
    /// <param name="rotation"></param>
    /// <para>specified release rotation</para>
    /// <para>指定旋转值</para>
    /// <returns></returns>

    #endregion

    public static GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation)
    {
#if UNITY_EDITOR
        if (!_dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager could not find prefab:" + prefab.name);
            return null;
        }
#endif
        return _dictionary[prefab].PreparedObject(position, rotation);
    }

    #region XML注释

    /// <summary>
    /// <para>Return a specified<paramref name="prefab"/>gameObject in pool</para>
    /// <para>根据传入的<paramref name="prefab"/>参数,返回对象池中预备好的游戏对象</para>
    /// </summary>
    /// <param name="prefab"></param>
    /// <para>Specified gameObject in the pool</para>
    /// <para>对象池中预备好的游戏对象</para>
    /// <param name="position"></param>
    /// <para>specified release position</para>
    /// <para>指定释放位置</para>
    /// <param name="rotation"></param>
    /// <para>specified release rotation</para>
    /// <para>指定旋转值</para>
    /// <param name="localScale"></param>
    /// <para>specified Scale</para>
    /// <para>指定缩放值</para>
    /// <returns></returns>

    #endregion

    public static GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation, Vector3 localScale)
    {
#if UNITY_EDITOR
        if (!_dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager could not find prefab:" + prefab.name);
            return null;
        }
#endif
        return _dictionary[prefab].PreparedObject(position, rotation, localScale);
    }

    #endregion
}