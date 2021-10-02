using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class SquareMovement : MonoBehaviour
{
    private readonly float X_FORCE = 2.5f;
    private readonly Vector3 ROTATE_LEFT_VECTOR = new Vector3(0, 0, 360.0f);
    private readonly Vector3 ROTATE_RIGHT_VECTOR = new Vector3(0, 180.0f, 360.0f);


    [Header("Configs")]
    [SerializeField] private SquareSO _squareSO;

    [Header("Validation")]
    [SerializeField] private bool _isFailedConfig;

    private bool _isCollided, _isStartJump;
    private Rigidbody2D _squareRb;
    private Transform _cacheTransform;


    private void OnValidate()
    {
        CustomLogs.Instance.Warning(_squareSO == null, "squareSO is missing!!!");

        _isFailedConfig = _squareSO == null;
    }

    private void Awake()
    {
        CacheComponents();

        if (_isFailedConfig)
            enabled = false;

        _isStartJump = true;
    }

    private void OnEnable()
    {
        SquareCollision.OnBorderCollide += param =>
        {
            CheckIsCollided(param);
            Jump();
        };

    }

    private void OnDisable()
    {
        SquareCollision.OnBorderCollide -= param =>
        {
            CheckIsCollided(param);
            Jump();
        };
    }


    /// <summary>
    /// Raise by InputManager in Herachy
    /// </summary>
    public void Jump(InputAction.CallbackContext ctx)
    {
        if (_isFailedConfig)
            return;

        if (ctx.started)
        {
            // Jump to an exact height
            var jumpForce = Mathf.Sqrt(_squareSO.JumpHeight * -2
                * (Physics2D.gravity.y * _squareRb.gravityScale));

            _squareRb.velocity = Vector2.zero;

            if (_isCollided)
            {
                _squareRb.AddForce(new Vector2(-X_FORCE, jumpForce), ForceMode2D.Impulse);
                Rotate360(ROTATE_LEFT_VECTOR);
            }
            else
            {
                _squareRb.AddForce(new Vector2(X_FORCE, jumpForce), ForceMode2D.Impulse);
                Rotate360(ROTATE_RIGHT_VECTOR);
            }
        }
    }

    private void Jump()
    {
        if (_isFailedConfig)
            return;

        if (_isStartJump)
        {
            _isStartJump = false;
            return;
        }

        // Jump to an exact height
        var jumpForce = Mathf.Sqrt(_squareSO.JumpHeight * -2
            * (Physics2D.gravity.y * _squareRb.gravityScale));

        _squareRb.velocity = Vector2.zero;

        if (_isCollided)
        {
            _squareRb.AddForce(new Vector2(-X_FORCE, jumpForce), ForceMode2D.Impulse);
            Rotate360(ROTATE_LEFT_VECTOR);
        }
        else
        {
            _squareRb.AddForce(new Vector2(X_FORCE, jumpForce), ForceMode2D.Impulse);
            Rotate360(ROTATE_RIGHT_VECTOR);
        }
    }

    private void CheckIsCollided(bool isCollided)
    {
        _isCollided = isCollided;

        if (_isStartJump)
            if (_isCollided)
                _cacheTransform.eulerAngles = Vector3.zero;
            else
                _cacheTransform.eulerAngles = Vector3.up * 180.0f;
    }

    private void Rotate360(Vector3 rotateVector)
    {
        _cacheTransform.DORotate(rotateVector, _squareSO.RotateDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.OutSine);
    }

    private void CacheComponents()
    {
        _cacheTransform = transform;

        _squareRb = GetComponent<Rigidbody2D>();
    }
}
