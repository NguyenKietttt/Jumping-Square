using UnityEngine;

public class UIPanel : StateBase
{
    [Header("Title HUD")]
    [SerializeField] private GameObject _titleCanvas;
    [SerializeField] private RectTransform _titlePanel;

    [Header("Gameplay HUD")]
    [SerializeField] private GameObject _gameplayCanvas;
    [SerializeField] private RectTransform _gameplayPanel;

    [Header("Validation")]
    [SerializeField] private bool _isFailedConfig;


    private void OnValidate()
    {
        CustomLogs.Instance.Warning(_titleCanvas == null, "titleCanvas is missing!!!");
        CustomLogs.Instance.Warning(_titlePanel == null, "titlePanel is missing!!!");

        CustomLogs.Instance.Warning(_gameplayCanvas == null, "gameplayCanvas is missing!!!");
        CustomLogs.Instance.Warning(_gameplayPanel == null, "gameplayPanel is missing!!!");

        _isFailedConfig = _titleCanvas == null || _titlePanel == null || _gameplayCanvas == null 
            || _gameplayPanel == null;
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
    }

    private void OnDisable() 
    {
        StateController.OnTitleEvent -= OnTitleMenu;
        StateController.OnTitleToGameplayEvent -= OnTitleToGameplay;
    }


    public override void OnTitleMenu()
    {
        HideGameplayPanel();
        ShowTitlePanel();
    }

    public override void OnTitleToGameplay()
    {
        HideTitlePanel();
    }


    #region Title Panel

    private void ShowTitlePanel()
    {
        _titleCanvas.SetActive(true);
    }

    private void HideTitlePanel()
    {
        _titleCanvas.SetActive(false);
    }

    #endregion

    #region Gameplay Panel

    private void ShowGameplayPanel()
    {
         _gameplayCanvas.SetActive(true);
    }

    private void HideGameplayPanel()
    {
        _gameplayCanvas.SetActive(false);
    }

    #endregion
}
