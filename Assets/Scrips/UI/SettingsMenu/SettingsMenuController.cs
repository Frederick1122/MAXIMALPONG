using System;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenuController : UIController<SettingsMenuView, SettingsMenuModel>
{

    private const string UI_PARAM = "UIVolume";

    
    private AudioMixer _mixer;
    private SettingsMenuModel _model;

    public override void Init()
    {
        // Load values
        _mixer = SoundManager.Instance.GetMixer();
        _model = SaveManager.Instance.SettingsSaveData.Load();

        _view.OnChangeMusicParameter += UpdateMusic;
        _view.OnChangeEffectParameter += UpdateEffects;

        UpdateSound(SliderType.Music, _model.musicVolume);
        UpdateSound(SliderType.Effects, _model.effectsVolume);

        base.Init();
    }

    private void OnDestroy()
    {
        if(_view == null)
            return;
        
        _view.OnChangeMusicParameter -= UpdateMusic;
        _view.OnChangeEffectParameter -= UpdateEffects;
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
        
        if(_model != null)
            SaveManager.Instance.SettingsSaveData.Save(_model);
    }

    public override void UpdateView()
    {
        base.UpdateView();
        _view.UpdateView(_model);
    }

    private void UpdateMusic(int value) => UpdateSound(SliderType.Music, value);
    private void UpdateEffects(int value) => UpdateSound(SliderType.Effects, value);

    private void UpdateSound(SliderType sliderType, float value)
    {
        float valueMixer = Mathf.Log10(value) * 20;
        _mixer.SetFloat(sliderType.ToString(), valueMixer);

        switch (sliderType)
        {
            case SliderType.Music:
                _model.musicVolume = value;
                break;

            case SliderType.Effects:
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
