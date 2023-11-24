using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/SoundLibrary", fileName = "SoundLibrary")]
public class SoundLibraryConfig : SerializedScriptableObject
{
    public Dictionary<SoundType, AudioClip> sounds = new Dictionary<SoundType, AudioClip>();
}

public enum SoundType
{
    CLICK,
    BALL_PUNCH
}