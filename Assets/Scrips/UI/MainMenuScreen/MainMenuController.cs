
public class MainMenuController : UIController<MainMenuView, MainMenuModel>
{
    public override void Init()
    {
        _view.OpenSettingsAction += OpenSettings;
        _view.StartGameAction += StartGame;
        _view.OpenHowToPlayAction += OpenHowToPlay;
        base.Init();
    }

    private void StartGame() => UIManager.Instance.SetActiveScreen(ScreenType.LevelMenu);

    private void OpenSettings() => UIManager.Instance.SetActiveScreen(ScreenType.Settings);

    private void OpenHowToPlay() => UIManager.Instance.SetActiveScreen(ScreenType.HowToPlay);

    private void OnDestroy()
    {
        if(_view == null)
            return;
        
        _view.OpenSettingsAction -= OpenSettings;
        _view.StartGameAction -= StartGame;
        _view.OpenHowToPlayAction -= OpenHowToPlay;
    }
}