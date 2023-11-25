using System;
using TMPro;
using UnityEngine;

public abstract class CustomParameter : MonoBehaviour
{
    public Action<int> OnUpdateValue;

    [SerializeField] protected TMP_Text _name;
    
    protected int _steps = 1;
    protected int _value;

    public void Setup(CustomSliderData customSliderData)
    {
        Setup(customSliderData.name, customSliderData.minValue, customSliderData.maxValue, customSliderData.steps, customSliderData.initValue);
    }
    
    public virtual void Setup(string barName, int minValue, int maxValue, int steps, int initValue)
    {
        _steps = steps;
    }

    protected virtual void UpdateValue(float value)
    {
        OnUpdateValue?.Invoke(_value);
    }
}