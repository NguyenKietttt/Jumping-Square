using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Button Data", menuName = "SciptableObject/UI/Button Data")]
public class ButtonSO : ScriptableObject
{
    [Header("SFX")]
    [SerializeField] private List<AudioDict> sfxDict;


    #region Properties

    public List<AudioDict> SfxDict => sfxDict;

    #endregion


    public AudioClip GetSFXByName(string sfxName)
    {
        return SfxDict.Find(p => p.Name == sfxName).Clip;
    }
}
