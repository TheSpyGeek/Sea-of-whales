using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform target;
    public Vector3 offset;


    public float smoothSpeed = 0.125f;


    private void LateUpdate() {
        Vector3 diseredPos = target.position + offset;
        Vector3 smoothPos = Vector3.Lerp(transform.position, diseredPos, smoothSpeed);
        transform.position = smoothPos;
        
    }

}
