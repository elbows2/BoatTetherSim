using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
public class Boat : MonoBehaviour
{
    Rigidbody rb;
    
    public float maxLinearVelocity = 3f; //3m/s taken from the Blue Boat Docs.
    public float maxAngularVelocity = 3.14f ; //Limit to half rotation per second?

    public float maxForce = 50f;
    public float maxTorque = 50f;
    Vector3 cmdForce;
    Vector3 cmdTorque;

    void Start()
    {
        gameObject.SetActive(true);
        rb = GetComponent<Rigidbody>();
        rb.maxLinearVelocity = maxLinearVelocity;
        rb.maxAngularVelocity = maxAngularVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddRelativeForce(cmdForce);
        rb.AddRelativeTorque(cmdTorque);
    }

    //Command Expects a Float between -1 and 1 for each value describing the range between full reverse
    //and full forward.
    public void Command(float fwd, float yaw ) {
        
        fwd = Math.Clamp(fwd, -1f, 1f);
        yaw = Math.Clamp(yaw,-1f,1f);
        cmdForce = new Vector3(fwd * maxForce, 0, 0);
        cmdTorque = new Vector3(0, yaw * maxTorque, 0);
    }
}
