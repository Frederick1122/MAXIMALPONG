using System;
using System.Collections;
using System.Collections.Generic;
using Base;
using UnityEngine;
using Random = UnityEngine.Random;

public class MatchManager : Singleton<MatchManager>
{
    public Action OnChangeActiveBallsAction;
    
    [Space]
    [SerializeField] private List<Team> _teams;
    [Space]
    [Header("Balls")]
    [SerializeField] private Transform _ballSpawnPoint;
    [SerializeField] private List<Ball> _activeBalls = new List<Ball>(); // balls in scene
    [SerializeField] private List<Ball> _ballPrefabs = new List<Ball>();

    private Coroutine _levelRoutine;
    private Coroutine _generateFirstBallsRoutine;
    private LevelConfig _currentLevel;
    private YieldInstruction _second = new WaitForSeconds(1f);
    
    private void Start()
    {
        OnChangeActiveBallsAction?.Invoke();
    }

    public void UpdateScore(TeamType gateType, TeamType lastPunch)
    {
        var teamType = TeamType.None;
        
        if(_currentLevel == null)
            return;

        teamType = _currentLevel.gameType switch
        {
            GameType.AddPointsIfMadeLastPunch when gateType != lastPunch => lastPunch,
            GameType.RemovePointsIfConcedeAnOwnGoal => gateType,
            _ => teamType
        };

        if (_currentLevel.ballSpawnType == BallSpawnType.ByScore) 
            SpawnNewBall();
        
        if(teamType == TeamType.None)
            return;

        UIManager.Instance.IncrementScore(teamType);
        foreach (var team in _teams)
        {
            if (team.GetTeamType() != teamType)
                continue;
            
            team.Score++;
            return;
        }
    }
    
    public void FinishLevel()
    {
        var ballsCount = _activeBalls.Count;
        for (var i = 0; i < ballsCount; i++)
        {
            if (_activeBalls[0] != null)
                DestroyBall(_activeBalls[0]);
            else
                _activeBalls.RemoveAt(0);
        }
        
        _activeBalls = new List<Ball>();

        if (!HasPlayerWin())
            return;
        
        SaveManager.Instance.LevelsSaveData.Save(LoadingManager.Instance.GetCurrentLevel() + 2);
    }

    public void StartLevel(LevelConfig levelConfig)
    {
        _currentLevel = levelConfig;
        GenerateFirstBalls();

        if (_currentLevel.time <= 0)
            return;
        
        if (_levelRoutine != null)
            StopCoroutine(_levelRoutine);

        _levelRoutine = StartCoroutine(LevelRoutine(_currentLevel.time));
    }
    
    public List<Ball> GetActiveBalls()
    {
        var activeBalls = new List<Ball>();
        foreach (var ball in _activeBalls)
        {
            if(ball == null)
                continue;
            
            activeBalls.Add(ball);
        }

        _activeBalls = activeBalls;
        
        return _activeBalls;
    }

    public void DestroyBall(Ball ball)
    {
        _activeBalls.Remove(ball);
        OnChangeActiveBallsAction?.Invoke();
        Destroy(ball.gameObject); 
    }

    public void AddNewActiveBall(Ball newBall)
    {
        if (!_activeBalls.Contains(newBall)) 
            _activeBalls.Add(newBall);
        
        OnChangeActiveBallsAction?.Invoke();
    }

    public bool HasPlayerWin()
    {
        var _playerScore = 0;
        var _biggestScore = 0;
        foreach (var team in _teams)
        {
            if (team.GetTeamType() == TeamType.Player)
            {
                if (team.Score < _biggestScore)
                    return false;
                
                _playerScore = team.Score;                
                continue;
            }
            
            _biggestScore = team.Score > _biggestScore ? team.Score : _biggestScore;
        }

        return _playerScore > _biggestScore;
    }
    
    private void SpawnNewBall()
    {
        var newBall = Instantiate(_ballPrefabs[Random.Range(0, _ballPrefabs.Count)], _ballSpawnPoint);
        newBall.transform.parent = null;
    }
    
    private void GenerateFirstBalls()
    {
        _activeBalls.Clear();

        if (_generateFirstBallsRoutine != null) 
            StopCoroutine(_generateFirstBallsRoutine);

        _generateFirstBallsRoutine = StartCoroutine(GenerateFirstBallsRoutine());
    }

    private IEnumerator LevelRoutine(int time)
    {
        yield return new WaitForSeconds(time);
        UIManager.Instance.SetActiveScreen(ScreenType.EndMenu);
        FinishLevel();
    }

    private IEnumerator GenerateFirstBallsRoutine()
    {
        for (int i = 0; i < _currentLevel.ballCount; i++)
        {
            SpawnNewBall();
            OnChangeActiveBallsAction?.Invoke();
            yield return _second;
        }
    }
}

[Serializable]
public class Team
{
    [HideInInspector] public int Score = 0;
    [SerializeField] private TeamType _teamType = TeamType.None;
    [SerializeField] private string _nameTeam = "";

    public TeamType GetTeamType() => _teamType;
    
    public string GetNameType() => _nameTeam;
}

