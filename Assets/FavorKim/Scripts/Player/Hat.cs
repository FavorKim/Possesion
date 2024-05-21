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
    //[SerializeField] LayerMask monsterLayer;

    RaycastHit hit;
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
        Sledge();

        if (!other.CompareTag("Player")&&!other.CompareTag("Camera"))
        {
            if (other.CompareTag("Monster"))
            {
                hatM.SetHitParticle(transform.position);
                player.SetState(other.GetComponentInParent<Monsters>());
            }
            ResetHat();
        }
    }

    public void ResetHat()
    {
        gameObject.SetActive(false);
        hatM.GetHatImg().SetActive(true);
    }


    void Sledge()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit, 15f))
        {
            hit.collider.gameObject.SendMessage("Damage", 100f, SendMessageOptions.DontRequireReceiver);
            Debug.Log("Dmg");
        }
    }
}
