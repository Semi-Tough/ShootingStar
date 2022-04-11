/****************************************************
    文件：Singleton.cs
    作者：wzq
    邮箱：1693416984@qq.com
    日期：2022/04/11 12:06:03
    功能：单例基类
*****************************************************/
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        Instance = this as T;
    }
}
