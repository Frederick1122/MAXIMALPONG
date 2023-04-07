using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _maxSpeed = 500f;
    
    [SerializeField] private float _minAngle = -35f;
    [SerializeField] private float _maxAngle = 35f;
    
    private Vector3 _direction;
    private Rigidbody _rigidbody;

    private void OnCollisionEnter(Collision collision)
    {
        var ballStopper = collision.gameObject.GetComponent<BallStopper>();
        if (ballStopper != null)
        {
            var multiplierVector = ballStopper.GetMultiplierVector();
            _direction = new Vector3(multiplierVector.x * _direction.x, 0, multiplierVector.z * _direction.z);
            _speed = Mathf.Clamp(_speed * ballStopper.GetMultiplier(), 1f, _maxSpeed);
            return;
        }

        var border = collision.gameObject.GetComponent<Border>();
        if (border != null)
        {
            border.UpdateScore();
            GameManager.Instance.DestroyBall(this);
            return;
        }

        _rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
        _rigidbody.useGravity = false;
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        var multiplier = Random.Range(0, 2) == 1 ? 1 : -1; 
        var angle = Random.Range(_minAngle, _maxAngle) * multiplier;
        var direction = Utils.GetVectorFromAngle((360 + angle) % 360);
        _direction = new Vector3(direction.x, 0, direction.y);
        Debug.Log($"{angle} Direction: {_direction.normalized}");
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
