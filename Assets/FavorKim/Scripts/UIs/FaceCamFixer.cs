using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class FaceCamFixer : MonoBehaviour
{
    Vector3 orgPosition;
    quaternion orgRotation;

    private void Awake()
    {
        orgPosition = transform.position;
        orgRotation = transform.rotation;
        
    }
    void Update()
    {
        /*
        transform.position = new Vector3(transform.position.x, fixPosY, transform.position.z);
        transform.eulerAngles = new Vector3(0,transform.eulerAngles.y, 0);*/
        transform.rotation = orgRotation;
    }
}
