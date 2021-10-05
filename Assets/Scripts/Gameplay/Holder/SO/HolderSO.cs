using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "New Holder Data", menuName = "SciptableObject/Gameplay/Holder Data")]
public class HolderSO : ScriptableObject
{
    [Header("Holder")]
    [SerializeField] private float holderShowDuration;
    [SerializeField] private Ease holderShowEase;

    [Header("Spikes")]
    [SerializeField] private float spikeShowDuration;
    [SerializeField] private Ease spikeShowEase;


    #region Properties

    public float HolderShowDuration => holderShowDuration;
    public Ease HolderShowEase => holderShowEase;
    public float SpikeShowDuration => spikeShowDuration;
    public Ease SpikeShowEase => spikeShowEase;

    #endregion
}
