using Base;
using GamePush;
using Lean.Localization;
using System;

public class GameBus : Singleton<GameBus>
{
    public event Action OnRestartGame;
    public event Action OnContinueGame;
    public event Action OnEndGame;

    private bool _isContinuedGame;
    private LevelConfig _currentLevelConfig;
    private LevelType _currentLevelType = LevelType.MainMenu;
    
    public void EndGame()
    {
        OnEndGame?.Invoke();
    }

    public void RestartGame()
    {
        OnRestartGame?.Invoke();
        _isContinuedGame = false;
    }

    public void ContinueGame()
    {
        OnContinueGame?.Invoke();
        _isContinuedGame = true;
    }

    public bool IsContinuedGame()
    {
        return _isContinuedGame;
    }

    public LevelConfig GetLevelConfig()
    {
        return _currentLevelConfig;
    }

    public LevelType GetLevelType()
    {
        return _currentLevelType;
    }

    public void SetLevel(LevelConfig newLevel, LevelType newType)
    {
        _currentLevelType = newType;
        _currentLevelConfig = newLevel;
    }
    
    private void Start()
    {
        LeanLocalization.SetCurrentLanguageAll(GPManager.Instance.GetBrowserLanguage() == Language.Russian ? "Russian" : "English");
    }
}
