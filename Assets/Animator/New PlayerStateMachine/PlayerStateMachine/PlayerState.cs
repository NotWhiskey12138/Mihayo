using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NotWhiskey.newStateMachine
{
    public class PlayerState
    {
        public Player player;
        public PlayerStateMachine stateMachine;
        public PlayerData playerData;

        protected bool isAnimationFinished;

        public string animBoolName;    //要播放的动画名字

        public PlayerState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
        {
            this.player = player;
            this.stateMachine = stateMachine;
            this.playerData = playerData;
            this.animBoolName = animBoolName;
        }

        public virtual void Enter()
        {
            DoCheck();
            player.anim.SetBool(animBoolName, true);
            isAnimationFinished = false;
        }

        public virtual void Exit()
        {
            player.anim.SetBool(animBoolName, false);
        }

        public virtual void LogicUpdate()
        {
            
        }

        public virtual void PhysicsUpdate()
        {
            DoCheck();
        }

        public virtual void DoCheck() { }
        
        public virtual void AnimationTrigger(){}

        public virtual void AnimationFinishedTrigger() => isAnimationFinished = true;
    }
}

