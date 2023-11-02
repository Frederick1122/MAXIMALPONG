using System;

public class EndScreenController : UIController<EndScreenView, EndScreenModel>
{
    private const string LOSE = "You lose";
    private const string WIN = "You win";
    private const string PLAYER_1_WIN = "Player 1 win";
    private const string DRAW = "Draw";
    private const string PLAYER_2_WIN = "Player 2 win";
    
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
                model.endText = PLAYER_1_WIN;
            else if (player1Score == player2Score)
                model.endText = DRAW;
            else
                model.endText = PLAYER_2_WIN;
        }
        else
            model.endText = model.isWinner ? WIN : LOSE;

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
