using Base;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioMixer _mixer;

    public AudioMixer GetMixer()
    {
        return _mixer;
    }
}