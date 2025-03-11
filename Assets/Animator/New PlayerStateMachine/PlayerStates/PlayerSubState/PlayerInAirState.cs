using UnityEngine;

namespace NotWhiskey.newStateMachine
{
    public class PlayerInAirState : PlayerState
    {
        private float xInput;
        private float yInput;

        private bool isGrounded;

        private bool jumpInput;

        private bool jumpInputStop;

        public PlayerInAirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }
        public override void Enter()
        {
            base.Enter(); 
            
            player.JumpState.ResetAmountOfJumpLeft();
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
            jumpInputStop = player.inputHandler.JumpInputStop;

            if (isGrounded && player.currentVelocity.y < 0.01f)
            {
                stateMachine.ChangeState(player.LandState);
            }
            else if (jumpInput && player.JumpState.CanJump())
            {
                stateMachine.ChangeState(player.JumpState);
            }
            else
            {
                player.SetVelocityHorizontal(playerData.runSpeed * xInput, playerData.runSpeed * yInput);

                player.anim.SetFloat("yVelocity", player.currentVelocity.y);
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