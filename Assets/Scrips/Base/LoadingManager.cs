using System.Collections.Generic;
using Base;
using UnityEngine;

public class LoadingManager : Singleton<LoadingManager>
{
    private const string LEVELS_PATH = "Levels/";

    [Header("Level")] 
    [SerializeField] private LevelConfig _initLevel;
    [SerializeField] private GameObject _activeLevel;
    [SerializeField] private List<LevelConfig> _levels = new List<LevelConfig>();
    [Space]
    [SerializeField] private GameObject _levelSpawnPoint;
    
    private LevelConfig _currentLevel;

    public LevelConfig GetCurrentLevel()
    {
        return _currentLevel;
    }

    public void StartLoadingNewLevel(int levelNumber)
    {
        StartLoadingNewLevel(_levels[levelNumber], LevelType.Game);
    }
    
    public void StartLoadingNewLevel(LevelConfig levelConfig = null, LevelType levelType = LevelType.MainMenu)
    {
        levelConfig ??= _initLevel;
        GameBus.Instance.SetLevel(levelConfig, levelType);
        
        _currentLevel = levelConfig;
        UIManager.Instance.OnLoadingScreenIsDone += FinishLoadingNewLevel;
        UIManager.Instance.SetActiveLoadingScreen(true);
    }
    
    private void FinishLoadingNewLevel()
    {
        UIManager.Instance.OnLoadingScreenIsDone -= FinishLoadingNewLevel;

        MatchManager.Instance.FinishLevel();
        
        var newLevel = Resources.Load<GameObject>($"{LEVELS_PATH}{_currentLevel.levelName}");
        Destroy(_activeLevel);
        
        _activeLevel = Instantiate(newLevel, _levelSpawnPoint.transform);
        //_activeLevel.transform.SetParent(null);
        
        UIManager.Instance.SetActiveLoadingScreen(false);
        UIManager.Instance.StartLevel(GameBus.Instance.GetLevelType());
        MatchManager.Instance.StartLevel(_currentLevel);
    }
}