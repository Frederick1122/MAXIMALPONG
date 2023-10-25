using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class Bot : MonoBehaviour
{
    [SerializeField] private float _speed = 200f;
    [SerializeField] private float _maxDistance = 25f;
    private List<Ball> _activeBalls = new List<Ball>();
    private Transform _nearestBall = null;
    
    private Coroutine _findNearestBallCoroutine = null;
    private YieldInstruction _findTick = new WaitForSeconds(0.3f);

    private Vector3 _minColliderBounds;
    private Vector3 _maxColliderBounds;
    private Vector3 _nearestBallProjection;

    private Rigidbody _rigidbody;
    private BoxCollider _boxCollider;
    
    private void Start()
    {
        MatchManager.Instance.OnChangeActiveBallsAction += UpdateActiveBalls;

        _rigidbody = GetComponent<Rigidbody>();
        _boxCollider = GetComponent<BoxCollider>();
        
        var boxColliderBounds = _boxCollider.size;

       _minColliderBounds = -new Vector3(0, 0, boxColliderBounds.z / 2);
        _maxColliderBounds = new Vector3(0, 0, boxColliderBounds.z / 2);
    }

    private void FixedUpdate()
    {
        if (_nearestBall == null)
            return;
        
        var targetPos = _nearestBall.transform.position;
        targetPos.y = transform.position.y;
        
        var forward = transform.right;

        _nearestBallProjection = new Vector3(0, 0, transform.InverseTransformPoint(_nearestBall.position).z) ;
        _nearestBallProjection = transform.TransformPoint(_nearestBallProjection);

        // transform.position =
        //     Vector3.MoveTowards(transform.position, _nearestBallProjection, _speed * Time.deltaTime);
        if (transform.InverseTransformPoint(_nearestBall.transform.position).x < _maxDistance)
        {
            if (!_boxCollider.bounds.Contains(_nearestBallProjection))
            {
                var angle = Vector3.SignedAngle(targetPos - transform.position, forward, Vector3.up);
                var movement = angle > 0 ? transform.forward : -transform.forward;
                _rigidbody.velocity = movement * _speed * Time.deltaTime;
            }
        }
    }

    private IEnumerator FindNearestBallRoutine()
    {
        while (true)
        {
            FindNearestBall();
            yield return _findTick;
        }
    }

    private void FindNearestBall()
    {
        float minDistance = 0;
        foreach (var ball in _activeBalls)
        {
            var XDistance = transform.InverseTransformPoint(ball.transform.position).x;
            
            if (minDistance == 0 || XDistance < minDistance)
            {
                minDistance = XDistance;
                _nearestBall = ball.transform;
            }
        }
    }
    
    private void UpdateActiveBalls()
    {
        _activeBalls = MatchManager.Instance.GetActiveBalls();
        
            if (_findNearestBallCoroutine != null) 
            StopCoroutine(_findNearestBallCoroutine);
        _findNearestBallCoroutine = StartCoroutine(FindNearestBallRoutine());
    }

    private void OnDestroy()
    {
        if (_findNearestBallCoroutine != null)
        {
            MatchManager.Instance.OnChangeActiveBallsAction -= UpdateActiveBalls;
            StopCoroutine(_findNearestBallCoroutine);
            _findNearestBallCoroutine = null;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            if (_nearestBall != null)
            {  
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(transform.position, 0.1f);
                
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(transform.TransformPoint(_minColliderBounds), 0.1f);
                
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(transform.TransformPoint(_maxColliderBounds), 0.1f);

                if (_nearestBall != null)
                {
                    Gizmos.color = Color.white;
                    Gizmos.DrawSphere( _nearestBallProjection, 0.1f);

                    Gizmos.color = Color.red;
                    var newX = new Vector3(transform.InverseTransformPoint(_nearestBall.position).x, 0, 0) ;
                    Gizmos.DrawSphere( transform.TransformPoint(newX), 0.1f);
                }

                Debug.DrawLine( transform.TransformPoint(_minColliderBounds), _minColliderBounds + _nearestBall.transform.position, Color.red);
                Debug.DrawLine(transform.TransformPoint(_maxColliderBounds), _maxColliderBounds + _nearestBall.transform.position, Color.red);
                Debug.DrawLine(transform.position, _nearestBall.transform.position, Color.red);
            }
        }
    }
#endif
}
