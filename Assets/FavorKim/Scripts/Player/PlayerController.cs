using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Variable

    #region Mono

    private CharacterController CC;
    PlayerStateMachine state;

    #endregion

    #region Vector
    private Vector3 moveDir;
    #endregion

    #region float
    [SerializeField] private float moveSpeed;
    [SerializeField] private float gravityScale;
    [SerializeField] private float jumpForce;


    #endregion

    #endregion

    #region Getter
    public CharacterController GetCC() {  return CC; }
    public Vector3 GetMoveDir() { return moveDir; }
    public float GetMoveSpeed() { return moveSpeed; }
    public float GetGravityScale() {  return gravityScale; }
    public float GetJumpForce() {  return jumpForce; }
    #endregion

    private void Awake()
    {
        CC = GetComponent<CharacterController>();
        state = new PlayerStateMachine(this);
        
    }
    void Start()
    {
        
    }

    void Update()
    {
        state.StateUpdate();
    }

    void OnMove(InputValue val)
    {
        Vector2 dir = val.Get<Vector2>();
        moveDir = new Vector3(dir.x, 0, dir.y);
    }



    void OnJump(InputValue val) { if (val.isPressed) state.StateOnJump(); }
    
    void OnAttack(InputValue val) { if (val.isPressed) state.StateOnAttack(); }

    void OnSkill(InputValue val) { if (val.isPressed) state.StateOnSkill(); }
}
