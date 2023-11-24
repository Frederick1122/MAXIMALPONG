using System.Collections.Generic;
using Base;
using UnityEngine;

public class MusicManager : Singleton<MusicManager>
{
    [SerializeField] private AudioClip _mainMenuTheme;
    [SerializeField] private List<AudioClip> _gameClips = new List<AudioClip>();
    [Space]
    [SerializeField] private AudioSource _musicSource;
    
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
}
