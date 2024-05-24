using System.Collections;
using UnityEngine;

public class MonsterPlant : BaseMonster
{

    #region Fields

    // MonsterPlant만의 게임 오브젝트(Projectile)
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform spawnPosition;
    private float shootSpeed = 800.0f;

    // 애니메이터의 해시(Hash)
    private readonly int hashSkill2 = Animator.StringToHash("IsSkill2");

    #endregion Fields

    #region Override Methods

    // 스킬 초기화 함수
    protected override void InitSkills()
    {
        mstATK = 10.0f;
        mstSPD = 10.0f;

        attackCooltime = 2.0f;
        skill1Cooltime = 3.0f;
        skill2Cooltime = 3.0f;

        traceDistance = 20f;
        skillDistance = 10f;
        attackDistance = 2f;

        InitSkill(skill1Cooltime, skill2Cooltime);
    }

    // 스킬 1 함수
    public override void Skill1()
    {
        StartCoroutine(ProjectileAttack());
        
    }
    private IEnumerator ProjectileAttack()
    {
        if (agent.isActiveAndEnabled)
            agent.isStopped = true;

        float distance;
        animator.SetBool(hashAttack, true);
        GameObject pd = Instantiate(projectile, spawnPosition.position, spawnPosition.rotation);

        if (!isPlayer)
        {
            distance = Vector3.Distance(playerTrf.position, enemyTrf.position);
            pd.transform.LookAt(playerTrf.localPosition);
        }
        else
        {
            distance = 30.0f;
            pd.transform.LookAt(spawnPosition.position + new Vector3(0, 0, 10));
        }

        if (transform.parent != null)
        {
            pd.transform.LookAt(GameManager.Instance.Player.GetLookAt());
        }
        //pd.transform.LookAt(playerTrf.localPosition);
        //addforce 위치 정해줘야 함.
        pd.GetComponent<Rigidbody>().AddForce(pd.transform.forward * shootSpeed);
        pd.GetComponent<Rigidbody>().AddForce(pd.transform.up * distance * 5f);
        skill1_curCooltime = skill1Cooltime;

        yield return new WaitForSeconds(0.2f);
        animator.SetBool(hashAttack, false);
    }

    // 스킬 2 함수
    public override void Skill2()
    {
        animator.SetTrigger(hashSkill2);
        skill2_curCooltime = skill2Cooltime;
    }

    #endregion Override Methods
}
