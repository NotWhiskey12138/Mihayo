using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "NewPlayerData",fileName = "NewData/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("角色属性")] 
    public float walkSpeed;
    public float runSpeed;
    public float turnSpeed;
    
    [Header("跳跃属性")]
    public float jumpHeight;
    public int amountOfJumps = 1;
    
    [Header("地面检测")] 
    public float groundCheckRadius;     //地面检测半径
    public LayerMask whatIsGround;      //什么是地面
}
