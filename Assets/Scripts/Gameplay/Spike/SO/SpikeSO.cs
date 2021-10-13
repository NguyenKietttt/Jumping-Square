using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spike Data", menuName = "SciptableObject/Gameplay/Spike Data")]
public class SpikeSO : ScriptableObject
{
    [Header("Movement")]
    [SerializeField] [Range(0.0f, 1.0f)] private float spawnDeday;

    [Header("SFX")]
    [SerializeField] private List<AudioDict> sfxDict;


    #region Properties

    public float SpawnDeday => spawnDeday;

    public List<AudioDict> SfxDict => sfxDict;

    #endregion


    public AudioClip GetSFXByName(string sfxName)
    {
        return SfxDict.Find(p => p.Name == sfxName).Clip;
    }
}
