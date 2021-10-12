using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class SpikeController : StateBase
{
    private readonly float OLD_SPIKE_POSITION = -4.5f;
    private readonly float NEW_SPIKE_POSITION = -3.5f;


    [Header("Configs")]
    [SerializeField] private SpikeSO _spikeSO;

    [Header("References")]
    [SerializeField] private GameObject _leftSpikeHolder;
    [SerializeField] private GameObject _rightSpikeHolder;

    [Header("Validation")]
    [SerializeField] private bool _isFailedConfig;

    private Action<object> _onTitleRef, _onGameplayToGameoverRef, _displayScoreRef, _collidedSquareRef;
    private List<Transform> _leftSpikes, _rightSpikes;
    private int _spikesPerLevel;


    private void OnValidate()
    {
        CustomLogs.Instance.Warning(_spikeSO == null, "spikeSO is missing!!!");

        CustomLogs.Instance.Warning(_leftSpikeHolder == null, "leftSpikeHolder is missing!!!");
        CustomLogs.Instance.Warning(_rightSpikeHolder == null, "rightSpikeHolder is missing!!!");

        _isFailedConfig = _spikeSO == null || _leftSpikeHolder == null || _rightSpikeHolder == null;
    }

    private void Awake()
    {
        if (_isFailedConfig)
            enabled = false;

        _leftSpikes = GetHolderChilds(_leftSpikeHolder);
        _rightSpikes = GetHolderChilds(_rightSpikeHolder);

        CacheEvents();
    }

    private void OnEnable()
    {
        EventDispatcher.RegisterListener(EventsID.TITLE_STATE, _onTitleRef);
        EventDispatcher.RegisterListener(EventsID.GAMEPLAY_TO_GAMEOVER_STATE, _onGameplayToGameoverRef);

        EventDispatcher.RegisterListener(EventsID.COLLIDED_SQUARE, _collidedSquareRef);
        EventDispatcher.RegisterListener(EventsID.DISPLAY_SCORE, _displayScoreRef);
    }

    private void OnDisable()
    {
        EventDispatcher.RemoveListener(EventsID.TITLE_STATE, _onTitleRef);
        EventDispatcher.RemoveListener(EventsID.GAMEPLAY_TO_GAMEOVER_STATE, _onGameplayToGameoverRef);

        EventDispatcher.RemoveListener(EventsID.COLLIDED_SQUARE, _collidedSquareRef);
        EventDispatcher.RemoveListener(EventsID.DISPLAY_SCORE, _displayScoreRef);
    }


    public override void OnTitle()
    {
        _spikesPerLevel = 1;
    }

    public override void OnGameplayToGameover()
    {
        DOTween.Sequence()
            .AppendCallback(() => ReturnSpike(_leftSpikes))
            .AppendCallback(() => ReturnSpike(_rightSpikes))
            .AppendInterval(1.3f)
            .OnComplete(() => EventDispatcher.PostEvent(EventsID.HIDE_HOLDER));
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

    private void SetSpikeSpawnedByScore(object score)
    {
        var intScore = (int)score;

        if (intScore >= 5 && intScore < 10)
            _spikesPerLevel = 2;
        else if (intScore >= 10 && intScore < 15)
            _spikesPerLevel = 3;
        else if (intScore >= 15 && intScore < 100)
            _spikesPerLevel = 4;
    }

    private void MoveSelectedSpike(object isCollided)
    {
        var boolIsCollided = (bool)isCollided;

        if (boolIsCollided)
        {
            SpawnSpikes(_leftSpikes);
            ReturnSpike(_rightSpikes);
        }
        else
        {
            SpawnSpikes(_rightSpikes);
            ReturnSpike(_leftSpikes);
        }
    }

    private void SpawnSpikes(List<Transform> spikes)
    {
        EventDispatcher.PostEvent(EventsID.SPIKE_SFX, _spikeSO.GetSFXByName("Spike"));

        int spikeIndex;

        for (int i = 0; i < _spikesPerLevel; i++)
        {
            do
            {
                spikeIndex = UnityEngine.Random.Range(0, spikes.Count);
            }
            while (spikes[spikeIndex].tag == "OpenSpike");

            spikes[spikeIndex].tag = "OpenSpike";
            spikes[spikeIndex].DOLocalMoveY(NEW_SPIKE_POSITION, _spikeSO.SpawnDeday).SetEase(Ease.OutQuad);
        }
    }

    private void ReturnSpike(List<Transform> spikes)
    {
        EventDispatcher.PostEvent(EventsID.SPIKE_SFX, _spikeSO.GetSFXByName("Spike"));

        foreach (var spike in spikes)
        {
            if (spike.localPosition.y != OLD_SPIKE_POSITION)
            {
                spike.tag = "Spike";
                spike.DOLocalMoveY(OLD_SPIKE_POSITION, _spikeSO.SpawnDeday);
            }
        }
    }

    private void CacheEvents()
    {
        _onTitleRef = (param) => OnTitle();
        _onGameplayToGameoverRef = (param) => OnGameplayToGameover();

        _displayScoreRef = (param) => SetSpikeSpawnedByScore(param);

        _collidedSquareRef = (param) => MoveSelectedSpike(param);
    }
}
