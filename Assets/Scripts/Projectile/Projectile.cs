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
    [SerializeField] private GameObject hitVFX;
    [SerializeField] private float damage = 1f;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] protected Vector3 moveDirection;


    protected virtual void Update()
    {
        if (gameObject.activeSelf)
        {
            transform.Translate(moveDirection * (moveSpeed * Time.deltaTime));
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent(out Controller controller))
        {
            controller.TakeDamage(damage);
            ContactPoint2D point2D = other.GetContact(0);
            PoolManager.Release(hitVFX, point2D.point, Quaternion.LookRotation(point2D.normal));
            gameObject.SetActive(false);
        }
    }
}