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
        LoadingManager.Instance.StartLoadingNewLevel(0);
    }

    public void IncrementScore(TeamType _type)
    {
        _gameScreenController.IncrementScore(_type);
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
                _gameScreenController.StartNewLevel();
                break;
            case ScreenType.Shop:
                break;
            case ScreenType.Settings:
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
        
        SetColorAlpha(_loadingImage, 0f);
        _loadingImage.gameObject.SetActive(false);
        HideAll();
    }
    
    private void HideAll()
    {
        _mainMenuController.Hide();
        _gameScreenController.Hide();
        _endScreenController.Hide();
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
