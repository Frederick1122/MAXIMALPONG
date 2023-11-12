using System;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlayView : UIView<UIModel>
{
    public event Action OnGoToMainMenu;
    
    [SerializeField] private Button _goToMainMenuButton;

    public override void Init()
    {
        _goToMainMenuButton.onClick.AddListener(OnGoToMainMenu.Invoke);
    }

    private void OnDestroy()
    {
        _goToMainMenuButton.onClick.RemoveAllListeners();
    }
}
