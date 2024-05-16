using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VcamController : MonoBehaviour
{
    CinemachineVirtualCamera vcam;
    private void Awake()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            vcam.Priority = 2;
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
            vcam.Priority = 0;
    }
}
