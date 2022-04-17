/****************************************************
    文件：PersistentSingleton.cs
    作者：wzq
    邮箱：1693416984@qq.com
    日期：2022/04/16 20:18:50
    功能：持久化单例
*****************************************************/

using UnityEngine;

public class PersistentSingleton<T> : MonoBehaviour where T : Component
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;
        }
        else if (Instance != null)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
}