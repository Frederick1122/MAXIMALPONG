using System;
using System.Collections;
using System.Collections.Generic;
using Base;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public Action OnLoadingScreenIsDone;
    [SerializeField] private ScoreUpdater _scoreUpdater;
    [SerializeField] private Image _loadingImage;
    [SerializeField] private List<UIWindow> _windows;

    //loadingScreen
    private bool _isLoadingScreenFullAlpha;
    private bool _isStartLoadingScreen;
    private float _timePeriod;
    private float _alphaStep;
    //
    
    public void SetActiveWindow(UIType uiType)
    {
        foreach (var uiWindow in _windows) 
            uiWindow.Window.SetActive(uiWindow.Type == uiType);
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

    public void UpdateUIScore(TeamType teamType) => _scoreUpdater.UpdateScore(teamType);
    
    public void StartLevel()
    {
        GameManager.Instance.StartLoadingNewLevel("Level1", UIType.Game);
    }
    
    
    private void Start()
    {
        SetColorAlpha(_loadingImage, 0f);
        _loadingImage.gameObject.SetActive(false);
    }

    private void Update()
    {
        LoadingScreenUpdater();
    }

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
    
    [Serializable]
    public class UIWindow
    {
        public GameObject Window;
        public UIType Type;
    }
}
