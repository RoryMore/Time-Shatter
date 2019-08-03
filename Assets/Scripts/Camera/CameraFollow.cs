using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform target;
    public float smoothing = 5f;

    Vector3 offset;

    void Start()
    {
        target = GameObject.Find("Player").transform;
        //Offest value relative to the public variables of the camera's
        //location, updating the cam pos will not break this code :)
        offset = transform.position - target.position;
    }

    //Fixed update
    void FixedUpdate()
    {
        Vector3 targetCamPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);

        //Update angle to be equal to player facing angle
    }
}
