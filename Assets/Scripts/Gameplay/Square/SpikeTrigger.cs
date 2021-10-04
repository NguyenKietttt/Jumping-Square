using UnityEngine;

public class SpikeTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("OpenSpike"))
        {
            Time.timeScale = 0;
        }
    }
}
