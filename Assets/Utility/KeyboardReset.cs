using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Robotics.ROSTCPConnector;
using SaveResp = RosMessageTypes.Boat.SaveResponse;
using SaveReq = RosMessageTypes.Boat.SaveRequest;

public class Reset : MonoBehaviour
{
    public string sceneName;
    public string saveServiceName = "save";
    public string saveBaseDir = "data/";
    ROSConnection ros;

    int trialNum = 0;

    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        if (ros != null) {
            ros.RegisterRosService<SaveReq, SaveResp>(saveServiceName);
        }        
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
        if (ros != null) {
            SaveReq request = new SaveReq(string.Format("{0}/trial{1}",saveBaseDir, trialNum));
            ros.SendServiceMessage<SaveResp>(saveServiceName, request, SaveCallback);
        }
        trialNum += 1;
    }

    void SaveCallback(SaveResp resp) {
        Debug.Log("Saved");
    }

}