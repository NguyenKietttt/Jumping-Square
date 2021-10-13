using UnityEngine;

[System.Serializable]
public struct GameobjectDict
{
    [SerializeField] private string name;
    [SerializeField] private GameObject prefab;


    #region Properties

    public string Name => name;
    public GameObject Prefab => prefab;

    #endregion
}
