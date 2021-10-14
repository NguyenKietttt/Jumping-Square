using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Panel Data", menuName = "SciptableObject/UI/Panel Data")]
public class PanelSO : ScriptableObject
{
    [Header("SFX")]
    [SerializeField] private List<AudioDict> sfxDict;


    #region Properties

    public List<AudioDict> SfxDict => sfxDict;

    #endregion


    public AudioClip GetSFXByName(string sfxName)
    {
        return SfxDict.Find(p => p.Name.Equals(sfxName)).Clip;
    }
}
