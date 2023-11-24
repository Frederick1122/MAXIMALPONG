using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : UIView<MainMenuModel>
{
    public event Action StartGameAction;
    public event Action OpenSettingsAction;
    public event Action OpenHowToPlayAction;
    
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _howToPlayButton;

    public override void Init()
    {
        _playButton.onClick.AddListener(() => {
            StartGameAction.Invoke();
            SoundManager.Instance.AddNewEffect(SoundType.CLICK);
        });
        _settingsButton.onClick.AddListener(OpenSettingsAction.Invoke);
        _howToPlayButton.onClick.AddListener(OpenHowToPlayAction.Invoke);
    }

    private void OnDestroy()
    {
        _playButton.onClick.RemoveAllListeners();
        _settingsButton.onClick.RemoveAllListeners();
        _howToPlayButton.onClick.RemoveAllListeners();
    }
}

public class MainMenuModel : UIModel
{
    
}