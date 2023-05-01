using System;
using System.Collections;
using System.Collections.Generic;
using Base;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public Action OnLoadingScreenIsDone;
    
    [SerializeField] private Image _loadingImage;
    [SerializeField] private List<UIWindow> _windows;
    
    [SerializeField] private ScoreUpdater _scoreUpdater;
    [SerializeField] private TimerView _timerView;

    [Header("FOR DEBUGGING")] [SerializeField]
    private LevelConfig _levelConfig;
    //loadingScreen
    private bool _isLoadingScreenFullAlpha;
    private bool _isStartLoadingScreen;
    private float _timePeriod;
    private float _alphaStep;
    //
    
    
    public void StartLevel(LevelConfig levelConfig)
    {
        SetActiveWindow(levelConfig.levelType);
        
        if (levelConfig.levelType != LevelType.Game)
            return;
        
        _scoreUpdater.ResetScore();
        _timerView.Init(levelConfig.time);
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

    public void StartLevelDebugging() => GameManager.Instance.StartLoadingNewLevel(_levelConfig);


    public ScoreUpdater GetScoreUpdater() => _scoreUpdater;
    
    private void Start()
    {
        SetColorAlpha(_loadingImage, 0f);
        _loadingImage.gameObject.SetActive(false);
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

    private void SetActiveWindow(LevelType levelType)
    {
        foreach (var uiWindow in _windows) 
            uiWindow.Window.SetActive(uiWindow.Type == levelType);
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
        public LevelType Type;
    }
}
