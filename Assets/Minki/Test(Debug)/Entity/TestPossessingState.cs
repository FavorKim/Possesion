//using Enemy;
//using UnityEngine;

//public class TestPossessingState : TestPlayerState
//{
//    public TestPossessingState(TestPlayer controller) : base(controller)
//    {
//        durationGauge = player.GetDurationGauge();
//    }

//    Slider durationGauge;
//    float playerOrgJumpForce;

//    public override void Enter()
//    {

//        playerOrgJumpForce = orgJumpForce;
//        if (mon is Slime)
//        {
//            orgJumpForce += 10;
//            jumpForce += 10;
//        }


//        GameObject hatImg = player.GetHatManager().GetHatImg();
//        hatImg.SetActive(true);


//        player.CameraTransform = mon.transform;
//        GameManager.Instance.SetCameraFollow(player.CameraTransform);
//        GameManager.Instance.SetCameraLookAt(player.GetPlayerFoward());

//        //mon.SetSkill();
//        if (mon.skill1 != null)
//            SkillManager.SetSkill(mon.skill1, 1);
//        else
//            SkillManager.socket1.gameObject.SetActive(false);
//        if (mon.skill2 != null)
//            SkillManager.SetSkill(mon.skill2, 2);
//        else
//            SkillManager.socket2.gameObject.SetActive(false);

//        durationGauge.gameObject.SetActive(true);
//        durationGauge.value = 1;
//    }


//    public void GetMonster(Monsters _mon)
//    {
//        mon = _mon;

//        player.GetCC().Move(mon.transform.position - player.transform.position);


//        _mon.transform.parent = player.transform;
//        _mon.transform.localPosition = Vector3.zero;
//        _mon.transform.localEulerAngles = Vector3.zero;
//        if (_mon.GetComponent<Rigidbody>() != null)
//        {
//            _mon.GetComponent<Rigidbody>().velocity = Vector3.zero;
//            _mon.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
//            //_mon.GetComponent<Rigidbody>().isKinematic = true;
//        }

//        Enter();
//    }


//    public override void Move()
//    {
//        base.Move();
//    }

//    public override void StateUpdate()
//    {
//        base.StateUpdate();

//        if (mon.GetHP() <= 0)
//            player.SetState("Normal");
//        if (mon.skill1 != null)
//            mon.skill1.SetCurCD();
//        if (mon.skill2 != null)
//            mon.skill2.SetCurCD();
//        SetDuration();

//        //player.transform.position = mon.transform.position;
//        //mon.transform.localPosition = Vector3.zero;
//    }

//    public override void Jump()
//    {
//        base.Jump();
//        Debug.Log("poJump");
//    }
//    public override void Attack()
//    {
//        mon.Attack();
//    }
//    public override void Skill01()
//    {
//        mon.skill1?.UseSkill();
//    }
//    public override void Skill02()
//    {
//        mon.skill2?.UseSkill();
//    }

//    public override void Shift()
//    {
//        player.SetState("Normal");
//    }

//    public override void Exit()
//    {
//        mon.transform.parent = null;
//        player.GetCC().Move(mon.transform.position - player.transform.position);

//        mon.GetDamage((int)(mon.GetHP() / 10.0f));

//        player.CameraTransform = player.transform;

//        FXManager.Instance.PlayFX("PoExit", player.transform.position);

//        GameManager.Instance.SetCameraFollow(player.CameraTransform);
//        GameManager.Instance.SetCameraLookAt(player.GetPlayerFoward());

//        durationGauge.gameObject.SetActive(false);

//        orgJumpForce = playerOrgJumpForce;
//        jumpForce = playerOrgJumpForce;
//    }

//    void SetDuration()
//    {
//        durationGauge.value -= Time.deltaTime / player.GetDuration();
//        if (durationGauge.value <= 0) Shift();
//    }


//}
