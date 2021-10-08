using System.Collections;
using UnityEngine;

public class AutoDestroyVFX : MonoBehaviour
{
	private void OnEnable()
	{
		StartCoroutine(CheckIfAlive());
	}
	
	private IEnumerator CheckIfAlive()
	{
		ParticleSystem ps = this.GetComponent<ParticleSystem>();
		
		while(true && ps != null)
		{
			yield return new WaitForSeconds(0.5f);

			if (!ps.IsAlive(true))
				GameObject.Destroy(this.gameObject);
		}
	}
}
