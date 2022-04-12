/****************************************************
    文件：Projectile.cs
    作者：wzq
    邮箱：1693416984@qq.com
    日期：2022/04/11 12:05:39
    功能：子弹基类
*****************************************************/

using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] protected Vector3 moveDirection;


    protected virtual void Update()
    {
        if (gameObject.activeSelf)
        {
            transform.Translate(moveDirection * (moveSpeed * Time.deltaTime));
        }
    }
}