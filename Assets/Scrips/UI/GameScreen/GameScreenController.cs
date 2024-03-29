using System.Collections;
using UnityEngine;

public class GameScreenController : UIController<GameScreenView, GameScreenModel>
{
    private YieldInstruction _second = new WaitForSeconds(1f);
    private Coroutine _timerRoutine;
    private GameScreenModel _model = new GameScreenModel();
    public override void Init()
    {
        _view.OnPause += StartPause;
        base.Init();
    }

    private void OnDestroy()
    {
        if(_view != null)
            _view.OnPause -= StartPause;
    }

    public void IncrementScore(TeamType teamType)
    {
        if (_model.teamScores.ContainsKey(teamType))
        {
            _model.teamScores[teamType]++;
        }
        else
        {
            _model.teamScores.Add(teamType, 1);
        }

        _view.UpdateView(_model);
    }

    public void StartNewLevel()
    {
        if (GameBus.Instance.GetLevelType() == LevelType.MainMenu)
            return;

        var level = GameBus.Instance.GetLevelConfig();

        _model = new GameScreenModel();
        _model.remainingTime = level.time;

        if (_timerRoutine != null)
            StopCoroutine(_timerRoutine);

        _view.ResetScore();
        _view.UpdateView(_model);
        _timerRoutine = StartCoroutine(TimerRoutine());
    }

    private IEnumerator TimerRoutine()
    {
        while (_model.remainingTime > 0)
        {
            yield return _second;
            _model.remainingTime--;
            _view.UpdateView(_model);
        }
    }

    private void StartPause()
    {
        UIManager.Instance.SetActiveScreen(ScreenType.Settings);
    } 
}