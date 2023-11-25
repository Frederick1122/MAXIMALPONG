using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomParameterButtons : CustomParameter
{
    [SerializeField] private SoundType _buttonSoundType;
    [SerializeField] protected TMP_Text _parameterCount;
    [SerializeField] protected Button _leftButton;
    [SerializeField] protected Button _rightButton;

    private int _currentStep;
    private int _stepValue;
    private int _minValue;
    
    public override void Setup(string barName, int minValue, int maxValue, int steps, int initValue)
    {
        base.Setup(barName, minValue, maxValue, steps, initValue);
        
        _leftButton.onClick.RemoveAllListeners();
        _rightButton.onClick.RemoveAllListeners();
        _leftButton.onClick.AddListener(() => UpdateValue(-1));
        _leftButton.onClick.AddListener(() => SoundManager.Instance.AddNewEffect(_buttonSoundType));
        _rightButton.onClick.AddListener(() => UpdateValue(1));
        _rightButton.onClick.AddListener(() => SoundManager.Instance.AddNewEffect(_buttonSoundType));
        
        if (barName != "")
            _name.text = barName;

        _minValue = minValue;
        _stepValue = (maxValue - minValue) / steps;
        var currentStep = 0;
        while (currentStep * _stepValue < initValue) 
            currentStep++;

        UpdateValue(currentStep);
    }

    protected override void UpdateValue(float value)
    {
        _currentStep = Mathf.Clamp(_currentStep + (int) value, 0, _steps);
        _value = _currentStep * _stepValue + _minValue;
        _parameterCount.text = _value.ToString();
        base.UpdateValue(value);
    }
}