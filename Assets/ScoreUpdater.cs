using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUpdater : MonoBehaviour
{
   [SerializeField]private List<TeamText> _teams;

   public void UpdateScore(TeamType teamType)
   {
      foreach (var team in _teams)
      {
         if (team.TeamType == teamType)
         {
            var score = Int16.Parse(team.Text.text);
            score++;
            team.Text.text = score.ToString();
         }
      }
   }
   
   [Serializable]
   internal class TeamText
   {
      public TeamType TeamType;
      public TMP_Text Text;
   }
}
