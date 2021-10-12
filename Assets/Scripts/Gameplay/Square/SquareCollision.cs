using System;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(BoxCollider2D))]
public class SquareCollision : MonoBehaviour
{
    private Action<object> _setFirstCollided;


    [Header("Configs")]
    [SerializeField] private SquareSO _squareSO;

    [Header("Validation")]
    [SerializeField] private bool _isFailedConfig;

    private Transform _transform;
    private BoxCollider2D _boxCollider2D;
    private bool _isCollided;


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
        CacheEvents();
    }

    private void OnEnable()
    {
        EventDispatcher.RegisterListener(EventsID.FIRST_COLLIDED_SQUARE, _setFirstCollided);
    }

    private void OnDisable()
    {
        EventDispatcher.RemoveListener(EventsID.FIRST_COLLIDED_SQUARE, _setFirstCollided);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Border"))
        {
            EventDispatcher.PostEvent(EventsID.SQUARE_COLLIDE_SFX, _squareSO.GetSFXByName("Impact"));

            _isCollided = !_isCollided;

            EventDispatcher.PostEvent(EventsID.COLLIDED_SQUARE, _isCollided);
            EventDispatcher.PostEvent(EventsID.SQUARE_COLLIDED_VFX, _isCollided);
            
            CheckDoubleJump();
        }
    }

    private void SetFirstCollided()
    {
        _isCollided = RandomJumpDirection();

        EventDispatcher.PostEvent(EventsID.COLLIDED_SQUARE, _isCollided);
    }

    private void CheckDoubleJump()
    {
        DOTween.Sequence()
            .AppendCallback(() => _boxCollider2D.enabled = false)
            .AppendInterval(0.2f)
            .OnComplete(() => _boxCollider2D.enabled = true);
    }

    private bool RandomJumpDirection()
    {
        return (UnityEngine.Random.Range(0, 2) == 0) ? true : false;
    }

    private void CacheComponents()
    {
        _transform = GetComponent<Transform>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void CacheEvents()
    {
        _setFirstCollided = (param) => SetFirstCollided();
    }
}
