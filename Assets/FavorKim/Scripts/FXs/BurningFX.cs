using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningFX : MonoBehaviour
{
    private void OnParticleSystemStopped()
    {
        Debug.Log(GetComponentInParent<Animator>());
        GetComponentInParent<Animator>().SetTrigger("Break");
    }
}
