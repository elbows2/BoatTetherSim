using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ToggleActiveMono : MonoBehaviour
{
    public static bool isActive;
    public GameObject target;

    void Start() {
        isActive = target.activeSelf;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isActive = !isActive;
            target.SetActive(isActive);
        }
    }
}