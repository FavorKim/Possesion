using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Monsters
{
    public override void Move()
    {
        Debug.Log("슬라임 이동");
    }


    public override void Attack()
    {
        Debug.Log("슬라임 공격");
    }

    public override void Skill1()
    {
        Debug.Log("슬라임 스킬 1");

    }
    public override void Skill2()
    {
        Debug.Log("슬라임 스킬 2");
    }

}
