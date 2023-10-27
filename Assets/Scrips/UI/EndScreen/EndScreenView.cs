using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndScreenView : UIView<EndScreenModel>
{
    public event Action OnGoToMainMenu;

    [SerializeField] private TMP_Text _result;
    [SerializeField] private Button _goToMenuButton;

    [SerializeField] private Color _winColor;
    [SerializeField] private Color _loseColor;
    
    public override void UpdateView(EndScreenModel uiModel)
    {
        base.UpdateView(uiModel);
        if (uiModel.isWinner)
        {
            _result.text = "Win";
            _result.color = _winColor;
        }
        else
        {
            _result.text = "Lose";
            _result.color = _loseColor;
        }
    }

    public override void Init()
    {
        _goToMenuButton.onClick.AddListener(OnGoToMainMenu.Invoke);
    }

   
    private void OnDestroy()
    {
        _goToMenuButton.onClick.RemoveAllListeners();
    }
}

public class EndScreenModel : UIModel
{
    public bool isWinner;
}
