using UnityEngine;
using DG.Tweening;
using System;

public class HolderController : MonoBehaviour
{
    private readonly float TOP_HOLDER_POS = 0.0f;
    private readonly float TOP_HOLDER_OLD_POS = 1.0f;
    private readonly float SPIKE_HOLDER_POS = 4.5f;
    private readonly float SPIKE_HOLDER_OLD_POS = 5.5f;
    private readonly float BORDERS_POS = 8.5f;
    private readonly float BORDERS_OLD_POS = 10.5f;


    private Action<object> _showHolderRef, _hideHolderRef;


    [Header("Configs")]
    [SerializeField] private HolderSO _holderSO;

    [Header("Holders")]
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

        CacheEvents();
    }

    private void OnEnable()
    {
        EventDispatcher.RegisterListener(EventsID.SHOW_HOLDER, _showHolderRef);
        EventDispatcher.RegisterListener(EventsID.HIDE_HOLDER, _hideHolderRef);
    }

    private void OnDisable()
    {
        EventDispatcher.RemoveListener(EventsID.SHOW_HOLDER, _showHolderRef);
        EventDispatcher.RemoveListener(EventsID.HIDE_HOLDER, _hideHolderRef);
    }

    
    #region Holder

    private void ShowHolder()
    {
        EventDispatcher.PostEvent(EventsID.HOLDER_SFX, _holderSO.GetSFXByName("Open"));

        MoveHolders(TOP_HOLDER_POS, _holderSO.HolderShowDuration, _holderSO.HolderShowEase);
        MoveBorders(BORDERS_POS, _holderSO.HolderShowDuration, _holderSO.HolderShowEase,
            () => ShowSpikeHolder());
    }

    private void HideHolder()
    {
        EventDispatcher.PostEvent(EventsID.HOLDER_SFX, _holderSO.GetSFXByName("Close"));

        MoveHolders(TOP_HOLDER_OLD_POS, _holderSO.HolderHideDuration, _holderSO.HolderHideEase);
        MoveBorders(BORDERS_OLD_POS, _holderSO.HolderHideDuration, _holderSO.HolderHideEase,
            () => HideSpikeHolder());
    }

    private void MoveHolders(float yHolderPos, float duration, Ease ease)
    {
        _topSpikesHolder.DOMoveY(yHolderPos, duration).SetEase(ease);
        _bottomSpikesHolder.DOMoveY(-yHolderPos, duration).SetEase(ease);
    }

    #endregion

    #region Borders

    private void MoveBorders(float xBorderPos, float duration, Ease ease, TweenCallback callBack)
    {
        _rightBorders.DOMoveX(xBorderPos, duration).SetEase(ease);
        _leftBorders.DOMoveX(-xBorderPos, duration).SetEase(ease)
            .OnComplete(() => callBack());
    }

    #endregion

    #region Spike Holder
    
    private void ShowSpikeHolder()
    {
        MoveSpike(SPIKE_HOLDER_POS, _holderSO.SpikeShowDuration, 1.0f, _holderSO.SpikeShowEase,
            () => EventDispatcher.PostEvent(EventsID.SHOW_SQUARE));
    }

    private void HideSpikeHolder()
    {
        MoveSpike(SPIKE_HOLDER_OLD_POS, _holderSO.SpikeShowDuration, 0.0f, Ease.Unset,
            () => StateController.RaiseGameoverEvent());
    }

    private void MoveSpike(float ySpikeHolderPos, float duration, float delay, Ease ease, TweenCallback callback)
    {
        DOTween.Sequence().OnStart(() =>
        {
            _leftSpikesHolder.DOLocalMoveY(-ySpikeHolderPos, duration).SetEase(ease);
            _rightSpikesHolder.DOLocalMoveY(ySpikeHolderPos, duration).SetEase(ease);
        })
        .AppendInterval(delay)
        .OnComplete(() => callback());
    }

    #endregion

    private void CacheEvents()
    {
        _showHolderRef = (param) => ShowHolder();
        _hideHolderRef = (param) => HideHolder();
    }
}
