using System;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    public static event Action<int> displayScoreEvent;


    private int _totalScore;
    private bool _isStartJump;


    private void Awake() 
    {
        _totalScore = 0;
        _isStartJump = true;
    }

    private void OnEnable()
    {
        SquareCollision.OnBorderCollideEvent += param => UpdateScore();
    }

    private void OnDisable()
    {
        SquareCollision.OnBorderCollideEvent -= param => UpdateScore();
    }


    public void UpdateScore()
    {
        if (_isStartJump)
        {
            _isStartJump = false;
            return;
        }
        
        _totalScore++;

        displayScoreEvent?.Invoke(_totalScore);
    }
}
