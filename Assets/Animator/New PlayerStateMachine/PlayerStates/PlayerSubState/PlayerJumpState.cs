using UnityEngine;

namespace NotWhiskey.newStateMachine
{
    public class PlayerJumpState : PlayerAbilityState
    {
        private int amountOfJumpsLeft;
        
        public PlayerJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
            amountOfJumpsLeft = playerData.amountOfJumps;
        }

        public override void Enter()
        {
            base.Enter();
            player.SetVelocityVertical(playerData.jumpHeight);
            isAbilityDone = true;
            DecreaseAmountOfJumpLeft();
        }

        public bool CanJump()
        {
            if (amountOfJumpsLeft > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        public void ResetAmountOfJumpLeft() => amountOfJumpsLeft = playerData.amountOfJumps;

        public void DecreaseAmountOfJumpLeft() => amountOfJumpsLeft--;
    }
}