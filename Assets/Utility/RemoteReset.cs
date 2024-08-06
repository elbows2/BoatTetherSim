using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Robotics.ROSTCPConnector;
using ResetResp = RosMessageTypes.Boat.ResetResponse;
using ResetReq = RosMessageTypes.Boat.ResetRequest;
using SaveResp = RosMessageTypes.Boat.SaveResponse;
using SaveReq = RosMessageTypes.Boat.SaveRequest;

public class RemoteReset : MonoBehaviour
{
    public string sceneName;
    public string resetServiceName = "/sim/reset";
    public string saveServiceName = "save";
    public string saveBaseDir = "data/";
    ROSConnection ros;

    void Awake()
    {
        ros = ROSConnection.GetOrCreateInstance();
        if (ros != null) {
            ros.ImplementService<ResetReq, ResetResp>(resetServiceName, ResetScene);
            ros.RegisterRosService<SaveReq, SaveResp>(saveServiceName);
        }        
        if (sceneName == null) {
            sceneName = SceneManager.GetActiveScene().name;
        }
    }

    private ResetResp ResetScene(ResetReq req)
    {
        SceneManager.LoadScene(sceneName);
        Debug.Log("Reset Scene!");
          if (ros != null) {
            SaveReq request = new SaveReq(string.Format("{0}/trial{1}",saveBaseDir, EpisodeManager.Instance.episodeNumber));
            ros.SendServiceMessage<SaveResp>(saveServiceName, request, SaveCallback);
        }
        EpisodeManager.Instance.episodeNumber += 1;
        //Response can't be null or empty or it will break the unity - ros connection.
        return new ResetResp("boat");
    }
    void SaveCallback(SaveResp resp) {
        Debug.Log("Saved");
    }

}