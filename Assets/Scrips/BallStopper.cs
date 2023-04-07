using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BallStopper : MonoBehaviour
{
   [Range(0,2)]
   [SerializeField] private float _multiplier = 1f;
   [SerializeField] private XZAxis _axis;

   private Vector3 _multiplierVector = Vector3.zero;


   public Vector3 GetMultiplierVector()
   {
      if (_multiplierVector == Vector3.zero)
         _multiplierVector = _axis == XZAxis.X ? Vector3.left + Vector3.forward : Vector3.right + Vector3.back;

      return _multiplierVector;
   }

   public float GetMultiplier() => _multiplier;
}

public enum XZAxis
{
   X,
   Z
}