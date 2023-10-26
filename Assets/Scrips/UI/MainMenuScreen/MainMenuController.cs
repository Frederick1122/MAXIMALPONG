
public class MainMenuController : UIController<MainMenuView, MainMenuModel>
{
    public override void Init()
    {
        _view.OpenSettingsAction += OpenSettings;
        _view.StartGameAction += StartGame;
        _view.Init(null);
    }

    private void StartGame()
    {
        UIManager.Instance.StartLevelDebugging();        
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