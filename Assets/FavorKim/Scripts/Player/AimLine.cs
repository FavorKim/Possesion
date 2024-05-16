using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimLine : MonoBehaviour
{
    LineRenderer lR;
    [SerializeField] float length;

    private void Awake()
    {
        lR = GetComponent<LineRenderer>();
        lR.SetPosition(0, transform.position);
        lR.SetPosition(1, transform.position + Vector3.forward * length);
        //lR.SetPosition(1, transform.parent.localPosition * length);
        lR.startWidth = 0.03f;
    }

    private void Update()
    {
        //lR.SetPosition(1, transform.parent.localPosition * length);
        lR.SetPosition(1, transform.position + Vector3.forward * length);


    }

}
