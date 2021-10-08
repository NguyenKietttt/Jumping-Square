using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIPanel : StateBase
{
    [Header("Title HUD")]
    [SerializeField] private GameObject _titleCanvas;
    [SerializeField] private RectTransform _titlePanel;

    [Header("Gameplay HUD")]
    [SerializeField] private GameObject _gameplayCanvas;
    [SerializeField] private RectTransform _gameplayPanel;

    [Header("Gameover HUD")]
    [SerializeField] private GameObject _gameoverCanvas;
    [SerializeField] private RectTransform _gameoverPanel;

    [Header("Validation")]
    [SerializeField] private bool _isFailedConfig;

    private GraphicRaycaster _raycasterTitle, _raycasterGameover;

    private bool _isFirstPlay;


    private void OnValidate()
    {
        CustomLogs.Instance.Warning(_titleCanvas == null, "titleCanvas is missing!!!");
        CustomLogs.Instance.Warning(_titlePanel == null, "titlePanel is missing!!!");

        CustomLogs.Instance.Warning(_gameplayCanvas == null, "gameplayCanvas is missing!!!");
        CustomLogs.Instance.Warning(_gameplayPanel == null, "gameplayPanel is missing!!!");

        CustomLogs.Instance.Warning(_gameoverCanvas == null, "gameoverCanvas is missing!!!");
        CustomLogs.Instance.Warning(_gameoverPanel == null, "gameoverPanel is missing!!!");

        _isFailedConfig = _titleCanvas == null || _titlePanel == null || _gameplayCanvas == null
            || _gameplayPanel == null || _gameoverCanvas == null || _gameoverPanel == null;
    }

    private void Awake()
    {
        if (_isFailedConfig)
            enabled = false;

        CacheComponents();

        _isFirstPlay = true;
    }

    private void Start()
    {
        StateController.RaiseTitleEvent();
    }

    private void OnEnable()
    {
        StateController.OnTitleEvent += OnTitleMenu;
        StateController.OnTitleToGameplayEvent += OnTitleToGameplay;

        StateController.OnGameplayEvent += OnGameplay;

        StateController.OnGameoverEvent += OnGameOver;
    }

    private void OnDisable()
    {
        StateController.OnTitleEvent -= OnTitleMenu;
        StateController.OnTitleToGameplayEvent -= OnTitleToGameplay;

        StateController.OnGameplayEvent -= OnGameplay;

        StateController.OnGameoverEvent -= OnGameOver;
    }


    public override void OnTitleMenu()
    {
        if (!_isFirstPlay)
        {
            Sequence titleSeq = DOTween.Sequence()
                .AppendCallback(() => HideGameoverHUD())
                .AppendInterval(1.2f)
                .OnComplete(() => ShowTitleHUD());
        }
        else
        {
            _isFirstPlay = false;

            ShowTitleHUD();
        }
    }

    public override void OnGameplay()
    {
        ShowGameplayHUD();
    }

    public override void OnGameOver()
    {
        Sequence gameoverSeq = DOTween.Sequence()
            .AppendCallback(() => HideGameplayHUD())
            .AppendInterval(1.2f)
            .OnComplete(() => ShowGameoverHUD());
    }


    #region Title Panel

    private void ShowTitleHUD()
    {
        Sequence showSeq = DOTween.Sequence()
            .OnStart(() =>
            {
                _titleCanvas.SetActive(true);
                _titlePanel.DOScale(Vector3.one, 1.0f).SetEase(Ease.OutBack);
            })
            .AppendInterval(1.2f)
            .OnComplete(() => _raycasterTitle.enabled = true);
    }

    public void HideTitleHUD()
    {
        Sequence hideSeq = DOTween.Sequence()
            .OnStart(() => _raycasterTitle.enabled = false)
            .Append(_titlePanel.DOScale(Vector3.zero, 0.8f).SetEase(Ease.InBack))
            .AppendCallback(() => _titleCanvas.SetActive(false))
            .OnComplete(() => StateController.RaiseTitleToGameplayEvent());
    }

    #endregion

    #region Gameplay Panel

    private void ShowGameplayHUD()
    {
        Sequence showSeq = DOTween.Sequence()
            .OnStart(() => _gameplayCanvas.SetActive(true))
            .Append(_gameplayPanel.DOAnchorPos(Vector2.zero, 0.8f).SetEase(Ease.OutBack));
    }

    private void HideGameplayHUD()
    {
        Sequence hideSeq = DOTween.Sequence()
            .Append(_gameplayPanel.DOAnchorPos(new Vector2(0.0f, 1000.0f), 0.8f).SetEase(Ease.InBack))
            .OnComplete(() => _gameplayCanvas.SetActive(false));
    }

    #endregion

    #region Gameover Panel

    private void ShowGameoverHUD()
    {
        Sequence showSeq = DOTween.Sequence()
            .OnStart(() =>
            {
                _gameoverCanvas.SetActive(true);
                _gameoverPanel.DOScale(Vector3.one, 1.0f).SetEase(Ease.OutBack);
            })
            .AppendInterval(1.2f)
            .OnComplete(() => _raycasterGameover.enabled = true);
    }

    private void HideGameoverHUD()
    {
        Sequence hideSeq = DOTween.Sequence()
            .OnStart(() => _raycasterGameover.enabled = false)
            .Append(_gameoverPanel.DOScale(Vector3.zero, 0.8f).SetEase(Ease.InBack))
            .OnComplete(() => _gameoverCanvas.SetActive(false));
    }

    #endregion

    private void CacheComponents()
    {
        _raycasterTitle = _titleCanvas.GetComponent<GraphicRaycaster>();
        _raycasterGameover = _gameoverCanvas.GetComponent<GraphicRaycaster>();
    }
}
