using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelConfig", menuName = "Configs/NewLevelConfig")]
public class LevelConfig : ScriptableObject
{
    public string levelName;
    [Space]
    public int ballCount;
    public int time;
    [Space]
    // public LevelType levelType;
    public BallSpawnType ballSpawnType;
    public GameType gameType;
}
