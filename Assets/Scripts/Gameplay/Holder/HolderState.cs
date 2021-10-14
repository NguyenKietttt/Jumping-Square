using System;

public class HolderState : StateBase
{
    private Action<object> _onTitleToGameplayRef;


    private void Awake()
    {
        CacheEvents();
    }

    private void OnEnable() 
    {
        EventDispatcher.RegisterListener(EventsID.TITLE_TO_GAMEPLAY_STATE, _onTitleToGameplayRef);
    }

    private void OnDisable()
    {
        EventDispatcher.RemoveListener(EventsID.TITLE_TO_GAMEPLAY_STATE, _onTitleToGameplayRef);
    }


    public override void OnTitleToGameplay()
    {
        EventDispatcher.PostEvent(EventsID.SHOW_HOLDER);
    }


    private void CacheEvents()
    {
        _onTitleToGameplayRef = (param) => OnTitleToGameplay();
    }
}
