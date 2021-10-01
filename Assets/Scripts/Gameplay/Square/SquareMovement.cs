using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class SquareMovement : MonoBehaviour
{
    private const float X_FORCE = 2.5f;


    [Header("Configs")]
    [SerializeField] private SquareSO _squareSO;

    [Header("Validation")]
    [SerializeField] private bool _isFailedConfig;

    private bool _isCollided;
    private Rigidbody2D _squareRb;


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
    }

    private void OnEnable()
    {
        SquareCollision.OnBorderCollide += param => Jump(param);
    }

    private void OnDisable()
    {
        SquareCollision.OnBorderCollide -= param => Jump(param);
    }


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
                _squareRb.AddForce(new Vector2(-X_FORCE, jumpForce), ForceMode2D.Impulse);
            else
                _squareRb.AddForce(new Vector2(X_FORCE, jumpForce), ForceMode2D.Impulse);
        }
    }

    private void Jump(bool isCollied)
    {
        if (_isFailedConfig)
            return;

        _isCollided = isCollied;

        // Jump to an exact height
        var jumpForce = Mathf.Sqrt(_squareSO.JumpHeight * -2
            * (Physics2D.gravity.y * _squareRb.gravityScale));

        _squareRb.velocity = Vector2.zero;

        if (_isCollided)
            _squareRb.AddForce(new Vector2(-X_FORCE, jumpForce), ForceMode2D.Impulse);
        else
            _squareRb.AddForce(new Vector2(X_FORCE, jumpForce), ForceMode2D.Impulse);
    }

    private void CacheComponents()
    {
        _squareRb = GetComponent<Rigidbody2D>();
    }
}
