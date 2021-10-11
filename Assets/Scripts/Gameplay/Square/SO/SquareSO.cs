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
    [SerializeField] private GameObject collidedVFX;
    [SerializeField] private GameObject jumpVFX;
    [SerializeField] private GameObject explodeVFX;

    [Header("SFX")]
    [SerializeField] private List<SFXDict> sfxDict;
    

    #region Properties

    public float JumpHeight => jumpHeight;
    public float JumpLength => jumpLength;
    public float RotateDuration => rotateDuration;
    
    public GameObject CollidedVFX => collidedVFX;
    public GameObject JumpVFX => jumpVFX;
    public GameObject ExplodeVFX => explodeVFX;

    public List<SFXDict> SfxDict => sfxDict;

    #endregion


    public AudioClip GetSFXByName(string sfxName)
    {
        return SfxDict.Find(p => p.Name == sfxName).SFX;
    }
}
