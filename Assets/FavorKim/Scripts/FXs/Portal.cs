using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    Transform destination;

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<PlayerController>()?.GetCC().Move(destination.position);
    }
}
