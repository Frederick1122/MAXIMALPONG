using UnityEngine;

public class BallStopper : MonoBehaviour
{
   [Range(0,2)]
   [SerializeField] private float _multiplier = 1f;

   public float GetMultiplier() => _multiplier;
}