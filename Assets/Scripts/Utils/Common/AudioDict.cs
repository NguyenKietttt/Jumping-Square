using UnityEngine;

[System.Serializable]
public struct AudioDict
{
    [SerializeField] private string name;
    [SerializeField] private AudioClip clip;


    #region Properties

    public string Name => name;
    public AudioClip Clip => clip;

    #endregion
}
