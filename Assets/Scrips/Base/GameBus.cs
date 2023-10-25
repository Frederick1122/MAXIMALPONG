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
    private screenType _currentScreenType = screenType.MainMenu;
    
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

    public screenType GetLevelType()
    {
        return _currentScreenType;
    }

    public void SetLevel(LevelConfig newLevel, screenType newType)
    {
        _currentScreenType = newType;
        _currentLevelConfig = newLevel;
    }
    
    private void Start()
    {
        LeanLocalization.SetCurrentLanguageAll(GPManager.Instance.GetBrowserLanguage() == Language.Russian ? "Russian" : "English");
    }
}
