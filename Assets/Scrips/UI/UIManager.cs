using System;
using Base;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public event Action OnLoadingScreenIsDone;
    
    [SerializeField] private Image _loadingImage;
    
    [SerializeField] private MainMenuController _mainMenuController;
    [SerializeField] private GameScreenController _gameScreenController; 
    [SerializeField] private EndScreenController _endScreenController;
    [SerializeField] private LevelMenuController _levelMenuController;
    [SerializeField] private CustomLevelScreenController _customLevelMenuController;
    [SerializeField] private SettingsMenuController _settingsMenuController;
    [SerializeField] private HowToPlayController _howToPlayController;

    private const float START_LOADING_SCREEN = 0.01f;
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
        _alphaStep = multiplier / _timePeriod;
        
        _loadingImage.gameObject.SetActive(_isLoadingScreenFullAlpha);
        SetColorAlpha(_loadingImage, _loadingImage.color.a + _alphaStep * START_LOADING_SCREEN);
    }

    public void StartLevelDebugging()
    {
        LoadingManager.Instance.StartLoadingNewLevel(0);
    }

    public void IncrementScore(TeamType _type)
    {
        _gameScreenController.IncrementScore(_type);
    }

    public void StartNewLevel()
    {
        _gameScreenController.StartNewLevel();
    }

    public void SetLocalMultiplayerStage(bool isLocalMultiplayer)
    {
        _customLevelMenuController.SetLocalMultiplayerStage(isLocalMultiplayer);
    }

    public void SetActiveScreen(ScreenType screenType)
    {
        HideAll();
        switch (screenType)
        {
            case ScreenType.MainMenu:
                _mainMenuController.Show();
                break;
            case ScreenType.Game:
                _gameScreenController.Show();
                break;
            case ScreenType.Shop:
                break;
            case ScreenType.Settings:
                _settingsMenuController.Show();
                break;
            case ScreenType.LevelMenu:
                _levelMenuController.Show();
                break;
            case ScreenType.CustomLevelMenu:
                _customLevelMenuController.Show();
                break;
            case ScreenType.HowToPlay:
                _howToPlayController.Show();
                break;
            case ScreenType.EndMenu:
                _endScreenController.Show();
                break;
        }
    }

    private void Start()
    {
        _gameScreenController.Init();
        _mainMenuController.Init();
        _endScreenController.Init();
        _levelMenuController.Init();
        _customLevelMenuController.Init();
        _settingsMenuController.Init();
        _howToPlayController.Init();
        
        SetColorAlpha(_loadingImage, 0f);
        _loadingImage.gameObject.SetActive(false);
        HideAll();
    }
    
    private void HideAll()
    {
        _mainMenuController.Hide();
        _gameScreenController.Hide();
        _endScreenController.Hide();
        _levelMenuController.Hide();
        _customLevelMenuController.Hide();
        _settingsMenuController.Hide();
        _howToPlayController.Hide();
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
    
    private void SetColorAlpha(Image image, float newAlpha)
    {
        newAlpha = Mathf.Clamp(newAlpha, 0f, 1f);
        var color = image.color;
        color.a = newAlpha;
        _loadingImage.color = color;
    }
}
