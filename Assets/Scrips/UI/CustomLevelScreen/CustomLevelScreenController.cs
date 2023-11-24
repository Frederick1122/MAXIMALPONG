using UnityEngine;

public class CustomLevelScreenController : UIController<CustomLevelScreenView, CustomLoadingScreenModel>
{
    private const string LOCAL_MULTIPLAYER_LEVEL_NAME = "LevelLocalMultiplayer";
    
    [SerializeField] private CustomLevelConfig _config;
    private CustomLoadingScreenModel _model;

    private int _time;
    private int _botComplexity;
    private int _ballQuantity;
    
    public override void Init()
    {
        _model = new CustomLoadingScreenModel()
        {
            LevelConfig = _config
        };

        _view.OnPlay += LaunchCustomLevel;
        _view.OnGoToMainMenu += BackToLevelMenu;
        _view.OnTimeUpdate += SetTime;
        _view.OnBotComplexityUpdate += SetBotComplexity;
        _view.OnBallQuantityUpdate += SetBallQuantity;
        
        base.Init();
    }

    private void OnDestroy()
    {
        if(_view == null)
            return;

        _view.OnPlay -= LaunchCustomLevel;
        _view.OnGoToMainMenu -= BackToLevelMenu;
        _view.OnTimeUpdate -= SetTime;
        _view.OnBotComplexityUpdate -= SetBotComplexity;
        _view.OnBallQuantityUpdate -= SetBallQuantity;
    }

    public override void Show()
    {
        _view.UpdateView(_model);
        base.Show();
    }
    
    public void SetLocalMultiplayerStage(bool isLocalMultiplayer)
    {
        _model.isLocalMultiplayer = isLocalMultiplayer;
    }

    private void SetTime(int time)
    {
        _time = time;
    }
    
    private void SetBotComplexity(int botComplexity)
    {
        _botComplexity = botComplexity;
    }
    
    private void SetBallQuantity(int ballQuantity)
    {
        _ballQuantity = ballQuantity;
    }
    
    private void LaunchCustomLevel()
    {
        var levelConfig = ScriptableObject.CreateInstance<LevelConfig>();
        if (_model.isLocalMultiplayer)
        {
            levelConfig.levelName = LOCAL_MULTIPLAYER_LEVEL_NAME;
            levelConfig.isLocalMultiplayer = true;
        }
        levelConfig.time = _time;
        levelConfig.botSpeed = 200 * _botComplexity;
        levelConfig.ballCount = _ballQuantity;
        levelConfig.isCustomLevel = true;
        LoadingManager.Instance.StartLoadingNewLevel(levelConfig);
    }

    private void BackToLevelMenu()
    {
        UIManager.Instance.SetActiveScreen(ScreenType.LevelMenu);
    }
}

