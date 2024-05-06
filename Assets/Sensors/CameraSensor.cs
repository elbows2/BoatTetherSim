using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;
using Image = RosMessageTypes.Sensor.ImageMsg;
using RosMessageTypes.Geometry;
using RosMessageTypes.Std;

public class CameraSensor : MonoBehaviour {
    
    ROSConnection ros;
    Camera camera;
    RenderTexture rawImage;
    public string imageTopic = "default/image";
    public float publishFrequency = 10f;
    private float publishPeriod;
    private float timeElapsed;

    private Texture2D imageTexture;


    void Start()
    {
        camera = gameObject.GetComponent<Camera>();
        ros = ROSConnection.GetOrCreateInstance();
        //ToDo use NavSat Messages instead here.
        ros.RegisterPublisher<Image>(imageTopic);
        publishPeriod = 1f/publishFrequency;
        timeElapsed = 0f;


    }

    void Update()
    {
        timeElapsed += Time.deltaTime;
         
        if (timeElapsed > publishPeriod) {
            HeaderMsg header = new HeaderMsg();
            RenderTexture.active = camera.targetTexture;
            imageTexture = new Texture2D(camera.targetTexture.width, camera.targetTexture.height);            
            imageTexture.ReadPixels(new Rect(0,0, camera.targetTexture.width, camera.targetTexture.height), 0, 0);
            Image imageMsg= imageTexture.ToImageMsg(header);
                        
            ros.Publish(imageTopic, imageMsg);
            timeElapsed = 0f;

        }

    }
}