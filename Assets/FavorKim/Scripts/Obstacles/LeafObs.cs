using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafObs : InteractableObstacles
{
    [SerializeField] GameObject fire;
    [SerializeField] GameObject cutter;

    private void Awake()
    {
        fire.SetActive(false);
        cutter.SetActive(false);
        myType = ITypeInteractable.Type.LEAF;
    }

    public override void Interact(ITypeInteractable.Type attackedType)
    {
        switch (attackedType)
        {
            case ITypeInteractable.Type.CUTTER:
                cutter.SetActive(true);
                break;

            case ITypeInteractable.Type.FIRE:
                fire.SetActive(true);
                break;
        }
    }

    public void VineBreak()
    {
        gameObject.SetActive(false);
    }
}
