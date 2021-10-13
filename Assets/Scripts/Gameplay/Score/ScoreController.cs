using System;

public class ScoreController : StateBase
{
    private Action<object> _onTitleRef, _collidedSquareRef;


    private int _totalScore;
    private bool _isStartJump;


    private void Awake()
    {
        CacheEvents();
    }

    private void OnEnable()
    {
        EventDispatcher.RegisterListener(EventsID.TITLE_STATE, _onTitleRef);
        EventDispatcher.RegisterListener(EventsID.COLLIDED_SQUARE, _collidedSquareRef);
    }

    private void OnDisable()
    {
        EventDispatcher.RemoveListener(EventsID.TITLE_STATE, _onTitleRef);
        EventDispatcher.RemoveListener(EventsID.COLLIDED_SQUARE, _collidedSquareRef);
    }


    public override void OnTitle()
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

        EventDispatcher.PostEvent(EventsID.DISPLAY_SCORE, _totalScore);
    }

    private void CacheEvents()
    {
        _onTitleRef = (param) => OnTitle();

        _collidedSquareRef = (param) => UpdateScore();
    }
}
