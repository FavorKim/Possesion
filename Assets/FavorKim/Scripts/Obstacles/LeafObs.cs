using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafObs : Obstacles
{
    [SerializeField] GameObject fire;
    [SerializeField] ParticleSystem cutter;


    private void Awake()
    {
        fire.SetActive(false);
        //cutter.Stop();
        type = Type.LEAF;
    }

    public override void OnTypeAttacked(Type attackedType)
    {
        switch (attackedType)
        {
            case Type.CUTTER:
                //cutter.Play();
                break;

            case Type.FIRE:
                fire.SetActive(true);
                break;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            GameManager.Instance.GetDamage(this, other.gameObject);
    }
}
