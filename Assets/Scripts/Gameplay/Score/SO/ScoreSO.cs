using UnityEngine;

[CreateAssetMenu(fileName = "New Score Data", menuName = "SciptableObject/Gameplay/Score Data")]
public class ScoreSO : ScriptableObject
{
    [Header("Movement")]
    [SerializeField] [Range(0.0f, 1.0f)] private float rotateTime;

    [Header("Color")]
    [SerializeField] private Color normalScore;
    [SerializeField] private Color milestoneScore;


    #region Properties

    public float RotateTime => rotateTime;
    public Color NormalScore => normalScore;
    public Color MilestoneScore => milestoneScore;

    #endregion
}
