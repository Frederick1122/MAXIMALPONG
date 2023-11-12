using System;

public class HowToPlayController : UIController<HowToPlayView, UIModel>
{
    public override void Init()
    {
        _view.OnGoToMainMenu += GoToMainMenu;
        base.Init();
    }

    private void OnDestroy()
    {
        _view.OnGoToMainMenu -= GoToMainMenu;
    }

    private void GoToMainMenu() => UIManager.Instance.SetActiveScreen(ScreenType.MainMenu);
}