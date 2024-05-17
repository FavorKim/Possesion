using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Obstacles : MonoBehaviour
{
    public enum Type
    {
        NONE, LEAF = 10, FIRE = 20, CUTTER
    }

    /*
    몬스터의 스킬 내지 공격이 타입을 갖고있어서
    이걸로 플랫포머의 장애물 등등을 극복하는 형태
    덤불 장애물은 CUTTER나 FIRE로 극복
    FIRE, CUTTER >> LEAF
    몬스터 타입은 일단 추후에.
    */

    ParticleSystem ps;
    [SerializeField] private int damage;
    [SerializeField] protected Type type;
    public int Damage { get { return damage; } }
    public Type GetObsType() { return type; }

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        if (ps == null) return;
        ParticleSystem.CollisionModule col = ps.collision;
        col.enabled = true;
        col.type = ParticleSystemCollisionType.World;
        col.sendCollisionMessages = true;
    }

    
    private void OnParticleCollision(GameObject other)
    {
        GameManager.Instance.GetDamage(this, other);
    }

    public virtual void OnTypeAttacked(Type attackedType) { }

}
