using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuView : UIView<SettingsMenuModel>
{
    public event Action OnViewClose;

    [SerializeField] private Button _backButton;
    //[SerializeField] private Button _helpButton;

    [field: Header("Sliders")]
    [field: SerializeField] public SliderAction MusicSlider { get; private set; }
    [field: SerializeField] public SliderAction EffectsSlider { get; private set; }

    public override void Init()
    {
        base.Init();
        MusicSlider.slider.maxValue = 1;
        MusicSlider.slider.minValue = 0.0001f;

        EffectsSlider.slider.maxValue = 1;
        EffectsSlider.slider.minValue = 0.0001f;

        MusicSlider.slider.onValueChanged.AddListener(MusicSlider.Sub);
        EffectsSlider.slider.onValueChanged.AddListener(EffectsSlider.Sub);
        
    }

    public override void Show()
    {
        base.Show();
        _backButton.onClick.AddListener(OnViewClose.Invoke);
    }

    public override void Hide()
    {
        _backButton.onClick.RemoveAllListeners();
        base.Hide();
    }

    public override void UpdateView(SettingsMenuModel uiModel)
    {
        base.UpdateView(uiModel);

        MusicSlider.slider.value = uiModel.musicVolume;
        EffectsSlider.slider.value = uiModel.effectsVolume;
    }
}

public class SettingsMenuModel : UIModel
{
    public float musicVolume = 1;
    public float effectsVolume = 1;
}

[Serializable]
public class SliderAction
{
    public Slider slider;
    public SliderType sliderType;
    public event Action<SliderType, float> OnValueChanged;

    public void Sub(float value)
    {
        slider.onValueChanged.AddListener(delegate (float value) { OnValueChanged?.Invoke(sliderType, value); });
    }
}

