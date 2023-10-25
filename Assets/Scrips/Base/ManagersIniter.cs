using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagersIniter : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(this);
        SceneManager.LoadScene(1);
    }
}
