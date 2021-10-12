using System;
using UnityEngine;

public class SquareVFX : MonoBehaviour
{
    private readonly Vector3 VFX_JUMP_LEFT_OFFSET = new Vector3(0.3f, -0.5f, 0);
    private readonly Vector3 VFX_JUMP_RIGHT_OFFSET = new Vector3(-0.3f, -0.5f, 0);
    private readonly Quaternion INVERT_ROTATION = Quaternion.Euler(0, 0, -180.0f);


    private Action<object> _jumpVFXRef, _collidedVFXRef, _deadVFXRef;


    [Header("Configs")]
    [SerializeField] private SquareSO _squareSO;

    [Header("Validation")]
    [SerializeField] private bool _isFailedConfig;

    private Transform _transform;
    private Renderer _renderer;


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
        EventDispatcher.RegisterListener(EventsID.SQUARE_JUMP_VFX, _jumpVFXRef);
        EventDispatcher.RegisterListener(EventsID.SQUARE_COLLIDED_VFX, _collidedVFXRef);
        EventDispatcher.RegisterListener(EventsID.SQUARE_DEAD_VFX, _deadVFXRef);
    }

    private void OnDisable()
    {
        EventDispatcher.RemoveListener(EventsID.SQUARE_JUMP_VFX, _jumpVFXRef);
        EventDispatcher.RemoveListener(EventsID.SQUARE_COLLIDED_VFX, _collidedVFXRef);
        EventDispatcher.RemoveListener(EventsID.SQUARE_DEAD_VFX, _deadVFXRef);
    }


    private void SpawnJumpVFX(object isFacingLeft)
    {
        var boolIsFacingLeft = (bool)isFacingLeft;

        if (boolIsFacingLeft)
            Instantiate(_squareSO.JumpVFX, _transform.position + VFX_JUMP_LEFT_OFFSET, Quaternion.identity);
        else
            Instantiate(_squareSO.JumpVFX, _transform.position + VFX_JUMP_RIGHT_OFFSET, Quaternion.identity);
    }

    private void SpawnCollidedVFX(object isFacingLeft)
    {
        var boolIsFacingLeft = (bool)isFacingLeft;

        if (boolIsFacingLeft)
            Instantiate(_squareSO.CollidedVFX, _renderer.bounds.center, Quaternion.identity);
        else
            Instantiate(_squareSO.CollidedVFX, _renderer.bounds.center, INVERT_ROTATION);
    }

    private void SpawnDeadVFX()
    {
        Instantiate(_squareSO.ExplodeVFX, transform.position, Quaternion.identity);
    }

    private void CacheComponents()
    {
        _transform = GetComponent<Transform>();
        _renderer = GetComponent<Renderer>();
    }

    private void CacheEvents()
    {
        _jumpVFXRef = (param) => SpawnJumpVFX(param);
        _collidedVFXRef = (param) => SpawnCollidedVFX(param);
        _deadVFXRef = (param) => SpawnDeadVFX();
    }
}
