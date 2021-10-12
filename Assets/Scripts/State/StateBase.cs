using UnityEngine;

public abstract class StateBase : MonoBehaviour
{
    public virtual void OnTitle() { }

    public virtual void OnTitleToGameplay() { }

    public virtual void OnGameplay() { }

    public virtual void OnGameplayToGameover() { }

    public virtual void OnGameOver() { }
}
