using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttack : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<IDamagable>()?.GetDamage(10);
    }
}
