using System.Collections;
using UnityEngine;

public class TurtleShell : BaseMonster
{
    #region Fields

    // TurtleShell만의 파티클 시스템(Particle System)
    [SerializeField] private ParticleSystem rollAttack;

    // 애니메이터의 해시(Hash)
    private readonly int hashSkill1 = Animator.StringToHash("IsSkill1");
    private readonly int hashDefend = Animator.StringToHash("IsDefend");

    #endregion Fields

    #region Override Methods

    // 스킬 초기화 함수
    protected override void InitSkills()
    {
        mstATK = 10.0f;
        mstSPD = 10.0f;

        attackCooltime = 0.0f;
        skill1Cooltime = 3.0f;
        skill2Cooltime = 3.0f;

        traceDistance = 10f;
        skillDistance = 10f;
        attackDistance = 2f;
    }

    // 공격 함수
    public override void Attack()
    {
        animator.SetBool(hashAttack, true);
    }

    // 스킬 1 함수
    public override void Skill1()
    {
        // 기왕이면 다 돌아가고 나서 스킬 발동?
        float distance = Vector3.Distance(playerTrf.position, enemyTrf.position);
        animator.SetBool(hashSkill1, true);
    }

    // 스킬 2 함수
    public override void Skill2()
    {
        skill2_curCooltime = skill2Cooltime;
        StartCoroutine(Defend());
    }

    // 특수기?
    public void RAttack()
    {
        StartCoroutine(RollingAttack());
    }

    private IEnumerator RollingAttack()
    {
        agent.isStopped = true;
        ParticleSystem ps = Instantiate(rollAttack, this.transform); //, Quaternion.identity
        rb.AddRelativeForce(Vector3.forward * 20f, ForceMode.VelocityChange);
        yield return new WaitForSeconds(0.8f);

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        skill1_curCooltime = skill1Cooltime;
        Destroy(ps);
        animator.SetBool(hashSkill1, false);
    }

    private IEnumerator Defend()
    {
        animator.SetBool(hashDefend, true);
        yield return new WaitForSeconds(1.5f);
        animator.SetBool(hashDefend, false);
    }

    #endregion Override Methods
}