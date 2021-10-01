using UnityEngine;

[CreateAssetMenu(fileName = "New Square Data", menuName = "SciptableObject/Square Data")]
public class SquareSO : ScriptableObject
{
    [Header("Movement")]
    [SerializeField] private float jumpHeight;
    

    #region Properties

    public float JumpHeight => jumpHeight;

    #endregion
}
