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

   public void ResetScore()
   {
      foreach (var team in _teams) 
         team.Text.text = "0";
   }

   [Serializable]
   internal class TeamText
   {
      public TeamType TeamType;
      public TMP_Text Text;
   }
}
