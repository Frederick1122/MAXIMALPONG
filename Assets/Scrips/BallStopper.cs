using UnityEngine;

public class BallStopper : MonoBehaviour
{
   [Range(0,2)]
   public float Multiplier = 1f;
   public TeamType TeamType = TeamType.None;
}