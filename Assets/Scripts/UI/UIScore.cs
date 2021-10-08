using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class UIScore : StateBase
{
    private readonly Vector3 vector90 = new Vector3(0, 90.0f, 0);


    [Header("Configs")]
    [SerializeField] private ScoreSO _scoreSO;

    [Header("References")]
    [SerializeField] private GameObject _score;

    [Header("Validation")]
    [SerializeField] private bool _isFailedConfig;

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
    }

    private void OnEnable()
    {
        StateController.OnTitleEvent += OnTitleMenu;

        ScoreController.displayScoreEvent += param => StartCoroutine(DisplayScore(param));
    }

    private void OnDisable()
    {
        StateController.OnTitleEvent -= OnTitleMenu;

        ScoreController.displayScoreEvent -= param => StartCoroutine(DisplayScore(param));
    }

    
    public override void OnTitleMenu()
    {
        _scoreText.text = "0";
    }


    private IEnumerator DisplayScore(int score)
    {
        _scoreTextRect.DORotate(_scoreTextRect.eulerAngles + vector90, _scoreSO.RotateTime);

        yield return new WaitForSeconds(_scoreSO.RotateTime);

        _scoreText.text = score.ToString();

        ChangeTextColor(score);

        _scoreTextRect.DORotate(_scoreTextRect.eulerAngles - vector90, _scoreSO.RotateTime);
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
}
