using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Monsters, ITyped
{


    protected override void Awake()
    {
        InitSkill(0);
    }

    public override void Attack()
    {
        Debug.Log("슬라임공격");
    }
    public override void Skill1()
    {
        Debug.Log("슬라임 스킬 1");
    }


}
