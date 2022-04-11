/****************************************************
    文件：Player.cs
    作者：wzq
    邮箱：1693416984@qq.com
    日期：2022/04/11 12:03:55
    功能：角色控制
*****************************************************/
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] private PlayerInput input;
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float moveRotationAngle = 30f;
    [SerializeField] private float paddingX = 0.8f;
    [SerializeField] private float paddingY = 0.22f;
    [SerializeField] private float accelerationTime = 2f;
    [SerializeField] private float decelerationTime = 2f;
    [SerializeField] private float fireInterval = 0.2f;

    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform muzzle;

    private WaitForSeconds waitForSeconds;
    private Rigidbody2D _rigidbody2D;
    private Vector2 _moveInput;
    private bool _onMove;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
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
        waitForSeconds = new WaitForSeconds(fireInterval);
    }

    private void Update()
    {
        if (_onMove)
        {
            MoveLerp(moveSpeed * _moveInput,
                Quaternion.AngleAxis(moveRotationAngle * _moveInput.y, Vector3.right), accelerationTime);
        }
        else
        {
            MoveLerp(Vector2.zero, Quaternion.identity, decelerationTime);
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

    private void MoveLerp(Vector2 moveVelocity, Quaternion moveRotation, float time)
    {
        float t = 0;
        if (!(t < time)) return;
        t += Time.fixedDeltaTime / time;
        _rigidbody2D.velocity = Vector2.Lerp(_rigidbody2D.velocity, moveVelocity, t / time);
        transform.rotation = Quaternion.Lerp(transform.rotation, moveRotation, t / time);
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
            Instantiate(projectile, muzzle.position, Quaternion.identity);
            yield return waitForSeconds;
        }
        // ReSharper disable once IteratorNeverReturns
    }

    #endregion
}