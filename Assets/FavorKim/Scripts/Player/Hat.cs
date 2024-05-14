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

    PlayerController player;

    private void Awake()
    {
        hatM = FindObjectOfType<HatManager>();
        rb = GetComponent<Rigidbody>();
        dO = GetComponent<DOTweenAnimation>();
        player = FindAnyObjectByType<PlayerController>().GetComponent<PlayerController>();
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
            if (other.CompareTag("Monster"))
            {
                hatM.SetHitParticle(transform.position);
                player.SetState(other.GetComponent<Monsters>());
            }
            ResetHat();
        }
    }

    public void ResetHat()
    {
        gameObject.SetActive(false);
        hatM.GetHatImg().SetActive(true);
    }
}
