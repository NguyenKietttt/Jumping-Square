using UnityEngine;

public class UIPanel : StateBase
{
    [Header("Title HUD")]
    [SerializeField] private GameObject _titleCanvas;
    [SerializeField] private RectTransform _titlePanel;

    [Header("Gameplay HUD")]
    [SerializeField] private GameObject _gameplayCanvas;
    [SerializeField] private RectTransform _gameplayPanel;

    [Header("Gameplay HUD")]
    [SerializeField] private GameObject _gameoverCanvas;
    [SerializeField] private RectTransform _gameoverPanel;

    [Header("Validation")]
    [SerializeField] private bool _isFailedConfig;


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
        HideGameplayHUD();
        HideGameoverHUD();

        ShowTitleHUD();
    }

    public override void OnTitleToGameplay()
    {
        HideTitleHUD();
    }

    public override void OnGameplay()
    {
        ShowGameplayHUD();
    }

    public override void OnGameOver()
    {
        HideGameplayHUD();

        ShowGameoverHUD();
    }


    #region Title Panel

    private void ShowTitleHUD()
    {
        _titleCanvas.SetActive(true);
    }

    private void HideTitleHUD()
    {
        _titleCanvas.SetActive(false);
    }

    #endregion

    #region Gameplay Panel

    private void ShowGameplayHUD()
    {
        _gameplayCanvas.SetActive(true);
    }

    private void HideGameplayHUD()
    {
        _gameplayCanvas.SetActive(false);
    }

    #endregion

    #region Gameover Panel

    private void ShowGameoverHUD()
    {
        _gameoverCanvas.SetActive(true);
    }

    private void HideGameoverHUD()
    {
        _gameoverCanvas.SetActive(false);
    }

    #endregion
}
