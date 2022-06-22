using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject target;
    public GameObject target2;
    public bool triggered;

    public float xDistance, yDistance, zDistance;

    // Update is called once per frame
    void Update()
    {
        if(triggered == false){
            transform.position = target.transform.position + new Vector3(xDistance,yDistance,zDistance);
            transform.LookAt(target.transform.position);
        }else{
            transform.position = target2.transform.position + new Vector3(xDistance,yDistance,zDistance);
            transform.LookAt(target2.transform.position);
        }
        
    }
}
