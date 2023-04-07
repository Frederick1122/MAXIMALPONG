using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Border : MonoBehaviour
{
    [SerializeField] private TeamType teamType;

    public void UpdateScore()
    {
        GameManager.Instance.UpdateScore(teamType);
    }
}