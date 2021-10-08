using System;
using UnityEngine;

public class ScoreController : StateBase
{
    public static event Action<int> displayScoreEvent;


    private int _totalScore;
    private bool _isStartJump;


    private void OnEnable()
    {
        StateController.OnTitleEvent += OnTitleMenu;

        SquareCollision.OnBorderCollideEvent += param => UpdateScore();
    }

    private void OnDisable()
    {
        StateController.OnTitleEvent -= OnTitleMenu;

        SquareCollision.OnBorderCollideEvent -= param => UpdateScore();
    }


    public override void OnTitleMenu()
    {
        _totalScore = 0;
        _isStartJump = true;
    }


    private void UpdateScore()
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
