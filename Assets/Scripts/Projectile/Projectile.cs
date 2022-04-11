/****************************************************
    文件：Projectile.cs
    作者：wzq
    邮箱：1693416984@qq.com
    日期：2022/04/11 12:05:39
    功能：玩家普通子弹
*****************************************************/
using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private Vector3 moveDirection;

    private void OnEnable()
    {
        StartCoroutine(MoveDirectly());
    }

    IEnumerator MoveDirectly()
    {
        while (gameObject.activeSelf)
        {
            transform.Translate(moveDirection * (moveSpeed * Time.deltaTime));
            yield return null;
        }
    }
}