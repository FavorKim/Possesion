using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    Transform destination;
    bool closed;
    
    public void SetDestination(Transform dest) {  destination = dest; }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")&&!closed)
        {
            other.GetComponent<PlayerController>().GetCC().enabled = false;
            other.transform.position = destination.position;
            other.GetComponent<PlayerController>().GetCC().enabled = true;
            closed = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
            closed = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            closed = false;
    }
}
