using System;
using UnityEngine;

namespace NotWhiskey.oldStateMachine
{
    public class PlayerAnimation : MonoBehaviour
    {
        private Animator anim;
        private Rigidbody rigid;
        private Player player;
    
        private void Awake()
        {
            anim = GetComponent<Animator>();
            rigid = GetComponent<Rigidbody>();
            player = GetComponent<Player>();
        }
    
        private void Update()
        {
            SetAnimations();
        }
    
        private void SetAnimations()
        {
            anim.SetFloat("speed", player.currentSpeed);
            anim.SetFloat("yVelocity", player.currentVelocity.y);
            anim.SetBool("isGround", player.isGrounded);
            anim.SetBool("isAttack", player.isAttack);
            anim.SetBool("isHurt", player.isHurt);
            anim.SetBool("isDead", player.isDead);
        }
    }
}

