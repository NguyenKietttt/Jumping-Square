using UnityEngine;

[CreateAssetMenu(fileName = "New Square Data", menuName = "SciptableObject/Gameplay/Square Data")]
public class SquareSO : ScriptableObject
{
    [Header("Movement")]
    [SerializeField] private float jumpHeight;
    [SerializeField] private float jumpLength;
    [SerializeField] [Range(0.0f, 1.0f)] private float rotateDuration;
    

    #region Properties

    public float JumpHeight => jumpHeight;
    public float JumpLength => jumpLength;
    public float RotateDuration => rotateDuration;


    #endregion
}
