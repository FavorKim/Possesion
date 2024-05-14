using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMonster : MonoBehaviour
{
    public enum MonsterState
    {
        IDLE,
        TRACE,
        ATTACK,
        DEAD
    }

    [SerializeField]
    float mstATK = 10.0f;
    float mstSPD = 10.0f;
    public float mstSkill1Cooltime = 3.0f;
    public float mstSkill2Cooltime = 3.0f;

    public float traceDistance = 10f;
    public float skillDistance = 10f;
    public float attackDistance = 2f;

    public bool isDie = false;

    public abstract void Attack();
    public abstract void Skill1();
    public abstract void Skill2();
}
