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
        EventDispatcher.RegisterListener(EventsID.SQUARE_MOVEMENT_SFX, (param) => PlaySFX(param));
        EventDispatcher.RegisterListener(EventsID.SQUARE_COLLIDE_SFX, (param) => PlaySFX(param));
        EventDispatcher.RegisterListener(EventsID.SQUARE_TRIGGER_SFX, (param) => PlaySFX(param));

        EventDispatcher.RegisterListener(EventsID.HOLDER_SFX, (param) => PlaySFX(param));
        EventDispatcher.RegisterListener(EventsID.SPIKE_SFX, (param) => PlaySFX(param));

        EventDispatcher.RegisterListener(EventsID.PANEL_SFX, (param) => PlaySFX(param));

        EventDispatcher.RegisterListener(EventsID.BUTTON_PLAY_SFX, (param) => PlaySFX(param));
        EventDispatcher.RegisterListener(EventsID.BUTTON_SOUND_SFX, (param) => PlaySFX(param));
    }

    private void Start()
    {
        PlayMusic();
    }

    private void OnDisable()
    {
        EventDispatcher.RemoveListener(EventsID.SQUARE_MOVEMENT_SFX, (param) => PlaySFX(param));
        EventDispatcher.RemoveListener(EventsID.SQUARE_COLLIDE_SFX, (param) => PlaySFX(param));
        EventDispatcher.RemoveListener(EventsID.SQUARE_TRIGGER_SFX, (param) => PlaySFX(param));

        EventDispatcher.RemoveListener(EventsID.HOLDER_SFX, (param) => PlaySFX(param));
        EventDispatcher.RemoveListener(EventsID.SPIKE_SFX, (param) => PlaySFX(param));

        EventDispatcher.RemoveListener(EventsID.PANEL_SFX, (param) => PlaySFX(param));

        EventDispatcher.RemoveListener(EventsID.BUTTON_PLAY_SFX, (param) => PlaySFX(param));
        EventDispatcher.RemoveListener(EventsID.BUTTON_SOUND_SFX, (param) => PlaySFX(param));
    }


    private void PlaySFX(object clip)
    {
        _sfxSource.PlayOneShot(clip as AudioClip);
    }

    private void PlayMusic()
    {
        _musicSource.DOFade(1.0f, 0.5f);
    }

    public void ToggleAudio()
    {
        if (_isToggle)
            _musicSource.Stop();
        else
            _musicSource.Play();

        _isToggle = !_isToggle;
    }
}
