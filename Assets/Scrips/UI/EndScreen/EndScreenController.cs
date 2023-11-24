using Lean.Localization;

public class EndScreenController : UIController<EndScreenView, EndScreenModel>
{
    private const string UI_WIN  = "UI_PLAYER_WIN";
    private const string UI_LOSE = "UI_PLAYER_LOSE";
    private const string UI_DRAW = "UI_PLAYER_DRAW";
    private const string UI_PLAYER_1_WIN = "UI_PLAYER_1_WIN";
    private const string UI_PLAYER_2_WIN = "UI_PLAYER_2_WIN";
    
    public override void Init()
    {
        _view.OnGoToMainMenu += GoToMainMenu;
        base.Init();
    }

    public override void Show()
    {
        var model = new EndScreenModel
        {
            isWinner = MatchManager.Instance.HasPlayerWin()
        };
        if (LoadingManager.Instance.GetCurrentLevel().isLocalMultiplayer)
        {
            var player1Score = MatchManager.Instance.GetScore(TeamType.Player1);
            var player2Score = MatchManager.Instance.GetScore(TeamType.Player2);
            
            if (player1Score > player2Score)
                model.endText =  LeanLocalization.GetTranslationText(UI_PLAYER_1_WIN);
            else if (player1Score == player2Score)
                model.endText = LeanLocalization.GetTranslationText(UI_DRAW);
            else
                model.endText = LeanLocalization.GetTranslationText(UI_PLAYER_2_WIN);
        }
        else
            model.endText = LeanLocalization.GetTranslationText(model.isWinner ? UI_WIN : UI_LOSE);

        _view.UpdateView(model);
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
        LoadingManager.Instance.StartLoadingMainMenu();
    }
}
