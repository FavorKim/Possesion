using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamFixer : MonoBehaviour
{
    float fixPosY;

    private void Awake()
    {
        fixPosY = transform.position.y;
        
    }
    void Update()
    {
        transform.position = new Vector3(transform.position.x, fixPosY, transform.position.z);
        //transform.eulerAngles = new Vector3(0,transform.eulerAngles.y, 0);
    }
}
