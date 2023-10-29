using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class CustomButton : MonoBehaviour
{
    public event Action OnClickButton;
    
    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Image _image;
    [SerializeField] private List<Icon> _iconSwitcher;

    public void SetText(string text, bool hideImage = true)
    {
        _image.enabled = !hideImage;
        _text.text = text;
        _text.enabled = true;
    }
    
    public void SetIcon(string key, bool hideText = true)
    {
        _text.enabled = !hideText;

        foreach (var icon in _iconSwitcher)
        {
            if (icon.key != key)
                continue;
            
            _image.sprite = icon.icon;
            _image.enabled = true;
            return;
        }
        
        Debug.LogError($"Key: {key} not found. Check {this.name} icon switcher");
    }
    
    private void Awake()
    {
        _button.onClick.AddListener(Click);
        _image.enabled = false;
    }

    private void OnDestroy()
    {
        if(_button != null)
            _button.onClick.RemoveAllListeners();
    }

    private void Click()
    {
        OnClickButton?.Invoke();
    } 
}

[Serializable]
public class Icon
{
    public string key;
    public Sprite icon;
}
