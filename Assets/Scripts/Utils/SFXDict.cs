using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SFXDict
{
    [SerializeField] private string name;
    [SerializeField] private AudioClip sfx;


    #region Properties

    public string Name => name;
    public AudioClip SFX => sfx;

    #endregion
}
