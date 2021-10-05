using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "New Holder Data", menuName = "SciptableObject/Gameplay/Holder Data")]
public class HolderSO : ScriptableObject
{
    [Header("Hide")]
    [SerializeField] private float holderShowDuration;
    [SerializeField] private Ease holderShowEase;

    [Space(15.0f)]
    
    [SerializeField] private float spikeShowDuration;
    [SerializeField] private Ease spikeShowEase;
    
    [Header("Show")]
    [SerializeField] private float holderHideDuration;
    [SerializeField] private Ease holderHideEase;
    
    [Space(15.0f)]

    [SerializeField] private float spikeHideDuration;


    #region Properties

    public float HolderShowDuration => holderShowDuration;
    public Ease HolderShowEase => holderShowEase;
    public float HolderHideDuration => holderHideDuration;
    public Ease HolderHideEase => holderHideEase;

    public float SpikeShowDuration => spikeShowDuration;
    public Ease SpikeShowEase => spikeShowEase;
    public float SpikeHideDuration => spikeHideDuration;

    #endregion
}
