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
    #region Health

    [SerializeField] private StatsBarHUD playerHealthBarHUD;
    [SerializeField] private bool regenerateHealth = true;
    [SerializeField] private float regenerateTime = 1;
    [SerializeField] private float regeneratePercent = 1;

    #endregion

    [Header("--------Input--------")]

    #region Input

    [SerializeField] private PlayerInput input;

    private Vector2 moveInput;

    #endregion

    [Header("--------Move--------")]

    #region Move

    [SerializeField] private float moveSpeed = 6f;

    [SerializeField] private float moveRotationAngle = 30f;
    [SerializeField] private float paddingX = 0.8f;
    [SerializeField] private float paddingY = 0.22f;
    [SerializeField] private float accelerationTime = 2f;
    [SerializeField] private float decelerationTime = 2f;

    private Rigidbody2D playerRig;
    private bool isMove;

    #endregion

    [Header("--------Fire--------")]

    #region Fire

    [SerializeField, Range(0, 2)] private int weaponLevel = 1;

    [SerializeField] private float fireInterval = 0.2f;
    [SerializeField] private AudioData launchAudioData;
    [SerializeField] private GameObject projectileTop;
    [SerializeField] private GameObject projectileMiddle;
    [SerializeField] private GameObject projectileBottom;
    [SerializeField] private GameObject projectileOverdrive;
    [SerializeField] private Transform muzzleTop;
    [SerializeField] private Transform muzzleMiddle;
    [SerializeField] private Transform muzzleBottom;

    #endregion

    [Header("--------Dodge--------")]

    #region Dodge

    [SerializeField, Range(0, 100)] private float dodgeEnergy = 25;

    [SerializeField] private float maxRoll = 720f;
    [SerializeField] private float rollSpeed = 360f;
    [SerializeField] private Vector3 dodgeScale = new Vector3(0.5f, 0.5f, 0.5f);
    [SerializeField] private AudioData dodgeAudioData;
    [SerializeField] private float dodgeBulletTimeDuration;
    [SerializeField] private float dodgeBulletTimeIn;
    [SerializeField] private float dodgeBulletTimeOut;

    private CapsuleCollider2D playerCol;
    private float currentRoll;
    private bool isDodge;

    #endregion

    [Header("--------Overdrive--------")]

    #region Overdrive

    [SerializeField] private float overdriveDodgeFactor = 2;

    [SerializeField] private float overdriveSpeedFactor = 1.2f;
    [SerializeField] private float overdriveFireFactor = 1.2f;
    [SerializeField] private float overdriveBulletTimeDuration;
    private WaitForSeconds waitForOverdriveFire;

    public bool isOverdrive;

    #endregion

    [SerializeField] private AudioData deathAudioData;
    private Coroutine regenerateCoroutine;
    private Coroutine moveAndRotationLerpCoroutine;
    private WaitForSeconds waitForFire;
    private WaitForSeconds waitForRegenerate;
    private WaitForFixedUpdate waitForFixedUpdate;

    private void Awake()
    {
        playerRig = GetComponent<Rigidbody2D>();
        playerCol = GetComponent<CapsuleCollider2D>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        input.StartMoveAction += StartMoveAction;
        input.StopMoveAction += StopMoveAction;
        input.StartFireAction += StartFireAction;
        input.StopFireAction += StopFireAction;
        input.DodgeAction += DodgeAction;
        input.OverdriveAction += OverdriveAction;

        PlayerEnergy.StartOverdriveAction += OverDriveOn;
        PlayerEnergy.StopOverdriveAction += OverDriveOff;
    }

    private void Start()
    {
        playerRig.gravityScale = 0;
        input.EnableGamePlayInput();
        waitForFire = new WaitForSeconds(fireInterval);
        waitForRegenerate = new WaitForSeconds(regenerateTime);
        waitForFixedUpdate = new WaitForFixedUpdate();
        waitForOverdriveFire = new WaitForSeconds(fireInterval / overdriveFireFactor);
        playerHealthBarHUD.Initialize(CurrentHealth, maxHealth);
    }

    private void Update()
    {
        if (isMove)
        {
            if (moveAndRotationLerpCoroutine != null) StopCoroutine(moveAndRotationLerpCoroutine);
            moveAndRotationLerpCoroutine = StartCoroutine(MoveAndRotationLerp(moveSpeed * moveInput,
                Quaternion.AngleAxis(moveRotationAngle * moveInput.y, Vector3.right), accelerationTime));
        }
        else
        {
            if (moveAndRotationLerpCoroutine != null) StopCoroutine(moveAndRotationLerpCoroutine);
            moveAndRotationLerpCoroutine =
                StartCoroutine(MoveAndRotationLerp(Vector2.zero, Quaternion.identity, decelerationTime));
        }

        if (playerRig.velocity.magnitude > 0.1f)
        {
            transform.position = Viewport.Instance.PlayerMobilePosition(transform.position, paddingX, paddingY);
        }
    }

    private void OnDisable()
    {
        input.StartMoveAction -= StartMoveAction;
        input.StopMoveAction -= StopMoveAction;
        input.StartFireAction -= StartFireAction;
        input.StopFireAction -= StopFireAction;
        input.DodgeAction -= DodgeAction;
        input.OverdriveAction -= OverdriveAction;

        PlayerEnergy.StartOverdriveAction -= OverDriveOn;
        PlayerEnergy.StopOverdriveAction -= OverDriveOff;
    }

    #region Damage

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        playerHealthBarHUD.UpdateStats(CurrentHealth, maxHealth);

        if (!gameObject.activeSelf) return;
        if (!regenerateHealth) return;
        if (regenerateCoroutine != null)
        {
            StopCoroutine(regenerateCoroutine);
        }

        regenerateCoroutine = StartCoroutine(HealthRegenerateCoroutine(waitForRegenerate, regeneratePercent));
    }

    protected override void RestoreHealth(float value)
    {
        base.RestoreHealth(value);
        playerHealthBarHUD.UpdateStats(CurrentHealth, maxHealth);
    }

    protected override void Die()
    {
        AudioManager.Instance.PlayRandomPitch(deathAudioData);
        playerHealthBarHUD.UpdateStats(0, maxHealth);
        base.Die();
    }

    #endregion

    #region Move

    private void StartMoveAction(Vector2 dir)
    {
        this.moveInput = dir.normalized;
        isMove = true;
    }

    private void StopMoveAction()
    {
        isMove = false;
    }

    private IEnumerator MoveAndRotationLerp(Vector2 moveVelocity, Quaternion moveRotation, float lerpTime)
    {
        float timer = 0;
        Vector3 preVelocity = playerRig.velocity;
        Quaternion preRotation = transform.rotation;

        while (timer < lerpTime)
        {
            timer += Time.fixedDeltaTime;
            playerRig.velocity = Vector2.Lerp(preVelocity, moveVelocity, timer);
            transform.rotation = Quaternion.Lerp(preRotation, moveRotation, timer);
            yield return waitForFixedUpdate;
        }
    }

    #endregion

    #region Fire

    private void StartFireAction()
    {
        StartCoroutine(nameof(FireCoroutine));
    }

    private void StopFireAction()
    {
        StopCoroutine(nameof(FireCoroutine));
    }

    private IEnumerator FireCoroutine()
    {
        while (true)
        {
            if (isDodge) yield return new WaitUntil(() => isDodge == false);

            switch (weaponLevel)
            {
                case 0:
                    PoolManager.Release(isOverdrive ? projectileOverdrive : projectileMiddle, muzzleMiddle.position);
                    break;
                case 1:
                    PoolManager.Release(isOverdrive ? projectileOverdrive : projectileMiddle, muzzleTop.position);
                    PoolManager.Release(isOverdrive ? projectileOverdrive : projectileMiddle, muzzleBottom.position);
                    break;
                case 2:
                    PoolManager.Release(isOverdrive ? projectileOverdrive : projectileTop, muzzleTop.position);
                    PoolManager.Release(isOverdrive ? projectileOverdrive : projectileMiddle, muzzleMiddle.position);
                    PoolManager.Release(isOverdrive ? projectileOverdrive : projectileBottom, muzzleBottom.position);
                    break;
            }


            AudioManager.Instance.PlayRandomPitch(launchAudioData);
            yield return isOverdrive ? waitForOverdriveFire : waitForFire;
        }
        // ReSharper disable once IteratorNeverReturns
    }

    #endregion

    #region Dodge

    private void DodgeAction()
    {
        if (isDodge) return;
        print(dodgeEnergy);
        if (!PlayerEnergy.Instance.EnergyIsEnough(dodgeEnergy)) return;
        StartCoroutine(DodgeCoroutine());
        TimeManager.Instance.BulletTime(dodgeBulletTimeDuration, dodgeBulletTimeIn, dodgeBulletTimeOut);

    }

    private IEnumerator DodgeCoroutine()
    {
        PlayerEnergy.Instance.EnergyExpend(dodgeEnergy);
        playerCol.isTrigger = true;
        isDodge = true;
        currentRoll = 0f;
        AudioManager.Instance.PlayRandomPitch(dodgeAudioData);
        while (currentRoll < maxRoll)
        {
            currentRoll += rollSpeed * Time.deltaTime;
            transform.rotation = Quaternion.AngleAxis(currentRoll, Vector3.right);

            transform.localScale = Vector3.Lerp(
                Vector3.Lerp(Vector3.one, dodgeScale, currentRoll / maxRoll),
                Vector3.Lerp(dodgeScale, Vector3.one, currentRoll / maxRoll),
                currentRoll / maxRoll);

            yield return null;
        }

        playerCol.isTrigger = false;
        isDodge = false;
    }

    #endregion

    #region Overdrive

    private void OverdriveAction()
    {
        if (!PlayerEnergy.Instance.EnergyIsEnough(PlayerEnergy.MaxEnergy)) return;
        PlayerEnergy.StartOverdriveAction.Invoke();
    }

    private void OverDriveOn()
    {
        isOverdrive = true;
        dodgeEnergy *= overdriveDodgeFactor;
        moveSpeed *= overdriveSpeedFactor;
        TimeManager.Instance.BulletTime(overdriveBulletTimeDuration);
    }

    private void OverDriveOff()
    {
        isOverdrive = false;
        dodgeEnergy /= overdriveDodgeFactor;
        moveSpeed /= overdriveSpeedFactor;
    }

    #endregion
}