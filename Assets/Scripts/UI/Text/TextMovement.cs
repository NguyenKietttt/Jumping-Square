using System;
using DG.Tweening;
using UnityEngine;

public class TextMovement : StateBase
{
    private Action<object> _onTitleRef, _onTitleToGameplayRef;


    private RectTransform _rectTransform;
    private Sequence tween;


    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();

        _onTitleRef = (param) => OnTitle();
        _onTitleToGameplayRef = (param) => OnTitleToGameplay();
    }

    private void OnEnable()
    {
        EventDispatcher.RegisterListener(EventsID.TITLE_STATE, _onTitleRef);
        EventDispatcher.RegisterListener(EventsID.TITLE_TO_GAMEPLAY_STATE, _onTitleToGameplayRef);
    }

    private void OnDisable()
    {
        EventDispatcher.RemoveListener(EventsID.TITLE_STATE, _onTitleRef);
        EventDispatcher.RemoveListener(EventsID.TITLE_TO_GAMEPLAY_STATE, _onTitleToGameplayRef);
    }


    public override void OnTitle()
    {
        _rectTransform.DOScale(Vector3.one * 1.2f, 1.0f).SetEase(Ease.OutCirc).SetLoops(-1, LoopType.Yoyo);
    }

    public override void OnTitleToGameplay()
    {
        DOTween.Kill(_rectTransform);
        
        _rectTransform.DOScale(Vector3.one, 0.0f);
    }
}
