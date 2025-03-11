using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotWhiskey.newStateMachine
{
    public class Player : MonoBehaviour
    {
        
        public PlayerStateMachine stateMachine { get; private set; }
        
        public PlayerIdleState IdleState { get; private set; }
        public PlayerMoveState MoveState { get; private set; }
        public PlayerInAirState InAirState { get; private set; }
        public PlayerJumpState JumpState { get; private set; }
        public PlayerLandState LandState { get; private set; }

        [SerializeField] private PlayerData playerData;

        public Animator anim { get; private set; }
        public PlayerInputHandler inputHandler { get; private set; }
        public Rigidbody rigid { get; private set; }
        public CapsuleCollider MovementCollider { get; private set; }

        private Vector3 workspace; //中间值
        private Vector3 Direciton; //朝向
        private float turnAmount;//旋转参数
        public Vector3 currentVelocity { get; private set; }

        private void Awake()
        {
            stateMachine = new PlayerStateMachine();

            IdleState = new PlayerIdleState(this, stateMachine, playerData, "idle");
            MoveState = new PlayerMoveState(this, stateMachine, playerData, "move");
            InAirState = new PlayerInAirState(this, stateMachine, playerData, "inAir");
            JumpState = new PlayerJumpState(this, stateMachine, playerData, "inAir");
            LandState = new PlayerLandState(this, stateMachine, playerData, "land");
        }

        private void Start()
        {
            anim = GetComponent<Animator>();
            inputHandler = GetComponent<PlayerInputHandler>();
            rigid = GetComponent<Rigidbody>();
            MovementCollider = GetComponent<CapsuleCollider>();
            
            stateMachine.InitializeState(IdleState);
        }

        private void Update()
        {
            stateMachine.currentState.LogicUpdate();
            
            Debug.Log(currentVelocity);
        }

        private void FixedUpdate()
        {
            stateMachine.currentState.PhysicsUpdate();
        }
        
        
        #region CheckFuctions
    
        /// <summary>
        /// 检测地面并设置是否与地面接触
        /// </summary>
        public bool CheckIfTouchingGrounded()
        {
            var collider = Physics.OverlapSphere(transform.position, playerData.groundCheckRadius, playerData.whatIsGround);
    
            if (collider.Length != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    
        /// <summary>
        /// 将检测体在Unity引擎中显示出来
        /// </summary>
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, playerData.groundCheckRadius);
        }
    
        #endregion
        
        
        #region setFuction

        /// <summary>
        /// 设置速度为0
        /// </summary>
        public void SetVelocityZero()
        {
            workspace = Vector3.zero;
            rigid.velocity = workspace;
            currentVelocity = workspace;
        }
    
        /// <summary>
        /// 设置水平速度或位移
        /// </summary>
        /// <param name="xInput">x轴输入</param>
        /// <param name="zInput">z轴输入</param>
        public void SetVelocityHorizontal(float xInput, float zInput)
        {
            workspace.Set(xInput, currentVelocity.y, zInput);
            rigid.velocity = workspace;
            currentVelocity = workspace;
        }
    
        /// <summary>
        /// 设置垂直速度或位移
        /// </summary>
        /// <param name="yInput">垂直变量</param>
        public void SetVelocityVertical(float yInput)
        {
            workspace.Set(currentVelocity.x, yInput, currentVelocity.z);
            rigid.velocity = workspace;
            currentVelocity = workspace;
        }

        /// <summary>
        /// 设置旋转
        /// </summary>
        /// <param name="xInput"></param>
        /// <param name="yInput"></param>
        public void SetRotation(float xInput, float yInput, float turnSpeed)
        {
            Direciton = transform.InverseTransformVector(new Vector3(xInput, 0, yInput));
            turnAmount = Mathf.Atan2(Direciton.x, Direciton.z);
            rigid.MoveRotation(rigid.rotation * Quaternion.Euler(0, turnAmount * turnSpeed, 0));
        }

        #endregion

        private void AnimationTrigger() => stateMachine.currentState.AnimationTrigger();
        
        private void AnimationFinishedTrigger() => stateMachine.currentState.AnimationFinishedTrigger();
    }

}
