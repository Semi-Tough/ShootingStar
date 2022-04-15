/****************************************************
    文件：PlayerController.cs
    作者：wzq
    邮箱：1693416984@qq.com
    日期：2022/04/11 12:03:55
    功能：玩家角色控制器
*****************************************************/

using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : Controller
{
    [SerializeField] private StatsBarHUD PlayerHealthBarHUD;
    [SerializeField] private bool regenerateHealth = true;
    [SerializeField] private float regenerateTime = 1;
    [SerializeField] private float regeneratePercent = 1;

    [Header("--------Input--------")]
    [SerializeField] private PlayerInput input;

    [Header("--------Move--------")]
    [SerializeField] private float moveSpeed = 6f;

    [SerializeField] private float moveRotationAngle = 30f;
    [SerializeField] private float paddingX = 0.8f;
    [SerializeField] private float paddingY = 0.22f;
    [SerializeField] private float accelerationTime = 2f;
    [SerializeField] private float decelerationTime = 2f;

    [Header("--------Fire--------")]
    [SerializeField, Range(0, 2)] private int weaponLevel = 1;

    [SerializeField] private float fireInterval = 0.2f;
    [SerializeField] private GameObject projectileTop;
    [SerializeField] private GameObject projectileMiddle;
    [SerializeField] private GameObject projectileBottom;
    [SerializeField] private Transform muzzleTop;
    [SerializeField] private Transform muzzleMiddle;
    [SerializeField] private Transform muzzleBottom;

    private Coroutine regenerateCoroutine;
    private WaitForSeconds waitForFire;
    private WaitForSeconds waitForRegenerate;
    private Rigidbody2D _rigidbody2D;
    private Vector2 _moveInput;
    private bool _onMove;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        input.StartMove += StartMove;
        input.StopMove += StopMove;
        input.StartFire += StartFire;
        input.StopFire += StopFire;
    }

    private void OnDisable()
    {
        input.StartMove -= StartMove;
        input.StopMove -= StopMove;
        input.StartFire -= StartFire;
        input.StopFire -= StopFire;
    }

    private void Start()
    {
        _rigidbody2D.gravityScale = 0;
        input.EnableGamePlayInput();
        waitForFire = new WaitForSeconds(fireInterval);
        waitForRegenerate = new WaitForSeconds(regenerateTime);
        PlayerHealthBarHUD.Initialize(CurrentHealth, maxHealth);
    }


    private void Update()
    {
        if (_onMove)
        {
            MoveAndRotationLerp(moveSpeed * _moveInput,
                Quaternion.AngleAxis(moveRotationAngle * _moveInput.y, Vector3.right), accelerationTime);
        }
        else
        {
            MoveAndRotationLerp(Vector2.zero, Quaternion.identity, decelerationTime);
        }

        if (_rigidbody2D.velocity.magnitude > 0.1f)
        {
            transform.position = Viewport.Instance.PlayerMobilePosition(transform.position, paddingX, paddingY);
        }
    }

    #region Move

    private void StartMove(Vector2 moveInput)
    {
        _moveInput = moveInput.normalized;
        _onMove = true;
    }

    private void StopMove()
    {
        _onMove = false;
    }

    private void MoveAndRotationLerp(Vector2 moveVelocity, Quaternion moveRotation, float lerpTime)
    {
        float timer = 0;
        if (!(timer < lerpTime)) return;
        timer += Time.fixedDeltaTime;
        _rigidbody2D.velocity = Vector2.Lerp(_rigidbody2D.velocity, moveVelocity, timer);
        transform.rotation = Quaternion.Lerp(transform.rotation, moveRotation, timer);
    }

    #endregion

    #region Fire

    private void StartFire()
    {
        StartCoroutine(nameof(FireCoroutine));
    }

    private void StopFire()
    {
        StopCoroutine(nameof(FireCoroutine));
    }

    IEnumerator FireCoroutine()
    {
        while (true)
        {
            // switch (weaponLevel)
            // {
            //     case 0:
            //         Instantiate(projectileMiddle, muzzleMiddle.position, Quaternion.identity);
            //         break;
            //     case 1:
            //         Instantiate(projectileMiddle, muzzleTop.position, Quaternion.identity);
            //         Instantiate(projectileMiddle, muzzleBottom.position, Quaternion.identity);
            //         break;
            //     case 2:
            //         Instantiate(projectileTop, muzzleTop.position, Quaternion.identity);
            //         Instantiate(projectileMiddle, muzzleMiddle.position, Quaternion.identity);
            //         Instantiate(projectileBottom, muzzleBottom.position, Quaternion.identity);
            //         break;
            // } 
            switch (weaponLevel)
            {
                case 0:
                    PoolManager.Release(projectileMiddle, muzzleMiddle.position);
                    break;
                case 1:
                    PoolManager.Release(projectileMiddle, muzzleTop.position);
                    PoolManager.Release(projectileMiddle, muzzleBottom.position);
                    break;
                case 2:
                    PoolManager.Release(projectileTop, muzzleTop.position);
                    PoolManager.Release(projectileMiddle, muzzleMiddle.position);
                    PoolManager.Release(projectileBottom, muzzleBottom.position);
                    break;
            }

            yield return waitForFire;
        }
        // ReSharper disable once IteratorNeverReturns
    }

    #endregion

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        PlayerHealthBarHUD.UpdateStats(CurrentHealth, maxHealth);

        if (gameObject.activeSelf)
        {
            if (regenerateHealth)
            {
                if (regenerateCoroutine != null)
                {
                    StopCoroutine(regenerateCoroutine);
                }

                regenerateCoroutine = StartCoroutine(HealthRegenerateCoroutine(waitForRegenerate, regeneratePercent));
            }
        }
    }

    protected override void RestoreHealth(float value)
    {
        base.RestoreHealth(value);
        PlayerHealthBarHUD.UpdateStats(CurrentHealth, maxHealth);
    }

    protected override void Die()
    {
        PlayerHealthBarHUD.UpdateStats(0, maxHealth);
        base.Die();
    }
}