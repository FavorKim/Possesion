using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningFX : MonoBehaviour
{
    private void OnParticleSystemStopped()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
