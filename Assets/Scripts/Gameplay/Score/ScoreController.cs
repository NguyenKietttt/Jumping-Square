using System;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    private Action<object> _resetScoreRef, _collidedSquareRef;


    private int _totalScore;
    private bool _isStartJump;


    private void Awake()
    {
        CacheEvents();
    }

    private void OnEnable()
    {
        EventDispatcher.RegisterListener(EventsID.RESET_SCORE, _resetScoreRef);
        EventDispatcher.RegisterListener(EventsID.COLLIDED_SQUARE, _collidedSquareRef);
    }

    private void OnDisable()
    {
        EventDispatcher.RemoveListener(EventsID.RESET_SCORE, _resetScoreRef);
        EventDispatcher.RemoveListener(EventsID.COLLIDED_SQUARE, _collidedSquareRef);
    }


    public void ResetScore(object score)
    {
        CustomLogs.Instance.Log("<color=green> Listen " + EventsID.RESET_SCORE + "</color>");

        _totalScore = (int) score;
        _isStartJump = true;
    }


    private void UpdateScore()
    {
        if (_isStartJump)
        {
            _isStartJump = false;
            return;
        }

        if (_totalScore <= 99)
            _totalScore++;
        

        EventDispatcher.PostEvent(EventsID.DISPLAY_SCORE, _totalScore);
    }

    private void CacheEvents()
    {
        _resetScoreRef = (param) => ResetScore(param);
        _collidedSquareRef = (param) => UpdateScore();
    }
}
