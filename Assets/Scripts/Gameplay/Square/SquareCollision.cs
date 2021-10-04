using System.Collections;
using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class SquareCollision : MonoBehaviour
{
    public static event Action<bool> OnBorderCollideEvent;


    private BoxCollider2D boxCollider2D;
    private bool _isCollided;


    private void Start() 
    {
        _isCollided = RandomCollided();
        
        OnBorderCollideEvent?.Invoke(_isCollided);
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.CompareTag("Border"))
        {
           _isCollided = !_isCollided;

           OnBorderCollideEvent?.Invoke(_isCollided);

           StartCoroutine(CheckDoubleJump());
        }
    }


    private void CacheComponents()
    {
        boxCollider2D = gameObject.GetComponent<BoxCollider2D>();
    }

    private IEnumerator CheckDoubleJump()
    {
        boxCollider2D.enabled = false;

        yield return new WaitForSeconds(0.2f);

        boxCollider2D.enabled = true;
    }

    private bool RandomCollided()
    {
        var result = (UnityEngine.Random.Range(0, 2) == 0) ? true : false;

        return result;
    }
}
