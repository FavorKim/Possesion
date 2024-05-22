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
        myType = ITyped.Type.LEAF;
    }

    public override void OnTypeAttacked(ITyped.Type attackedType)
    {
        switch (attackedType)
        {
            case ITyped.Type.CUTTER:
                cutter.SetActive(true);
                break;

            case ITyped.Type.FIRE:
                fire.SetActive(true);
                break;
        }
    }
}
