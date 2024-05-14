using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Monsters
{
    public override void InitSkill()
    {
        skill1 = new Skill("슬라임 스킬1", 5, Skill1);
        skill2 = new Skill("슬라임 스킬2", 3, Skill2);
    }

    public override void Move()
    {
        //Debug.Log("슬라임 이동");
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

    public override void SetSkill()
    {
        SkillManager.SetSkill(skill1, 1);
        SkillManager.SetSkill(skill2, 2);
    }
}
