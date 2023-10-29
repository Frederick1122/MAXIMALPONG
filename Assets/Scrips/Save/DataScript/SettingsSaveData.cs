using UnityEngine;

public class SettingsSaveData : SaveData<SettingsMenuModel>
{
    private const string EFFECTS = "EFFECTS_VOLUME";
    private const string MUSIC = "MUSIC_VOLUME";

    public override void PreLoad()
    {
        data.effectsVolume = PlayerPrefs.GetFloat(EFFECTS, 1f);
        data.musicVolume = PlayerPrefs.GetFloat(MUSIC, 1f);
    }

    public override void Save(SettingsMenuModel obj)
    {
        data.effectsVolume = obj.effectsVolume;
        data.musicVolume = obj.musicVolume;

        PlayerPrefs.SetFloat(EFFECTS, data.effectsVolume);
        PlayerPrefs.SetFloat(MUSIC, data.musicVolume);
    }
}
