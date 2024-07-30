using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Robotics.ROSTCPConnector;
using EnvParams = RosMessageTypes.Boat.EnvParamsMsg;
using EnvInitReq = RosMessageTypes.Boat.EnvInitRequest;
using EnvInitResp = RosMessageTypes.Boat.EnvInitResponse;
using ResetReq = RosMessageTypes.Boat.ResetRequest;


public class Environment : MonoBehaviour
{

    public string serviceName = "/sim/init";
    ROSConnection ros;

    public Tether tether;
    public Rigidbody boat;
    public Rigidbody end;

    public float maxSimSpeed = 20.0f;

    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        if (ros != null) {
            ros.ImplementService<EnvInitReq, EnvInitResp>(serviceName, Init);
        }  
    }

    private EnvInitResp Init(EnvInitReq request)
    {
        EnvParams requested = request.requested;
        EnvParams response = new EnvParams();
        if (requested.n_tether_segments > tether.maxLinks) {
            response.n_tether_segments = tether.maxLinks;
        } else if (requested.n_tether_segments < 0 ) {
            response.n_tether_segments = 0;
        } else {
            response.n_tether_segments = requested.n_tether_segments;
        }
        
        while (response.n_tether_segments > tether.nLinks) {
            tether.addLink();
        }
        while (response.n_tether_segments < tether.nLinks) {
            tether.removeLink();
        }

        response.n_tether_segments = tether.nLinks;

        if (requested.sim_speed > maxSimSpeed) {
            response.sim_speed = maxSimSpeed;
        } else if (requested.sim_speed < 1.0f) {
            response.sim_speed = 1.0f;
        } else {
            response.sim_speed = requested.sim_speed;
        }

        Time.timeScale = requested.sim_speed;
        return new EnvInitResp(response);
    }


}
