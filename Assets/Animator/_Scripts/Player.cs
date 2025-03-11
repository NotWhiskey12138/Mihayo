using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NotWhiskey.oldStateMachine
{
    public class Player : MonoBehaviour
    {
        private InputController inputController;
        private Rigidbody rigid;
        private Animator anim;
    
        [Header("角色属性")] 
        public float walkSpeed; //走路速度
        public float runSpeed; //跑步速度
        public float jumpHeight; //跳跃高度
        public float turnSpeed; //旋转速度
        public float playerHealth; //角色血量
        public float hitDamage; //每次收到的血量伤害
    
        [Header("检测相关数值")] 
        public float groundCheckRadius; //地面检测半径
        public LayerMask whatIsGround; //地面图层
    
        [Header("显示当前血量UI")] 
        public TextMeshProUGUI health;
    
        public float currentSpeed { get; private set; } //当前速度
        public Vector3 currentVelocity { get; private set; } //当前速度方向
        private Vector3 Direciton; //朝向
        private Vector3 workSpace; //转换中间量
        private Vector3 inputDireciton; //输入的向量
    
        private float xInput; //x输入
        private float yInput; //y输入
        private float turnAmount; //旋转参数
    
        public bool isRunning { get; private set; } //是否正在跑步
        public bool isJumping { get; private set; } //是否正在跳跃
        public bool isGrounded { get; private set; } //是否在地面上
        public bool isAttack { get; private set; } //是否在攻击
        public bool isHurt { get; private set; } //是否受伤
        public bool isDead { get; private set; } //是否死亡
    
        private void Awake()
        {
            rigid = GetComponent<Rigidbody>();
            anim = GetComponent<Animator>();
            if (inputController == null) //初始化
                inputController = new InputController();
    
            //跳跃
            inputController.GamePlay.Jump.started += OnJump;
    
            //走路和跑步
            inputController.GamePlay.Run.performed += context => { isRunning = true; };
    
            inputController.GamePlay.Run.canceled += context => { isRunning = false; };
    
            //攻击
            inputController.GamePlay.Attack.performed += OnAttack;
            
            //受伤
            inputController.GamePlay.Hit.performed += OnHurt;
        }
    
        private void Update()
        {
            OnHealth();
    
            inputDireciton = inputController.GamePlay.Movement.ReadValue<Vector2>();
            xInput = inputDireciton.x;
            yInput = inputDireciton.y;
    
            //Debug.Log(inputDireciton);
            //Debug.Log(xInput);
            //Debug.Log(yInput);
            //Debug.Log(currentVelocity.y);
            currentVelocity = rigid.velocity;
    
    
            OnMove();
            CheckIfTouchingGround();
            //Debug.Log(isGrounded);
    
            health.text = playerHealth.ToString();

            Debug.Log(currentVelocity);
        }
    
        private void OnEnable()
        {
            inputController.Enable();
        }
    
        private void OnDisable()
        {
            inputController.Disable();
        }
    
        /// <summary>
        /// 设置移动
        /// </summary>
        private void OnMove()
        {
            Direciton = transform.InverseTransformVector(new Vector3(xInput, 0, yInput));
            turnAmount = Mathf.Atan2(Direciton.x, Direciton.z);
            rigid.MoveRotation(rigid.rotation * Quaternion.Euler(0, turnAmount * turnSpeed, 0));
    
    
            if (isRunning)
            {
                SetVelocityHorizontal(xInput * runSpeed, yInput * runSpeed);
                currentSpeed = new Vector3(xInput, 0, yInput).magnitude * runSpeed;
            }
            else
            {
                SetVelocityHorizontal(xInput * walkSpeed, yInput * walkSpeed);
                currentSpeed = new Vector3(xInput, 0, yInput).magnitude * walkSpeed;
            }
        }
    
        /// <summary>
        /// 设置跳跃
        /// </summary>
        /// <param name="context"></param>
        private void OnJump(InputAction.CallbackContext context)
        {
            if (isGrounded && !isAttack && !isHurt)
            {
                anim.SetTrigger("jump");
                SetVelocityVertical(jumpHeight);
            }
    
        }
    
        /// <summary>
        /// 攻击键
        /// </summary>
        /// <param name="context"></param>
        private void OnAttack(InputAction.CallbackContext context)
        {
            if (isGrounded && !isJumping && !isHurt)
            {
                isAttack = true;
                anim.SetTrigger("attack");
            }
        }
    
        private void OnHurt(InputAction.CallbackContext context)
        {
            if (!isDead && !isHurt)
            {
                isHurt = true;
                playerHealth -= hitDamage;
            }
        }
    
        private void OnHealth()
        {
            if (playerHealth <= 0)
            {
                SetVelocityZero();
                SetVelocityVertical(-3);
                isDead = true;
                inputController.GamePlay.Disable();
            }    
        }
        
        #region CheckFuctions
    
        /// <summary>
        /// 检测地面并设置是否与地面接触
        /// </summary>
        private void CheckIfTouchingGround()
        {
            var collider = Physics.OverlapSphere(transform.position, groundCheckRadius, whatIsGround);
    
            if (collider.Length != 0)
            {
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
        }
    
        /// <summary>
        /// 将检测体在Unity引擎中显示出来
        /// </summary>
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, groundCheckRadius);
        }
    
        #endregion
    
    
        #region SetFunctions
    
        /// <summary>
        /// 设置速度为0
        /// </summary>
        private void SetVelocityZero()
        {
            workSpace = Vector3.zero;
            rigid.velocity = workSpace;
            currentVelocity = workSpace;
        }
    
        /// <summary>
        /// 设置水平速度或位移
        /// </summary>
        /// <param name="xInput">x轴输入</param>
        /// <param name="zInput">z轴输入</param>
        private void SetVelocityHorizontal(float xInput, float zInput)
        {
            workSpace.Set(xInput, currentVelocity.y, zInput);
            rigid.velocity = workSpace;
            currentVelocity = workSpace;
        }
    
        /// <summary>
        /// 设置垂直速度或位移
        /// </summary>
        /// <param name="yInput">垂直变量</param>
        private void SetVelocityVertical(float yInput)
        {
            workSpace.Set(currentVelocity.x, yInput, currentVelocity.z);
            rigid.velocity = workSpace;
            currentVelocity = workSpace;
        }
    
        /// <summary>
        /// 玩家失去控制
        /// </summary>
        private void limitController()
        {
            inputController.GamePlay.Movement.Disable();
        }
    
        /// <summary>
        /// 玩家恢复控制
        /// </summary>
        private void ResetController()
        {
            inputController.GamePlay.Movement.Enable();
        }
    
        /// <summary>
        /// 设置攻击动画结束
        /// </summary>
        private void AttackAnimationFinished()
        {
            isAttack = false;
        }
    
        /// <summary>
        /// 设置受伤动画结束
        /// </summary>
        private void HurtAnimationFinished()
        {
            isHurt = false;
        }
    
    #endregion
        
        
        
    }
}

