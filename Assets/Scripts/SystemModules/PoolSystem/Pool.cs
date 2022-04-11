/****************************************************
    文件：Pool.cs
    作者：wzq
    邮箱：1693416984@qq.com
    日期：2022/04/11 12:47:07
    功能：对象池
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class Pool
{
    public GameObject Prefab => prefab;

    [SerializeField] private GameObject prefab;
    [SerializeField] private int size = 1;

    private Queue<GameObject> queue;
    private Transform parent;

    public void Initialize(Transform poolParent)
    {
        queue = new Queue<GameObject>();
        for (int i = 0; i < size; i++)
        {
            parent = poolParent;
            queue.Enqueue(Copy());
        }
    }

    private GameObject Copy()
    {
        GameObject go = Object.Instantiate(prefab, parent);
        go.SetActive(false);
        return go;
    }

    private GameObject AvailableGameObject()
    {
        GameObject availableGameObject;
        if (queue.Count > 0 && !queue.Peek().activeSelf)
        {
            availableGameObject = queue.Dequeue();
        }
        else
        {
            availableGameObject = Copy();
            queue.Enqueue(availableGameObject);
        }

        queue.Enqueue(availableGameObject);

        return availableGameObject;
    }

    public GameObject PreparedObject()
    {
        GameObject preparedObject = AvailableGameObject();
        preparedObject.SetActive(true);
        return preparedObject;
    }

    public GameObject PreparedObject(Vector3 position)
    {
        GameObject preparedObject = AvailableGameObject();
        preparedObject.transform.position = position;
        preparedObject.SetActive(true);
        return preparedObject;
    }

    public GameObject PreparedObject(Vector3 position, Quaternion rotation)
    {
        GameObject preparedObject = AvailableGameObject();
        preparedObject.transform.position = position;
        preparedObject.transform.rotation = rotation;
        preparedObject.SetActive(true);
        return preparedObject;
    }

    public GameObject PreparedObject(Vector3 position, Quaternion rotation, Vector3 localScale)
    {
        GameObject preparedObject = AvailableGameObject();
        preparedObject.transform.position = position;
        preparedObject.transform.rotation = rotation;
        preparedObject.transform.localScale = localScale;
        preparedObject.SetActive(true);
        return preparedObject;
    }
}