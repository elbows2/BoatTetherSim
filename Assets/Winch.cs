using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Winch : MonoBehaviour
{

    public float winchSpeed =1f;
    private Tether tether;
    private int nLinks;
    public int maxLinks = 25;
    public int minLinks = 2;

    void Awake()
    {
        tether = transform.parent.GetComponent<Tether>();
        nLinks = tether.nLinks;    
    }

    public void Rotate(float speed) {
        if (speed > 0 && tether != null  && nLinks < maxLinks) {
            transform.Rotate(0,0,speed);
            tether.addLink();
            nLinks++;
        }
        else if(speed < 0 && tether != null && nLinks > minLinks) {
            transform.Rotate(0,0,speed);
            tether.removeLink();
            nLinks--;
        }
    }

}
