using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class TrajectoryLogger : MonoBehaviour
{
    
    public string folder;
    public string filename;
    private StreamWriter outfile;

    public GameObject[] tracked;
    public Tether tether;
    private string formatType = "F8";

    // Start is called before the first frame update

    void Start()
    {
        outfile = new StreamWriter(Path.Combine(folder, filename));
    }

    // Update is called once per frame
    void Update()
    {
        string datapoint = Time.time.ToString(formatType);
        foreach (GameObject point in tracked) {
            datapoint += "," +  point.transform.position.ToString(formatType) + "," + point.transform.rotation.ToString(formatType);
        }
        if(tether != null) {
            datapoint +="," + tether.nLinks;
        }
        outfile.WriteLine(datapoint);
    }

    void OnDestroy()
    {
        outfile.Close();
    }
}
