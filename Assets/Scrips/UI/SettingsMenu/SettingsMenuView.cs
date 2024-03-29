using System;
using Base;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuView : UIView<SettingsMenuModel>
{
    public event Action OnGoToGame;
    public event Action OnGoToMainMenu;
    public event Action<int> OnChangeMusicParameter;
    public event Action<int> OnChangeEffectParameter;

    [SerializeField] private Button _goToGameButton;
    [SerializeField] private Button _goToMainMenuButton;
    //[SerializeField] private Button _helpButton;

    [Header("Params")]
    [SerializeField] private CustomParameter _musicParameter;
    [SerializeField] private CustomParameter _effectsParameter;
    
    public override void Init()
    {
        var model = SaveManager.Instance.SettingsSaveData.Load();
        
        _musicParameter.Setup("", 0, 100, 10, (int)model.musicVolume);
        _effectsParameter.Setup("", 0, 100, 10, (int)model.effectsVolume);

        _musicParameter.OnUpdateValue += OnChangeMusicParameter;
        _effectsParameter.OnUpdateValue += OnChangeEffectParameter;
        
        _goToGameButton.onClick.AddListener(OnGoToGame.Invoke);
        _goToMainMenuButton.onClick.AddListener(OnGoToMainMenu.Invoke);
    }

    public override void Show()
    { 
        _goToGameButton.gameObject.SetActive(GameBus.Instance.GetLevelType() == LevelType.Game);
        
        if (GameBus.Instance.GetLevelType() == LevelType.Game) 
            TimeFreezer.FreezeTime(0f);
        
        base.Show();
    }

    public override void Hide()
    {
        TimeFreezer.FreezeTime(1f);
        base.Hide();
    }
}

public class SettingsMenuModel : UIModel
{
    public float musicVolume = 1;
    public float effectsVolume = 1;
}

