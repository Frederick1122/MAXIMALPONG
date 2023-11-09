using System;
using Base;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuView : UIView<SettingsMenuModel>
{
    private const string MUSIC_PARAM = "MusicVolume";
    private const string EFFECTS_PARAM = "EffectsVolume";
    
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
        _musicParameter.Setup(MUSIC_PARAM, 0, 100, 10);
        _effectsParameter.Setup(EFFECTS_PARAM, 0, 100, 10);

        _musicParameter.OnUpdateValue += OnChangeMusicParameter;
        _musicParameter.OnUpdateValue += OnChangeEffectParameter;
        
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

