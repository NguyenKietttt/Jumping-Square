using DG.Tweening;
using UnityEngine;

public class PlayButton : MonoBehaviour, IButtonAction
{
    private readonly float Duration = 0.3f;


    [Header("Configs")]
    [SerializeField] private ButtonSO _buttonSO;

    [Header("Validation")]
    [SerializeField] private bool _isFailedConfig;

    private RectTransform _rectTransform;


    private void OnValidate()
    {
        CustomLogs.Instance.Warning(_buttonSO == null, "buttonSO is missing!!!");

        _isFailedConfig = _buttonSO == null;
    }

    private void Awake() 
    {
        if (_isFailedConfig)
            enabled = false;

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
        EventDispatcher.PostEvent(EventsID.BUTTON_PLAY_SFX, _buttonSO.GetSFXByName("Click"));
        
        _rectTransform.DOScale(Vector3.one * 2.0f, Duration).SetEase(Ease.OutCirc);
    }
}
