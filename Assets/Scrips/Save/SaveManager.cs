using Base;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    internal SettingsSaveData SettingsSaveData { get; } = new();
    internal LevelsSaveData LevelsSaveData { get; } = new();

    protected override void Awake()
    {
        base.Awake();
        PreLoad();
    }

    private void PreLoad()
    {
        SettingsSaveData.PreLoad();
        LevelsSaveData.PreLoad();
    }
}