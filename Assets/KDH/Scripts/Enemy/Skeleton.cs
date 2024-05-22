using System.Collections;
using UnityEngine;

public class Skeleton : BaseMonster
{

    #region Fields
    // MonsterPlant만의 게임 오브젝트(Projectile)
    [SerializeField] private GameObject projectile;
    [SerializeField] private float shootSpeed = 800.0f;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private ParticleSystem stabAttack;

    // 애니메이터의 해시(Hash)
    private readonly int hashSkill1 = Animator.StringToHash("IsSkill1");
    private readonly int hashSkill2 = Animator.StringToHash("IsSkill2");

    #endregion Fields

    #region Override Methods

    // 스킬 초기화 함수
    protected override void InitSkills()
    {
        mstATK = 10.0f;
        mstSPD = 10.0f;

        skill1Cooltime = 3.0f;
        skill2Cooltime = 3.0f;

        traceDistance = 10f;
        skillDistance = 5f;
        attackDistance = 2f;

        InitSkill(skill1Cooltime, skill2Cooltime);
    }

    // 공격 함수
    public override void Attack()
    {
        animator.SetBool(hashAttack, true);
    }

    // 스킬 1 함수
    public override void Skill1()
    {
        float distance = Vector3.Distance(playerTrf.position, enemyTrf.position);
        skill1_curCooltime = skill1Cooltime;
        
        StartCoroutine(SlashAttack());

        IEnumerator SlashAttack()
        {
            agent.isStopped = true;
            animator.SetBool(hashSkill1, true);
            GameObject pd = Instantiate(projectile, spawnPosition.position, Quaternion.identity) as GameObject;
            pd.transform.LookAt(playerTrf.localPosition);
            pd.GetComponent<Rigidbody>().AddForce(pd.transform.forward * shootSpeed);
           
            yield return new WaitForSeconds(1.5f);
            animator.SetBool(hashSkill1, false);
        }
    }

    // 스킬 2 함수
    public override void Skill2()
    {
        animator.SetBool(hashSkill2, true);
    }

    void SAttack()
    {
        StartCoroutine(StabAttack());
    }

    IEnumerator StabAttack()
    {
        agent.isStopped = true;

        ParticleSystem ps = Instantiate(stabAttack, this.transform);
        //ps.transform.LookAt(gameObject.transform.localPosition + Vector3.forward);
        ps.transform.position = spawnPosition.position;
        rb.AddRelativeForce(Vector3.forward * 20f, ForceMode.VelocityChange);
        yield return new WaitForSeconds(0.4f);

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        skill2_curCooltime = skill2Cooltime;
        Destroy(ps);
        animator.SetBool(hashSkill2, false);
    }

    #endregion Override Methods
}
