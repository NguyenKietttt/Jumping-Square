using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Audio Data", menuName = "SciptableObject/Audio/Audio Data")]
public class AudioSO : ScriptableObject
{
    [Header("Music")]
    [SerializeField] private List<AudioDict> musicDict;


    #region Properties

    public List<AudioDict> MusicDict => musicDict;

    #endregion


    public AudioClip GetTrackByName(string track)
    {
        return MusicDict.Find(p => p.Name == track).Clip;
    }
}
