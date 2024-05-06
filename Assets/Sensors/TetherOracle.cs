using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using RosMessageTypes.Boat;
using RosMessageTypes.Geometry;
using RosMessageTypes.Std;

public class TetherOracle : MonoBehaviour {
    
    ROSConnection ros;
    Tether tether;
    public string topicName = "default/tether";
    public float publishFrequency = 10f;
    private float publishPeriod;
    private float timeElapsed;

    //Message Definitions

    private HeaderMsg header;
    private PointMsg[] positions;
    

    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        tether = gameObject.GetComponent<Tether>();

        //ToDo use NavSat Messages instead here.
        ros.RegisterPublisher<TetherPoseMsg>(topicName);
        publishPeriod = 1f/publishFrequency;
        timeElapsed = 0f;

    }

    void Update()
    {
        timeElapsed += Time.deltaTime;
         
        if (timeElapsed > publishPeriod) {
            header = new HeaderMsg();
            positions = new PointMsg[tether.nLinks];
            GameObject[] segments = tether.GetSegments();
            for(int i = 0; i < tether.nLinks; i++){
                positions[i]=segments[i].transform.position.To<FLU>();
            }
            
            TetherPoseMsg msg = new TetherPoseMsg(header,positions, tether.nLinks,tether.GetLength() );
                        
            ros.Publish(topicName, msg);
            timeElapsed = 0f;
        }

    }
}