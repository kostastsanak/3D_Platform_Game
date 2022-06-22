using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 endPosition = new Vector3(55,22,22);
    public float LerpTime= 2f;
    private float ElapsedTime;

    void Start()
    {
        startPosition = transform.position;    
    }

    void Update()
    {
        ElapsedTime += Time.deltaTime;
        float LerpPercentage = Mathf.PingPong(ElapsedTime/LerpTime, 1f);    
        transform.position = Vector3.Lerp(startPosition,endPosition,Mathf.SmoothStep(0,1,LerpPercentage));
    }
}
