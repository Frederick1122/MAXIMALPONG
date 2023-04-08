using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour
{
    [SerializeField] private float _deviation = 0.5f;
    [Space]
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _maxSpeed = 500f;
    
    [SerializeField] private float _minAngle = -35f;
    [SerializeField] private float _maxAngle = 35f;
    
    private Vector3 _direction;
    private Rigidbody _rigidbody;
    private TeamType _preLastPunch = TeamType.None;
    private TeamType _lastPunch = TeamType.None;
    
    private void OnCollisionEnter(Collision collision)
    {
        var ballStopper = collision.gameObject.GetComponent<BallStopper>();
        if (ballStopper != null)
        {
            _direction = Vector3.Reflect(_direction, collision.contacts[0].normal);
            _direction = new Vector3(_direction.x * Random.Range(1f - _deviation, 1f + _deviation), 0,
                _direction.z * Random.Range(1f - _deviation, 1f + _deviation));
            _speed = Mathf.Clamp(_speed * ballStopper.Multiplier, 1f, _maxSpeed);

            if (ballStopper.TeamType != TeamType.None)
            {
                if(_lastPunch != _preLastPunch)
                    _preLastPunch = _lastPunch;
                
                _lastPunch = ballStopper.TeamType;
            }

            if (ballStopper.IsPlane)
            {
                _rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
                _rigidbody.useGravity = false;
            }

            return;
        }

        var border = collision.gameObject.GetComponent<Border>();
        if (border != null)
        {
            if (_lastPunch == border.TeamType)
                _lastPunch = _preLastPunch;
            
            GameManager.Instance.UpdateScore(border.TeamType, _lastPunch);
            GameManager.Instance.DestroyBall(this);
        }
    }

    private void Start()
    {
        GameManager.Instance.AddNewActiveBall(this);
        
        _rigidbody = GetComponent<Rigidbody>();

        var multiplier = Random.Range(0, 2) == 1 ? 1 : -1; 
        var angle = Random.Range(_minAngle, _maxAngle) * multiplier;
        var direction = Utils.GetVectorFromAngle((360 + angle) % 360);
        _direction = new Vector3(direction.x, 0, direction.y);
        
        //Debug.Log($"{angle} Direction: {_direction.normalized}");
    }

    private void FixedUpdate()
    {
        if(_rigidbody.useGravity)
            return;
        
        _rigidbody.velocity = _direction.normalized * _speed * Time.fixedDeltaTime;
    }

    private void OnDestroy()
    {
        
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        //Gizmos.DrawLine(transform.position, );
        Debug.DrawRay(transform.position, _direction.normalized * 100, Color.yellow);
    }
#endif
}
