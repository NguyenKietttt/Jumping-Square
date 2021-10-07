using UnityEngine;

public class SquareTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("OpenSpike"))
        {
            StateController.RaiseOnGameplayToGameoverEvent();
        }
    }
}
