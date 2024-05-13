using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : Monsters
{
    public override void Move()
    {
        Debug.Log("고블린 이동");
    }

    public override void Attack()
    {
        Debug.Log("고블린 공격");
    }

    public override void Skill1()
    {
        Debug.Log("고블린 스킬 1");

    }
    public override void Skill2()
    {
        Debug.Log("고블린 스킬 2");
    }
}
