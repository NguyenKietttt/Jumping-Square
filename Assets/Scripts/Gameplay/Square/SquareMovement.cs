using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class SquareMovement : StateBase
{
    private readonly Vector3 ROTATE_VECTOR = new Vector3(0, 0, 360.0f);
    private readonly Vector3 VFX_JUMP_LEFT_OFFSET = new Vector3(0.3f, -0.5f, 0);
    private readonly Vector3 VFX_JUMP_RIGHT_OFFSET = new Vector3(-0.3f, -0.5f, 0);


    [Header("Configs")]
    [SerializeField] private SquareSO _squareSO;

    [Header("Validation")]
    [SerializeField] private bool _isFailedConfig;

    private bool _isAllowJump, _isFirstJump, _isCollided;
    private Rigidbody2D _squareRb;
    private Transform _transform;


    private void OnValidate()
    {
        CustomLogs.Instance.Warning(_squareSO == null, "squareSO is missing!!!");

        _isFailedConfig = _squareSO == null;
    }

    private void Awake()
    {

        if (_isFailedConfig)
            enabled = false;

        CacheComponents();
    }

    private void OnEnable()
    {
        StateController.OnTitleEvent += OnTitleMenu;
        StateController.OnTitleToGameplayEvent += OnTitleToGameplay;
        StateController.OnGameplayEvent += OnGameplay;
        StateController.OnGameplayToGameoverEvent += OnGameplayToGameover;

        HolderController.endTitleToGameplayEvent += ShowSquare;

        SquareCollision.OnBorderCollideEvent += param => Jump(param);
    }

    private void OnDisable()
    {
        StateController.OnTitleEvent -= OnTitleMenu;
        StateController.OnTitleToGameplayEvent -= OnTitleToGameplay;
        StateController.OnGameplayEvent -= OnGameplay;
        StateController.OnGameplayToGameoverEvent -= OnGameplayToGameover;

        HolderController.endTitleToGameplayEvent -= ShowSquare;

        SquareCollision.OnBorderCollideEvent -= param => Jump(param);
    }


    public override void OnTitleMenu()
    {
        _isAllowJump = false;

        _squareRb.bodyType = RigidbodyType2D.Kinematic;
    }

    public override void OnTitleToGameplay()
    {
        _isFirstJump = true;
    }

    public override void OnGameplay()
    {
        _squareRb.bodyType = RigidbodyType2D.Dynamic;

        Jump(_isCollided);
    }

    public override void OnGameplayToGameover()
    {
        HideSquare();
    }


    /// <summary>
    /// Raise by InputManager in Hierarchy
    /// </summary>
    public void Jump(InputAction.CallbackContext ctx)
    {
        if (_isFailedConfig)
            return;

        if (!_isAllowJump)
            return;

        if (_isFirstJump)
        {
            StateController.RaiseGameplayEvent();

            _isFirstJump = false;
            return;
        }

        if (ctx.started)
        {
            // Jump to an exact height
            var jumpForce = Mathf.Sqrt(_squareSO.JumpHeight * -2
                * (Physics2D.gravity.y * _squareRb.gravityScale));

            _squareRb.velocity = Vector2.zero;


            if (_isCollided)
            {
                SpawnJumpVFX(VFX_JUMP_LEFT_OFFSET);
                
                _squareRb.AddForce(new Vector2(-_squareSO.JumpLength, jumpForce), ForceMode2D.Impulse);
                Rotate360(ROTATE_VECTOR);
            }
            else
            {
                SpawnJumpVFX(VFX_JUMP_RIGHT_OFFSET);

                _squareRb.AddForce(new Vector2(_squareSO.JumpLength, jumpForce), ForceMode2D.Impulse);
                Rotate360(-ROTATE_VECTOR);
            }
        }
    }

    private void Jump(bool isCollided)
    {
        if (_isFailedConfig)
            return;

        _isCollided = isCollided;

        // Jump to an exact height
        var jumpForce = Mathf.Sqrt(_squareSO.JumpHeight * -2.0f
            * (Physics2D.gravity.y * _squareRb.gravityScale));

        _squareRb.velocity = Vector2.zero;

        if (_isCollided)
        {
            _squareRb.AddForce(new Vector2(-_squareSO.JumpLength, jumpForce), ForceMode2D.Impulse);
            Rotate360(ROTATE_VECTOR);
        }
        else
        {
            _squareRb.AddForce(new Vector2(_squareSO.JumpLength, jumpForce), ForceMode2D.Impulse);
            Rotate360(-ROTATE_VECTOR);
        }
    }

    private void ShowSquare()
    {
        DOTween.Sequence()
            .OnStart(() =>
                {
                    _transform.DOScale(new Vector2(0.8f, 0.8f), 1.0f).SetEase(Ease.OutBack);
                    Rotate360(ROTATE_VECTOR);
                })
            .AppendInterval(1.2f)
            .OnComplete(() => _isAllowJump = true);
    }

    private void HideSquare()
    {
        DOTween.Kill(_transform);

        _isAllowJump = false;

        _squareRb.velocity = Vector2.zero;
        _squareRb.bodyType = RigidbodyType2D.Kinematic;

        _transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        _transform.localScale = Vector3.zero;

    }

    private void Rotate360(Vector3 rotateVector)
    {
        _transform.DORotate(rotateVector, _squareSO.RotateDuration, RotateMode.WorldAxisAdd)
            .SetEase(Ease.OutSine);
    }

    private void SpawnJumpVFX(Vector3 offset)
    {
        Instantiate(_squareSO.JumpVFX, _transform.position + offset, Quaternion.identity);
    }

    private void CacheComponents()
    {
        _transform = transform;
        _squareRb = GetComponent<Rigidbody2D>();
    }
}
