using System.Collections;
using System;
using UnityEngine;

public class SquareCollision : MonoBehaviour
{
    public static event Action<bool> OnBorderCollideEvent;


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

    private IEnumerator CheckDoubleJump()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;

        yield return new WaitForSeconds(0.2f);

        gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }

    private bool RandomCollided()
    {
        var result = (UnityEngine.Random.Range(0, 2) == 0) ? true : false;

        return result;
    }
}
