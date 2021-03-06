using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SoundButton : MonoBehaviour, IButtonAction
{
    private readonly float Duration = 0.3f;
    private readonly Vector3 VECTOR_90 = new Vector3(0, 90.0f, 0);


    [Header("Configs")]
    [SerializeField] private ButtonSO _buttonSO;

    [Header("References")]
    [SerializeField] private Sprite[] _soundBtnSprites;
    [SerializeField] private GraphicRaycaster _raycasterTitle;

    [Header("Validation")]
    [SerializeField] private bool _isFailedConfig;


    private RectTransform _rectTransform;
    private Image _imageSrc;
    private bool _isSoundOn;


    private void OnValidate()
    {
        CustomLogs.Instance.Warning(_buttonSO == null, "buttonSO is missing!!!");

        _isFailedConfig = _buttonSO == null;
    }

    private void Awake()
    {
        if (_isFailedConfig)
            enabled = false;

        CacheComponents();

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
        EventDispatcher.PostEvent(EventsID.BUTTON_SOUND_SFX, _buttonSO.GetSFXByName("Click"));

        DOTween.Sequence()
            .OnStart(() => _raycasterTitle.enabled = false)
            .Append(_rectTransform.DORotate(_rectTransform.eulerAngles + VECTOR_90, Duration))
            .AppendCallback(() => ChangeSoundIcon())
            .OnComplete(() => _rectTransform.DORotate(_rectTransform.eulerAngles - VECTOR_90, Duration));

        DOTween.Sequence()
            .AppendInterval(0.5f)
            .AppendCallback(() => _raycasterTitle.enabled = true);
    }


    private void ChangeSoundIcon()
    {
        if (_isSoundOn)
            _imageSrc.sprite = _soundBtnSprites[1];
        else
            _imageSrc.sprite = _soundBtnSprites[0];

        _isSoundOn = !_isSoundOn;
    }

    private void CacheComponents()
    {
        _rectTransform = GetComponent<RectTransform>();
        _imageSrc = GetComponent<Image>();
    }
}
