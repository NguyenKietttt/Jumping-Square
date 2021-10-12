using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class SpikeController : StateBase
{
    private readonly float OLD_SPIKE_POSITION = -4.5f;
    private readonly float NEW_SPIKE_POSITION = -3.5f;


    public static event Action endGameplayToGameoverEvent;
    public static event Action<AudioClip> spikeSFXEvent;


    [Header("Configs")]
    [SerializeField] private SpikeSO _spikeSO;

    [Header("References")]
    [SerializeField] private GameObject _leftSpikeHolder;
    [SerializeField] private GameObject _rightSpikeHolder;

    [Header("Validation")]
    [SerializeField] private bool _isFailedConfig;

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
    }

    private void OnEnable()
    {
        StateController.OnTitleEvent += OnTitleMenu;
        StateController.OnGameplayToGameoverEvent += OnGameplayToGameover;

        SquareCollision.onBorderCollideEvent += param => MoveSelectedSpike(param);
        ScoreController.displayScoreEvent += param => SetSpikeSpawnedByScore(param);
    }

    private void OnDisable()
    {
        StateController.OnTitleEvent -= OnTitleMenu;
        StateController.OnGameplayToGameoverEvent -= OnGameplayToGameover;

        SquareCollision.onBorderCollideEvent -= param => MoveSelectedSpike(param);
        ScoreController.displayScoreEvent -= param => SetSpikeSpawnedByScore(param);
    }


    public override void OnTitleMenu()
    {
        _spikesPerLevel = 1;
    }

    public override void OnGameplayToGameover()
    {
        DOTween.Sequence()
            .AppendCallback(() => ReturnSpike(_leftSpikes))
            .AppendCallback(() => ReturnSpike(_rightSpikes))
            .AppendInterval(1.3f)
            .OnComplete(() => endGameplayToGameoverEvent?.Invoke());
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

    private void SetSpikeSpawnedByScore(int score)
    {
        if (score >= 5 && score < 10)
            _spikesPerLevel = 2;
        else if (score >= 10 && score < 15)
            _spikesPerLevel = 3;
        else if (score >= 15 && score < 100)
            _spikesPerLevel = 4;
    }

    private void MoveSelectedSpike(bool isCollided)
    {
        if (isCollided)
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
        spikeSFXEvent?.Invoke(_spikeSO.GetSFXByName("Spike"));

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
        spikeSFXEvent?.Invoke(_spikeSO.GetSFXByName("Spike"));

        foreach (var spike in spikes)
        {
            if (spike.localPosition.y != OLD_SPIKE_POSITION)
            {
                spike.tag = "Spike";
                spike.DOLocalMoveY(OLD_SPIKE_POSITION, _spikeSO.SpawnDeday);
            }
        }
    }
}
