using System;
using Base;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public Action OnLoadingScreenIsDone;
    
    [SerializeField] private Image _loadingImage;
    //[SerializeField] private List<UIWindow> _windows;
    [SerializeField] private MainMenuController _mainMenuController;
    [SerializeField] private GameScreenController _gameScreenController; 
    
    // [SerializeField] private ScoreUpdater _scoreUpdater;
    // [SerializeField] private TimerView _timerView;

    [Header("FOR DEBUGGING")] [SerializeField]
    private LevelConfig _levelConfig;
    
    //loadingScreen
    private bool _isLoadingScreenFullAlpha;
    private bool _isStartLoadingScreen;
    private float _timePeriod;
    private float _alphaStep;
    //

    public void StartLevel(LevelType levelType)
    {
        var screenType = levelType switch
        {
            LevelType.MainMenu => ScreenType.MainMenu,
            LevelType.Game => ScreenType.Game,
            _ => ScreenType.MainMenu
        };

        SetActiveScreen(screenType);
    }
    
    public void SetActiveLoadingScreen(bool isFullAlpha, float timePeriod = 1f)
    {
        _timePeriod = timePeriod;
        _isLoadingScreenFullAlpha = isFullAlpha; 
        _isStartLoadingScreen = true;
        
        var multiplier = _isLoadingScreenFullAlpha ? 1f : -1f;
        _alphaStep = multiplier / (_timePeriod);
        
        _loadingImage.gameObject.SetActive(_isLoadingScreenFullAlpha);
        SetColorAlpha(_loadingImage, _loadingImage.color.a + _alphaStep * Time.deltaTime);
    }

    public void StartLevelDebugging()
    {
        MatchManager.Instance.StartLoadingNewLevel(_levelConfig, LevelType.Game);
    }

    public void HideAll()
    {
        _mainMenuController.Hide();
        _gameScreenController.Hide();
    }

    public void IncrementScore(TeamType _type)
    {
        _gameScreenController.IncrementScore(_type);
    }

    private void Start()
    {
        _gameScreenController.Init();
        _mainMenuController.Init();
        
        SetColorAlpha(_loadingImage, 0f);
        _loadingImage.gameObject.SetActive(false);
        SetActiveScreen(ScreenType.MainMenu);
    }

    private void Update() => LoadingScreenUpdater();

    private void LoadingScreenUpdater()
    {
        if (_isStartLoadingScreen)
        {
            if (_loadingImage.color.a != 0 && _loadingImage.color.a != 1)
                SetColorAlpha(_loadingImage, _loadingImage.color.a + _alphaStep * Time.deltaTime);
            else
            {
                _isStartLoadingScreen = false;
                OnLoadingScreenIsDone?.Invoke();
            }
        }
    }

    private void SetActiveScreen(ScreenType screenType)
    {
        HideAll();
        switch (screenType)
        {
            case ScreenType.MainMenu:
                _mainMenuController.Show();
                break;
            case ScreenType.Game:
                _gameScreenController.Show();
                _gameScreenController.StartNewLevel();
                break;
            case ScreenType.Shop:
                break;
            case ScreenType.Settings:
                break;
        }
    }
    
    private void SetColorAlpha(Image image, float newAlpha)
    {
        newAlpha = Mathf.Clamp(newAlpha, 0f, 1f);
        var color = image.color;
        color.a = newAlpha;
        _loadingImage.color = color;
    }
    
    [Serializable]
    public class UIWindow
    {
        public GameObject Window;
        public ScreenType Type;
    }
}
