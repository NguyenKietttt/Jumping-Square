using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(AudioSource))]
public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private AudioSource _musicSource;

    private bool _isToggle;


    private void Awake()
    {
        _isToggle = true;
    }

    private void OnEnable()
    {
        SquareMovement.momventSFXEvent += param => PlaySFX(param);
        SquareCollision.impactSFXEvent += param => PlaySFX(param);
        SquareTrigger.deadSFXEvent += param => PlaySFX(param);

        HolderController.holderSFXEvent += param => PlaySFX(param);
        SpikeController.spikeSFXEvent += param => PlaySFX(param);

        UIPanel.panelSFXEvent += param => PlaySFX(param);

        PlayButton.playButtonSFXEvent += param => PlaySFX(param);
        SoundButton.soundButtonSFXEvent += param => PlaySFX(param);
    }

    private void Start()
    {
        PlayMusic();
    }

    private void OnDisable()
    {
        SquareMovement.momventSFXEvent -= param => PlaySFX(param);
        SquareCollision.impactSFXEvent -= param => PlaySFX(param);
        SquareTrigger.deadSFXEvent -= param => PlaySFX(param);

        HolderController.holderSFXEvent -= param => PlaySFX(param);
        SpikeController.spikeSFXEvent -= param => PlaySFX(param);

        UIPanel.panelSFXEvent -= param => PlaySFX(param);

        PlayButton.playButtonSFXEvent -= param => PlaySFX(param);
        SoundButton.soundButtonSFXEvent -= param => PlaySFX(param);
    }


    private void PlaySFX(AudioClip clip)
    {
        _sfxSource.PlayOneShot(clip);
    }

    private void PlayMusic()
    {
        _musicSource.DOFade(1.0f, 0.5f);
    }

    public void ToggleAudio()
    {
        if (_isToggle)
            _musicSource.volume = 0;
        else
            _musicSource.volume = 1;

        _isToggle = !_isToggle;
    }
}
