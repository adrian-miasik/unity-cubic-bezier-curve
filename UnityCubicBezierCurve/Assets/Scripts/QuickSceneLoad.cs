using UnityEngine;
using UnityEngine.SceneManagement;

public class QuickSceneLoad : MonoBehaviour
{
    [SerializeField] private int sceneIndex = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }
}