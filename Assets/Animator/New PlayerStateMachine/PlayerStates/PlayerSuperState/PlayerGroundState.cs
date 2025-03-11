using UnityEngine;

namespace NotWhiskey.newStateMachine
{
    public class PlayerGroundState : PlayerState
    {
        protected float xInput;
        protected float yInput;
        protected bool jumpInput;

        private bool isGrounded;
        public PlayerGroundState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            xInput = player.inputHandler.xInput;
            yInput = player.inputHandler.yInput;
            jumpInput = player.inputHandler.JumpInput;

            if (jumpInput && player.JumpState.CanJump())
            {
                stateMachine.ChangeState(player.JumpState);
                player.inputHandler.UseJumpInput();
            }
            else if (!isGrounded)
            {
                player.JumpState.DecreaseAmountOfJumpLeft();
                stateMachine.ChangeState(player.InAirState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        public override void DoCheck()
        {
            base.DoCheck();
            
            isGrounded = player.CheckIfTouchingGrounded();
        }
    }
}