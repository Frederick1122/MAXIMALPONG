using System;
using System.Collections;
using System.Collections.Generic;
using Base;
using UnityEngine;
using Random = UnityEngine.Random;

public class MatchManager : Singleton<MatchManager>
{
    public Action OnChangeActiveBallsAction;

    [Space] [Header("Balls")] [SerializeField]
    private Transform _ballSpawnPoint;

    [Space] [SerializeField] private List<Ball> _activeBalls = new List<Ball>(); // balls in scene
    [SerializeField] private List<Ball> _ballPrefabs = new List<Ball>();
    
    private Dictionary<TeamType, int> _teamScores = new Dictionary<TeamType, int>();
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

        if (_currentLevel == null)
            return;

        teamType = _currentLevel.gameType switch
        {
            GameType.AddPointsIfMadeLastPunch when gateType != lastPunch => lastPunch,
            GameType.RemovePointsIfConcedeAnOwnGoal => gateType,
            _ => teamType
        };

        SpawnNewBall();

        if (teamType == TeamType.None)
            return;

        UIManager.Instance.IncrementScore(teamType);
        if (_teamScores.ContainsKey(teamType))
            _teamScores[teamType]++;
        else
            _teamScores.Add(teamType, 1);
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

        if (_levelRoutine != null)
            StopCoroutine(_levelRoutine);

        if (HasPlayerWin() && !_currentLevel.isCustomLevel)
        {
            var lastLevelIndex = SaveManager.Instance.LevelsSaveData.Load().lastOpenLevel;
            var currentLevelIndex = LoadingManager.Instance.GetCurrentLevelIndex() + 2;
            if(lastLevelIndex < currentLevelIndex)
                SaveManager.Instance.LevelsSaveData.Save(currentLevelIndex);
        }

        _teamScores.Clear();
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
        UIManager.Instance.StartNewLevel();
    }

    public List<Ball> GetActiveBalls()
    {
        var activeBalls = new List<Ball>();
        foreach (var ball in _activeBalls)
        {
            if (ball == null)
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

    private void AddNewActiveBall(Ball newBall)
    {
        if (!_activeBalls.Contains(newBall))
            _activeBalls.Add(newBall);

        OnChangeActiveBallsAction?.Invoke();
    }

    public int GetScore(TeamType teamType)
    {
        return _teamScores.ContainsKey(teamType) ? _teamScores[teamType] : 0;
    }

    public bool HasPlayerWin()
    {
        var playerScore = 0;
        var biggestScore = 0;
        foreach (var team in _teamScores)
        {
            if (team.Key == TeamType.Player1)
            {
                if (team.Value < biggestScore)
                    return false;

                playerScore = team.Value;
                continue;
            }

            biggestScore = team.Value > biggestScore ? team.Value : biggestScore;
        }

        return playerScore > biggestScore;
    }

    [ContextMenu("Spawn New Ball")]
    private void SpawnNewBall()
    {
        var newBall = Instantiate(_ballPrefabs[Random.Range(0, _ballPrefabs.Count)], _ballSpawnPoint);
        newBall.transform.parent = null;
        newBall.SetDirection(GetRandomDirection());
        AddNewActiveBall(newBall);
    }

    private void GenerateFirstBalls()
    {
        _activeBalls.Clear();

        if (_generateFirstBallsRoutine != null)
            StopCoroutine(_generateFirstBallsRoutine);

        _generateFirstBallsRoutine = StartCoroutine(GenerateFirstBallsRoutine());
    }

    private Vector3 GetRandomDirection()
    {
        var multiplier = Random.Range(0, 2) == 1 ? 1 : -1;
        var vector = Vector3.right + Vector3.forward;
        var directionVector = Vector3.Lerp(vector, vector * -1, Random.Range(0f, 1f)) * multiplier;
        return directionVector;
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