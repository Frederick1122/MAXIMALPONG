using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenuController : UIController<SettingsMenuView, SettingsMenuModel>
{
    private const string MUSIC_PARAM = "MusicVolume";
    private const string EFFECTS_PARAM = "EffectsVolume";
    private const string UI_PARAM = "UIVolume";

    [SerializeField] private AudioMixer _mixer;

    private SettingsMenuModel _model;

    public override void Init()
    {
        base.Init();

        // Load values
        _model = SaveManager.Instance.SettingsSaveData.Load();

        _view.EffectsSlider.OnValueChanged += UpdateSound;
        _view.MusicSlider.OnValueChanged += UpdateSound;

        UpdateSound(SliderType.Music, _model.musicVolume);
        UpdateSound(SliderType.Effects, _model.effectsVolume);

        _view.Init();
    }

    private void LoadParametres(SettingsMenuModel from)
    {
        _model = new();
        _model.musicVolume = from.musicVolume;
        _model.effectsVolume = from.effectsVolume;
    }

    public override void Show()
    {
        _view.OnViewClose += Hide;
        UpdateView();
        base.Show();
    }

    public override void Hide()
    {
        // Save Params
        base.Hide();
        _view.OnViewClose -= Hide;
        SaveManager.Instance.SettingsSaveData.Save(_model);
    }

    public override void UpdateView()
    {
        base.UpdateView();
        _view.UpdateView(_model);
    }

    private void UpdateSound(SliderType sliderType, float value)
    {
        float valueMixer = Mathf.Log10(value) * 20;

        switch (sliderType)
        {
            case SliderType.Music:
                _mixer.SetFloat(MUSIC_PARAM, valueMixer);
                _model.musicVolume = value;
                break;

            case SliderType.Effects:
                _mixer.SetFloat(EFFECTS_PARAM, valueMixer);
                _mixer.SetFloat(UI_PARAM, valueMixer);
                _model.effectsVolume = value;
                break;
        }

        SaveManager.Instance.SettingsSaveData.Save(_model);
    }
}

public enum SliderType
{
    Music = 0,
    Effects = 1,
}
