using UnityEngine;

namespace NotWhiskey.newStateMachine
{
    public class PlayerMoveState : PlayerGroundState
    {

        public PlayerMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
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

            if (xInput == 0 && yInput == 0)
            {
                stateMachine.ChangeState(player.IdleState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            player.SetVelocityHorizontal(xInput * playerData.runSpeed, yInput * playerData.runSpeed);
            player.SetRotation(xInput, yInput, playerData.turnSpeed);
        }

        public override void DoCheck()
        {
            base.DoCheck();
        }
    }
}