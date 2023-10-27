using System;

public class EndScreenController : UIController<EndScreenView, EndScreenModel>
{
    public override void Init()
    {
        _view.OnGoToMainMenu += GoToMainMenu;
        base.Init();
    }

    public override void Show()
    {
        _view.UpdateView(new EndScreenModel()
        {
            isWinner = MatchManager.Instance.HasPlayerWin()
        });
        _view.Show();
    }

    private void OnDestroy()
    {
        if(_view == null)
            return;
        
        _view.OnGoToMainMenu -= GoToMainMenu;
    }

    private void GoToMainMenu()
    {
        LoadingManager.Instance.StartLoadingNewLevel();
    }
}
