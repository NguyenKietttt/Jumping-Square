using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpikeController : MonoBehaviour
{
    [SerializeField] private GameObject _leftSpikeHolder;
    [SerializeField] private GameObject _rightSpikeHolder;

    private List<Transform> _leftSpikes;
    private List<Transform> _rightSpikes;


    private void Awake()
    {
        _leftSpikes = GetHolderChilds(_leftSpikeHolder);
        _rightSpikes = GetHolderChilds(_rightSpikeHolder);
    }

    private void OnEnable()
    {
        SquareCollision.OnBorderCollide += param => SpawnSpike(param);
    }

    private void OnDisable()
    {
        SquareCollision.OnBorderCollide -= param => SpawnSpike(param);
    }


    public void SpawnSpike(bool isCollided)
    {
        Debug.Log(isCollided);

        var spikeIndex = Random.Range(0, _leftSpikes.Count);

        if (isCollided)
            _leftSpikes[spikeIndex].DOLocalMoveY(-3.5f, 0.5f);
        else
            _rightSpikes[spikeIndex].DOLocalMoveY(-3.5f, 0.5f);
    }

    private List<Transform> GetHolderChilds(GameObject holder)
    {
        List<Transform> childs = new List<Transform>();

        for (int i = 0; i < holder.transform.childCount; i++)
        {
            childs.Add(holder.transform.GetChild(i).transform);
        }

        return childs;
    }
}
