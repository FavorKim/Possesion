using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafObs : Obstacles
{
    [SerializeField] GameObject fire;
    [SerializeField] GameObject cutter;


    private void Awake()
    {
        fire.SetActive(false);
        cutter.SetActive(false);
        type = Type.LEAF;
    }

    public override void OnTypeAttacked(Type attackedType)
    {
        switch (attackedType)
        {
            case Type.CUTTER:
                cutter.SetActive(true);
                break;

            case Type.FIRE:
                fire.SetActive(true);
                break;
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
            GameManager.Instance.GetDamage(this, other.gameObject);
    }
}
