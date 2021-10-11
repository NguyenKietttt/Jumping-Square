using System;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(BoxCollider2D))]
public class SquareCollision : StateBase
{
    private readonly Quaternion INVERT_ROTATION = Quaternion.Euler(0, 0, -180.0f);


    public static event Action<bool> onBorderCollideEvent;
    public static event Action<AudioClip> impactSFXEvent;


    [Header("Configs")]
    [SerializeField] private SquareSO _squareSO;

    [Header("Validation")]
    [SerializeField] private bool _isFailedConfig;

    private Transform _transform;
    private BoxCollider2D _boxCollider2D;
    private Renderer _renderer;
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
    }

    private void OnEnable()
    {
        StateController.OnTitleEvent += OnTitleMenu;
        StateController.OnGameplayEvent += OnGameplay;
    }

    private void OnDisable()
    {
        StateController.OnTitleEvent -= OnTitleMenu;
        StateController.OnGameplayEvent -= OnGameplay;
    }


    public override void OnGameplay()
    {
        _isCollided = RandomJumpDirection();

        onBorderCollideEvent?.Invoke(_isCollided);
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Border"))
        {
            impactSFXEvent?.Invoke(_squareSO.GetSFXByName("Impact"));

            _isCollided = !_isCollided;

            onBorderCollideEvent?.Invoke(_isCollided);

            SpawnCollidedVFX();
            CheckDoubleJump();
        }
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

    private void SpawnCollidedVFX()
    {
        if (_isCollided)
            Instantiate(_squareSO.CollidedVFX, _renderer.bounds.center, Quaternion.identity);
        else
            Instantiate(_squareSO.CollidedVFX, _renderer.bounds.center, INVERT_ROTATION);
    }

    private void CacheComponents()
    {
        _transform = GetComponent<Transform>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _renderer = GetComponent<Renderer>();
    }
}
