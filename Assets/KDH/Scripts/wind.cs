using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wind : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        Rigidbody body = other.GetComponent<Rigidbody>();
        if (body)
        {
            Vector3 direction = other.transform.position - transform.position;
            direction = direction.normalized;
            body.AddForce(direction * 5);
        }
    }
}
