using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningObs : Obstacles
{
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public override void OnTypeAttacked(ITyped.Type attackedType)
    {
        if(attackedType == ITyped.Type.WEB)
        {
            StartCoroutine(CorSlowDown());
        }
    }
    IEnumerator CorSlowDown()
    {
        while (anim.speed > 0.1f)
        {
            yield return new WaitForSeconds(0.05f);
            anim.speed *= 0.99f;
        }
    }

}
