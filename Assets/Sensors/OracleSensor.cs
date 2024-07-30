using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using Imu = RosMessageTypes.Sensor.ImuMsg;
using RosMessageTypes.Geometry;
using RosMessageTypes.Std;


public class OracleSensor : MonoBehaviour {
    
    Rigidbody rb;
    ROSConnection ros;


    public EpisodeManager episode;
    public string imuTopic = "default/imu";
    public string poseTopic = "default/pose";
    public string velocityTopic = "default/vel";
    public float publishFrequency = 10f;
    private float publishPeriod;
    private float timeElapsed;

    private Vector3 previousVelocity;
    private Vector3 acceleration;

    //Message Definitions

    private HeaderMsg header;
    private QuaternionMsg orientation;
    private double[] orientationCov;
    private Vector3Msg angularVel;
    private double[] angularVelCov;

    private Vector3Msg linearAcc;
    private double[] linearAccCov;

    private PointMsg position;
    private PoseMsg pose;
    

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        ros = ROSConnection.GetOrCreateInstance();
        //ToDo use NavSat Messages instead here.
        ros.RegisterPublisher<PoseStampedMsg>(poseTopic);
        ros.RegisterPublisher<Imu>(imuTopic);
        ros.RegisterPublisher<Vector3Msg>(velocityTopic);
        publishPeriod = 1f/publishFrequency;
        timeElapsed = 0f;
        previousVelocity = rb.velocity;

        //Measurements are exact so no covariance. // 
        orientationCov = new double[9];
        angularVelCov = new double[9];
        linearAccCov = new double[9];
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;
         
        if (timeElapsed > publishPeriod) {
            header = new HeaderMsg();
            header.frame_id = "" + EpisodeManager.Instance.episodeNumber;
            orientation = rb.transform.rotation.To<FLU>();
            angularVel = rb.angularVelocity.To<FLU>();
            acceleration = (rb.velocity - previousVelocity)/timeElapsed;
            // Debug.Log(acceleration);
            linearAcc= acceleration.To<FLU>();
            Imu imuMsg = new Imu (header,
                                orientation,
                                orientationCov,
                                angularVel,
                                angularVelCov,
                                linearAcc,
                                linearAccCov);
            
            position = rb.transform.position.To<FLU>();
            pose = new PoseMsg(position, orientation);
            PoseStampedMsg poseMsg = new PoseStampedMsg(header,pose);
                        
            ros.Publish(imuTopic, imuMsg);
            ros.Publish(poseTopic, poseMsg);
            ros.Publish(velocityTopic, (Vector3Msg)rb.velocity.To<FLU>());
            timeElapsed = 0f;
            previousVelocity = rb.velocity;

        }

    }
}