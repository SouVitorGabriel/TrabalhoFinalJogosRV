using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _TriggerSensor : MonoBehaviour
{
    public bool collidiSim = false;
    private string isColliding;
    void OnCollisionEnter(Collider other)
    {
        isColliding = other.gameObject.tag;
        Debug.Log("COLLIDI COM: " + IsColliding);
        collidiSim = true;
    }
    // void OnTriggerExit(Collider other)
    // {
    //     isColliding = "null";
    //     collidiSim = false;
    // }
    public string IsColliding {get {return isColliding;}}
}
