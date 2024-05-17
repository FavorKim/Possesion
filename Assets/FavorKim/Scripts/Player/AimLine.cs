using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimLine : MonoBehaviour
{
    LineRenderer lR;
    [SerializeField] GameObject point;
    [SerializeField] float length;
    [SerializeField] LayerMask aimLayer;
    float orgLength;
    RaycastHit hit;

    private void Awake()
    {
        
        lR = GetComponent<LineRenderer>();
        lR.SetPosition(0, transform.position);
        lR.SetPosition(1, transform.position + Vector3.forward * length);
        lR.startWidth = 0.03f;
        orgLength = length;
        point.transform.position = Vector3.zero;
    }

    private void Update()
    {
        //lR.SetPosition(1, transform.parent.localPosition * length);

        DrawAim();
        lR.SetPosition(1, transform.position + transform.forward * length);
        lR.SetPosition(0, transform.position);
    }



    void DrawAim()
    {
        if (!Input.GetMouseButton(1))
        {
            lR.enabled = false;
            point.transform.position = Vector3.zero;
            return;
        }
        else
        {
            lR.enabled = true;

            if (Physics.Raycast(transform.position, transform.forward, out hit, length,aimLayer))
            {
                length = hit.distance;
                point.SetActive(true);
                point.transform.position = hit.point;
            }
            else
            {
                length = orgLength;
                point.transform.position = Vector3.zero;
            }
        }

        //else if (!Physics.Raycast(transform.position, transform.forward, out hit, length))
        //{
        //    length = orgLength;
        //    point.SetActive(false);
        //}
    }
}
