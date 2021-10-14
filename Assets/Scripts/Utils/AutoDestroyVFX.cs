using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class AutoDestroyVFX : MonoBehaviour
{
    private ParticleSystem _vfx;


    private void Awake()
    {
        CacheComponents();
    }

    private void OnEnable()
    {
        StartCoroutine(CheckIfAlive());
    }


    private IEnumerator CheckIfAlive()
    {
        while (true && _vfx != null)
        {
            yield return new WaitForSeconds(0.5f);

            if (!_vfx.IsAlive(true))
                Destroy(gameObject);
        }
    }

    private void CacheComponents()
    {
        _vfx = GetComponent<ParticleSystem>();
    }
}
