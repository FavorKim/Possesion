using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkillManager
{
    #region Fields

    [SerializeField] private Slider skill01Socket;
    [SerializeField] private Slider skill02Socket;

    #endregion Fields

    #region Constructor

    public SkillManager(Slider socket01, Slider socket02)
    {
        skill01Socket = socket01;
        skill02Socket = socket02;
    }

    #endregion Constructor

    #region Custom Methods

    public void SetSkill(Skill skill, int socket)
    {
        skill01Socket.gameObject.SetActive(true);
        skill02Socket.gameObject.SetActive(true);

        //if (socket == 1)
        //    skill.gauge = socket1;
        //else if (socket == 2)
        //    skill.gauge = socket2;

        //skill.SetCurCDto0();
    }

    public void ResetSkill()
    {
        skill01Socket.gameObject.SetActive(false);
        skill02Socket.gameObject.SetActive(false);
    }

    // curCoolTime -= Time.deltaTime; // 점점 감소한다.
    // curCoolTime = Mathf.Max(0, curCoolTime); // 0 미만으로는 감소하지 않는다.

    #endregion Custom Methods
}

public class Skill
{
    // Entity; 코루틴을 실행시키기 위한 변수
    private Entity entity;

    // 스킬의 내용
    private UnityAction skillAction;

    // 재사용 대기 시간 변수
    private float maxCoolTime;
    private float curCoolTime = 0;

    // 생성자
    public Skill(Entity entity, UnityAction skillAction, float maxCoolTime)
    {
        this.entity = entity;
        this.skillAction = skillAction;
        this.maxCoolTime = maxCoolTime;
    }

    // 스킬을 사용하는 함수
    public void UseSkill()
    {
        // 현재 재사용 대기 중인지 판별한다.
        bool isCoolTime = (curCoolTime > 0) ? true : false;

        // 지금 바로 사용할 수 있다면,
        if (!isCoolTime)
        {
            // 스킬 내용을 호출하고, 재사용 대기를 시작한다.
            skillAction.Invoke();
            curCoolTime = maxCoolTime;

            // 관련 코루틴을 실행한다.
            entity.StartCoroutine(CoolDown());
        }
    }

    // 재사용 대기 시간을 감소시키는 코루틴 함수
    private IEnumerator CoolDown()
    {
        // 남은 재사용 대기 시간이 0 이하일 때까지 반복한다.
        while (curCoolTime >= 0)
        {
            // 시간에 비례하여 재사용 대기 시간이 감소한다.
            curCoolTime -= Time.deltaTime;

            yield return null;
        }
    }
}
