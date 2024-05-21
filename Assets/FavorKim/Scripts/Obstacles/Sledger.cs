using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sledger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.SendMessage("Damage", 1f, SendMessageOptions.DontRequireReceiver);
    }
}
