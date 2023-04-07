using System;
using System.Collections.Generic;
using Base;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    public Action OnChangeActiveBallsAction;
    
    [SerializeField] private List<Team> _teams;
    [Space(5)] 
    [SerializeField] private List<Ball> _activeBalls = new List<Ball>(); // balls in scene
    [SerializeField] private BallSpawnType _ballSpawnType;
    [SerializeField] private Transform _ballSpawnPoint;
    [SerializeField] private List<Ball> _ballPrefabs = new List<Ball>();

    private void OnValidate()
    {
        UpdateFields();
    }

    private void Start()
    {
        UpdateFields();

        OnChangeActiveBallsAction?.Invoke();
    }

    public void UpdateScore(TeamType teamType)
    {
        if(teamType == TeamType.None)
            return;

        foreach (var team in _teams)
        {
            if (team.GetTeamType() == teamType)
            {
                team.Score++;

                if (_ballSpawnType == BallSpawnType.byScore) 
                    SpawnNewBall();
                
                return;
            }
        }
        
        Debug.LogError($"{teamType} not founded. check GameManager");
    }

    public void SpawnNewBall()
    {
        _activeBalls.Add(Instantiate(_ballPrefabs[Random.Range(0, _ballPrefabs.Count)], _ballSpawnPoint)); 
        OnChangeActiveBallsAction?.Invoke();
    }

    public List<Ball> GetActiveBalls() => _activeBalls;

    public void DestroyBall(Ball ball)
    {
        _activeBalls.Remove(ball);
        OnChangeActiveBallsAction?.Invoke();
        Destroy(ball.gameObject); 
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

