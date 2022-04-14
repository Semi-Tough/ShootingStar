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
    [SerializeField] private bool playerProjectile;

    [SerializeField] private GameObject hitVFX;
    [SerializeField] private float damage = 1f;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] protected Vector3 moveDirection;
    private Vector3 lastPosition;
    private Vector3 nextPosition;

    protected virtual void Update()
    {
        if (!gameObject.activeSelf) return;
        lastPosition = transform.position;
        transform.Translate(moveDirection * (moveSpeed * Time.deltaTime));
        // ReSharper disable once Unity.InefficientPropertyAccess
        nextPosition = transform.position;


        RaycastHit2D hit2D = Physics2D.BoxCast(nextPosition, new Vector2(0.2f, 0.1f), 0,
            (nextPosition - lastPosition).normalized, 0.1f);

        if (!hit2D) return;
        if (playerProjectile)
        {
            if (hit2D.collider.CompareTag("Enemy"))
            {
                if (!hit2D.transform.TryGetComponent(out Controller controller)) return;
                controller.TakeDamage(damage);
                PoolManager.Release(hitVFX, hit2D.point, Quaternion.LookRotation(hit2D.normal));
                gameObject.SetActive(false);
            }
        }
        else
        {
            if (hit2D.collider.CompareTag("Player"))
            {
                if (!hit2D.transform.TryGetComponent(out Controller controller)) return;
                controller.TakeDamage(damage);
                PoolManager.Release(hitVFX, hit2D.point, Quaternion.LookRotation(hit2D.normal));
                gameObject.SetActive(false);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawCube(nextPosition, new Vector2(0.2f, 0.1f));
    }
    // private void OnCollisionEnter2D(Collision2D other)
    // {
    //     if (!other.gameObject.TryGetComponent(out Controller controller)) return;
    //     controller.TakeDamage(damage);
    //     ContactPoint2D point2D = other.GetContact(0);
    //     PoolManager.Release(hitVFX, point2D.point, Quaternion.LookRotation(point2D.normal));
    //     gameObject.SetActive(false);
    // }
}