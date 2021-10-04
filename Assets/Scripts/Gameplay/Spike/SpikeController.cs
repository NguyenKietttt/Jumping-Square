using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpikeController : MonoBehaviour
{
    private readonly float OLD_POSITION = -4.5f;
    private readonly float NEW_POSITION = -3.5f;


    [Header("Configs")]
    [SerializeField] private SpikeSO _spikeSO;

    [Header("References")]
    [SerializeField] private GameObject _leftSpikeHolder;
    [SerializeField] private GameObject _rightSpikeHolder;

    [Header("Validation")]
    [SerializeField] private bool _isFailedConfig;


    private List<Transform> _leftSpikes, _rightSpikes;
    private int _spikesPerLevel = 2;


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
        SquareCollision.OnBorderCollide += param => MoveSelectedSpike(param);
    }

    private void OnDisable()
    {
        SquareCollision.OnBorderCollide -= param => MoveSelectedSpike(param);
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
        int spikeIndex;

        for (int i = 0; i < _spikesPerLevel; i++)
        {
            do
            {
                spikeIndex = Random.Range(0, spikes.Count);
            }
            while (spikes[spikeIndex].tag == "OpenSpike");

            spikes[spikeIndex].tag = "OpenSpike";
            spikes[spikeIndex].DOLocalMoveY(NEW_POSITION, _spikeSO.SpawnDeday);
        }
    }

    private void ReturnSpike(List<Transform> spikes)
    {
        foreach (var spike in spikes)
        {
            if (spike.localPosition.y == NEW_POSITION)
            {
                spike.tag = "Spike";
                spike.DOLocalMoveY(OLD_POSITION, _spikeSO.SpawnDeday);
            }
        }
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
}
