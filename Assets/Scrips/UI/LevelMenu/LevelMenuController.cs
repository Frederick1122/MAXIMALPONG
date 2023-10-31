using System;

public class LevelMenuController : UIController<LevelMenuView, LevelMenuModel>
{
    private LevelMenuModel _model;
    
    public override void Init()
    {
        _view.onLaunchLevel += TryLaunchLevel;
        _view.onLaunchCustomLevel += LaunchCustomLevel;
        _view.onLaunchLocalMultiplayer += LaunchLocalMultiplayer;
        _view.onBackToMainMenu += BackToMainMenu;
        base.Init();
    }

    private void OnDestroy()
    {
        if(_view == null)
            return;

        _view.onLaunchLevel -= TryLaunchLevel;
        _view.onLaunchCustomLevel -= LaunchCustomLevel;
        _view.onLaunchLocalMultiplayer -= LaunchLocalMultiplayer;
        _view.onBackToMainMenu -= BackToMainMenu;
    }

    public override void Show()
    {
        _model = new LevelMenuModel {lastOpenLevel = SaveManager.Instance.LevelsSaveData.Load().lastOpenLevel};
        _view.UpdateView(_model);
        base.Show();
    }

    private void TryLaunchLevel(int levelIndex)
    {
        if(levelIndex > _model.lastOpenLevel) 
            return;
        
        LoadingManager.Instance.StartLoadingNewLevel(levelIndex);
    }

    private void LaunchCustomLevel()
    {
        UIManager.Instance.SetLocalMultiplayerStage(false);
        UIManager.Instance.SetActiveScreen(ScreenType.CustomLevelMenu);
    }

    private void LaunchLocalMultiplayer()
    {
        UIManager.Instance.SetLocalMultiplayerStage(true);
        UIManager.Instance.SetActiveScreen(ScreenType.CustomLevelMenu);
    }

    private void BackToMainMenu()
    {
        UIManager.Instance.SetActiveScreen(ScreenType.MainMenu);
    }
}