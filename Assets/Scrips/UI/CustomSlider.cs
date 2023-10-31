using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomSlider : MonoBehaviour
{
    public Action<int> OnUpdateValue;

    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _minValue;
    [SerializeField] private TMP_Text _maxValue;
    [SerializeField] private Slider _slider;

    private int _steps = 1;
    private int _value;
    private string _barName = "";
    
    private void Start()
    {
        _slider.onValueChanged.AddListener(UpdateValue);
    }

    private void OnDestroy()
    {
        _slider?.onValueChanged.RemoveAllListeners();
    }

    public void Setup(CustomSliderData customSliderData)
    {
        Setup(customSliderData.name, customSliderData.minValue, customSliderData.maxValue, customSliderData.steps);
    }
    
    private void Setup(string barName, int minValue, int maxValue, int steps)
    {
        _minValue.text = minValue.ToString();
        _maxValue.text = maxValue.ToString();
        _slider.maxValue = maxValue;
        _slider.minValue = minValue;
        _barName = barName;
        _steps = steps;
        
        
        _slider.value = (minValue + maxValue) / 2.0f;
        UpdateValue(_slider.value);
    }

    private void UpdateValue(float value)
    {
        var valueStep = (_slider.maxValue - _slider.minValue) / _steps;
        for (var i = 0; i < _steps + 1; i++)
        {
            if (value >= _slider.maxValue - valueStep * i)
            {
                _value = (int) (_slider.minValue + valueStep * (_steps - i));
                _name.text = $"{_barName} : {_value}";
                break;
            }
        }
        
        OnUpdateValue?.Invoke(_value);
    }
}
