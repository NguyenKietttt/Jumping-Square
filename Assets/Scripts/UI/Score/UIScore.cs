using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class UIScore : StateBase
{
    private readonly Vector3 VECTOR_90 = new Vector3(0, 90.0f, 0);


    [Header("Configs")]
    [SerializeField] private ScoreSO _scoreSO;

    [Header("References")]
    [SerializeField] private GameObject _score;

    [Header("Validation")]
    [SerializeField] private bool _isFailedConfig;

    private Action<object> _onTitleRef, _displayScoreRef;
    private TextMeshProUGUI _scoreText;
    private RectTransform _scoreTextRect;


    private void OnValidate()
    {
        CustomLogs.Instance.Warning(_scoreSO == null, "scoreSO is missing!!!");
        CustomLogs.Instance.Warning(_score == null, "scoreGameObject is missing!!!");

        _isFailedConfig = _score == null;
    }

    private void Awake()
    {
        if (_isFailedConfig)
            enabled = false;

        CacheComponents();
        CacheCallbacks();
    }

    private void OnEnable()
    {
        EventDispatcher.RegisterListener(EventsID.TITLE_STATE, _onTitleRef);

        EventDispatcher.RegisterListener(EventsID.DISPLAY_SCORE, _displayScoreRef);
    }

    private void OnDisable()
    {
        EventDispatcher.RemoveListener(EventsID.TITLE_STATE, _onTitleRef);

        EventDispatcher.RemoveListener(EventsID.DISPLAY_SCORE, _displayScoreRef);
    }


    public override void OnTitle()
    {
        _scoreText.text = "0";
        _scoreText.color = _scoreSO.NormalScore;
    }


    private void DisplayScore(object score)
    {
        var intScore = (int)score;
        
        DOTween.Sequence()
            .Append(_scoreTextRect.DORotate(_scoreTextRect.eulerAngles + Vector3.up * 90.0f, _scoreSO.RotateTime))
            .AppendCallback(() =>
            {
                _scoreText.text = score.ToString();

                ChangeTextColor(intScore);
            })
            .OnComplete(() =>
            {
                _scoreTextRect.DORotate(_scoreTextRect.eulerAngles - Vector3.up * 90.0f, _scoreSO.RotateTime);
            });
    }

    private void ChangeTextColor(int score)
    {
        if (score == 5 || score == 10 || score == 15)
            _scoreText.DOColor(_scoreSO.MilestoneScore, 1.0f);
        else
            _scoreText.DOColor(_scoreSO.NormalScore, 1.0f);
    }

    private void CacheComponents()
    {
        _scoreText = _score.GetComponent<TextMeshProUGUI>();
        _scoreTextRect = _score.GetComponent<RectTransform>();
    }

    private void CacheCallbacks()
    {
        _onTitleRef = (param) => OnTitle();

        _displayScoreRef = (param) => DisplayScore(param);
    }
}
