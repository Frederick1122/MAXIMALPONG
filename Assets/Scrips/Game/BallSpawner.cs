using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] private Ball _ball;

    public Ball GenerateBall()
    {
        return Instantiate(_ball, transform);
    }
}
