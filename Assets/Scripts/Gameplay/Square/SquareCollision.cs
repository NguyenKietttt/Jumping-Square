using System.Collections;
using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class SquareCollision : StateBase
{
    private readonly Quaternion INVERT_ROTATION = Quaternion.Euler(0, 0, 180.0f);


    public static event Action<bool> OnBorderCollideEvent;


    [Header("Configs")]
    [SerializeField] private SquareSO _squareSO;

    [Header("Validation")]
    [SerializeField] private bool _isFailedConfig;

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

        OnBorderCollideEvent?.Invoke(_isCollided);
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Border"))
        {
            _isCollided = !_isCollided;

            OnBorderCollideEvent?.Invoke(_isCollided);

            SpawnCollidedVFX();
            StartCoroutine(CheckDoubleJump());
        }
    }


    private IEnumerator CheckDoubleJump()
    {
        _boxCollider2D.enabled = false;

        yield return new WaitForSeconds(0.2f);

        _boxCollider2D.enabled = true;
    }

    private bool RandomJumpDirection()
    {
        var result = (UnityEngine.Random.Range(0, 2) == 0) ? true : false;

        return result;
    }

    private void SpawnCollidedVFX()
    {
        GameObject dustVFX;

        if (_isCollided)
            dustVFX = Instantiate(_squareSO.CollidedVFX, _renderer.bounds.center, Quaternion.identity);
        else
            dustVFX = Instantiate(_squareSO.CollidedVFX, _renderer.bounds.center, INVERT_ROTATION);
    }

    private void CacheComponents()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _renderer = GetComponent<Renderer>();
    }
}
