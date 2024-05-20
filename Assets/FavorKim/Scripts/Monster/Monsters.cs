using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Monsters : MonoBehaviour, ITyped
{
    [SerializeField] float curHP;
    [SerializeField] float maxHP;
    float invincibleTime = 1.0f;

    bool isInvincible = false;

    [SerializeField] Slider HPSlider;
    GameObject HPHUDObj;

    [SerializeField] protected ITyped.Type myType;
    public ITyped.Type type { get { return myType; } }

    public float GetHP() { return curHP; }

    /// <summary>
    /// InitSkill 구체화(Skill1 쿨타임, Skill2 쿨타임) 스킬 없으면 Awake 비워두기
    /// </summary>
    protected virtual void Awake() 
    {
        InitHPUI();
        /*
        스킬 1개인 애랑
        2개인 애
        없는 애
        InitSkill(); << 스킬 UI에 등록을 시킬 수 있다.
        */
    }

    /*
    
    공격1
    
    공격2
    
    공격3
    
    */


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

    /// <summary>
    /// 몬스터 피격 함수
    /// </summary>
    /// <param name="dmg">공격자 공격력</param>
    public void GetDamage(int dmg)
    {
        if (!isInvincible)
        {
            curHP -= dmg;
            HPSlider.value = curHP / maxHP;
            StartCoroutine(CorInvincible());
            if (curHP <= 0)
                gameObject.SetActive(false);
        }
    }

    public void EnterPossess()
    {
        OnPossessed();
    }



    public virtual void OnTypeAttacked(Obstacles attacker)
    {
        if ((int)attacker.type > (int)type)
            GetDamage(attacker.Damage * 2);
        else if ((int)attacker.type == (int)type)
            GetDamage(attacker.Damage);
        else
            GetDamage(attacker.Damage / 2);
    }

    void InitHPUI()
    {
        HPHUDObj = Instantiate(Resources.Load<GameObject>("HP_HUD"), transform);
        HPSlider = HPHUDObj.GetComponentInChildren<Slider>();
        HPSlider.value = curHP / maxHP;
    }

    /// <summary>
    /// 빙의 상태 진입 시 호출되는 이벤트. Awake(혹은 Start)에서 OnPossessed+=으로 구독.
    /// </summary>
    protected event Action OnPossessed;



    IEnumerator CorInvincible()
    {
        isInvincible = true;
        float org = invincibleTime;
        while (true)
        {
            yield return null;
            invincibleTime -= Time.deltaTime;
            if (invincibleTime < 0)
            {
                isInvincible = false;
                invincibleTime = org;
                StopCoroutine(CorInvincible());
                break;
            }
        }
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
        if (skill2 == null) return;
        SkillManager.SetSkill(skill2, 2);
    }
}
