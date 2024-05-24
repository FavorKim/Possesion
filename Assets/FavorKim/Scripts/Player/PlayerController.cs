using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour, IDamagable
{
    public float tempKnockBack;

    public Transform tempKnockBackdirect;
    #region Variable

    #region Mono

    private CharacterController CC;
    PlayerStateMachine state;
    Animator anim;
    ParticleSystem invinFX;

    private Transform camTransform;
    [SerializeField] Transform playerFoward;
    [SerializeField]private Transform lookAtTransform;
    [SerializeField] HatManager hatM;

    [SerializeField] TextMeshProUGUI t_fullHP;
    [SerializeField] TextMeshProUGUI t_curHP;

    [SerializeField] Slider hpBar;
    [SerializeField] Slider skill1Gauge;
    [SerializeField] Slider skill2Gauge;

    [SerializeField] Slider durationGauge;

    [SerializeField] GameObject playerOF;

    SkillManager sM;

    #endregion

    #region Vector
    public Vector3 MoveDir { get; private set; }
    Vector3 heading;
    Vector2 dir;
    #endregion

    #region float
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float gravityScale;
    [SerializeField]
    private float jumpForce;
    [SerializeField] float rayDist;
    [SerializeField] float fullHP;
    [SerializeField] float curHP;

    [SerializeField] float duration;

    [SerializeField] float sensitivity;

    [SerializeField] float invincibleTime;

    #endregion


    [SerializeField] public bool isGround { get; private set; }
    bool isDead = false;
    bool isInvincible = false;
    public bool isKnockBack = false;


    //Dictionary<string, GameObject> outFits = new Dictionary<string, GameObject>();

    #endregion

    #region Getter
    public CharacterController GetCC() { return CC; }
    public Animator GetAnimator() { return anim; }
    public Slider GetDurationGauge() { return durationGauge; }
    public Transform CameraTransform { get { return camTransform; } set {  camTransform = value; } }
    public Transform GetPlayerFoward()  { return playerFoward; }
    public Transform GetLookAt() { return lookAtTransform; }
    public float GetMoveSpeed() { return moveSpeed; }
    public float GetGravityScale() { return gravityScale; }
    public float GetJumpForce() { return jumpForce; }
    public float GetDuration() { return duration; }
    #endregion

    #region LifeCycle
    private void Awake()
    {
        CC = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        state = new PlayerStateMachine(this);
        sM = new SkillManager(skill1Gauge, skill2Gauge);

        invinFX = GetComponentInChildren<ParticleSystem>();
        invinFX.Stop();
        OnDead += DeadCheck;
        OnDead += SetHPUI;
        camTransform = transform;

        t_fullHP.text = fullHP.ToString();

        lookAtTransform = Instantiate(new GameObject("LookAt"), Camera.main.transform).transform;
        lookAtTransform.localPosition = new Vector3(0, 0, 56f);

    }

    void Update()
    {
        if (isDead) return;
        state.StateUpdate();
        SetHPUI();

        playerFoward.position = camTransform.position + new Vector3(0, 1.0f, 0);
        PlayerMove();
    }

    private void FixedUpdate()
    {
        CheckLand();
    }

    private void LateUpdate()
    {
        if (isKnockBack)
            CC.Move((transform.position - tempKnockBackdirect.position).normalized * Time.deltaTime * tempKnockBack);
    }

    #endregion

    #region Method

    void PlayerMove()
    {
        /*
        heading = Camera.main.transform.localRotation * Vector3.forward;
        heading.y = 0;
        heading = heading.normalized;
        MoveDir = heading * dir.y * Time.deltaTime * moveSpeed;
        MoveDir += Quaternion.Euler(0, 90, 0) * heading * dir.x * Time.deltaTime * moveSpeed;
        */
        MoveDir = camTransform.TransformDirection(new Vector3(dir.x, 0, dir.y));
        MoveDir *= moveSpeed * Time.deltaTime;
    }



    void SetHPUI()
    {
        t_curHP.text = curHP.ToString();
        hpBar.value = curHP / fullHP;
    }

    void CheckLand()
    {
        Debug.DrawRay(transform.position, Vector3.down * rayDist, Color.red);

        if (Physics.Raycast(transform.position, Vector3.down, rayDist))
            isGround = true;
        else
            isGround = false;
    }

    public void ThrowHat()
    {
        hatM.ShootHat(lookAtTransform.position);
    }

    public void SetState(string name)
    {
        state.ChangeState(name);
        playerOF.SetActive(true);
    }

    public void SetState(Monsters mon)
    {
        state.ChangeState(mon);
        playerOF.SetActive(false);
    }

    public void GetDamage(int dmg)
    {
        if (isInvincible || isDead || dmg == 0) return;

        if (state.IsPossessing())
        {
            return;
        }

        curHP -= dmg;
        anim.SetTrigger("Hit");
        StartCoroutine(CorInvincible());
        OnDead();
    }

    void DeadCheck()
    {
        if (curHP <= 0)
        {
            curHP = 0;
            isDead = true;
            isInvincible = true;
            anim.SetBool("isDead", true);
            anim.SetTrigger("Dead");

        }
    }

    public void KnockBack(Vector3 dir, float duration, float length)
    {
        transform.DOMove((dir + transform.position).normalized * length, duration);
    }

    #endregion

    #region Event

    void OnMove(InputValue val)
    {

        dir = val.Get<Vector2>();

        MoveDir = camTransform.TransformDirection(new Vector3(dir.x, 0, dir.y));
        MoveDir *= moveSpeed * Time.deltaTime;
        if (MoveDir != Vector3.zero)
        {
            anim.SetBool("isRun", true);
        }
        else
        {
            anim.SetBool("isRun", false);
        }



        anim.SetFloat("vecX", dir.x);
        anim.SetFloat("vecY", dir.y);
        //PlayerMove();
        //Debug.Log(heading);
    }

    void OnJump(InputValue val) { if (val.isPressed) state.StateOnJump(); }

    void OnAttack(InputValue val) { if (val.isPressed) state.StateOnAttack(); KnockBack(transform.forward, 2, 50); }

    void OnThrowHat(InputValue val)
    {
        if (val.isPressed)
        {
            state.StateOnHat();
        }
    }

    void OnSkill1(InputValue val)
    {
        if (val.isPressed)
        {
            state.StateOnSkill1();
        }
    }

    void OnSkill2(InputValue val)
    {
        if (val.isPressed)
        { state.StateOnSkill2(); }
    }

    void OnCursor(InputValue val)
    {
        Vector2 delta = val.Get<Vector2>();
        //float deltaX = delta.x;
        //transform.Rotate(new Vector3(0, delta.x, delta.y) * sensitivity * Time.deltaTime);

        if (!isDead && Input.GetMouseButton(1))
        {
            LookAtPlayer(camTransform);
        }
        // 산나비 때 썼던 쉐이더 그래프 끌고와서 조준선으로 만들면 좋을 것
    }

    public void LookAtPlayer(Transform dest)
    {
        dest.LookAt(lookAtTransform);
        dest.eulerAngles = new Vector3(0, dest.eulerAngles.y, 0);
        heading = Camera.main.transform.localRotation * Vector3.forward;
    }


    event Action OnDead;

    #endregion

    IEnumerator CorInvincible()
    {
        isInvincible = true;
        invinFX.Play();
        yield return new WaitForSeconds(invincibleTime);
        invinFX.Stop();
        isInvincible = false;
    }
}

public class Skill
{
    public Skill(float maxCD, Action effect)
    {
        this.effect = effect;
        this.maxCD = maxCD;
    }

    public Slider gauge;

    float maxCD;
    float curCD;

    public bool CanUse() { return curCD > maxCD; }

    Action effect;

    public void UseSkill()
    {
        if (!CanUse()) return;
        effect();
        SetCurCDto0();
    }

    public void SetCurCDto0()
    {
        curCD = 0;
    }

    public void SetCurCD() { curCD += Time.deltaTime; gauge.value = curCD / maxCD; }
}

public class SkillManager
{
    public SkillManager(Slider socket1, Slider socket2)
    {
        SkillManager.socket1 = socket1;
        SkillManager.socket2 = socket2;
    }
    public static Slider socket1;
    public static Slider socket2;

    public static void SetSkill(Skill skill, int socket)
    {
        socket1.gameObject.SetActive(true);
        socket2.gameObject.SetActive(true);

        if (socket == 1)
            skill.gauge = socket1;
        else if (socket == 2)
            skill.gauge = socket2;

        skill.SetCurCDto0();
    }
    public static void ResetSkill()
    {
        socket1.gameObject.SetActive(false);
        socket2.gameObject.SetActive(false);
    }
}
