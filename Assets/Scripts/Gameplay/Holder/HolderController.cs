using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;

public class HolderController : StateBase
{
    private readonly float TOP_HOLDER_POS = 0.0f;
    private readonly float TOP_HOLDER_OLD_POS = 1.0f;
    private readonly float LEFT_HOLDER_POS = 4.5f;
    private readonly float LEFT_HOLDER_OLD_POS = 5.5f;
    private readonly float BORDERS_POS = 8.5f;
    private readonly float BORDERS_OLD_POS = 10.5f;


    public static event Action endTitleToGameplayEvent;


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
        SpikeController.endGameplayToGameoverEvent += HideHolder;
    }

    private void OnDisable()
    {
        StateController.OnTitleToGameplayEvent -= OnTitleToGameplay;
        SpikeController.endGameplayToGameoverEvent -= HideHolder;
    }


    public override void OnTitleToGameplay()
    {
        ShowHolder();
    }


    private void ShowHolder()
    {
        _topSpikesHolder.DOMoveY(TOP_HOLDER_POS, _holderSO.HolderShowDuration)
            .SetEase(_holderSO.HolderShowEase);
        _bottomSpikesHolder.DOMoveY(TOP_HOLDER_POS, _holderSO.HolderShowDuration)
            .SetEase(_holderSO.HolderShowEase);

        _rightBorders.DOMoveX(BORDERS_POS, _holderSO.HolderShowDuration).SetEase(_holderSO.HolderShowEase);
        _leftBorders.DOMoveX(-BORDERS_POS, _holderSO.HolderShowDuration).SetEase(_holderSO.HolderShowEase)
            .OnComplete(() => StartCoroutine(ShowSpikes()));
    }

    private IEnumerator ShowSpikes()
    {
        yield return new WaitForSeconds(0.5f);

        _leftSpikesHolder.DOLocalMoveY(-LEFT_HOLDER_POS, _holderSO.SpikeShowDuration)
            .SetEase(_holderSO.SpikeShowEase);
        _rightSpikesHolder.DOLocalMoveY(LEFT_HOLDER_POS, _holderSO.SpikeShowDuration)
            .SetEase(_holderSO.SpikeShowEase)
            .OnComplete(() => endTitleToGameplayEvent?.Invoke());
    }

    private void HideHolder()
    {
        _topSpikesHolder.DOMoveY(TOP_HOLDER_OLD_POS, _holderSO.HolderHideDuration)
            .SetEase(_holderSO.HolderHideEase);
        _bottomSpikesHolder.DOMoveY(-TOP_HOLDER_OLD_POS, _holderSO.HolderHideDuration)
            .SetEase(_holderSO.HolderHideEase);

        _rightBorders.DOMoveX(BORDERS_OLD_POS, _holderSO.HolderHideDuration).SetEase(_holderSO.HolderHideEase);
        _leftBorders.DOMoveX(-BORDERS_OLD_POS, _holderSO.HolderHideDuration).SetEase(_holderSO.HolderHideEase)
            .OnComplete(() => HideSpikes());
    }

    private void HideSpikes()
    {
        _leftSpikesHolder.DOLocalMoveY(-LEFT_HOLDER_OLD_POS, _holderSO.SpikeHideDuration);
        _rightSpikesHolder.DOLocalMoveY(LEFT_HOLDER_OLD_POS, _holderSO.SpikeHideDuration)
            .OnComplete(() => StateController.RaiseGameoverEvent());
    }
}
