using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wind : MonoBehaviour
{
    /*private void OnParticleCollision(GameObject other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.Player.isKnockBack = true;
        }
        else
        {
            GameManager.Instance.Player.isKnockBack = false;
        }
    }*/
    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.Player.isKnockBack = true;
        //GameManager.Instance.Player.SetKnockBack(멀어질 대상(Transform) ,)
    }

    private void OnTriggerExit(Collider other)
    {
        GameManager.Instance.Player.isKnockBack = false;
    }
}
