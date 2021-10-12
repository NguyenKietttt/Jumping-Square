using UnityEngine;

[System.Serializable]
public struct AudioDict
{
    [SerializeField] private string name;
    [SerializeField] private AudioClip sfx;


    #region Properties

    public string Name => name;
    public AudioClip SFX => sfx;

    #endregion
}
