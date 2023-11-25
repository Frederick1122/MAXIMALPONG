using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomParameterButtons : CustomParameter
{
    [SerializeField] private SoundType _buttonSoundType;
    [SerializeField] protected TMP_Text _parameterCount;
    [SerializeField] protected Button _leftButton;
    [SerializeField] protected Button _rightButton;

    private int _currentStep = 1;
    private int _stepValue;

    public override void Setup(string barName, int minValue, int maxValue, int steps)
    {
        base.Setup(barName, minValue, maxValue, steps);
        
        _leftButton.onClick.RemoveAllListeners();
        _rightButton.onClick.RemoveAllListeners();
        _leftButton.onClick.AddListener(() => UpdateValue(-1));
        _leftButton.onClick.AddListener(() => SoundManager.Instance.AddNewEffect(_buttonSoundType));
        _rightButton.onClick.AddListener(() => UpdateValue(1));
        _rightButton.onClick.AddListener(() => SoundManager.Instance.AddNewEffect(_buttonSoundType));
        
        if (barName != "")
            _name.text = barName;
        
        _stepValue = (maxValue - minValue) / steps;
        UpdateValue(_steps / 2);
    }

    protected override void UpdateValue(float value)
    {
        _currentStep = Mathf.Clamp(_currentStep + (int) value, 1, _steps);
        _value = _currentStep * _stepValue;
        _parameterCount.text = _value.ToString();
        base.UpdateValue(value);
    }
}