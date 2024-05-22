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

    public override void OnTypeAttacked(Obstacles attackedType)
    {
        if(attackedType.type == ITyped.Type.WEB)
        {
            StartCoroutine(CorSlowDown());
        }
    }
    IEnumerator CorSlowDown()
    {
        while (anim.speed > 0.05f)
        {
            yield return new WaitForSeconds(0.2f);
            anim.speed *= 0.9f;
            Debug.Log("slow");
        }
    }

}
