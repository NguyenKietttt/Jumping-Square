using UnityEngine;

[CreateAssetMenu(fileName = "New Spike Data", menuName = "SciptableObject/Gameplay/Spike Data")]
public class SpikeSO : ScriptableObject
{
    [Header("Movement")]
    [SerializeField] [Range(0.0f, 1.0f)] private float spawnDeday;


    #region Properties

    public float SpawnDeday => spawnDeday;

    #endregion
}
