using System;

public class ScoreState : StateBase
{
    private Action<object> _onTitleRef;


    private void Awake()
    {
        CacheEvents();
    }

    private void OnEnable()
    {
        EventDispatcher.RegisterListener(EventsID.TITLE_STATE, _onTitleRef);
    }

    private void OnDisable()
    {
        EventDispatcher.RemoveListener(EventsID.TITLE_STATE, _onTitleRef);
    }


    public override void OnTitle()
    {
        CustomLogs.Instance.Log("<color=green> Listen " + EventsID.TITLE_STATE + "</color>");
        CustomLogs.Instance.Log("<color=green> Start " + EventsID.RESET_SCORE + "</color>");

        EventDispatcher.PostEvent(EventsID.RESET_SCORE, 0);
    }


    private void CacheEvents()
    {
        _onTitleRef = (param) => OnTitle();
    }
}
