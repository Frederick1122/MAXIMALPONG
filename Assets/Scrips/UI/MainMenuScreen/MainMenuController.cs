
public class MainMenuController : UIController<MainMenuView, MainMenuModel>
{
    public override void Init()
    {
        _view.OpenSettingsAction += OpenSettings;
        _view.StartGameAction += StartGame;
        base.Init();
    }

    private void StartGame()
    {
        UIManager.Instance.SetActiveScreen(ScreenType.LevelMenu);        
    }

    private void OpenSettings()
    {
        
    }
    
    private void OnDestroy()
    {
        if(_view == null)
            return;
        
        _view.OpenSettingsAction -= OpenSettings;
        _view.StartGameAction -= StartGame;
    }
}