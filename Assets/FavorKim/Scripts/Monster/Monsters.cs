using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class Monsters : MonoBehaviour, ITyped
{
    #region Fields
    // 필드(Fields)
    [SerializeField] private float curHP; // 몬스터의 현재 체력
    [SerializeField] private float maxHP; // 몬스터의 최대 체력
    
    private float invincibleTime = 1.0f; // 무적 시간
    private bool isInvincible = false; // 무적 여부

    private GameObject HP_HUD_Obj; // 몬스터의 체력을 나타내는 패널(Panel)
    [SerializeField] private Slider HPSlider; // 패널 내의 슬라이더


    // ※ 아래의 스킬 목록을
    public Skill skill1 { get; set; } // 몬스터의 스킬 1 (스킬(Skill) 클래스는 PlayerController에서 정의하고 있다.)
    public Skill skill2; // 몬스터의 스킬 2
    // → 아래와 같이 배열로 구성하는 것이 더 좋지 않을까?
    public Skill[] skills { get; protected set; } // 몬스터의 스킬 목록 (최소 0 ~ 최대 3 정도로 고려 중)

    public ITyped.Type type { get; protected set; }

    public float GetHP() { return curHP; }

    #endregion Fields

    #region Start()

    private void Start()
    {
        // 체력 패널을 초기화(생성)한다.
        HP_HUD_Obj = Instantiate(Resources.Load<GameObject>("HP_HUD"), transform);
        HPSlider = HP_HUD_Obj.GetComponentInChildren<Slider>();

        // 슬라이더의 값을 (현재 체력 / 최대 체력)으로 한다.
        HPSlider.value = curHP / maxHP;
    }

    #endregion Start()

    #region Virtual Methods

    /// <summary>
    /// InitSkill 구체화(Skill1 쿨타임, Skill2 쿨타임) 스킬 없으면 Awake 비워두기
    /// </summary>
    public abstract void Awake();

    /// <summary>
    /// 몬스터의 기본 공격(평타)을 구현하는 함수입니다. (Abstract; 필수)
    /// </summary>
    public abstract void Attack();

    /// <summary>
    /// 몬스터의 첫 번째 스킬을 구현하는 함수입니다. (Virtual; 임의)
    /// </summary>
    public virtual void Skill1() { }

    /// <summary>
    /// 몬스터의 두 번째 스킬을 구현하는 함수입니다. (Virtual; 임의)
    /// </summary>
    public virtual void Skill2() { }

    #endregion Virtual Methods

    #region Custom Methods

    /// <summary>
    /// 몬스터의 스킬을 초기화합니다. 재사용 대기시간을 지정합니다.
    /// </summary>
    /// <param name="coolTimes">스킬의 재사용 대기시간</param>
    public void InitSkill(params float[] coolTimes)
    {
        int skillCount = coolTimes.Length;

        switch (skillCount)
        {
            case 1:
                skill1 = new Skill(coolTimes[0], Skill1);
                break;
            case 2:
                skill1 = new Skill(coolTimes[0], Skill1);
                skill2 = new Skill(coolTimes[1], Skill2);
                break;
            default:
                Debug.Log("입력한 매개변수의 개수가 올바르지 않습니다.");
                break;
        }
    }

    /// <summary>
    /// 몬스터가 피격했을 때 호출되는 함수입니다. 받은 대미지만큼 체력이 감소합니다.
    /// </summary>
    public void GetDamage(int damage)
    {
        // 무적 상태가 아닐 경우,
        if (!isInvincible)
        {
            // 대미지만큼 체력을 감소시킨다.
            curHP -= damage;

            // 감소한 체력을 체력 패널에 적용한다.
            HPSlider.value = curHP / maxHP;

            // 무적 상태에 돌입한다.
            StartCoroutine(CorInvincible());

            // 체력이 0 이하일 경우(죽음), 그 몬스터를 비활성화한다.
            if (curHP <= 0)
                gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 피격했을 때 타입을 비교하는 함수입니다. 타입의 상성에 따라 받는 대미지가 변화합니다.
    /// </summary>
    public virtual void OnTypeAttacked(Obstacles attacker)
    {
        // 공격자의 타입이 우위일 경우,
        if ((int)attacker.type > (int)type)
            // 받는 대미지가 2배가 된다.
            GetDamage(attacker.Damage * 2);
        // 타입이 동위일 경우,
        else if ((int)attacker.type == (int)type)
            // 받는 대미지가 1배가 된다.
            GetDamage(attacker.Damage);
        // 공격자의 타입이 열위일 경우,
        else
            // 받는 대미지가 절반이 된다.
            GetDamage(attacker.Damage / 2);
    }

    // 무적을 구현하는 코루틴 함수
    IEnumerator CorInvincible()
    {
        //isInvincible = true;
        //float org = invincibleTime;
        //while (true)
        //{
        //    yield return null;
        //    invincibleTime -= Time.deltaTime;
        //    if (invincibleTime < 0)
        //    {
        //        isInvincible = false;
        //        invincibleTime = org;
        //        StopCoroutine(CorInvincible());
        //        break;
        //    }
        //}

        // → 간단하게 구현할 수 있다.
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }

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

    #endregion Custom Methods
}
