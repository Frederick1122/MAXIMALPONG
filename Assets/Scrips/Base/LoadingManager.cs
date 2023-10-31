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
    private int _currentLevelIndex;

    public int GetCurrentLevelIndex()
    {
        return _currentLevelIndex;
    }
    
    public LevelConfig GetCurrentLevel()
    {
        return _currentLevel;
    }

    public void StartLoadingNewLevel(int levelNumber)
    {
        if (levelNumber >= _levels.Count)
        {
            Debug.LogError($"Loading manager not found level with index: {levelNumber}. Level count: {_levels.Count}");
            return;
        }
        
        _currentLevelIndex = levelNumber;
        StartLoadingNewLevel(_levels[levelNumber], LevelType.Game);
    }

    public void StartLoadingNewLevel(LevelConfig levelConfig)
    {
        StartLoadingNewLevel(levelConfig, LevelType.Game);
    }
    
    public void StartLoadingMainMenu()
    {
        StartLoadingNewLevel(_initLevel, LevelType.MainMenu);
    }
    
    private void StartLoadingNewLevel(LevelConfig levelConfig, LevelType levelType)
    {
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
        DestroyImmediate(_activeLevel);
        
        _activeLevel = Instantiate(newLevel, _levelSpawnPoint.transform);
        
        UIManager.Instance.SetActiveLoadingScreen(false);
        UIManager.Instance.StartLevel(GameBus.Instance.GetLevelType());
        MatchManager.Instance.StartLevel(_currentLevel);
    }
}