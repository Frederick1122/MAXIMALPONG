using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class Bot : MonoBehaviour
{
    [SerializeField] private float _maxDistance = 25f;
    
    private List<Ball> _activeBalls = new();
    private Transform _nearestBall;

    private Vector3 _minColliderBounds;
    private Vector3 _maxColliderBounds;
    private Vector3 _nearestBallProjection;

    private Rigidbody _rigidbody;
    private BoxCollider _boxCollider;
    private int _speed = 200;

    private void Start()
    {
        UpdateActiveBalls();
        MatchManager.Instance.OnChangeActiveBallsAction += UpdateActiveBalls;
        _speed = LoadingManager.Instance.GetCurrentLevel().botSpeed;
        
        _rigidbody = GetComponent<Rigidbody>();
        _boxCollider = GetComponent<BoxCollider>();

        var boxColliderBounds = _boxCollider.size;

        _minColliderBounds = -new Vector3(0, 0, boxColliderBounds.z / 2);
        _maxColliderBounds = new Vector3(0, 0, boxColliderBounds.z / 2);
    }

    private void OnDestroy()
    {
        if (MatchManager.Instance != null)
            MatchManager.Instance.OnChangeActiveBallsAction -= UpdateActiveBalls;
    }

    public void Update()
    {
        FindNearestBall();
        Move();
    }

    private void Move()
    {
        if (_nearestBall == null)
            return;

        _nearestBallProjection = new Vector3(0, 0, transform.InverseTransformPoint(_nearestBall.position).z);
        _nearestBallProjection = transform.TransformPoint(_nearestBallProjection);

        if( _boxCollider.bounds.Contains(_nearestBallProjection) )
            return;

        if (transform.InverseTransformPoint(_nearestBall.transform.position).x < _maxDistance)
        {
            var targetPos = _nearestBall.transform.position;
            targetPos.y = transform.position.y;
            var forward = transform.right;
            var angle = Vector3.SignedAngle(targetPos - transform.position, forward, Vector3.up);
            var movement = angle > 0 ? transform.forward : -transform.forward;
            _rigidbody.velocity = movement * _speed * Time.fixedDeltaTime;
        }
    }

    private void FindNearestBall()
    {
        float minDistance = 0;
        foreach (var ball in _activeBalls)
        {
            if (Vector3.Angle(transform.right, ball.GetDirection()) < 90 || Vector3.Angle(transform.right,
                    ball.transform.localPosition - transform.localPosition) > 90)
            {
                if (_nearestBall != null && ball.transform == _nearestBall)
                    _nearestBall = null;

                continue;
            }

            var xDistance = transform.InverseTransformPoint(ball.transform.position).x;

            if (minDistance == 0 || xDistance < minDistance)
            {
                minDistance = xDistance;
                _nearestBall = ball.transform;
            }
        }
    }

    private void UpdateActiveBalls() => _activeBalls = MatchManager.Instance.GetActiveBalls();

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward, Color.blue);
        Debug.DrawLine(transform.position, transform.position + transform.right, Color.black);
        if( !Application.isPlaying || _nearestBall == null)
            return;
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.1f);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.TransformPoint(_minColliderBounds), 0.1f);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.TransformPoint(_maxColliderBounds), 0.1f);

        if (_nearestBall != null)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawSphere(_nearestBallProjection, 0.1f);

            Gizmos.color = Color.red;
            var newX = new Vector3(transform.InverseTransformPoint(_nearestBall.position).x, 0, 0);
            Gizmos.DrawSphere(transform.TransformPoint(newX), 0.1f);
        }

        Debug.DrawLine(transform.TransformPoint(_minColliderBounds * transform.right.x),
            _minColliderBounds + _nearestBall.transform.position, Color.red);
        Debug.DrawLine(transform.TransformPoint(_maxColliderBounds * transform.right.x),
            _maxColliderBounds + _nearestBall.transform.position, Color.red);
        Debug.DrawLine(transform.position, _nearestBall.transform.position, Color.red);
    }
#endif
}