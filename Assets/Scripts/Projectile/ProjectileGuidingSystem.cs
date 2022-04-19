/****************************************************
    文件：ProjectileGuidingSystem.cs
    作者：wzq
    邮箱：1693416984@qq.com
    日期：2022/04/19 15:58:10
    功能：子弹制导系统
*****************************************************/

using System.Collections;
using UnityEngine;

public class ProjectileGuidingSystem : MonoBehaviour
{
    [SerializeField] private float minBallisticAngle = -90f;
    [SerializeField] private float maxBallisticAngle = 90f;

    private Vector3 targetDir;
    private float ballisticAngle;

    public IEnumerator GuidingCoroutine(GameObject target)
    {
        ballisticAngle = Random.Range(minBallisticAngle, maxBallisticAngle);
        while (gameObject.activeSelf)
        {
            if (!target.activeSelf) break;
            targetDir = target.transform.position - transform.position;
            transform.rotation =
                Quaternion.AngleAxis(Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg, Vector3.forward);
            // ReSharper disable once Unity.InefficientPropertyAccess
            transform.rotation *= Quaternion.Euler(0, 0, ballisticAngle);
            yield return null;
        }
    }
}