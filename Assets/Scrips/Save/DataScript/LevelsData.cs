using UnityEngine;

public class LevelsSaveData : SaveData<LevelsData>
{
    private const string LAST_OPEN_LEVEL = "LAST_OPEN_LEVEL";

    
    public override void PreLoad()
    {
        data.lastOpenLevel = PlayerPrefs.GetInt(LAST_OPEN_LEVEL, 1);
    }

    public override void Save(LevelsData obj)
    {
        data.lastOpenLevel = obj.lastOpenLevel;
        
        PlayerPrefs.SetInt(LAST_OPEN_LEVEL, data.lastOpenLevel);
    }
}

public class LevelsData
{
    public int lastOpenLevel;
}