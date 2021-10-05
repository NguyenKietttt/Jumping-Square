using UnityEngine;

public abstract class StateBase : MonoBehaviour
{
    public virtual void OnTitleMenu() { }

    public virtual void OnTitleToGameplay() { }

    public virtual void OnGameplay() { }

    public virtual void OnGameOver() { }
}
