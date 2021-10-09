using DG.Tweening;
using UnityEngine;

public class PlayButton : MonoBehaviour, IButtonAction
{
    private readonly float Duration = 0.3f;

    private RectTransform _rectTransform;


    private void Awake() 
    {
        _rectTransform = GetComponent<RectTransform>();
    }


    public void OnEnter()
    {
        _rectTransform.DOScale(Vector3.one * 1.2f, Duration).SetEase(Ease.OutCirc);
    }

    public void OnExit()
    {
        _rectTransform.DOScale(Vector3.one, Duration).SetEase(Ease.OutCirc);
    }

    public void OnClick()
    {
        _rectTransform.DOScale(Vector3.one * 2.0f, Duration).SetEase(Ease.OutCirc);
    }
}
