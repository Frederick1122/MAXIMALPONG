using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomParameterSlider : CustomParameter
{
    [SerializeField] private TMP_Text _minValueText;
    [SerializeField] private TMP_Text _maxValueText;
    [SerializeField] private Slider _slider;

    private string _barName = "";

    private void Start()
    {
        _slider.onValueChanged.AddListener(UpdateValue);
    }

    private void OnDestroy()
    {
        _slider?.onValueChanged.RemoveAllListeners();
    }

    public override void Setup(string barName, int minValue, int maxValue, int steps, int initValue)
    {
        base.Setup(barName, minValue, maxValue, steps, initValue);
        _barName = barName;
        _slider.maxValue = maxValue;
        _slider.minValue = minValue;
        _slider.value = (minValue + maxValue) / 2.0f;
        _minValueText.text = minValue.ToString();
        _maxValueText.text = maxValue.ToString();
        UpdateValue(_slider.value);
    }

    protected override void UpdateValue(float value)
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
        
        base.UpdateValue(value);
    }
}
