using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using Command = RosMessageTypes.Boat.CommandMsg;

public class ROSController : MonoBehaviour
{

    ROSConnection ros;

    public string subscriptionTopic = "cmd";
    public Boat boat;
    public Tether tether;

    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.Subscribe<Command>(subscriptionTopic,CommandCallback);

    }

    void CommandCallback(Command cmd)
    {
       boat.Command(cmd.vel, cmd.yaw);
       tether.Command(cmd.winch);
    }
}