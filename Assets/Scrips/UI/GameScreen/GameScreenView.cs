using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameScreenView : UIView<GameScreenModel>
{
    [SerializeField] private List<TeamText> _teams;
    [SerializeField] private TMP_Text _timerText;

    public override void UpdateView(GameScreenModel uiModel)
    {
        SetTime(uiModel.remainingTime);
        foreach (var teamScore in uiModel.teamScores)
        {
            foreach (var team in _teams)
            {
                var teamType = team.TeamTypes.Find(type => type == teamScore.Key);
                if (teamType == TeamType.None)
                    continue;
                
                team.Text.text = teamScore.Value.ToString();
                break;
            }
        }
    }

    private void SetTime(int time)
    {
        _timerText.text = TimeSpan.FromSeconds(time).ToString()[3..];
    }
    
    public void ResetScore()
    {
        foreach (var team in _teams) 
            team.Text.text = "0";
    }
}

[Serializable]
internal class TeamText
{
    public List<TeamType> TeamTypes;
    public TMP_Text Text;
}

public class GameScreenModel : UIModel
{
    public int remainingTime;
    public Dictionary<TeamType, int> teamScores = new Dictionary<TeamType, int>();
}
