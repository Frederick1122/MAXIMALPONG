using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class Bot : MonoBehaviour
{
    private List<Ball> _activeBalls = new List<Ball>();
    private Transform _nearestBall = null;
    private float _speed = 200f;
    
    private Coroutine _findNearestBallCoroutine = null;
    private YieldInstruction _findTick = new WaitForSeconds(0.3f);
    
    private Vector3 _minColliderBounds;
    private Vector3 _maxColliderBounds;

    private Rigidbody _rigidbody;
    
    private void Start()
    {
        GameManager.Instance.OnChangeActiveBallsAction += UpdateActiveBalls;

        _rigidbody = GetComponent<Rigidbody>();
        
        var boxColliderBounds = GetComponent<BoxCollider>().size;
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
        
        var angle = Vector3.SignedAngle(targetPos - transform.position, forward, Vector3.up);
        
        if (angle * angle > 10 * 10)
        {
            var movement = angle > 0 ? transform.forward : -transform.forward;
            _rigidbody.velocity = movement * _speed * Time.fixedDeltaTime;
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
        _activeBalls = GameManager.Instance.GetActiveBalls();
        
            if (_findNearestBallCoroutine != null) 
            StopCoroutine(_findNearestBallCoroutine);
        _findNearestBallCoroutine = StartCoroutine(FindNearestBallRoutine());
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
                    var newZ = new Vector3(0, 0, transform.InverseTransformPoint(_nearestBall.position).z) ;
                    Gizmos.DrawSphere( transform.TransformPoint(newZ), 0.1f);

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
