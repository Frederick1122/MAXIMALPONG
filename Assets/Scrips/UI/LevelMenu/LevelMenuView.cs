using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelMenuView : UIView<LevelMenuModel>
{
    private const string UI_LOCKED_LEVEL = "LOCKED";
    
    public event Action<int> onLaunchLevel;
    public event Action onLaunchCustomLevel;
    public event Action onLaunchLocalMultiplayer;
    
    [SerializeField] private List<CustomButton> _levels;
    [SerializeField] private CustomButton _customLevel;
    [SerializeField] private CustomButton _localMultiplayer;

    public override void Init()
    {
        for (var i = 0; i < _levels.Count; i++)
        {
            var index = i;
            _levels[i].OnClickButton += () => onLaunchLevel?.Invoke(index); // sry for this
        }

        _customLevel.OnClickButton += onLaunchCustomLevel;
        _localMultiplayer.OnClickButton += onLaunchLocalMultiplayer;
    }

    private void OnDestroy()
    {
        if(_customLevel != null)
            _customLevel.OnClickButton -= onLaunchCustomLevel;

        if(_localMultiplayer != null)
            _localMultiplayer.OnClickButton -= onLaunchLocalMultiplayer;
    }

    public override void UpdateView(LevelMenuModel uiModel)
    {
        for (int i = 0; i < _levels.Count; i++)
        {
            if (i < uiModel.lastOpenLevel)
            {
                _levels[i].SetText($"Level {i}");
            }
            else
            {
                _levels[i].SetIcon(UI_LOCKED_LEVEL);
            }
        }
    }
}

public class LevelMenuModel : UIModel
{
    public int lastOpenLevel;
}