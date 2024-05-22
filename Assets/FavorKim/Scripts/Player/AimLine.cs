using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimLine : MonoBehaviour
{
    LineRenderer lR;
    [SerializeField] GameObject point;
    [SerializeField] Transform lookat;
    PlayerController player;

    [SerializeField] float length;
    [SerializeField] LayerMask aimLayer;
    float orgLength;
    RaycastHit hit;

    private void Awake()
    {
        lR = GetComponent<LineRenderer>();
        lR.SetPosition(0, transform.position);
        lR.startWidth = 0.03f;
        orgLength = length;
        point.transform.position = Vector3.zero;
        player = GetComponentInParent<PlayerController>();
    }

    private void Update()
    {
        lookat = player.GetLookAt();
        transform.LookAt(lookat);
        DrawAim();
        lR.SetPosition(1, point.transform.position);
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
            point.SetActive(true);
            lR.enabled = true;

            if (Physics.Raycast(transform.position, transform.forward, out hit, length, aimLayer))
            {
                length = hit.distance;
                point.transform.position = hit.point;
            }
            else
            {
                lR.enabled = false;
                length = orgLength;
                point.transform.position = Vector3.zero;
            }
        }
    }
}
