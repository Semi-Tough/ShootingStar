/****************************************************
    文件：EnemyController.cs
    作者：wzq
    邮箱：1693416984@qq.com
    日期：2022/04/12 15:38:37
    功能：敌人控制器
*****************************************************/

using System.Collections;
using UnityEngine;

public class EnemyController : Controller
{
    [SerializeField] private int energy = 3;
    [SerializeField] private int score = 5;

    [Header("--------Move--------")]
    [SerializeField] private float paddingX;

    [SerializeField] private float paddingY;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float moveRotationAngle = 30f;
    [SerializeField] private float smoothTime = 1f;

    [Header("--------Fire--------")]
    [SerializeField] private AudioData[] launchAudioData;

    [SerializeField] private float minIntervalFire;
    [SerializeField] private float maxIntervalFire;
    [SerializeField] private GameObject[] projectiles;
    [SerializeField] private Transform muzzle;


    [SerializeField] private AudioData[] deathAudioData;
    private Vector3 targetPosition;


    protected override void OnEnable()
    {
        base.OnEnable();
        transform.position = Viewport.Instance.RandomEnemySpawnPosition(paddingX, paddingY);
        targetPosition = Viewport.Instance.RandomRightHalfPosition(paddingX, paddingY);
        StartCoroutine(nameof(RandomFire));
    }

    private void OnDisable()
    {
        StopCoroutine(nameof(RandomFire));
    }

    private void Update()
    {
        if (!gameObject.activeSelf) return;
        Vector3 position = transform.position;

        if (Vector3.Distance(position, targetPosition) > moveSpeed * Time.deltaTime)
        {
            transform.position =
                Vector3.MoveTowards(position, targetPosition, moveSpeed * Time.deltaTime);

            RotationLerp(Quaternion.AngleAxis(moveRotationAngle * (targetPosition - position).normalized.y,
                Vector3.right), smoothTime);
        }
        else
        {
            targetPosition = Viewport.Instance.RandomRightHalfPosition(paddingX, paddingY);
        }
    }

    private void RotationLerp(Quaternion moveRotation, float duration)
    {
        float timer = 0f;
        if (!(timer < duration)) return;
        timer += Time.fixedDeltaTime;
        transform.rotation = Quaternion.Lerp(transform.rotation, moveRotation, timer);
    }

    private IEnumerator RandomFire()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minIntervalFire, maxIntervalFire));
            foreach (GameObject projectile in projectiles)
            {
                PoolManager.Release(projectile, muzzle.position);
            }

            AudioManager.Instance.PlayRandomPitch(launchAudioData);
        }
        // ReSharper disable once IteratorNeverReturns
    }

    protected override void Die()
    {
        AudioManager.Instance.PlayRandomPitch(deathAudioData);
        base.Die();
        PlayerEnergy.Instance.EnergyObtain(energy);
        EnemyManager.Instance.RemoveFromList(gameObject);
        ScoreManager.Instance.AddScore(score);
    }
}