using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tether : MonoBehaviour
{
    public int nLinks = 5;
    public float linkLength = .125f;
    public int maxLinks = 25;
    public int minLinks = 1;
    public float winchSpeed = 0.0f;

    public Rigidbody fixedPoint;
    public GameObject ropeSegment;
    public GameObject endEffector;
    private LinkedList<GameObject> tetherSegments;

    private float timeSinceAdd = 0f;
    private float tetherLength = 0f;
    // Start is called before the first frame update
    void Start()
    {
        tetherSegments = new LinkedList<GameObject>();
        CreateRope();
        
    }

    // Update is called once per frame
    void Update()
    {

        tetherLength -= Time.deltaTime * winchSpeed;
        if(tetherLength > linkLength) {
            addLink();
            tetherLength = 0f;
        } else if (tetherLength < 0) {
            removeLink();
            tetherLength = linkLength;
        }
    }

    void CreateRope()
    {
        Rigidbody prevRb = fixedPoint;        
        for (int i = 0; i < nLinks; i++) {
            GameObject link = Instantiate(ropeSegment);
            link.transform.parent = transform;
            link.transform.position = transform.position  + Vector3.down * .25f;

            CharacterJoint joint = link.GetComponent<CharacterJoint>();
            joint.connectedBody = prevRb;
            tetherSegments.AddLast(link);
            prevRb = link.GetComponent<Rigidbody>();
        }
        endEffector.transform.parent = transform;
        endEffector.transform.position = transform.position + Vector3.down * .25f;
        endEffector.GetComponent<CharacterJoint>().connectedBody = prevRb;
    }

    public void addLink()
    {
        if (nLinks >= maxLinks) {
            return;
        }
        GameObject newLink = Instantiate(ropeSegment);
        newLink.transform.parent = transform;
        newLink.transform.position = transform.position + Vector3.down * .25f;
        
        CharacterJoint joint = newLink.GetComponent<CharacterJoint>();
        joint.connectedBody = fixedPoint;

        CharacterJoint previousTop = tetherSegments.First.Value.GetComponent<CharacterJoint>();
        previousTop.connectedBody = newLink.GetComponent<Rigidbody>();
        // previousTop.GetComponent<Segment>().resetAnchor();
        tetherSegments.AddFirst(newLink);
        nLinks++;
    }

    public void removeLink()
    {
        if (nLinks <= minLinks) {
            return;
        }
        GameObject top = tetherSegments.First.Value;
        tetherSegments.RemoveFirst();
        Destroy(top);
        top = tetherSegments.First.Value;
        top.GetComponent<CharacterJoint>().connectedBody = fixedPoint;
        nLinks--;
    }

    public void Command(float cmd) {
        this.winchSpeed = cmd;
    }

    public GameObject[] GetSegments() {
        GameObject[] array = new GameObject[nLinks];
        this.tetherSegments.CopyTo(array,0);
        return array;
    }

    public float GetLength() {
        return nLinks * linkLength;
    }

}
