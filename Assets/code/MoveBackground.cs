using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackground : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 endPosition = new Vector3(40,172,600);
    public float LerpTime= 70f;
    private float ElapsedTime;
    void Start()
    {
        startPosition = transform.position;  
    }

    // Update is called once per frame
    void Update()
    {
        ElapsedTime += Time.deltaTime;
        float LerpPercentage = Mathf.PingPong(ElapsedTime/LerpTime, 1f);
        transform.position = Vector3.Lerp(startPosition,endPosition,Mathf.SmoothStep(0,1,LerpPercentage));
    }
}
