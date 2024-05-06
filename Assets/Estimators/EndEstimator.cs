using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using Vector3Msg = RosMessageTypes.Geometry.Vector3Msg;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;

public class EndEstimator : MonoBehaviour
{

    ROSConnection ros;

    public string subscriptionTopic = "/end/estimate";
    Vector3 center;

    void Start()
    {
        center = new Vector3(0,0,0);
        ros = ROSConnection.GetOrCreateInstance();
        ros.Subscribe<Vector3Msg>(subscriptionTopic,EstimateCallback);
    }

    void Update()
    {
        transform.SetPositionAndRotation(center, Quaternion.identity);
    }
    void EstimateCallback(Vector3Msg estimate)
    {
        center = estimate.From<FLU>();
    }
}