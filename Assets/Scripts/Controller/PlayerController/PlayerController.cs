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

    #region Input

    [Header("--------Input--------")]
    [SerializeField] private PlayerInput input;

    private Vector2 _moveInput;

    #endregion

    #region Move

    [Header("--------Move--------")]
    [SerializeField] private float moveSpeed = 6f;

    [SerializeField] private float moveRotationAngle = 30f;
    [SerializeField] private float paddingX = 0.8f;
    [SerializeField] private float paddingY = 0.22f;
    [SerializeField] private float accelerationTime = 2f;
    [SerializeField] private float decelerationTime = 2f;

    private Rigidbody2D playerRig;
    private bool isMove;

    #endregion

    #region Fire

    [Header("--------Fire--------")]
    [SerializeField, Range(0, 2)] private int weaponLevel = 1;

    [SerializeField] private float fireInterval = 0.2f;
    [SerializeField] private AudioData launchAudioData;
    [SerializeField] private GameObject projectileTop;
    [SerializeField] private GameObject projectileMiddle;
    [SerializeField] private GameObject projectileBottom;
    [SerializeField] private Transform muzzleTop;
    [SerializeField] private Transform muzzleMiddle;
    [SerializeField] private Transform muzzleBottom;

    #endregion

    #region Dodge

    [Header("--------Dodge--------")]
    [SerializeField, Range(0, 100)] private int dodgeEnergy = 25;

    [SerializeField] private float maxRoll = 720f;
    [SerializeField] private float rollSpeed = 360f;
    [SerializeField] private Vector3 dodgeScale = new Vector3(0.5f, 0.5f, 0.5f);
    [SerializeField] private AudioData dodgeAudioData;

    private CapsuleCollider2D playerCol;
    private float currentRoll;
    private bool isDodge;

    #endregion

    [SerializeField] private AudioData deathAudioData;

    private Coroutine regenerateCoroutine;
    private WaitForSeconds waitForFire;
    private WaitForSeconds waitForRegenerate;

    private void Awake()
    {
        playerRig = GetComponent<Rigidbody2D>();
        playerCol = GetComponent<CapsuleCollider2D>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        input.StartMove += StartMove;
        input.StopMove += StopMove;
        input.StartFire += StartFire;
        input.StopFire += StopFire;
        input.Dodge += Dodge;
    }

    private void OnDisable()
    {
        input.StartMove -= StartMove;
        input.StopMove -= StopMove;
        input.StartFire -= StartFire;
        input.StopFire -= StopFire;
        input.Dodge -= Dodge;
    }

    private void Start()
    {
        playerRig.gravityScale = 0;
        input.EnableGamePlayInput();
        waitForFire = new WaitForSeconds(fireInterval);
        waitForRegenerate = new WaitForSeconds(regenerateTime);
        playerHealthBarHUD.Initialize(CurrentHealth, maxHealth);
    }

    private void Update()
    {
        if (isMove)
        {
            MoveAndRotationLerp(moveSpeed * _moveInput,
                Quaternion.AngleAxis(moveRotationAngle * _moveInput.y, Vector3.right), accelerationTime);
        }
        else
        {
            MoveAndRotationLerp(Vector2.zero, Quaternion.identity, decelerationTime);
        }

        if (playerRig.velocity.magnitude > 0.1f)
        {
            transform.position = Viewport.Instance.PlayerMobilePosition(transform.position, paddingX, paddingY);
        }
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        playerHealthBarHUD.UpdateStats(CurrentHealth, maxHealth);

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
        playerHealthBarHUD.UpdateStats(CurrentHealth, maxHealth);
    }

    protected override void Die()
    {
        AudioManager.Instance.PlayRandomPitch(deathAudioData);
        playerHealthBarHUD.UpdateStats(0, maxHealth);
        base.Die();
    }


    #region Move

    private void StartMove(Vector2 moveInput)
    {
        _moveInput = moveInput.normalized;
        isMove = true;
    }

    private void StopMove()
    {
        isMove = false;
    }

    private void MoveAndRotationLerp(Vector2 moveVelocity, Quaternion moveRotation, float lerpTime)
    {
        float timer = 0;
        if (!(timer < lerpTime)) return;
        timer += Time.fixedDeltaTime;
        playerRig.velocity = Vector2.Lerp(playerRig.velocity, moveVelocity, timer);
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

    private IEnumerator FireCoroutine()
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

            AudioManager.Instance.PlayRandomPitch(launchAudioData);
            yield return waitForFire;
        }
        // ReSharper disable once IteratorNeverReturns
    }

    #endregion

    #region Dodge

    private void Dodge()
    {
        if (isDodge) return;
        if (!PlayerEnergy.Instance.EnergyIsEnough(dodgeEnergy)) return;
        StartCoroutine(DodgeCoroutine());
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


            #region Method1

            // Vector3 scale = transform.localScale;

            // float rollValue = Time.deltaTime * dodgeDuration;
            // if (currentRoll < maxRoll / 2)
            // {
            //     scale.x = Mathf.Clamp(scale.x - rollValue, dodgeScale.x, 1f);
            //     scale.y = Mathf.Clamp(scale.y - rollValue, dodgeScale.y, 1f);
            //     scale.z = Mathf.Clamp(scale.z - rollValue, dodgeScale.z, 1f);
            // }
            // else
            // {
            //     scale.x = Mathf.Clamp(scale.x + rollValue, dodgeScale.x, 1f);
            //     scale.y = Mathf.Clamp(scale.y + rollValue, dodgeScale.y, 1f);
            //     scale.z = Mathf.Clamp(scale.z + rollValue, dodgeScale.z, 1f);
            // } 
            // transform.localScale = scale;

            #endregion

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
}