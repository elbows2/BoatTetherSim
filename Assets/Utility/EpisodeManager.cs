using UnityEngine;

public class EpisodeManager : MonoBehaviour
{
    public int episodeNumber = 0;
    public float timeScale = 1f;

    public static EpisodeManager Instance;
    private void Awake(){
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Time.timeScale = timeScale;
    }
}