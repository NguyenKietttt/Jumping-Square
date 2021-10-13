using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using System;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class SquareMovement : MonoBehaviour
{
    private readonly Vector3 ROTATE_VECTOR = new Vector3(0, 0, 360.0f);


    private Action<object> _showSquareRef, _hideSquareRef, _collidedSquareRef, _setAllowJumpRef, _setFirstJumpRef;


    [Header("Configs")]
    [SerializeField] private SquareSO _squareSO;

    [Header("Validation")]
    [SerializeField] private bool _isFailedConfig;

    private bool _isAllowJump, _isFirstJump, _isFacingLeft;
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
        CacheCallbacks();
    }

    private void OnEnable()
    {
        EventDispatcher.RegisterListener(EventsID.SHOW_SQUARE, _showSquareRef);
        EventDispatcher.RegisterListener(EventsID.HIDE_SQUARE, _hideSquareRef);

        EventDispatcher.RegisterListener(EventsID.ALLOW_JUMP_SQUARE, _setAllowJumpRef);
        EventDispatcher.RegisterListener(EventsID.FIRST_JUMP_SQUARE, _setFirstJumpRef);

        EventDispatcher.RegisterListener(EventsID.COLLIDED_SQUARE, _collidedSquareRef);
    }

    private void OnDisable()
    {
        EventDispatcher.RemoveListener(EventsID.SHOW_SQUARE, _showSquareRef);
        EventDispatcher.RemoveListener(EventsID.HIDE_SQUARE, _hideSquareRef);

        EventDispatcher.RemoveListener(EventsID.ALLOW_JUMP_SQUARE, _setAllowJumpRef);
        EventDispatcher.RemoveListener(EventsID.FIRST_JUMP_SQUARE, _setFirstJumpRef);

        EventDispatcher.RemoveListener(EventsID.COLLIDED_SQUARE, _collidedSquareRef);
    }


    /// <summary>
    /// Raise by InputManager in Hierarchy
    /// </summary>
    public void ClickToJump(InputAction.CallbackContext ctx)
    {
        if (_isFailedConfig || !_isAllowJump)
            return;

        if (_isFirstJump)
        {
            _isFirstJump = false;
            StateController.RaiseGameplayEvent();

            return;
        }

        if (ctx.started)
            Jump();
    }

    private void JumpWhenCollided(object isFacingLeft)
    {
        _isFacingLeft = (bool)isFacingLeft;

        Jump();
    }

    private void Jump()
    {
        // Jump to an exact height
        var jumpForce = Mathf.Sqrt(_squareSO.JumpHeight * -2.0f
            * (Physics2D.gravity.y * _squareRb.gravityScale));

        _squareRb.velocity = Vector2.zero;

        if (_isFacingLeft)
        {
            _squareRb.AddForce(new Vector2(-_squareSO.JumpLength, jumpForce), ForceMode2D.Impulse);
            Rotate360(ROTATE_VECTOR);
        }
        else
        {
            _squareRb.AddForce(new Vector2(_squareSO.JumpLength, jumpForce), ForceMode2D.Impulse);
            Rotate360(-ROTATE_VECTOR);
        }

        EventDispatcher.PostEvent(EventsID.SQUARE_MOVEMENT_SFX, _squareSO.GetSFXByName("Jump"));
        EventDispatcher.PostEvent(EventsID.SQUARE_JUMP_VFX, _isFacingLeft);
    }

    private void SetAllowJump(object condition)
    {
        _isAllowJump = (bool)condition;
    }

    private void SetFirstJump(object condition)
    {
        _isFirstJump = (bool)condition;
    }

    private void ShowSquare()
    {
        EventDispatcher.PostEvent(EventsID.SQUARE_MOVEMENT_SFX, _squareSO.GetSFXByName("Show"));

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

    private void CacheComponents()
    {
        _transform = transform;
        _squareRb = GetComponent<Rigidbody2D>();
    }

    private void CacheCallbacks()
    {
        _showSquareRef = (param) => ShowSquare();
        _hideSquareRef = (param) => HideSquare();

        _setAllowJumpRef = (param) => SetAllowJump(param);
        _setFirstJumpRef = (param) => SetFirstJump(param);

        _collidedSquareRef = (param) => JumpWhenCollided(param);
    }
}
