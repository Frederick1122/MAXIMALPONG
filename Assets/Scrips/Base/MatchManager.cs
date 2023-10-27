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
    private LevelConfig _currentLevel;
    
    private void OnValidate() => UpdateFields();

    private void Start()
    {
        UpdateFields();

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
    }

    public void StartLevel(LevelConfig levelConfig)
    {
        _currentLevel = levelConfig;
        
        if (_currentLevel.time <= 0)
            return;
        
        if (_levelRoutine != null)
            StopCoroutine(_levelRoutine);

        _levelRoutine = StartCoroutine(LevelRoutine(_currentLevel.time));
        // _currentLevel.levelName
    }
    
    public List<Ball> GetActiveBalls() => _activeBalls;

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
    
    private void UpdateFields()
    {
        if (_activeBalls.Count == 0)
        {
            var balls = FindObjectsOfType(typeof(Ball));
            if (balls.Length == 0)
                Debug.LogError("Active ball not found in scene");

            foreach (var ball in balls)
                _activeBalls.Add((Ball) ball);
        }
    }

    private IEnumerator LevelRoutine(int time)
    {
        yield return new WaitForSeconds(time);
        UIManager.Instance.SetActiveScreen(ScreenType.EndMenu);
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

