using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SoundButton : MonoBehaviour, IButtonAction
{
    private readonly float Duration = 0.3f;
    private Vector3 VECTOR_90 = new Vector3(0, 90.0f, 0);


    [SerializeField] private List<Sprite> _soundBtnSprites;

    private RectTransform _rectTransform;
    private Image _imageSrc;
    private bool _isSoundOn;


    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _imageSrc = GetComponent<Image>();

        _isSoundOn = true;
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
        DOTween.Sequence()
            .Append(_rectTransform.DORotate(_rectTransform.eulerAngles + VECTOR_90, Duration))
            .AppendCallback(() => ChangeSoundIcon())
            .OnComplete(() => _rectTransform.DORotate(_rectTransform.eulerAngles - VECTOR_90, Duration));
    }


    private void ChangeSoundIcon()
    {
        if (_isSoundOn)
        {
            _imageSrc.sprite = _soundBtnSprites[1];
            _isSoundOn = false;
        }
        else
        {
            _imageSrc.sprite = _soundBtnSprites[0];
            _isSoundOn = true;
        }
    }
}