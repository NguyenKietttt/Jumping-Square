using TMPro;
using UnityEngine;

public class UIScore : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI _scoreText;

    [Header("Validation")]
    [SerializeField] private bool _isFailedConfig;


    private void OnValidate()
    {
        CustomLogs.Instance.Warning(_scoreText == null, "scoreText is missing!!!");

        _isFailedConfig = _scoreText == null;
    }

    private void Awake()
    {
        if (_isFailedConfig)
            enabled = false;
    }

    private void OnEnable()
    {
        ScoreController.displayScoreEvent += param => DisplayScore(param);
    }

    private void OnDisable()
    {
        ScoreController.displayScoreEvent -= param => DisplayScore(param);
    }


    private void DisplayScore(int score)
    {
        _scoreText.text = score.ToString();
    }
}
