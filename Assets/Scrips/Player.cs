using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    
    private bool _isMobile;
    private Rigidbody _rigidbody;

    private void Start()
    {
        _isMobile = Application.isMobilePlatform;
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        MovementLogic();
    }

    private void MovementLogic()
    {
        float zMovement = 0f;
        if (_isMobile)
        {
            
        }
        else
        {
            zMovement = Input.GetAxis("Horizontal");
        }
        _rigidbody.WakeUp();
        _rigidbody.velocity = new Vector3(0, 0, zMovement)  * _speed * Time.fixedDeltaTime;
    }
}
