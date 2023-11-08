using System;
using UnityEngine;
using UnityEngine.UI;

public class CustomLoadingScreenView : UIView<CustomLoadingScreenModel>
{
    public event Action OnPlay;
    public event Action OnGoToMainMenu;

    public event Action<int> OnTimeUpdate;
    public event Action<int> OnBotComplexityUpdate;
    public event Action<int> OnBallQuantityUpdate;
    
    [SerializeField] private CustomParameter _timeParameterSlider;
    [SerializeField] private CustomParameter _botComplexityParameterSlider;
    [SerializeField] private CustomParameter _ballQuantityParameterSlider;

    [SerializeField] private Button _playButton;
    [SerializeField] private Button _goToMainMenuButton;

    public override void Init()
    {
        _playButton.onClick.AddListener(OnPlay.Invoke);
        _goToMainMenuButton.onClick.AddListener(OnGoToMainMenu.Invoke);

        _timeParameterSlider.OnUpdateValue += OnTimeUpdate;
        _botComplexityParameterSlider.OnUpdateValue += OnBotComplexityUpdate;
        _ballQuantityParameterSlider.OnUpdateValue += OnBallQuantityUpdate;
    }

    private void OnDestroy()
    {
        _playButton?.onClick.RemoveAllListeners();
        _goToMainMenuButton?.onClick.RemoveAllListeners();
        
        if(_timeParameterSlider != null)
            _timeParameterSlider.OnUpdateValue -= OnTimeUpdate;
        if(_botComplexityParameterSlider != null)
            _botComplexityParameterSlider.OnUpdateValue -= OnBotComplexityUpdate;
        if(_ballQuantityParameterSlider  != null)
            _ballQuantityParameterSlider.OnUpdateValue -= OnBallQuantityUpdate;
    }

    public override void UpdateView(CustomLoadingScreenModel uiModel)
    {
        base.UpdateView(uiModel);
        _timeParameterSlider.Setup(uiModel.LevelConfig.timeSliderData);
        _botComplexityParameterSlider.gameObject.SetActive(!uiModel.isLocalMultiplayer);
        _botComplexityParameterSlider.Setup(uiModel.LevelConfig.complexitySliderData);
        _ballQuantityParameterSlider.Setup(uiModel.LevelConfig.ballQuantitySliderData);
    }
}

public class CustomLoadingScreenModel : UIModel
{
    public CustomLevelConfig LevelConfig;
    public bool isLocalMultiplayer;
}