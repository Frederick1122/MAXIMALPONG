using System;
using System.Collections.Generic;
using Lean.Localization;
using UnityEngine;

public class LevelMenuView : UIView<LevelMenuModel>
{
    private const string UI_LOCKED_LEVEL = "LOCKED";
    private const string UI_LEVEL = "UI_LEVEL";
    
    public event Action<int> onLaunchLevel;
    public event Action onLaunchCustomLevel;
    public event Action onLaunchLocalMultiplayer;
    public event Action onBackToMainMenu;
    
    [SerializeField] private List<CustomButton> _levels;
    [SerializeField] private CustomButton _customLevel;
    [SerializeField] private CustomButton _localMultiplayer;
    [SerializeField] private CustomButton _backToMainMenu;

    public override void Init()
    {
        for (var i = 0; i < _levels.Count; i++)
        {
            var index = i;
            _levels[i].OnClickButton += () => onLaunchLevel?.Invoke(index); // sry for this
        }

        _customLevel.OnClickButton += onLaunchCustomLevel;
        _localMultiplayer.OnClickButton += onLaunchLocalMultiplayer;
        _backToMainMenu.OnClickButton += onBackToMainMenu;
    }

    private void OnDestroy()
    {
        if(_customLevel != null)
            _customLevel.OnClickButton -= onLaunchCustomLevel;

        if(_localMultiplayer != null)
            _localMultiplayer.OnClickButton -= onLaunchLocalMultiplayer;
        
        if(_backToMainMenu != null)
            _backToMainMenu.OnClickButton -= onBackToMainMenu;
    }

    public override void UpdateView(LevelMenuModel uiModel)
    {
        for (int i = 0; i < _levels.Count; i++)
        {
            if (i < uiModel.lastOpenLevel)
                _levels[i].SetText($"{LeanLocalization.GetTranslationText(UI_LEVEL)} {i + 1}");
            else
                _levels[i].SetIcon(UI_LOCKED_LEVEL);
        }
    }
}

public class LevelMenuModel : UIModel
{
    public int lastOpenLevel;
}