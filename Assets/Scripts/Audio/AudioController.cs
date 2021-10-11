using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioController : MonoBehaviour
{
    private AudioSource _sfxSource;


    private void Awake()
    {
        _sfxSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        SquareMovement.jumpSFXEvent += param => PlaySFX(param);
        SquareCollision.impactSFXEvent += param => PlaySFX(param);
        SquareTrigger.deadSFXEvent += param => PlaySFX(param);

        HolderController.holderSFXEvent += param => PlaySFX(param);
        SpikeController.spikeSFXEvent += param => PlaySFX(param);
    }

    private void OnDisable()
    {
        SquareMovement.jumpSFXEvent -= param => PlaySFX(param);
        SquareCollision.impactSFXEvent -= param => PlaySFX(param);
        SquareTrigger.deadSFXEvent -= param => PlaySFX(param);

        HolderController.holderSFXEvent -= param => PlaySFX(param);
        SpikeController.spikeSFXEvent -= param => PlaySFX(param);
    }


    private void PlaySFX(AudioClip clip)
    {
        _sfxSource.PlayOneShot(clip);
    }
}
