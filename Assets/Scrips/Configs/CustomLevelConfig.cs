using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomLevelConfig", menuName = "Configs/CustomLevelConfig")]
public class CustomLevelConfig : ScriptableObject
{
    public CustomSliderData timeSliderData;
    public CustomSliderData complexitySliderData;
    public CustomSliderData ballQuantitySliderData;
}

[Serializable]
public class CustomSliderData
{
    public string name;
    public int minValue;
    public int maxValue;
    public int steps;
}