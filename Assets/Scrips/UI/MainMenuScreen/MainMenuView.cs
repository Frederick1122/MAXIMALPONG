using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : UIView<MainMenuModel>
{
    public event Action StartGameAction;
    public event Action OpenSettingsAction;
    
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _settingsButton;

    public override void Init()
    {
        _playButton.onClick.AddListener(StartGameAction.Invoke);
        _settingsButton.onClick.AddListener(OpenSettingsAction.Invoke);
    }

    private void OnDestroy()
    {
        _playButton.onClick.RemoveAllListeners();
        _settingsButton.onClick.RemoveAllListeners();
    }
}

public class MainMenuModel : UIModel
{
    
}