using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hat : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float power;
    HatManager hatM;
    DOTweenAnimation dO;
    private void Awake()
    {
        hatM = FindObjectOfType<HatManager>();
        rb = GetComponent<Rigidbody>();
        dO = GetComponent<DOTweenAnimation>();
    }

    private void OnEnable()
    {
        rb.velocity = Vector3.zero;
        rb.AddForce(transform.forward * power, ForceMode.Impulse);
        dO.DORestartAllById("Shoot");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            hatM.GetHatImg().SetActive(true);
        }

    }
}
