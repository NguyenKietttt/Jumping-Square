using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class SpikeMovement : MonoBehaviour
{
    private readonly string SPIKE_TAG = "Spike";
    private readonly string OPEN_SPIKE_TAG = "OpenSpike";
    private readonly float OLD_SPIKE_POSITION = -4.5f;
    private readonly float NEW_SPIKE_POSITION = -3.5f;


    private Action<object> _setSpikeToSpawnRef, _displayScoreRef, _collidedSquareRef, _hideSpikeRef;


    [Header("Configs")]
    [SerializeField] private SpikeSO _spikeSO;

    [Header("Validation")]
    [SerializeField] private bool _isFailedConfig;

    private Transform[] _leftSpikes, _rightSpikes;
    private int _spikesPerLevel;


    private void OnValidate()
    {
        CustomLogs.Instance.Warning(_spikeSO == null, "spikeSO is missing!!!");

        _isFailedConfig = _spikeSO == null;
    }

    private void Awake()
    {
        if (_isFailedConfig)
            enabled = false;

        CacheCallbacks();
    }

    private void OnEnable()
    {
        EventDispatcher.RegisterListener(EventsID.GET_SPIKE_CHILD, (param) => GetSpikesChild(param));
        EventDispatcher.RegisterListener(EventsID.SET_SPIKE_TO_SPAWN, _setSpikeToSpawnRef);
        EventDispatcher.RegisterListener(EventsID.COLLIDED_SQUARE, _collidedSquareRef);
        EventDispatcher.RegisterListener(EventsID.DISPLAY_SCORE, _displayScoreRef);
        EventDispatcher.RegisterListener(EventsID.HIDE_SPIKE, _hideSpikeRef);
    }

    private void OnDisable()
    {
        EventDispatcher.RemoveListener(EventsID.GET_SPIKE_CHILD, (param) => GetSpikesChild(param));
        EventDispatcher.RemoveListener(EventsID.SET_SPIKE_TO_SPAWN, _setSpikeToSpawnRef);
        EventDispatcher.RemoveListener(EventsID.COLLIDED_SQUARE, _collidedSquareRef);
        EventDispatcher.RemoveListener(EventsID.DISPLAY_SCORE, _displayScoreRef);
        EventDispatcher.RemoveListener(EventsID.HIDE_SPIKE, _hideSpikeRef);
    }

    private void GetSpikesChild(object spikesChild)
    {
        CustomLogs.Instance.Log("<color=green> Listen " + EventsID.GET_SPIKE_CHILD + "</color>");

        var castedSpikesChild = (List<List<Transform>>) spikesChild;

        _leftSpikes = castedSpikesChild[0].ToArray();
        _rightSpikes = castedSpikesChild[1].ToArray();
    }

    private void SetSpikeToSpawn(object numberSpike)
    {
        CustomLogs.Instance.Log("<color=green> Listen " + EventsID.SET_SPIKE_TO_SPAWN + "</color>");
        
        var castedNumberSpike = (int) numberSpike;

        _spikesPerLevel = castedNumberSpike;
    }

    public void HideSpike()
    {
        DOTween.Sequence()
            .AppendCallback(() => ReturnSpike(_leftSpikes))
            .AppendCallback(() => ReturnSpike(_rightSpikes))
            .AppendInterval(1.3f)
            .OnComplete(() => EventDispatcher.PostEvent(EventsID.HIDE_HOLDER));
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

    private void SpawnSpikes(Transform[] spikes)
    {
        EventDispatcher.PostEvent(EventsID.SPIKE_SFX, _spikeSO.GetSFXByName("Move"));

        int spikeIndex;
        for (int i = 0; i < _spikesPerLevel; i++)
        {
            do
            {
                spikeIndex = UnityEngine.Random.Range(0, spikes.Length);
            }
            while (spikes[spikeIndex].tag.Equals(OPEN_SPIKE_TAG));

            spikes[spikeIndex].tag = OPEN_SPIKE_TAG;
            spikes[spikeIndex].DOLocalMoveY(NEW_SPIKE_POSITION, _spikeSO.SpawnDeday).SetEase(Ease.OutQuad);
        }
    }

    private void ReturnSpike(Transform[] spikes)
    {
        EventDispatcher.PostEvent(EventsID.SPIKE_SFX, _spikeSO.GetSFXByName("Move"));

        foreach (var spike in spikes)
        {
            if (spike.localPosition.y != OLD_SPIKE_POSITION)
            {
                spike.tag = SPIKE_TAG;
                spike.DOLocalMoveY(OLD_SPIKE_POSITION, _spikeSO.SpawnDeday);
            }
        }
    }

    private void CacheCallbacks()
    {
        _setSpikeToSpawnRef = (param) => SetSpikeToSpawn(param);
        _displayScoreRef = (param) => SetSpikeSpawnedByScore(param);
        _collidedSquareRef = (param) => MoveSelectedSpike(param);
        _hideSpikeRef = (param) => HideSpike();
    }
}
