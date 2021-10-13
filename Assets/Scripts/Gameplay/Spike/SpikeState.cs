using System;
using System.Collections.Generic;
using UnityEngine;

public class SpikeState : StateBase
{
    private Action<object> _onTitleRef, _onGameplayToGameoverRef;


    [Header("References")]
    [SerializeField] private GameObject _leftSpikeHolder;
    [SerializeField] private GameObject _rightSpikeHolder;

    [Header("Validation")]
    [SerializeField] private bool _isFailedConfig;

    private List<Transform> _leftSpikes, _rightSpikes;
    private int _spikesPerLevel;


    private void OnValidate()
    {
        CustomLogs.Instance.Warning(_leftSpikeHolder == null, "leftSpikeHolder is missing!!!");
        CustomLogs.Instance.Warning(_rightSpikeHolder == null, "rightSpikeHolder is missing!!!");

        _isFailedConfig = _leftSpikeHolder == null || _rightSpikeHolder == null;
    }

    private void Awake()
    {
        if (_isFailedConfig)
            enabled = false;

        CacheCallbacks();
    }

    private void OnEnable()
    {
        EventDispatcher.RegisterListener(EventsID.TITLE_STATE, _onTitleRef);
        EventDispatcher.RegisterListener(EventsID.GAMEPLAY_TO_GAMEOVER_STATE, _onGameplayToGameoverRef);
    }

    private void OnDisable()
    {
        EventDispatcher.RemoveListener(EventsID.TITLE_STATE, _onTitleRef);
        EventDispatcher.RemoveListener(EventsID.GAMEPLAY_TO_GAMEOVER_STATE, _onGameplayToGameoverRef);
    }


    public override void OnTitle()
    {
        if (_leftSpikes == null && _rightSpikes == null)
        {
            _leftSpikes = GetHolderChilds(_leftSpikeHolder);
            _rightSpikes = GetHolderChilds(_rightSpikeHolder);

            List<List<Transform>> spikesChild = new List<List<Transform>>
            {
                _leftSpikes, _rightSpikes
            };

            EventDispatcher.PostEvent(EventsID.GET_SPIKE_CHILD, spikesChild);
        }

        EventDispatcher.PostEvent(EventsID.SET_SPIKE_TO_SPAWN, 1);
    }

    public override void OnGameplayToGameover()
    {
        EventDispatcher.PostEvent(EventsID.HIDE_SPIKE);
    }


    private List<Transform> GetHolderChilds(GameObject holder)
    {
        var childs = new List<Transform>();

        for (int i = 0; i < holder.transform.childCount; i++)
        {
            childs.Add(holder.transform.GetChild(i).transform);
        }

        return childs;
    }

    private void CacheCallbacks()
    {
        _onTitleRef = (param) => OnTitle();
        _onGameplayToGameoverRef = (param) => OnGameplayToGameover();
    }
}
