using System.Collections;
using UnityEngine;
using DG.Tweening;

public class HolderController : StateBase
{
    [Header("Configs")]
    [SerializeField] private HolderSO _holderSO;

    [Header("Spikes")]
    [SerializeField] private Transform _topSpikesHolder;
    [SerializeField] private Transform _bottomSpikesHolder;
    [SerializeField] private Transform _leftSpikesHolder;
    [SerializeField] private Transform _rightSpikesHolder;

    [Header("Borders")]
    [SerializeField] private Transform _leftBorders;
    [SerializeField] private Transform _rightBorders;

    [Header("Validation")]
    [SerializeField] private bool _isFailedConfig;


    private void OnValidate()
    {
        CustomLogs.Instance.Warning(_holderSO == null, "holderSO is missing!!!");

        CustomLogs.Instance.Warning(_topSpikesHolder == null, "topSpikesHolder is missing!!!");
        CustomLogs.Instance.Warning(_bottomSpikesHolder == null, "bottomSpikesHolder is missing!!!");
        CustomLogs.Instance.Warning(_leftSpikesHolder == null, "leftSpikesHolder is missing!!!");
        CustomLogs.Instance.Warning(_rightSpikesHolder == null, "rightSpikesHolder is missing!!!");

        CustomLogs.Instance.Warning(_leftBorders == null, "leftBorders is missing!!!");
        CustomLogs.Instance.Warning(_rightBorders == null, "rightBorders is missing!!!");

        _isFailedConfig = _topSpikesHolder == null || _bottomSpikesHolder == null 
            || _leftSpikesHolder == null || _rightSpikesHolder == null || _leftBorders == null 
            || _rightBorders == null;
    }

    private void Awake() 
    {
        if (_isFailedConfig)
            enabled = false;
    }

    private void OnEnable()
    {
        StateController.OnTitleToGameplayEvent += OnTitleToGameplay;
    }

    private void OnDisable()
    {
        StateController.OnTitleToGameplayEvent -= OnTitleToGameplay;
    }


    public override void OnTitleToGameplay()
    {
        ShowHolder();
    }


    private void ShowHolder()
    {
        _topSpikesHolder.DOMoveY(0.0f, _holderSO.HolderShowDuration).SetEase(_holderSO.HolderShowEase);
        _bottomSpikesHolder.DOMoveY(0.0f, _holderSO.HolderShowDuration).SetEase(_holderSO.HolderShowEase);

        _rightBorders.DOMoveX(8.5f, _holderSO.HolderShowDuration).SetEase(_holderSO.HolderShowEase);
        _leftBorders.DOMoveX(-8.5f, _holderSO.HolderShowDuration).SetEase(_holderSO.HolderShowEase)
            .OnComplete(() => StartCoroutine(ShowSpikes()));
    }

    private IEnumerator ShowSpikes()
    {
        yield return new WaitForSeconds(0.5f);

        _leftSpikesHolder.DOLocalMoveY(-4.5f, _holderSO.SpikeShowDuration).SetEase(_holderSO.SpikeShowEase);
        _rightSpikesHolder.DOLocalMoveY(4.5f, _holderSO.SpikeShowDuration).SetEase(_holderSO.SpikeShowEase);
    }
}
