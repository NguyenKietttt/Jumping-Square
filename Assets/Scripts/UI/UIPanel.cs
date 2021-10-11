using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;

public class UIPanel : StateBase
{
    [Header("Title HUD")]
    [SerializeField] private GameObject _titleCanvas;
    [SerializeField] private RectTransform _titlePanel;
    [SerializeField] private List<Transform> _titleButtons;

    [Header("Gameplay HUD")]
    [SerializeField] private GameObject _gameplayCanvas;
    [SerializeField] private RectTransform _gameplayPanel;

    [Header("Gameover HUD")]
    [SerializeField] private GameObject _gameoverCanvas;
    [SerializeField] private RectTransform _gameoverPanel;
    [SerializeField] private List<Transform> _gameoverButtons;

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
            DOTween.Sequence()
                .AppendCallback(() =>
                {
                    HideHUD(_gameoverCanvas, _gameoverPanel, _gameoverButtons, _raycasterGameover,
                        () => { });
                })
                .AppendInterval(1.2f)
                .OnComplete(() =>
                {
                    ShowHUD(_titleCanvas, _titlePanel, _titleButtons,
                        () => _raycasterTitle.enabled = true);
                });
        }
        else
        {
            _isFirstPlay = false;

            ShowHUD(_titleCanvas, _titlePanel, _titleButtons,
                () => _raycasterTitle.enabled = true);
        }
    }

    public override void OnGameplay()
    {
        ShowGameplayHUD();
    }

    public override void OnGameOver()
    {
        DOTween.Sequence()
            .AppendCallback(() => HideGameplayHUD())
            .AppendInterval(1.2f)
            .OnComplete(() =>
            {
                ShowHUD(_gameoverCanvas, _gameoverPanel, _gameoverButtons,
                    () => _raycasterGameover.enabled = true);
            });
    }


    #region Title Panel & Gameover Panel

    private void ShowHUD(GameObject canvas, RectTransform panel,
        List<Transform> buttons, TweenCallback callback)
    {
        DOTween.Sequence()
            .OnStart(() =>
            {
                canvas.SetActive(true);
                panel.DOScale(Vector3.one, 1.0f).SetEase(Ease.OutBack);
            })
            .AppendInterval(1.2f)
            .AppendCallback(() => StartCoroutine(ShowButtons(buttons)))
            .AppendInterval(0.5f)
            .OnComplete(() => callback());
    }

    /// <summary>
    /// Raise by PlayButton in Hierarchy
    /// </summary>
    public void HideTitleHUD()
    {
        HideHUD(_titleCanvas, _titlePanel, _titleButtons, _raycasterTitle,
            () => StateController.RaiseTitleToGameplayEvent());
    }

    private void HideHUD(GameObject canvas, RectTransform panel, List<Transform> buttons,
        GraphicRaycaster raycaster, TweenCallback callback)
    {
        DOTween.Sequence()
            .AppendInterval(0.1f)
            .OnStart(() => raycaster.enabled = false)
            .AppendCallback(() => StartCoroutine(HideButtons(buttons)))
            .AppendInterval(0.5f)
            .Append(panel.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack))
            .AppendCallback(() => canvas.SetActive(false))
            .OnComplete(() => callback());
    }

    private IEnumerator ShowButtons(List<Transform> buttons)
    {
        foreach (var button in buttons)
        {
            DOTween.Sequence()
                .Append(button.DOScale(Vector3.one, 0.1f))
                .Append(button.DOPunchScale(Vector3.one * 0.6f, 0.3f, 6, 0.7f).SetEase(Ease.OutCirc));

            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator HideButtons(List<Transform> buttons)
    {
        for (int i = buttons.Count - 1; i >= 0; i--)
        {
            buttons[i].DOScale(Vector3.zero, 0.3f);

            yield return new WaitForSeconds(0.2f);
        }
    }

    #endregion

    #region Gameplay Panel

    private void ShowGameplayHUD()
    {
        DOTween.Sequence()
            .OnStart(() => _gameplayCanvas.SetActive(true))
            .Append(_gameplayPanel.DOAnchorPos(Vector2.zero, 0.8f).SetEase(Ease.OutBack));
    }

    private void HideGameplayHUD()
    {
        DOTween.Sequence()
            .Append(_gameplayPanel.DOAnchorPos(new Vector2(0.0f, 1000.0f), 0.8f).SetEase(Ease.InBack))
            .OnComplete(() => _gameplayCanvas.SetActive(false));
    }

    #endregion

    private void CacheComponents()
    {
        _raycasterTitle = _titleCanvas.GetComponent<GraphicRaycaster>();
        _raycasterGameover = _gameoverCanvas.GetComponent<GraphicRaycaster>();
    }
}
