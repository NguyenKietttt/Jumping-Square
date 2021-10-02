using System;
using UnityEngine;

public class SquareCollision : MonoBehaviour
{
    public static event Action<bool> OnBorderCollide;


    private bool _isCollided;


    private void Start() 
    {
        _isCollided = RandomCollided();
        OnBorderCollide?.Invoke(_isCollided);
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.CompareTag("Border"))
        {
           _isCollided = !_isCollided;

           OnBorderCollide?.Invoke(_isCollided);
        }
    }

    private bool RandomCollided()
    {
        var result = (UnityEngine.Random.Range(0, 2) == 0) ? true : false;

        return result;
    }
}
