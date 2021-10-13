using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Square Data", menuName = "SciptableObject/Gameplay/Square Data")]
public class SquareSO : ScriptableObject
{
    [Header("Movement")]
    [SerializeField] private float jumpHeight;
    [SerializeField] private float jumpLength;
    [SerializeField] [Range(0.0f, 1.0f)] private float rotateDuration;

    [Header("VFX")]
    [SerializeField] private List<GameobjectDict> vfxDict;

    [Header("SFX")]
    [SerializeField] private List<AudioDict> sfxDict;
    

    #region Properties

    public float JumpHeight => jumpHeight;
    public float JumpLength => jumpLength;
    public float RotateDuration => rotateDuration;
    
    public List<GameobjectDict> VfxDict => vfxDict;

    public List<AudioDict> SfxDict => sfxDict;

    #endregion


    public AudioClip GetSFXByName(string sfxName)
    {
        return SfxDict.Find(p => p.Name == sfxName).Clip;
    }

    public GameObject GetVFXByName(string vfxName)
    {
        return vfxDict.Find(p => p.Name == vfxName).Prefab;
    }
}
