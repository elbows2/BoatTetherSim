using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using Random = System.Random;
using Math = System.Math;
public class NoisyEstimator : MonoBehaviour
{

    public Transform toFollow;
    public float updatePeriod = 0.5f;
    Vector3 center;
    Random rnd;
    float timeElapsed;
    void Start()
    {
        center = new Vector3(0,0,0);
        rnd = new Random();
        timeElapsed = 0f;
    }

    float gaussian(float mean, float stdDev)
    {
        double u1 = 1.0-rnd.NextDouble(); //uniform(0,1] random doubles
        double u2 = 1.0-rnd.NextDouble();
        double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
        float randNormal = (float)(mean + stdDev * randStdNormal); //random normal(mean,stdDev^2)

        return randNormal;
    }
    void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed > updatePeriod) {
            timeElapsed = 0f;
            AddNoise(toFollow.position);
            transform.SetPositionAndRotation(center, Quaternion.identity);
        }
    }
    void AddNoise(Vector3 actual)
    {
        center = new Vector3(gaussian(actual.x, 0.01f), gaussian(actual.y, 0.01f), gaussian(actual.z, 0.01f));
    }
}