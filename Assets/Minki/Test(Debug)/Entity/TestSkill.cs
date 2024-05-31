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
    // 스킬의 내용
    private UnityAction skillAction;

    // 재사용 대기 시간 변수
    private float maxCoolTime;
    private float curCoolTime = 0;

    public Skill(UnityAction skillAction, float maxCoolTime)
    {
        this.skillAction = skillAction;
        this.maxCoolTime = maxCoolTime;
    }

    public void UseSkill()
    {
        bool isCoolTime = (curCoolTime > 0) ? true : false;

        if (!isCoolTime)
        {
            skillAction.Invoke();
            curCoolTime = maxCoolTime;
        }
    }
}
