using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuView : UIView<SettingsMenuModel>
{
    private const string MUSIC_PARAM = "MusicVolume";
    private const string EFFECTS_PARAM = "EffectsVolume";
    
    public event Action OnViewClose;
    public event Action OnGoToMainMenu;
    public event Action<int> OnChangeMusicParameter;
    public event Action<int> OnChangeEffectParameter;

    [SerializeField] private Button _backToGameButton;
    [SerializeField] private Button _goToMainMenuButton;
    //[SerializeField] private Button _helpButton;

    [Header("Params")]
    [SerializeField] private CustomParameter _musicParameter;
    [SerializeField] private CustomParameter _effectsParameter;
    
    public override void Init()
    {
        base.Init();
        _musicParameter.Setup(MUSIC_PARAM, 0, 100, 10);
        _effectsParameter.Setup(EFFECTS_PARAM, 0, 100, 10);

        _musicParameter.OnUpdateValue += OnChangeMusicParameter;
        _musicParameter.OnUpdateValue += OnChangeEffectParameter;
    }

    public override void Show()
    {
        base.Show();
        _backToGameButton.onClick.AddListener(OnViewClose.Invoke);
    }

    public override void Hide()
    {
        _backToGameButton.onClick.RemoveAllListeners();
        base.Hide();
    }
}

public class SettingsMenuModel : UIModel
{
    public float musicVolume = 1;
    public float effectsVolume = 1;
}

