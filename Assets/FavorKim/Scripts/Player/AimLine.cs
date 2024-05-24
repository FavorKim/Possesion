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
        //transform.localPosition = Vector3.zero;
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
        //transform.LookAt(lookat);
        lR.SetPosition(1, point.transform.position);
        lR.SetPosition(0, transform.position);
    }

    private void FixedUpdate()
    {
        DrawAim();
    }

    void DrawAim()
    {

        if (Input.GetMouseButton(1))
        {
            lR.enabled = true;
            point.gameObject.SetActive(true);

            if (Physics.Raycast(transform.position, lookat.position - transform.position, out hit, length, aimLayer))
            {
                length = hit.distance;
                point.transform.position = hit.point - transform.forward * 0.5f;
            }
            else
            {
                length = orgLength;
            }
        }
        else
        {
            point.gameObject.SetActive(false);
            lR.enabled = false;
        }
    }
}
