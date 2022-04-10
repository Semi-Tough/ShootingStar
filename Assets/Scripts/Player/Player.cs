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
    private Rigidbody2D _rigidbody2D;
    private Coroutine _moveCoroutine;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        input.StartMove += StartMove;
        input.StopMove += StopMove;
    }

    private void OnDisable()
    {
        input.StartMove -= StartMove;
        input.StopMove -= StopMove;
    }

    private void Start()
    {
        _rigidbody2D.gravityScale = 0;
        input.EnableGamePlayInput();
    }

    private void StartMove(Vector2 moveInput)
    {
        if (_moveCoroutine != null)
            StopCoroutine(_moveCoroutine);

        _moveCoroutine = StartCoroutine(MoveCoroutine(moveSpeed * moveInput, accelerationTime,
            Quaternion.AngleAxis(moveRotationAngle * moveInput.y, Vector3.right)));

        StartCoroutine(MovePositionLimitCoroutine());
    }

    private void StopMove()
    {
        if (_moveCoroutine != null)
            StopCoroutine(_moveCoroutine);


        _moveCoroutine = StartCoroutine(MoveCoroutine(Vector2.zero, decelerationTime, Quaternion.identity));
        StopCoroutine(MovePositionLimitCoroutine());
    }

    IEnumerator MovePositionLimitCoroutine()
    {
        while (true)
        {
            transform.position = Viewport.Instance.PlayerMobilePosition(transform.position, paddingX, paddingY);
            yield return null;
        }
        // ReSharper disable once IteratorNeverReturns
    }

    IEnumerator MoveCoroutine(Vector2 moveVelocity, float time, Quaternion moveRotation)
    {
        float t = 0;
        while (t < time)
        {
            t += Time.fixedDeltaTime / time;
            _rigidbody2D.velocity = Vector2.Lerp(_rigidbody2D.velocity, moveVelocity, t / time);
            transform.rotation = Quaternion.Lerp(transform.rotation, moveRotation, t / time);
            yield return null;
        }
    }
}
