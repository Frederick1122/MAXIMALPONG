using System;
using System.Collections;
using System.Collections.Generic;
using Base;
using UnityEngine;
using Random = UnityEngine.Random;

public class MatchManager : Singleton<MatchManager>
{
    private const string LEVELS_PATH = "Levels/";
    
    public Action OnChangeActiveBallsAction;
    [Header("Level")] 
    [SerializeField] private LevelConfig _initLevel;
    [Space]
    [SerializeField] private GameObject _levelSpawnPoint;
    [SerializeField] private GameObject _activeLevel;
    [Space]
    [SerializeField] private List<Team> _teams;
    [Space]
    [Header("Balls")]
    [SerializeField] private Transform _ballSpawnPoint;
    [SerializeField] private List<Ball> _activeBalls = new List<Ball>(); // balls in scene
    [SerializeField] private List<Ball> _ballPrefabs = new List<Ball>();

    private LevelConfig _currentLevel;
    private Coroutine _levelRoutine;

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

        foreach (var team in _teams)
        {
            if (team.GetTeamType() == teamType)
            {
                team.Score++;
                UIManager.Instance.IncrementScore(teamType);
                return;
            }
        }
        
        Debug.LogError($"{teamType} not founded. check GameManager");
    }
    
    public void StartLoadingNewLevel(LevelConfig levelConfig, screenType screenType)
    {
        GameBus.Instance.SetLevel(levelConfig, screenType);
        
        _currentLevel = levelConfig;
        UIManager.Instance.OnLoadingScreenIsDone += FinishLoadingNewLevel;
        UIManager.Instance.SetActiveLoadingScreen(true);
    }
    
    private void FinishLoadingNewLevel()
    {
        UIManager.Instance.OnLoadingScreenIsDone -= FinishLoadingNewLevel;

        var ballsCount = _activeBalls.Count;
        for (var i = 0; i < ballsCount; i++)
        {
            if (_activeBalls[0] != null)
                DestroyBall(_activeBalls[0]);
            else
                _activeBalls.RemoveAt(0);
        }
        
        _activeBalls = new List<Ball>();
        
        var newLevel = Resources.Load<GameObject>($"{LEVELS_PATH}{_currentLevel.levelName}");
        Destroy(_activeLevel);
        
        _activeLevel = Instantiate(newLevel,_levelSpawnPoint.transform);
        _activeLevel.transform.SetParent(null);
        
        UIManager.Instance.SetActiveLoadingScreen(false);
        UIManager.Instance.StartLevel();

        if (_currentLevel.time > 0)
        {
            if (_levelRoutine != null)
                StopCoroutine(_levelRoutine);

            _levelRoutine = StartCoroutine(LevelRoutine());
        }
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

    private IEnumerator LevelRoutine()
    {
        yield return new WaitForSeconds(_currentLevel.time);
        StartLoadingNewLevel(_initLevel, screenType.MainMenu);
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

