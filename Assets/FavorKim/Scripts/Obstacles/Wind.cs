using Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    [SerializeField] float power;
    [SerializeField] Transform pos;

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<PlayerController>()?.StartKnockBack(pos, power);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponentInChildren<Golem>() != null)
            other.GetComponentInParent<PlayerController>()?.StopKnockBack();
        else
            other.GetComponentInParent<PlayerController>()?.StartKnockBack(pos, power);
    }

    private void OnTriggerExit(Collider other)
    {
        other.GetComponentInParent<PlayerController>()?.StopKnockBack();
    }
}
