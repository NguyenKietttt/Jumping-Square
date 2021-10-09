using UnityEngine;
using DG.Tweening;

public class SquareTrigger : MonoBehaviour
{
    [Header("Configs")]
    [SerializeField] private SquareSO _squareSO;

    [Header("Validation")]
    [SerializeField] private bool _isFailedConfig;

    private Camera _mainCamera;


    private void OnValidate()
    {
        CustomLogs.Instance.Warning(_squareSO == null, "squareSO is missing!!!");

        _isFailedConfig = _squareSO == null;
    }

    private void Awake()
    {
        if (_isFailedConfig)
            enabled = false;

        _mainCamera = Camera.main;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("OpenSpike"))
        {
            Instantiate(_squareSO.ExplodeVFX, transform.position, Quaternion.identity);
            _mainCamera.DOShakePosition(0.5f, 0.5f);

            StateController.RaiseOnGameplayToGameoverEvent();
        }
    }
}
