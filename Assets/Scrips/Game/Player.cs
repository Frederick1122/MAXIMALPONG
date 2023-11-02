using UnityEngine.InputSystem;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    private const int SPEED_MODIFICATOR = 1000;
    
    [SerializeField] private bool _isSecondPlayer;
    [SerializeField] private bool _isXAxis;
    [SerializeField] private float _speed = 1f;
    
    private bool _isLocalMultiplayer;
    private bool _isMobile;
    private Rigidbody _rigidbody;
    private float _movement;
    private void Start()
    {
        _isMobile = Application.isMobilePlatform;
        _isLocalMultiplayer = LoadingManager.Instance.GetCurrentLevel().isLocalMultiplayer;
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        MovementLogic();
    }

    private void MovementLogic()
    {
        if (_isMobile)
        {
            
        }
        
        _rigidbody.WakeUp();
        _rigidbody.velocity = transform.forward * _movement * _speed * SPEED_MODIFICATOR * Time.fixedDeltaTime;
    }

    public void OnPlayer1Movement(InputValue input)
    {
        if(_isSecondPlayer && _isLocalMultiplayer || _isMobile)
            return;
        
        _movement = _isXAxis ? input.Get<Vector2>().x : input.Get<Vector2>().y;
    }
    
    public void OnPlayer2Movement(InputValue input)
    {
        if(!_isSecondPlayer && _isLocalMultiplayer || _isMobile)
            return;
        
        _movement = _isXAxis ? input.Get<Vector2>().x : input.Get<Vector2>().y;
    }
}
