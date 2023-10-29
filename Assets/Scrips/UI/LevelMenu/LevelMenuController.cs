using System;

public class LevelMenuController : UIController<LevelMenuView, LevelMenuModel>
{
    private LevelMenuModel _model;
    
    public override void Init()
    {
        base.Init();
        _view.onLaunchLevel += TryLaunchLevel;
        _view.onLaunchCustomLevel += LaunchCustomLevel;
        _view.onLaunchLocalMultiplayer += LaunchLocalMultiplayer;
    }

    private void OnDestroy()
    {
        if(_view == null)
            return;

        _view.onLaunchLevel -= TryLaunchLevel;
        _view.onLaunchCustomLevel -= LaunchCustomLevel;
        _view.onLaunchLocalMultiplayer -= LaunchLocalMultiplayer;
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
        
    }

    private void LaunchLocalMultiplayer()
    {
        
    }
}