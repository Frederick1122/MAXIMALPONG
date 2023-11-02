using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelConfig", menuName = "Configs/NewLevelConfig")]
public class LevelConfig : ScriptableObject
{
    public string levelName = "Level1";
    [Space]
    public int ballCount = 1;
    public int time = 30;
    [Space]
    //public BallSpawnType ballSpawnType;
    public GameType gameType = GameType.AddPointsIfMadeLastPunch;
    [Space] [Header("Bot settings")] 
    public int botSpeed = 200;
    
    [HideInInspector] public bool isCustomLevel = false;
    [HideInInspector] public bool isLocalMultiplayer = false;
}
