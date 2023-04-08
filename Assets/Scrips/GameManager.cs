using System;
using System.Collections;
using System.Collections.Generic;
using Base;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    private const string LEVELS_PATH = "Levels/";
    
    public Action OnChangeActiveBallsAction;
    [Header("Level")]
    [SerializeField] private GameObject _levelSpawnPoint;
    [SerializeField] private GameObject _activeLevel;
    [Space]
    [SerializeField] private List<Team> _teams;
    [Space]
    [Header("Balls")]
    [SerializeField] private Transform _ballSpawnPoint;
    [SerializeField] private List<Ball> _activeBalls = new List<Ball>(); // balls in scene
    [SerializeField] private List<Ball> _ballPrefabs = new List<Ball>();
    [Space]
    [Header("Settings")]
    [SerializeField] private BallSpawnType _ballSpawnType;
    [SerializeField] private GameType _gameType;

    private string _newLevel;
    private UIType _activeUIType;

    private void OnValidate()
    {
        UpdateFields();
    }

    private void Start()
    {
        UpdateFields();

        OnChangeActiveBallsAction?.Invoke();
    }

    public void UpdateScore(TeamType gateType, TeamType lastPunch)
    {
        var teamType = TeamType.None;
        
        if (_gameType == GameType.AddPointsIfMadeLastPunch && gateType != lastPunch)
            teamType = lastPunch;
        else if (_gameType == GameType.RemovePointsIfConcedeAnOwnGoal) 
            teamType = gateType;

        if (_ballSpawnType == BallSpawnType.ByScore) 
            SpawnNewBall();
        
        if(teamType == TeamType.None)
            return;

        foreach (var team in _teams)
        {
            if (team.GetTeamType() == teamType)
            {
                team.Score++;
                UIManager.Instance.UpdateUIScore(teamType);
                return;
            }
        }
        
        Debug.LogError($"{teamType} not founded. check GameManager");
    }

    public void StartLoadingNewLevel(string levelName, UIType uiType)
    {
        _newLevel = levelName;
        _activeUIType = uiType;
        UIManager.Instance.OnLoadingScreenIsDone += FinishLoadingNewLevel;
        UIManager.Instance.SetActiveLoadingScreen(true);
    }

    private void FinishLoadingNewLevel()
    {
        UIManager.Instance.OnLoadingScreenIsDone -= FinishLoadingNewLevel;
        _activeBalls = new List<Ball>();
        var newLevel = Resources.Load<GameObject>($"{LEVELS_PATH}{_newLevel}");
        Destroy(_activeLevel);
        _activeLevel = Instantiate(newLevel,_levelSpawnPoint.transform);
        _activeLevel.transform.SetParent(null);
        UIManager.Instance.SetActiveWindow(_activeUIType);
        UIManager.Instance.SetActiveLoadingScreen(false);
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

