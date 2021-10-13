using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;
using System;

public class UIPanelController : StateBase
{
    [Header("Configs")]
    [SerializeField] private PanelSO _panelSO;
    [SerializeField] private ButtonSO _buttonSO;

    [Header("Title HUD")]
    [SerializeField] private GraphicRaycaster _raycasterTitle;
    [SerializeField] private RectTransform _titlePanel;
    [SerializeField] private List<Transform> _titleButtons;

    [Header("Gameplay HUD")]
    [SerializeField] private RectTransform _gameplayPanel;

    [Header("Gameover HUD")]
    [SerializeField] private GraphicRaycaster _raycasterGameover;
    [SerializeField] private RectTransform _gameoverPanel;
    [SerializeField] private List<Transform> _gameoverButtons;

    [Header("Validation")]
    [SerializeField] private bool _isFailedConfig;

    private Action<object> _onTitleRef, _onTitleToGameplayRef, _onGameplayRef, _onGameoverRef;
    private bool _isFirstPlay;


    private void OnValidate()
    {
        CustomLogs.Instance.Warning(_panelSO == null, "panelSO is missing!!!");
        CustomLogs.Instance.Warning(_buttonSO == null, "buttonSO is missing!!!");

        CustomLogs.Instance.Warning(_raycasterTitle == null, "raycasterTitle is missing!!!");
        CustomLogs.Instance.Warning(_titlePanel == null, "titlePanel is missing!!!");

        CustomLogs.Instance.Warning(_gameplayPanel == null, "gameplayPanel is missing!!!");

        CustomLogs.Instance.Warning(_raycasterGameover == null, "raycasterGameover is missing!!!");
        CustomLogs.Instance.Warning(_gameoverPanel == null, "gameoverPanel is missing!!!");

        _isFailedConfig = _panelSO == null || _buttonSO == null || _raycasterTitle == null
            || _titlePanel == null || _gameplayPanel == null || _raycasterGameover == null 
            || _gameoverPanel == null;
    }

    private void Awake()
    {
        if (_isFailedConfig)
            enabled = false;

        CacheEvents();

        _isFirstPlay = true;
    }

    private void Start()
    {
        StateController.RaiseTitleState();
    }

    private void OnEnable()
    {
        EventDispatcher.RegisterListener(EventsID.TITLE_STATE, _onTitleRef);
        EventDispatcher.RegisterListener(EventsID.TITLE_TO_GAMEPLAY_STATE, _onTitleToGameplayRef);
        EventDispatcher.RegisterListener(EventsID.GAMEPLAY_STATE, _onGameplayRef);
        EventDispatcher.RegisterListener(EventsID.GAMEOVER_STATE, _onGameoverRef);
    }

    private void OnDisable()
    {
        EventDispatcher.RemoveListener(EventsID.TITLE_STATE, _onTitleRef);
        EventDispatcher.RemoveListener(EventsID.TITLE_TO_GAMEPLAY_STATE, _onTitleToGameplayRef);
        EventDispatcher.RemoveListener(EventsID.GAMEPLAY_STATE, _onGameplayRef);
        EventDispatcher.RemoveListener(EventsID.GAMEOVER_STATE, _onGameoverRef);
    }


    public override void OnTitle()
    {
        if (!_isFirstPlay)
        {
            DOTween.Sequence().AppendCallback(() =>
                {
                    HideHUD(_gameoverPanel, _gameoverButtons, _raycasterGameover,
                        () => { });
                })
                .AppendInterval(1.2f)
                .OnComplete(() =>
                {
                    ShowHUD(_titlePanel, _titleButtons,
                        () => _raycasterTitle.enabled = true);
                });
        }
        else
        {
            _isFirstPlay = false;

            ShowHUD(_titlePanel, _titleButtons,
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
                ShowHUD(_gameoverPanel, _gameoverButtons,
                    () => _raycasterGameover.enabled = true);
            });
    }


    #region Title Panel & Gameover Panel

    private void ShowHUD(RectTransform panel, List<Transform> buttons, TweenCallback callback)
    {
        EventDispatcher.PostEvent(EventsID.PANEL_SFX, _panelSO.GetSFXByName("Show"));

        DOTween.Sequence()
            .OnStart(() =>panel.DOScale(Vector3.one, 1.0f).SetEase(Ease.OutBack))
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
        HideHUD(_titlePanel, _titleButtons, _raycasterTitle,
            () => StateController.RaiseTitleToGameplayState());
    }

    private void HideHUD(RectTransform panel, List<Transform> buttons,
        GraphicRaycaster raycaster, TweenCallback callback)
    {
        DOTween.Sequence()
            .AppendInterval(0.1f)
            .OnStart(() => raycaster.enabled = false)
            .AppendCallback(() => StartCoroutine(HideButtons(buttons)))
            .AppendInterval(0.5f)
            .AppendCallback(() => EventDispatcher.PostEvent(EventsID.PANEL_SFX, _panelSO.GetSFXByName("Hide")))
            .Append(panel.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack))
            .OnComplete(() => callback());
    }

    private IEnumerator ShowButtons(List<Transform> buttons)
    {
        foreach (var button in buttons)
        {
            EventDispatcher.PostEvent(EventsID.PANEL_SFX, _buttonSO.GetSFXByName("Show"));

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
            .Append(_gameplayPanel.DOAnchorPos(Vector2.zero, 0.8f).SetEase(Ease.OutBack));
    }

    private void HideGameplayHUD()
    {
        DOTween.Sequence()
            .Append(_gameplayPanel.DOAnchorPos(new Vector2(0.0f, 1000.0f), 0.8f).SetEase(Ease.InBack));
    }

    #endregion


    private void CacheEvents()
    {
        _onTitleRef = (param) => OnTitle();
        _onTitleToGameplayRef = (param) => OnTitleToGameplay();
        _onGameplayRef = (param) => OnGameplay();
        _onGameoverRef = (param) => OnGameOver();
    }
}
