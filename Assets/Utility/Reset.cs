using UnityEngine;
using UnityEngine.SceneManagement;
public class Reset : MonoBehaviour
{
    public string sceneName;

    void Start()
    {
        if (sceneName == null) {
            sceneName = SceneManager.GetActiveScene().name;
        }
    }
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ResetScene();
        }
    }

    void ResetScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}