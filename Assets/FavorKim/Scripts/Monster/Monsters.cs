using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monsters : MonoBehaviour
{
    //protected float moveSpeed;
    //protected GameObject VFXPrefab;
    //protected Animator anim;

    // 이펙트, 이동속도, 애니메이터 등등... 

    /// <summary>
    /// InitSkill 구체화(Skill1 쿨타임, Skill2 쿨타임) 스킬 없으면 Awake 비워두기
    /// </summary>
    public abstract void Awake();

    /// <summary>
    /// 공격 추상함수
    /// </summary>
    public abstract void Attack();

    /// <summary>
    /// 첫번째 스킬 가상함수 (부모는 공란이므로 스킬을 사용할 것이라면 채워넣을 것)
    /// </summary>
    public virtual void Skill1() { }

    /// <summary>
    /// 두번째 스킬 가상함수 (부모는 공란이므로 스킬을 사용할 것이라면 채워넣을 것)
    /// </summary>
    public virtual void Skill2() { }

    /// <summary>
    /// 스킬 초기화 (스킬이 2개일 경우)
    /// </summary>
    /// <param name="firstCoolTime">첫번째 스킬의 쿨타임 (쿨타임 없으면 0)</param>
    /// <param name="secondCoolTime">두번째 스킬의 쿨타임 (쿨타임 없으면 0)</param>
    public void InitSkill(float firstCoolTime, float secondCoolTime) 
    {
        skill1 = new Skill(firstCoolTime, Skill1);
        skill2 = new Skill(secondCoolTime, Skill2);
    }
    /// <summary>
    /// 스킬 초기화 (스킬이 1개일 경우)
    /// </summary>
    /// <param name="firstCoolTime">스킬 쿨타임 (쿨타임 없으면 0)</param>
    public void InitSkill(float firstCoolTime)
    {
        skill1 = new Skill(firstCoolTime, Skill1);
    }

    public Skill skill1;
    public Skill skill2;

    /// <summary>
    /// 스킬을 스킬 UI에 등록
    /// </summary>
    public void SetSkill() 
    {
        if (skill1 == null) return;
        SkillManager.SetSkill(skill1, 1);
        if(skill2 == null) return;
        SkillManager.SetSkill(skill2, 2);
    }

    //public void InitAnim(Animator anim)
    //{
    //    this.anim = anim;
    //}


    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.collider.CompareTag("Obstacles"))
    //        GameManager.Instance.Player.SetState("Normal");
    //}
}
