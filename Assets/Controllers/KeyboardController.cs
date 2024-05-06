using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using Command = RosMessageTypes.Boat.CommandMsg;

public class KeyboardController : MonoBehaviour
{

    ROSConnection ros;
    public Boat boat;
    public Tether tether;

    public string topic = "/sim/cmd";
    public float publishFrequency = 10f;
    private float publishPeriod;
    private float timeElapsed;

    public KeyCode winchUpKey = KeyCode.K;
    public KeyCode winchDownKey = KeyCode.J;
    public float tetherSpeedScalar = 1.0f;
    public float torqueScalar = 1.0f;
    public float forceScalar = 1.0f;

    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<Command>(topic);
        publishPeriod = 1f/publishFrequency;
        timeElapsed = 0f;
    }

    void Update()
    {
        float winch = 0;
        if (Input.GetKey(winchUpKey)) {
            winch = tetherSpeedScalar;
        } else if(Input.GetKey(winchDownKey)) {
            winch = -tetherSpeedScalar;
        }
       
        tether.Command(winch);

        float turn = Input.GetAxis("Horizontal");
        float fwd = Input.GetAxis("Vertical");
        boat.Command(fwd,turn);
        timeElapsed += Time.deltaTime;
        if (timeElapsed > publishPeriod) {
            ros.Publish(topic, new Command(fwd, turn, winch));
            timeElapsed = 0f;
        }
        

    }

    void CommandCallback(Command cmd)
    {
       boat.Command(cmd.vel, cmd.yaw);
       tether.Command(cmd.winch);
    }
}