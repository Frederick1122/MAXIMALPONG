using System.Collections.Generic;
using Base;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : Singleton<SoundManager>
{
    private const string SOUND_LIBRARY_PATH = "Configs/SoundLibrary";

    [SerializeField] private AudioMixer _mixer;
    
    [SerializeField] private AudioClip _mainMenuTheme;
    [SerializeField] private List<AudioClip> _gameClips = new List<AudioClip>();
    [Space]
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _effectSource;

    private SoundLibraryConfig _soundLibrary;

    protected override void Awake()
    {
        base.Awake();
        _soundLibrary = Resources.Load<SoundLibraryConfig>(SOUND_LIBRARY_PATH);
    }

    public void StartMainMenuMusic()
    {
        _musicSource.clip = _mainMenuTheme;
        _musicSource.Play();
    }

    public void StartGameMusic()
    {
        _musicSource.clip = _gameClips[Random.Range(0, _gameClips.Count - 1)];
        _musicSource.Play();
    }

    public void AddNewEffect(SoundType type)
    {
        _effectSource.clip = _soundLibrary.sounds[type];
        _effectSource.Play();
    }
    
    public AudioMixer GetMixer()
    {
        return _mixer;
    }
}