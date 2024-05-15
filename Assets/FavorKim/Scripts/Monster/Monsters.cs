using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monsters : MonoBehaviour
{
    protected float moveSpeed;
    protected GameObject VFXPrefab;
    protected Animator anim;
    
    // 이펙트, 이동속도, 애니메이터 등등... 

    private void Awake()
    {
        InitSkill();
    }

    public abstract void Attack();

    public abstract void Skill1();
    public abstract void Skill2();

    public abstract void Move();

    public abstract void Dead();

    public abstract void InitSkill();

    public Skill skill1;
    public Skill skill2;

    public abstract void SetSkill();

    public void InitAnim(Animator anim)
    {
        this.anim = anim;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Obstacles"))
            GameManager.Instance.Player.SetState("Normal");
    }
}
