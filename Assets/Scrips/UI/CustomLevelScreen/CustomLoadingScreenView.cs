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
    
    [SerializeField] private CustomSlider _timeSlider;
    [SerializeField] private CustomSlider _botComplexitySlider;
    [SerializeField] private CustomSlider _ballQuantitySlider;

    [SerializeField] private Button _playButton;
    [SerializeField] private Button _goToMainMenuButton;

    public override void Init()
    {
        _playButton.onClick.AddListener(OnPlay.Invoke);
        _goToMainMenuButton.onClick.AddListener(OnGoToMainMenu.Invoke);

        _timeSlider.OnUpdateValue += OnTimeUpdate;
        _botComplexitySlider.OnUpdateValue += OnBotComplexityUpdate;
        _ballQuantitySlider.OnUpdateValue += OnBallQuantityUpdate;
    }

    private void OnDestroy()
    {
        _playButton?.onClick.RemoveAllListeners();
        _goToMainMenuButton?.onClick.RemoveAllListeners();
        
        if(_timeSlider != null)
            _timeSlider.OnUpdateValue -= OnTimeUpdate;
        if(_botComplexitySlider != null)
            _botComplexitySlider.OnUpdateValue -= OnBotComplexityUpdate;
        if(_ballQuantitySlider  != null)
            _ballQuantitySlider.OnUpdateValue -= OnBallQuantityUpdate;
    }

    public override void UpdateView(CustomLoadingScreenModel uiModel)
    {
        base.UpdateView(uiModel);
        _timeSlider.Setup(uiModel.LevelConfig.timeSliderData);
        _botComplexitySlider.gameObject.SetActive(!uiModel.isLocalMultiplayer);
        _botComplexitySlider.Setup(uiModel.LevelConfig.complexitySliderData);
        _ballQuantitySlider.Setup(uiModel.LevelConfig.ballQuantitySliderData);
    }
}

public class CustomLoadingScreenModel : UIModel
{
    public CustomLevelConfig LevelConfig;
    public bool isLocalMultiplayer;
}